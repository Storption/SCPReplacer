namespace SCPReplacer.API
{
    using Exiled.API.Features;
    using PlayerRoles;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Lets other plugins tell SCPReplacer about their custom SCPs.
    /// </summary>
    public static class CustomScps
    {
        private static readonly List<Func<Player, CustomScp?>> Providers = new();

        /// <summary>
        /// Registers a provider: a function that returns the custom SCP a player is, or null if they aren't one of yours.
        /// It's asked when an SCP leaves or forfeits, before their role changes. Registering the same provider twice does nothing.
        /// </summary>
        public static void RegisterProvider(Func<Player, CustomScp?> provider)
        {
            if (provider is null)
                throw new ArgumentNullException(nameof(provider));

            if (!Providers.Contains(provider))
                Providers.Add(provider);
        }

        /// <summary>
        /// Unregisters a provider.
        /// </summary>
        /// <returns>Whether the provider was registered.</returns>
        public static bool UnregisterProvider(Func<Player, CustomScp?> provider) => Providers.Remove(provider);

        /// <summary>
        /// Gets the custom SCP a player is, or null if no provider claims them.
        /// </summary>
        public static CustomScp? Get(Player player)
        {
            foreach (Func<Player, CustomScp?> provider in Providers)
            {
                // Another plugin's provider failing mustn't stop the lottery.
                try
                {
                    if (provider(player) is CustomScp scp)
                        return scp;
                }
                catch (Exception ex)
                {
                    Log.Error($"A custom SCP provider failed for {player.Nickname}: {ex.Message}");
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the custom SCP a player is, unless it's a custom zombie the config doesn't list as a full SCP.
        /// </summary>
        internal static CustomScp? GetReplaceable(Player player)
        {
            CustomScp? scp = Get(player);
            if (scp is null || scp.BaseRole != RoleTypeId.Scp0492)
                return scp;

            return Plugin.Instance!.Config.CustomZombieScps.Any(name => Util.SameScp(name, scp.Name)) ? scp : null;
        }
    }
}