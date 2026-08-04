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
        [Description("The broadcast shown to everyone when an SCP quits and the lottery opens. {0} is the lottery duration in seconds.")]
        public string LotteryOpenedBroadcast { get; set; } = "An SCP has left! Type .volunteer <number> within {0} seconds for a chance to take their place!";

        /// <summary>
        /// Gets or sets the message shown to a player after they successfully volunteer.
        /// </summary>
        [Description("The message shown to a player after they successfully volunteer. {0} is the SCP number.")]
        public string VolunteerConfirmed { get; set; } = "You've entered the lottery to become SCP-{0}!";

        /// <summary>
        /// Gets or sets the message shown to whoever wins the lottery.
        /// </summary>
        [Description("The message shown to whoever wins the lottery. {0} is the SCP number.")]
        public string LotteryWon { get; set; } = "You were selected! You are now SCP-{0}!";

        /// <summary>
        /// Gets or sets the message shown when nobody volunteers in time.
        /// </summary>
        [Description("The message shown when nobody volunteers in time.")]
        public string LotteryNoVolunteers { get; set; } = "Nobody volunteered in time - the SCP will not be replaced.";

        /// <summary>
        /// Gets or sets the message shown when the quit happened too late in the round.
        /// </summary>
        [Description("The message shown when the quit happened too late in the round.")]
        public string CutoffPassed { get; set; } = "Too much time has passed in the round for a replacement.";

        /// <summary>
        /// Gets or sets the message shown when the SCP didn't have enough health remaining to trigger a replacement.
        /// </summary>
        [Description("The message shown when the SCP didn't have enough health remaining.")]
        public string HealthTooLow { get; set; } = "That SCP didn't have enough health remaining to trigger a replacement.";

        /// <summary>
        /// Gets or sets the message shown when an SCP successfully gives up their role early.
        /// </summary>
        [Description("The message shown when an SCP successfully gives up their role early. {0} is the new role name.")]
        public string HumanForfeitConfirmed { get; set; } = "You have given up your SCP role and became a {0}.";

        /// <summary>
        /// Gets or sets the header prepended to every broadcast this plugin sends.
        /// </summary>
        [Description("The header prepended to every broadcast this plugin sends.")]
        public string BroadcastHeader { get; set; } = "<color=red>[SCP Replacer]</color>\n";

        /// <summary>
        /// Gets or sets the message broadcast to everyone else once an SCP has been successfully replaced.
        /// </summary>
        [Description("The message broadcast to everyone else once an SCP has been successfully replaced. {0} is the SCP number.")]
        public string ReplacementAnnouncement { get; set; } = "SCP-{0} has been replaced.";
    }
}