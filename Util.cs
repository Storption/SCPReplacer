namespace SCPReplacer
{
    using System.Text.RegularExpressions;
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
    }
}