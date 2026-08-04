namespace SCPReplacer.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Exiled.API.Features;
    using Exiled.API.Enums;
    using PlayerRoles;

    /// <summary>
    /// Tracks a single SCP currently awaiting replacement via the volunteer lottery.
    /// </summary>
    public class ScpToReplace
    {
        private static readonly List<ScpToReplace> Pending = new();

        private ScpToReplace(string scpName)
        {
            Name = scpName;
            Volunteers = new List<Player>();
        }

        /// <summary>
        /// Gets the SCP number this replacement is for (e.g. "079").
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the players who have volunteered so far.
        /// </summary>
        public List<Player> Volunteers { get; }

        /// <summary>
        /// Gets a value indicating whether the lottery countdown has already started.
        /// </summary>
        public bool LotteryStarted { get; private set; }

        /// <summary>
        /// Gets a value indicating whether anything is currently awaiting replacement.
        /// </summary>
        public static bool AnyPending => Pending.Count > 0;

        /// <summary>
        /// Gets the SCP numbers currently awaiting replacement.
        /// </summary>
        public static IEnumerable<string> PendingNames => Pending.Select(r => r.Name);

        /// <summary>
        /// Finds a pending replacement by SCP number, if one exists.
        /// </summary>
        public static ScpToReplace? Find(string scpName)
        {
            return Pending.FirstOrDefault(r => r.Name == scpName);
        }

        /// <summary>
        /// Registers a new SCP as awaiting replacement.
        /// </summary>
        public static ScpToReplace Create(string scpName)
        {
            ScpToReplace role = new(scpName);
            Pending.Add(role);
            return role;
        }

        /// <summary>
        /// Clears every pending replacement - called on round start.
        /// </summary>
        public static void ClearAll()
        {
            Pending.Clear();
        }

        /// <summary>
        /// Starts the lottery countdown, if it hasn't already started. Called when the first player volunteers.
        /// </summary>
        public async void StartLotteryIfNeeded()
        {
            if (LotteryStarted)
                return;

            LotteryStarted = true;
            await Task.Delay(TimeSpan.FromSeconds(Plugin.Instance!.Config.LotteryPeriodSeconds));
            Resolve();
        }

        private void Resolve()
        {
            Pending.Remove(this);

            Player? chosen = Volunteers
                .Where(p => p.IsAlive && !p.IsScp)
                .OrderBy(_ => Guid.NewGuid())
                .FirstOrDefault();

            Translation translation = Plugin.Instance!.Translation;

            if (chosen is null)
            {
                string noVolunteersMessage = translation.BroadcastHeader + translation.LotteryNoVolunteers;
                foreach (Player p in Player.List)
                    p.Broadcast(new Broadcast(noVolunteersMessage, 5));
                return;
            }

            RoleTypeId scpRole = Enum.GetValues(typeof(RoleTypeId))
                .Cast<RoleTypeId>()
                .First(r => r.ScpNumber() == Name);

            chosen.Role.Set(scpRole, SpawnReason.LateJoin);

            foreach (Player p in Player.List)
            {
                string message = p == chosen
                    ? translation.BroadcastHeader + string.Format(translation.LotteryWon, Name)
                    : translation.BroadcastHeader + string.Format(translation.ReplacementAnnouncement, Name);

                p.Broadcast(new Broadcast(message, 5));
            }
        }
    }
}