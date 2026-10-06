<p align="center"><img src="logo.svg" width="160" alt="SCPReplacer logo"></p>

<h1 align="center">SCPReplacer</h1>

<p align="center">
  <a href="https://github.com/Storption/SCPReplacer/releases/latest"><img src="https://img.shields.io/github/downloads/Storption/SCPReplacer/total?style=for-the-badge&logo=github&color=blue" alt="Downloads"></a>
  <a href="https://github.com/Storption/SCPReplacer/releases/latest"><img src="https://img.shields.io/github/v/release/Storption/SCPReplacer?include_prereleases&style=for-the-badge&logo=github&label=Latest%20Release&color=green" alt="Latest release"></a>
  <a href="https://join.storption.com"><img src="https://img.shields.io/discord/1114170053949128817?style=for-the-badge&color=5865F2&logo=discord&label=Discord&logoColor=white" alt="Discord"></a>
</p>

An [EXILED](https://github.com/ExMod-Team/EXILED) plugin for SCP: Secret Laboratory that lets players volunteer to take over an SCP role after the original player disconnects early in the round.

Inspired by / based on the concept from [jmoore34/ScpReplacer](https://github.com/jmoore34/ScpReplacer) — this is an independent rewrite, not a fork.

## How it works
If an SCP disconnects within a configurable time window at the start of the round, and had at least a configurable percentage of their health remaining, a broadcast opens a short lottery: any eligible player (spectators included) can type `.volunteer <number>` to enter, e.g. `.volunteer 49` or `.v 079`. The countdown starts as soon as the SCP leaves, and once it ends a random volunteer is chosen and takes over that SCP.

**Custom SCPs** - SCP roles from [UncomplicatedCustomRoles](https://github.com/UncomplicatedCustomServer/UncomplicatedCustomRoles) and EXILED's CustomRoles are replaced as themselves: the lottery is for e.g. "SCP-939-53" (`.volunteer 939-53`), the winner becomes that custom SCP, and takes over where it left. Custom roles built on a zombie (SCP-049-2) only get a lottery if they're listed in `custom_zombie_scps` (SCP-008 by default), so custom zombie variants stay as replaceable as normal zombies. Other plugins can add their own through the API below.

Optionally, a separate command (`.human` / `.no`, disabled by default) lets an SCP voluntarily give up their role early for a random human class. Their SCP slot then goes through the same volunteer lottery.

**Auto-update** - checks this plugin's own GitHub repo for a newer release, and if found, downloads it, verifies it against the release's SHA-256, and applies it. If restarting is enabled, players are told in-game and the server restarts once the round ends.

## Requirements

- [EXILED](https://github.com/ExMod-Team/EXILED) 9.14.2 or later

## Installation

1. Download the latest `SCPReplacer.dll` from the [Releases](https://github.com/Storption/SCPReplacer/releases) page.
2. Place it in your server's EXILED plugins folder (`%AppData%\EXILED\Plugins` on Windows, `~/.config/EXILED/Plugins` on Linux).
3. Restart your server. A default config will be generated on first load.

## Config

```yaml
# Whether the plugin is enabled.
is_enabled: true
# Whether debug messages are shown.
debug: false
# How many seconds into the round an SCP can disconnect and still trigger a replacement lottery.
quit_cutoff_seconds: 60
# The minimum health percentage (0-100) the SCP must have had remaining to trigger a replacement.
required_health_percent: 100
# How many seconds players have to volunteer once the lottery opens.
lottery_period_seconds: 15
# Whether the .human/.no forfeit command is enabled at all.
human_forfeit_enabled: false
# Whether SCP roles from UncomplicatedCustomRoles (if installed) are offered in the lottery as themselves, and given to the winner.
uncomplicated_custom_roles_support: true
# Whether SCP roles from EXILED's CustomRoles are offered in the lottery as themselves, and given to the winner.
exiled_custom_roles_support: true
# Custom roles built on SCP-049-2 that count as full SCPs and get a lottery, by name. Other custom zombies are left alone, like normal zombies.
custom_zombie_scps:
- SCP-008
# Whether to check for and automatically install updates.
auto_update_enabled: true
# Whether to keep a backup of the previous .dll before replacing it with an update.
auto_update_backup: true
# Whether to automatically restart the server once the current round ends, to apply a downloaded update. Never restarts mid-round.
auto_update_restart: true
```

All broadcast and message text, including the header shown on every plugin broadcast, is configurable via the generated translation file.

## For plugin developers

Plugins with their own custom SCPs can have them replaced as themselves. Register a provider that recognises your SCPs; it's asked when an SCP leaves or forfeits, before their role changes.

```csharp
using SCPReplacer.API;

CustomScp? GetMyScp(Player player) => IsMyScp(player)
    ? new CustomScp("SCP-1234", RoleTypeId.Scp939, winner => MakeMyScp(winner))
    : null;

CustomScps.RegisterProvider(GetMyScp);   // in OnEnabled
CustomScps.UnregisterProvider(GetMyScp); // in OnDisabled
```

- `Name` is shown in broadcasts, and players volunteer with it, with or without the "SCP-" prefix (`.volunteer 1234`).
- The winner is set to `BaseRole`, your `Apply` action runs, then they're moved to where the SCP left. Exceptions from providers or `Apply` are logged and never stop the lottery.
- Available from v1.3.0. To make SCPReplacer optional, reference `SCPReplacer.dll` without copying it, keep every call to `SCPReplacer.API` in one class of its own, and only call that class when `Exiled.Loader.Loader.Plugins` contains SCPReplacer v1.3.0 or later.
