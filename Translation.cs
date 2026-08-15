namespace SCPReplacer
{
    using Exiled.API.Interfaces;
    using System.ComponentModel;

    /// <summary>
    /// The plugin's user-facing messages.
    /// </summary>
    public class Translation : ITranslation
    {
        /// <summary>
        /// Gets or sets the broadcast shown to everyone when an SCP quits and the lottery opens.
        /// </summary>
        [Description("The broadcast shown to everyone when an SCP quits and the lottery opens. {0} is the colored 'SCP-XXX' label, {1} is the plain SCP number, {2} is the lottery duration in seconds.")]
        public string LotteryOpenedBroadcast { get; set; } = "{0} has left! Type .volunteer {1} within {2} seconds for a chance to take their place!";

        /// <summary>
        /// Gets or sets the message shown to a player after they successfully volunteer.
        /// </summary>
        [Description("The message shown to a player after they successfully volunteer. {0} is the colored 'SCP-XXX' label.")]
        public string VolunteerConfirmed { get; set; } = "You've entered the lottery to become {0}!";

        /// <summary>
        /// Gets or sets the message shown to whoever wins the lottery.
        /// </summary>
        [Description("The message shown to whoever wins the lottery. {0} is the colored 'SCP-XXX' label.")]
        public string LotteryWon { get; set; } = "You were selected! You are now {0}!";

        /// <summary>
        /// Gets or sets the message shown when nobody volunteers in time.
        /// </summary>
        [Description("The message shown when nobody volunteers in time.")]
        public string LotteryNoVolunteers { get; set; } = "<color=orange>Nobody volunteered in time</color> - the SCP will not be replaced.";

        /// <summary>
        /// Gets or sets the message shown when an SCP successfully gives up their role early.
        /// </summary>
        [Description("The message shown when an SCP successfully gives up their role early. {0} is the colored new role name.")]
        public string HumanForfeitConfirmed { get; set; } = "You have given up your SCP role and became a {0}.";

        /// <summary>
        /// Gets or sets the header prepended to every broadcast this plugin sends.
        /// </summary>
        [Description("The header prepended to every broadcast this plugin sends.")]
        public string BroadcastHeader { get; set; } = "<color=red>[SCP Replacer]</color>\n";

        /// <summary>
        /// Gets or sets the message broadcast to everyone else once an SCP has been successfully replaced.
        /// </summary>
        [Description("The message broadcast to everyone else once an SCP has been successfully replaced. {0} is the colored 'SCP-XXX' label.")]
        public string ReplacementAnnouncement { get; set; } = "{0} has been replaced.";
    }
}