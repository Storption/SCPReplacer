namespace SCPReplacer
{
    using System;
    using Exiled.API.Features;
    using SCPReplacer.API;
    using SCPReplacer.Integrations;
    using SCPReplacer.Models;
    using PlayerHandlers = Exiled.Events.Handlers.Player;
    using ServerHandlers = Exiled.Events.Handlers.Server;

    public class Plugin : Plugin<Config, Translation>
    {
        private const string LegacyLotteryBroadcast = "{0} has left! Type .volunteer {1} within {2} seconds for a chance to take their place!";

        private EventHandlers? eventHandlers;

        public static Plugin? Instance { get; private set; }

        public override string Author => "Storption";

        public override string Name => "SCPReplacer";

        public override string Prefix => "SCPReplacer";

        public override Version RequiredExiledVersion { get; } = new Version(9, 14, 2);

        public override Version Version { get; } = new Version(1, 4, 0);

        public override void OnEnabled()
        {
            Instance = this;

            if (Translation.LotteryOpenedBroadcast == LegacyLotteryBroadcast)
                Log.Warn("Translation file reset recommended: it still contains the pre-1.2.0 lottery message. Delete SCPReplacer's translation file (EXILED/Configs/Translations/SCPReplacer/<port>.yml) and restart to regenerate it with the updated text. If you use a single merged translations file, delete only the SCPReplacer section.");

            eventHandlers = new EventHandlers();
            PlayerHandlers.Left += eventHandlers.OnLeft;
            ServerHandlers.RoundStarted += ScpToReplace.ClearAll;
            ServerHandlers.WaitingForPlayers += ScpToReplace.ClearAll;
            ServerHandlers.RoundEnded += eventHandlers.OnRoundEnded;

            if (Config.UncomplicatedCustomRolesSupport && UcrScps.TryResolve())
                CustomScps.RegisterProvider(UcrScps.Get);

            if (Config.ExiledCustomRolesSupport)
                CustomScps.RegisterProvider(ExiledScps.Get);

            Modules.AutoUpdate.RegisterEvents();

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            if (eventHandlers is not null)
            {
                PlayerHandlers.Left -= eventHandlers.OnLeft;
                ServerHandlers.RoundEnded -= eventHandlers.OnRoundEnded;
            }

            ServerHandlers.RoundStarted -= ScpToReplace.ClearAll;
            ServerHandlers.WaitingForPlayers -= ScpToReplace.ClearAll;

            ScpToReplace.ClearAll();

            CustomScps.UnregisterProvider(UcrScps.Get);
            CustomScps.UnregisterProvider(ExiledScps.Get);

            Modules.AutoUpdate.UnregisterEvents();

            eventHandlers = null;
            Instance = null;

            base.OnDisabled();
        }
    }
}