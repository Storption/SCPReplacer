namespace SCPReplacer
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Exiled.API.Features;
    using Exiled.API.Extensions;
    using PlayerRoles;

    /// <summary>
    /// Shared helper methods used across the plugin.
    /// </summary>
    public static class Util
    {
        /// <summary>
        /// Gets the SCP number for a given role type (e.g. RoleTypeId.Scp079 becomes "079").
        /// </summary>
        public static string ScpNumber(this RoleTypeId role)
        {
            return Regex.Replace(role.ToString(), "[^0-9]", string.Empty);
        }

        /// <summary>
        /// Builds a "SCP-XXX" label colored with that SCP's own role color.
        /// </summary>
        public static string ColoredScpLabel(this RoleTypeId role)
        {
            string colorHex = role.GetColor().ToHex();
            return $"<color={colorHex}>SCP-{role.ScpNumber()}</color>";
        }

        /// <summary>
        /// Whether a player may enter, and be picked in, the replacement lottery. Spectators can, SCPs (other than SCP-049-2) and Overwatch can't.
        /// </summary>
        public static bool CanVolunteer(this Player player)
        {
            if (!player.IsConnected || player.Role.Type == RoleTypeId.Overwatch)
                return false;

            return !player.IsScp || player.Role.Type == RoleTypeId.Scp0492;
        }

        /// <summary>
        /// Removes a leading "SCP" or "SCP-" from a name, so "SCP-939-53" becomes "939-53".
        /// </summary>
        public static string StripScpPrefix(string name) => Regex.Replace(name.Trim(), "^scp[-_ ]?", string.Empty, RegexOptions.IgnoreCase);

        /// <summary>
        /// Gets whether two SCP names match, loosely: case, an "SCP" prefix and separators don't matter ("scp-939-53" matches "93953"),
        /// and numbers compare as numbers, so "49" matches "049".
        /// </summary>
        public static bool SameScp(string first, string second)
        {
            string a = Normalize(first);
            string b = Normalize(second);
            return a.Length > 0 && (a == b || (int.TryParse(a, out int x) && int.TryParse(b, out int y) && x == y));
        }

        private static string Normalize(string name) => Regex.Replace(StripScpPrefix(name), "[^0-9A-Za-z]", string.Empty).ToLowerInvariant();
    }
}