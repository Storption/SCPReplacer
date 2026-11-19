namespace SCPReplacer
{
    using System.ComponentModel;
    using Exiled.API.Interfaces;

    public class Config : IConfig
    {
        [Description("Whether the plugin is enabled.")]
        public bool IsEnabled { get; set; } = true;

        [Description("Whether debug messages are shown.")]
        public bool Debug { get; set; } = false;

        [Description("How many seconds into the round an SCP can disconnect and still trigger a replacement lottery.")]
        public int QuitCutoffSeconds { get; set; } = 60;

        [Description("The minimum health percentage (0-100) the SCP must have had remaining to trigger a replacement.")]
        public int RequiredHealthPercent { get; set; } = 100;

        [Description("How many seconds players have to volunteer once the lottery opens.")]
        public int LotteryPeriodSeconds { get; set; } = 15;

        [Description("Whether the .human/.no forfeit command is enabled at all.")]
        public bool HumanForfeitEnabled { get; set; } = false;

        [Description("Whether SCP roles from UncomplicatedCustomRoles (if installed) are offered in the lottery as themselves, and given to the winner.")]
        public bool UncomplicatedCustomRolesSupport { get; set; } = true;

        [Description("Whether SCP roles from EXILED's CustomRoles are offered in the lottery as themselves, and given to the winner.")]
        public bool ExiledCustomRolesSupport { get; set; } = true;

        [Description("Custom roles built on SCP-049-2 that count as full SCPs and get a lottery, by name. Other custom zombies are left alone, like normal zombies.")]
        public List<string> CustomZombieScps { get; set; } = new() { "SCP-008" };

        [Description("Whether to check for and automatically install updates.")]
        public bool AutoUpdateEnabled { get; set; } = true;

        [Description("Whether to keep a backup of the previous .dll before replacing it with an update.")]
        public bool AutoUpdateBackup { get; set; } = false;

        [Description("Whether to automatically restart the server once the current round ends, to apply a downloaded update. Never restarts mid-round.")]
        public bool AutoUpdateRestart { get; set; } = true;
    }
}