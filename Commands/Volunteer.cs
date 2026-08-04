namespace SCPReplacer.Commands
{
    using System;
    using CommandSystem;
    using Exiled.API.Extensions;
    using Exiled.API.Features;
    using PlayerRoles;
    using SCPReplacer.Models;

    /// <summary>
    /// The .volunteer command, letting a player enter the lottery for a specific SCP.
    /// </summary>
    [CommandHandler(typeof(ClientCommandHandler))]
    public class Volunteer : ICommand
    {
        /// <inheritdoc/>
        public string Command => "volunteer";

        /// <inheritdoc/>
        public string[] Aliases { get; set; } = { "v" };

        /// <inheritdoc/>
        public string Description => "Volunteer to become an SCP that left at the start of the round.";

        /// <inheritdoc/>
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count != 1)
            {
                response = "Usage: .volunteer <SCP number>. Example: .volunteer 079 or .v 079";
                return false;
            }

            if (Player.Get(sender) is not Player player)
            {
                response = "You must be a player to use this command.";
                return false;
            }

            if (player.IsScp && player.Role != RoleTypeId.Scp0492)
            {
                response = "SCPs cannot use this command.";
                return false;
            }

            string requestedScp = arguments.FirstElement().ScpNumber();
            ScpToReplace? role = ScpToReplace.Find(requestedScp);

            if (role is null)
            {
                response = ScpToReplace.AnyPending
                    ? "That SCP isn't available. Currently available: " + string.Join(", ", ScpToReplace.PendingNames)
                    : "No SCPs are currently eligible for replacement.";
                return false;
            }

            if (role.Volunteers.Contains(player))
            {
                response = "You have already volunteered to replace this SCP.";
                return false;
            }

            role.Volunteers.Add(player);
            role.StartLotteryIfNeeded();

            Translation translation = Plugin.Instance!.Translation;
            response = string.Format(translation.VolunteerConfirmed, role.Name);

            player.Broadcast(new Broadcast(
                translation.BroadcastHeader + response,
                5));

            return true;
        }
    }
}