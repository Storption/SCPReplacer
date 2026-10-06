namespace SCPReplacer.API
{
    using System;
    using Exiled.API.Features;
    using PlayerRoles;

    /// <summary>
    /// An SCP role added by another plugin, which SCPReplacer can offer in its lottery and give to the winner.
    /// </summary>
    public sealed class CustomScp
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomScp"/> class.
        /// </summary>
        /// <param name="name">The role's name, like "SCP-939-53".</param>
        /// <param name="baseRole">The vanilla role it's built on.</param>
        /// <param name="apply">Turns a player into this custom SCP.</param>
        public CustomScp(string name, RoleTypeId baseRole, Action<Player> apply)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            BaseRole = baseRole;
            Apply = apply ?? throw new ArgumentNullException(nameof(apply));
        }

        /// <summary>
        /// Gets the role's name, like "SCP-939-53", shown in broadcasts. Players volunteer with it, with or without its "SCP-" prefix.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the vanilla role it's built on. The winner is set to it before <see cref="Apply"/> runs.
        /// </summary>
        public RoleTypeId BaseRole { get; }

        /// <summary>
        /// Gets the action that turns the lottery winner into this custom SCP. Afterwards, the winner is moved to where the SCP left.
        /// </summary>
        public Action<Player> Apply { get; }
    }
}