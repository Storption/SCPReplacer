namespace SCPReplacer
{
    using Exiled.API.Features;
    using Exiled.Events.EventArgs.Player;
    using PlayerRoles;
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

            if (!player.IsScp || player.Role == RoleTypeId.Scp0492)
                return;

            Config config = Plugin.Instance!.Config;
            double elapsedSeconds = Round.ElapsedTime.TotalSeconds;
            double requiredHealth = config.RequiredHealthPercent / 100.0 * player.MaxHealth;

            if (config.Debug)
                Log.Debug($"{player.Nickname} left {elapsedSeconds:F1}s into the round as {player.Role}, with {player.Health}/{player.MaxHealth} HP ({requiredHealth:F1} required).");

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

            string scpNumber = player.Role.Type.ScpNumber();

            if (ScpToReplace.Find(scpNumber) is not null)
                return;

            ScpToReplace.Create(scpNumber);

            Translation translation = Plugin.Instance.Translation;
            string message = translation.BroadcastHeader + string.Format(translation.LotteryOpenedBroadcast, config.LotteryPeriodSeconds);

            Broadcast broadcastObj = new Broadcast(message, (ushort)config.LotteryPeriodSeconds);
            foreach (Player p in Player.List)
                p.Broadcast(broadcastObj);
        }

        /// <summary>
        /// Called when a new round starts.
        /// </summary>
        public void OnRoundStarted()
        {
            ScpToReplace.ClearAll();
        }
    }
}