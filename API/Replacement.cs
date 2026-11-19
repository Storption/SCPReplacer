namespace SCPReplacer.API
{
    /// <summary>
    /// Lets other plugins switch SCPReplacer off for a while, such as during a minigame where SCPs aren't the usual SCPs.
    /// </summary>
    public static class Replacement
    {
        /// <summary>
        /// Gets or sets whether replacements are paused: while true, an SCP leaving opens no lottery and <c>.human</c> is
        /// refused. Lotteries already open carry on.
        /// </summary>
        public static bool Paused { get; set; }
    }
}
