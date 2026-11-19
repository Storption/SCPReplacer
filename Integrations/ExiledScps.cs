namespace SCPReplacer.Integrations
{
    using Exiled.API.Features;
    using Exiled.CustomRoles.API;
    using Exiled.CustomRoles.API.Features;
    using PlayerRoles;
    using SCPReplacer.API;

    internal static class ExiledScps
    {
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