namespace SCPReplacer
{
    using Exiled.API.Interfaces;
    using System.ComponentModel;

    /// <summary>
    /// The plugin's configuration.
    /// </summary>
    public class Config : IConfig
    {
        /// <inheritdoc/>
        [Description("Whether the plugin is enabled.")]
        public bool IsEnabled { get; set; } = true;

        /// <inheritdoc/>
        [Description("Whether debug messages are shown.")]
        public bool Debug { get; set; } = false;

        /// <summary>
        /// Gets or sets the number of seconds into the round an SCP can quit and still trigger a replacement.
        /// </summary>
        [Description("How many seconds into the round an SCP can disconnect and still trigger a replacement lottery.")]
        public int QuitCutoffSeconds { get; set; } = 60;

        /// <summary>
        /// Gets or sets the minimum health percentage (0-100) the SCP must have had to trigger a replacement.
        /// </summary>
        [Description("The minimum health percentage (0-100) the SCP must have had remaining to trigger a replacement.")]
        public int RequiredHealthPercent { get; set; } = 100;

        /// <summary>
        /// Gets or sets how many seconds players have to volunteer once the lottery opens.
        /// </summary>
        [Description("How many seconds players have to volunteer once the lottery opens.")]
        public int LotteryPeriodSeconds { get; set; } = 15;

        /// <summary>
        /// Gets or sets a value indicating whether the .human command (an SCP voluntarily giving up their role) is enabled.
        /// </summary>
        [Description("Whether the .human/.no forfeit command is enabled at all.")]
        public bool HumanForfeitEnabled { get; set; } = false;
    }
}