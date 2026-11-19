namespace SCPReplacer
{
    using Exiled.API.Extensions;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs.Player;
    using Exiled.Events.EventArgs.Server;
    using PlayerRoles;
    using SCPReplacer.API;
    using SCPReplacer.Models;

    internal class EventHandlers
    {
        public void OnLeft(LeftEventArgs ev)
        {
            if (Replacement.Paused)
                return;

            Player player = ev.Player;
            CustomScp? custom = CustomScps.GetReplaceable(player);

            // Zombies are never replaced, unless they're a custom zombie listed in custom_zombie_scps.
            if (custom is null && (!player.IsScp || player.Role == RoleTypeId.Scp0492))
                return;

            Config config = Plugin.Instance!.Config;
            double elapsedSeconds = Round.ElapsedTime.TotalSeconds;
            double requiredHealth = config.RequiredHealthPercent / 100.0 * player.MaxHealth;

            Log.Debug($"{player.Nickname} left {elapsedSeconds:F1}s into the round as {custom?.Name ?? player.Role.ToString()}, with {player.Health}/{player.MaxHealth} HP ({requiredHealth:F1} required).");

            if (elapsedSeconds > config.QuitCutoffSeconds)
            {
                Log.Debug("Not eligible - quit cutoff already passed.");
                return;
            }

            if (player.Health < requiredHealth)
            {
                Log.Debug("Not eligible - health too low.");
                return;
            }

            ScpToReplace.Open(custom?.BaseRole ?? player.Role.Type, custom, player.UserId, player.Position);
        }

        public void OnRoundEnded(RoundEndedEventArgs ev) => ScpToReplace.ClearAll();
    }
}