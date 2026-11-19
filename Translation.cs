namespace SCPReplacer
{
    using System.ComponentModel;
    using Exiled.API.Interfaces;

    public class Translation : ITranslation
    {
        [Description("The broadcast shown to everyone when an SCP quits and the lottery opens. {0} is the colored 'SCP-XXX' label, {1} is the plain SCP number, {2} is the lottery duration in seconds.")]
        public string LotteryOpenedBroadcast { get; set; } = "{0} needs a replacement! Press ~ and type .volunteer {1} within {2} seconds for a chance to take over!";

        [Description("The message shown to a player after they successfully volunteer. {0} is the colored 'SCP-XXX' label.")]
        public string VolunteerConfirmed { get; set; } = "You've entered the lottery to become {0}!";

        [Description("The message shown to whoever wins the lottery. {0} is the colored 'SCP-XXX' label.")]
        public string LotteryWon { get; set; } = "You were selected! You are now {0}!";

        [Description("The message shown when nobody volunteers in time.")]
        public string LotteryNoVolunteers { get; set; } = "<color=orange>Nobody volunteered in time</color> - the SCP will not be replaced.";

        [Description("The message shown when an SCP successfully gives up their role early. {0} is the colored new role name.")]
        public string HumanForfeitConfirmed { get; set; } = "You have given up your SCP role and became a {0}.";

        [Description("The header prepended to every broadcast this plugin sends.")]
        public string BroadcastHeader { get; set; } = "<color=red>[SCP Replacer]</color>\n";

        [Description("The message broadcast to everyone else once an SCP has been successfully replaced. {0} is the colored 'SCP-XXX' label.")]
        public string ReplacementAnnouncement { get; set; } = "{0} has been replaced.";

        [Description("The response when a command is used by something other than a player, such as the server console.")]
        public string NotAPlayer { get; set; } = "You must be a player to use this command.";

        [Description("The usage text shown when .volunteer is used with the wrong arguments.")]
        public string VolunteerUsage { get; set; } = "Usage: .volunteer <SCP number>. Example: .volunteer 079 or .v 079";

        [Description("The response when a player who can't take part in the lottery (an SCP or Overwatch) tries to volunteer.")]
        public string VolunteerNotEligible { get; set; } = "You can't volunteer right now.";

        [Description("The response when the requested SCP isn't up for replacement. {0} is the list of SCP numbers that are.")]
        public string VolunteerUnavailable { get; set; } = "That SCP isn't available. Currently available: {0}";

        [Description("The response when no SCP is currently up for replacement.")]
        public string VolunteerNoneEligible { get; set; } = "No SCPs are currently eligible for replacement.";

        [Description("The response when a player volunteers twice for the same SCP.")]
        public string VolunteerAlreadyEntered { get; set; } = "You have already volunteered to replace this SCP.";

        [Description("The response when .human is used while the forfeit command is disabled.")]
        public string HumanDisabled { get; set; } = "This command is currently disabled.";

        [Description("The response when a player who isn't an SCP (or is SCP-049-2) uses .human.")]
        public string HumanNotScp { get; set; } = "You must be an SCP (other than SCP-049-2) to use this command.";

        [Description("The response when .human is used after the quit cutoff has passed.")]
        public string HumanTooLate { get; set; } = "This command must be used closer to the start of the round.";

        public string HumanPaused { get; set; } = "You can't forfeit your SCP right now.";

        [Description("The response when .human is used while below the required health.")]
        public string HumanLowHealth { get; set; } = "You are too low on health to use this command.";

        [Description("The response when the player who left or gave up an SCP tries to volunteer for it again.")]
        public string VolunteerFormerHolder { get; set; } = "You gave up that SCP, so you can't volunteer for it.";

        [Description("The broadcast shown to everyone when a plugin update has been installed and the server will restart once the round ends. {0} is the plugin's name. Leave empty to disable.")]
        public string AutoUpdateRestartBroadcast { get; set; } = "<color=orange>[Update]</color> {0} was updated - the server will restart after this round to apply it.";
    }
}