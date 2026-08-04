# SCPReplacer

[![Downloads](https://img.shields.io/github/downloads/Storption/SCPReplacer/total?style=for-the-badge&logo=github&color=%)](https://github.com/Storption/SCPReplacer/releases/latest)
[![Latest](https://img.shields.io/github/v/release/Storption/SCPReplacer?include_prereleases&style=for-the-badge&logo=github&label=Latest%20Release&color=%)](https://github.com/Storption/SCPReplacer/releases/latest)

## How it works
If an SCP disconnects within a configurable time window at the start of the round, and had at least a configurable percentage of their health remaining, a broadcast opens a short lottery: any eligible player can type `.volunteer <number>` to enter. Once the lottery period ends, a random volunteer is chosen and takes over that SCP.

Optionally, a separate command lets an SCP voluntarily give up their role early (for a random human class) instead of waiting to be replaced.

## Requirements

- [EXILED](https://github.com/ExMod-Team/EXILED) 9.14.2 or later

## Installation

1. Download the latest `SCPReplacer.dll` from the [Releases](https://github.com/Storption/SCPReplacer/releases) page.
2. Place it in your server's EXILED plugins folder (`%AppData%\EXILED\Plugins` on Windows).
3. Restart your server. A default config will be generated on first load.

## Config

```yaml
# Whether the plugin is enabled.
is_enabled: true
# Whether debug messages are shown.
debug: false
# How many seconds into the round an SCP can disconnect and still trigger a replacement lottery.
quit_cutoff_secconds: 60
# The minimum health percentage (0-100) the SCP must have had remaining to trigger a replacement.
required_health_percent: 100
# How many seconds players have to volunteer once the lottery opens.
lottery_period_seconds: 15
# Whether the .human/.no forfeit command is enabled at all.
human_forfeit_enabled: false
```

All broadcast and message text, including the header shown on every plugin broadcast, is configurable via the generated translation file.