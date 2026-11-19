namespace SCPReplacer.Commands
{
    using System;
    using CommandSystem;
    using Exiled.API.Extensions;
    using Exiled.API.Features;
    using SCPReplacer.Models;

    [CommandHandler(typeof(ClientCommandHandler))]
    public class Volunteer : ICommand
    {
        public string Command => "volunteer";

        public string[] Aliases { get; } = { "v" };

        public string Description => "Volunteer to become an SCP that left at the start of the round.";

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

            ScpToReplace? lottery = ScpToReplace.Find(arguments.FirstElement());

            if (lottery is null)
            {
                response = ScpToReplace.AnyPending
                    ? string.Format(translation.VolunteerUnavailable, string.Join(", ", ScpToReplace.PendingNames))
                    : translation.VolunteerNoneEligible;
                return false;
            }

            if (player.UserId == lottery.FormerUserId)
            {
                response = translation.VolunteerFormerHolder;
                return false;
            }

            if (lottery.Volunteers.Contains(player))
            {
                response = translation.VolunteerAlreadyEntered;
                return false;
            }

            lottery.Volunteers.Add(player);

            Log.Debug($"{player.Nickname} volunteered for {lottery.Name} ({lottery.Volunteers.Count} entered).");

            response = string.Format(translation.VolunteerConfirmed, lottery.Label);

            player.Broadcast(new Broadcast(translation.BroadcastHeader + response, 5));

            return true;
        }
    }
}