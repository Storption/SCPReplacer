namespace SCPReplacer
{
    using System.Text.RegularExpressions;
    using Exiled.API.Extensions;
    using Exiled.API.Features;
    using PlayerRoles;

    internal static class Util
    {
        public static string ScpNumber(this RoleTypeId role)
        {
            return Regex.Replace(role.ToString(), "[^0-9]", string.Empty);
        }

        public static string ColoredScpLabel(this RoleTypeId role)
        {
            string colorHex = role.GetColor().ToHex();
            return $"<color={colorHex}>SCP-{role.ScpNumber()}</color>";
        }

        public static bool CanVolunteer(this Player player)
        {
            if (!player.IsConnected || player.Role.Type == RoleTypeId.Overwatch)
                return false;

            return !player.IsScp || player.Role.Type == RoleTypeId.Scp0492;
        }

        public static string StripScpPrefix(string name) => Regex.Replace(name.Trim(), "^scp[-_ ]?", string.Empty, RegexOptions.IgnoreCase);

        public static bool SameScp(string first, string second)
        {
            string a = Normalize(first);
            string b = Normalize(second);
            return a.Length > 0 && (a == b || (int.TryParse(a, out int x) && int.TryParse(b, out int y) && x == y));
        }

        private static string Normalize(string name) => Regex.Replace(StripScpPrefix(name), "[^0-9A-Za-z]", string.Empty).ToLowerInvariant();
    }
}