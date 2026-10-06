namespace SCPReplacer.Integrations
{
    using Exiled.API.Features;
    using Exiled.CustomRoles.API;
    using Exiled.CustomRoles.API.Features;
    using PlayerRoles;
    using SCPReplacer.API;

    /// <summary>
    /// Offers SCPs made with EXILED's CustomRoles in the lottery.
    /// </summary>
    internal static class ExiledScps
    {
        /// <summary>
        /// Gets the EXILED custom SCP a player is, or null if they aren't one.
        /// </summary>
        public static CustomScp? Get(Player player)
        {
            foreach (CustomRole role in player.GetCustomRoles())
            {
                if (role.Role.GetTeam() == Team.SCPs)
                    return new CustomScp(role.Name, role.Role, role.AddRole);
            }

            return null;
        }
    }
}