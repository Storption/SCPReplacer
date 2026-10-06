namespace SCPReplacer
{
    using Exiled.API.Extensions;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs.Player;
    using Exiled.Events.EventArgs.Server;
    using PlayerRoles;
    using SCPReplacer.API;
    using SCPReplacer.Models;

    /// <summary>
    /// Handles the plugin's game event subscriptions.
    /// </summary>
    public class EventHandlers
    {
        /// <summary>
        /// Called when a player leaves the server.
        /// </summary>
        public void OnLeft(LeftEventArgs ev)
        {
            Player player = ev.Player;
            CustomScp? custom = CustomScps.GetReplaceable(player);

            // Zombies are never replaced, unless they're a custom zombie listed in custom_zombie_scps.
            if (custom is null && (!player.IsScp || player.Role == RoleTypeId.Scp0492))
                return;

            Config config = Plugin.Instance!.Config;
            double elapsedSeconds = Round.ElapsedTime.TotalSeconds;
            double requiredHealth = config.RequiredHealthPercent / 100.0 * player.MaxHealth;

            if (config.Debug)
                Log.Debug($"{player.Nickname} left {elapsedSeconds:F1}s into the round as {custom?.Name ?? player.Role.ToString()}, with {player.Health}/{player.MaxHealth} HP ({requiredHealth:F1} required).");

            if (elapsedSeconds > config.QuitCutoffSeconds)
            {
                if (config.Debug)
                    Log.Debug("Not eligible - quit cutoff already passed.");
                return;
            }

            if (player.Health < requiredHealth)
            {
                if (config.Debug)
                    Log.Debug("Not eligible - health too low.");
                return;
            }

            ScpToReplace.Open(custom?.BaseRole ?? player.Role.Type, custom, player.UserId, player.Position);
        }

        /// <summary>
        /// Called when a new round starts.
        /// </summary>
        public void OnRoundStarted()
        {
            ScpToReplace.ClearAll();
        }

        /// <summary>
        /// Called when the server returns to the lobby between rounds.
        /// </summary>
        public void OnWaitingForPlayers()
        {
            ScpToReplace.ClearAll();
        }

        /// <summary>
        /// Called when the round ends.
        /// </summary>
        public void OnRoundEnded(RoundEndedEventArgs ev)
        {
            ScpToReplace.ClearAll();
        }
    }
}