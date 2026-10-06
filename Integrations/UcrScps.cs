namespace SCPReplacer.Integrations
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Exiled.API.Features;
    using PlayerRoles;
    using SCPReplacer.API;
    using LabApiPlayer = LabApi.Features.Wrappers.Player;

    /// <summary>
    /// Offers UncomplicatedCustomRoles SCPs in the lottery. UCR is reached through reflection, so it stays optional.
    /// </summary>
    internal static class UcrScps
    {
        private static MethodInfo? tryGet;
        private static MethodInfo? summon;
        private static PropertyInfo? summonedRole;
        private static PropertyInfo? roleName;
        private static PropertyInfo? baseRole;

        /// <summary>
        /// Finds UCR's API, if UCR is installed.
        /// </summary>
        public static bool TryResolve()
        {
            Assembly? assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName().Name == "UncomplicatedCustomRoles");
            Type? summoned = assembly?.GetType("UncomplicatedCustomRoles.API.Features.SummonedCustomRole");
            Type? customRole = assembly?.GetType("UncomplicatedCustomRoles.API.Interfaces.ICustomRole");
            if (summoned is null || customRole is null)
                return false;

            tryGet = summoned.GetMethod("TryGet", new[] { typeof(ReferenceHub), summoned.MakeByRefType() });
            summon = summoned.GetMethod("Summon", new[] { typeof(LabApiPlayer), customRole });
            summonedRole = summoned.GetProperty("Role");
            roleName = customRole.GetProperty("Name");
            baseRole = customRole.GetProperty("Role");

            return tryGet is not null && summon is not null && summonedRole is not null && roleName is not null && baseRole is not null;
        }

        /// <summary>
        /// Gets the UCR SCP a player is, or null if they aren't one.
        /// </summary>
        public static CustomScp? Get(Player player)
        {
            object?[] args = { player.ReferenceHub, null };
            if (tryGet!.Invoke(null, args) is not true || args[1] is null)
                return null;

            object? role = summonedRole!.GetValue(args[1]);
            if (role is null || baseRole!.GetValue(role) is not RoleTypeId scpRole || scpRole.GetTeam() != Team.SCPs)
                return null;

            string name = roleName!.GetValue(role) as string ?? scpRole.ToString();
            return new CustomScp(name, scpRole, winner => summon!.Invoke(null, new[] { LabApiPlayer.Get(winner.ReferenceHub), role }));
        }
    }
}