namespace SCPReplacer
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Exiled.API.Extensions;
    using PlayerRoles;

    /// <summary>
    /// Shared helper methods used across the plugin.
    /// </summary>
    public static class Util
    {
        /// <summary>
        /// Strips everything except digits from user-typed input (e.g. "SCP-079" becomes "079").
        /// </summary>
        public static string ScpNumber(this string input)
        {
            return Regex.Replace(input, "[^0-9]", string.Empty);
        }

        /// <summary>
        /// Gets the SCP number for a given role type (e.g. RoleTypeId.Scp079 becomes "079").
        /// </summary>
        public static string ScpNumber(this RoleTypeId role)
        {
            return Regex.Replace(role.ToString(), "[^0-9]", string.Empty);
        }

        /// <summary>
        /// Finds the RoleTypeId matching a given SCP number string (e.g. "079" -> RoleTypeId.Scp079).
        /// </summary>
        public static RoleTypeId FindScpRole(string scpNumber)
        {
            return Enum.GetValues(typeof(RoleTypeId))
                .Cast<RoleTypeId>()
                .First(r => r.ScpNumber() == scpNumber);
        }

        /// <summary>
        /// Builds a "SCP-XXX" label colored with that SCP's own role color.
        /// </summary>
        public static string ColoredScpLabel(this RoleTypeId role)
        {
            string colorHex = role.GetColor().ToHex();
            return $"<color={colorHex}>SCP-{role.ScpNumber()}</color>";
        }
    }
}