namespace SCPReplacer.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Exiled.API.Enums;
    using Exiled.API.Extensions;
    using Exiled.API.Features;
    using Exiled.CustomRoles.API;
    using Exiled.CustomRoles.API.Features;
    using MEC;
    using PlayerRoles;
    using SCPReplacer.API;
    using UnityEngine;

    internal class ScpToReplace
    {
        private static readonly List<ScpToReplace> Pending = new();

        private CoroutineHandle timer;

        private ScpToReplace(RoleTypeId role, CustomScp? custom, string formerUserId, Vector3 position)
        {
            Role = role;
            Custom = custom;
            FormerUserId = formerUserId;
            Position = position;
            Name = custom is null ? role.ScpNumber() : Util.StripScpPrefix(custom.Name);
            Label = custom is null ? role.ColoredScpLabel() : $"<color={custom.BaseRole.GetColor().ToHex()}>{custom.Name}</color>";
        }

        public RoleTypeId Role { get; }

        public CustomScp? Custom { get; }

        public string Name { get; }

        public string Label { get; }

        public string FormerUserId { get; }

        public Vector3 Position { get; }

        public List<Player> Volunteers { get; } = new();

        public static bool AnyPending => Pending.Count > 0;

        public static IEnumerable<string> PendingNames => Pending.Select(r => r.Name);

        public static ScpToReplace? Find(string name) => Pending.FirstOrDefault(r => Util.SameScp(r.Name, name));

        public static void Open(RoleTypeId role, CustomScp? custom, string formerUserId, Vector3 position)
        {
            ScpToReplace replacement = new(role, custom, formerUserId, position);
            if (Find(replacement.Name) is not null)
                return;

            Config config = Plugin.Instance!.Config;
            Translation translation = Plugin.Instance!.Translation;

            replacement.timer = Timing.CallDelayed(config.LotteryPeriodSeconds, replacement.Resolve);
            Pending.Add(replacement);

            Log.Debug($"Lottery opened for {replacement.Name}{(custom is null ? string.Empty : $" (custom, base {custom.BaseRole})")}, resolving in {config.LotteryPeriodSeconds}s.");

            string message = translation.BroadcastHeader + string.Format(translation.LotteryOpenedBroadcast, replacement.Label, replacement.Name, config.LotteryPeriodSeconds);
            Broadcast broadcast = new(message, (ushort)config.LotteryPeriodSeconds);
            foreach (Player p in Player.List)
                p.Broadcast(broadcast);
        }

        public static void ClearAll()
        {
            foreach (ScpToReplace role in Pending)
                Timing.KillCoroutines(role.timer);

            Pending.Clear();
        }

        private void Resolve()
        {
            Pending.Remove(this);

            Player? chosen = Volunteers
                .Where(p => p.CanVolunteer())
                .OrderBy(_ => Guid.NewGuid())
                .FirstOrDefault();

            Translation translation = Plugin.Instance!.Translation;

            if (chosen is null)
            {
                Log.Debug($"No eligible volunteers for {Name} ({Volunteers.Count} entered) - it will not be replaced.");

                string noVolunteersMessage = translation.BroadcastHeader + translation.LotteryNoVolunteers;
                foreach (Player p in Player.List)
                    p.Broadcast(new Broadcast(noVolunteersMessage, 5));
                return;
            }

            Log.Debug($"{chosen.Nickname} won the lottery for {Name} out of {Volunteers.Count} volunteer(s).");

            foreach (CustomRole customRole in chosen.GetCustomRoles())
                customRole.RemoveRole(chosen);

            chosen.DisableAllEffects();

            if (Custom is null)
                chosen.Role.Set(Role, SpawnReason.LateJoin);
            else
                GiveCustom(chosen);

            foreach (Player p in Player.List)
            {
                string message = p == chosen
                    ? translation.BroadcastHeader + string.Format(translation.LotteryWon, Label)
                    : translation.BroadcastHeader + string.Format(translation.ReplacementAnnouncement, Label);

                p.Broadcast(new Broadcast(message, 5));
            }
        }

        // The base role first, the custom SCP on top of it, then back to where the SCP left: the replacement takes over
        // in place, whatever spawn the custom role would normally use.
        private void GiveCustom(Player chosen)
        {
            bool knownPosition = Position != Vector3.zero;
            chosen.Role.Set(Custom!.BaseRole, SpawnReason.LateJoin, knownPosition ? RoleSpawnFlags.AssignInventory : RoleSpawnFlags.All);

            try
            {
                Custom.Apply(chosen);
            }
            catch (Exception ex)
            {
                Log.Error($"Couldn't give {chosen.Nickname} the custom SCP {Custom.Name}, so they stay {Custom.BaseRole}: {ex.Message}");
            }

            if (knownPosition)
                chosen.Position = Position;
        }
    }
}