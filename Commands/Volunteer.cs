namespace SCPReplacer.Commands
{
    using System;
    using CommandSystem;
    using Exiled.API.Extensions;
    using Exiled.API.Features;
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
        public string[] Aliases { get; } = { "v" };

        /// <inheritdoc/>
        public string Description => "Volunteer to become an SCP that left at the start of the round.";

        /// <inheritdoc/>
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Translation translation = Plugin.Instance!.Translation;

            if (arguments.Count != 1)
            {
                response = translation.VolunteerUsage;
                return false;
            }

            if (Player.Get(sender) is not Player player)
            {
                response = translation.NotAPlayer;
                return false;
            }

            if (!player.CanVolunteer())
            {
                response = translation.VolunteerNotEligible;
                return false;
            }

            ScpToReplace? role = ScpToReplace.Find(arguments.FirstElement());

            if (role is null)
            {
                response = ScpToReplace.AnyPending
                    ? string.Format(translation.VolunteerUnavailable, string.Join(", ", ScpToReplace.PendingNames))
                    : translation.VolunteerNoneEligible;
                return false;
            }

            if (player.UserId == role.FormerUserId)
            {
                response = translation.VolunteerFormerHolder;
                return false;
            }

            if (role.Volunteers.Contains(player))
            {
                response = translation.VolunteerAlreadyEntered;
                return false;
            }

            role.Volunteers.Add(player);

            if (Plugin.Instance.Config.Debug)
                Log.Debug($"{player.Nickname} volunteered for {role.Name} ({role.Volunteers.Count} entered).");

            string coloredScpLabel = role.Label;
            response = string.Format(translation.VolunteerConfirmed, coloredScpLabel);

            player.Broadcast(new Broadcast(translation.BroadcastHeader + response, 5));

            return true;
        }
    }
}