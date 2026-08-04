namespace SCPReplacer
{
    using System;
    using Exiled.API.Features;
    using PlayerHandlers = Exiled.Events.Handlers.Player;
    using ServerHandlers = Exiled.Events.Handlers.Server;

    /// <summary>
    /// The main plugin class.
    /// </summary>
    public class Plugin : Plugin<Config, Translation>
    {
        private EventHandlers? eventHandlers;

        /// <summary>
        /// Gets the only existing instance of the <see cref="Plugin"/> class.
        /// </summary>
        public static Plugin? Instance { get; private set; }

        /// <inheritdoc/>
        public override string Author => "Storption";

        /// <inheritdoc/>
        public override string Name => "SCPReplacer";

        /// <inheritdoc/>
        public override string Prefix => "SCPReplacer";

        /// <inheritdoc/>
        public override Version RequiredExiledVersion { get; } = new Version(9, 14, 2);

        /// <inheritdoc/>
        public override Version Version { get; } = new Version(1, 0, 0);

        /// <inheritdoc/>
        public override void OnEnabled()
        {
            Instance = this;

            eventHandlers = new EventHandlers();
            PlayerHandlers.Left += eventHandlers.OnLeft;
            ServerHandlers.RoundStarted += eventHandlers.OnRoundStarted;

            base.OnEnabled();
        }

        /// <inheritdoc/>
        public override void OnDisabled()
        {
            if (eventHandlers is not null)
            {
                PlayerHandlers.Left -= eventHandlers.OnLeft;
                ServerHandlers.RoundStarted -= eventHandlers.OnRoundStarted;
            }

            eventHandlers = null;
            Instance = null;

            base.OnDisabled();
        }
    }
}