namespace SCPReplacer.Commands
{
    using System;
    using CommandSystem;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using PlayerRoles;

    /// <summary>
    /// The .human command, letting and SCP voluntarily give up their role early for a random human class.
    /// </summary>
    [CommandHandler(typeof(ClientCommandHandler))]
    public class Human : ICommand
    {
        /// <inheritdoc/>
        public string Command => "human";

        /// <inheritdoc/>
        public string[] Aliases { get; } = { "no" };

        /// <inheritdoc/>
        public string Description => "Forfeit being an SCP and become a random human class instead.";

        /// <inheritdoc/>
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Config config = Plugin.Instance!.Config;

            if (!config.HumanForfeitEnabled)
            {
                response = "This command is currently disabled.";
                return false;
            }

            if (Player.Get(sender) is not Player player)
            {
                response = "You must be a player to use this command.";
                return false;
            }

            if (!player.IsScp || player.Role == RoleTypeId.Scp0492)
            {
                response = "You must be an SCP (other than SCP-049-2) to use this command.";
                return false;
            }

            if (Round.ElapsedTime.TotalSeconds > config.QuitCutoffSeconds)
            {
                response = "This command must be used closer to the start of the round.";
                return false;
            }

            double requiredHealth = config.RequiredHealthPercent / 100.0 * player.MaxHealth;
            if (player.Health < requiredHealth)
            {
                response = "You are too low on health to use this command.";
                return false;
            }

            RoleTypeId newRole = UnityEngine.Random.value switch
            {
                < 0.45f => RoleTypeId.ClassD,
                < 0.9f => RoleTypeId.Scientist,
                _ => RoleTypeId.FacilityGuard,
            };

            player.Role.Set(newRole, SpawnReason.LateJoin, RoleSpawnFlags.All);

            Translation translation = Plugin.Instance.Translation;
            response = string.Format(translation.HumanForfeitConfirmed, newRole);

            player.Broadcast(new Broadcast(translation.BroadcastHeader + response, 5));

            return true;
        }
    }
}