# SCPReplacer

Lets players volunteer via `.volunteer <number>` to take over an SCP role within a time window after the original player disconnects, provided the SCP had enough health remaining. A separate `.human` command optionally lets an SCP forfeit their role early for a random human class.

Current version: **v1.2.0**. Independent rewrite (not a fork) of the concept from [jmoore34/ScpReplacer](https://github.com/jmoore34/ScpReplacer) — the original plugin this was based on was old and broken, rewritten from scratch rather than patched.

See `../CLAUDE.md` for shared plugin conventions and the `AutoUpdate` module design.

## Structure

- `Plugin.cs`, `Config.cs`, `Translation.cs`, `Util.cs`, `EventHandlers.cs`
- `Commands/` — `Volunteer.cs`, `Human.cs`
- `Models/` — `ScpToReplace.cs` (the lottery itself)
- `Modules/AutoUpdate.cs` — the shared module, see `../CLAUDE.md`

## Known history / gotchas

- The lottery timer starts when the SCP leaves (`ScpToReplace.Create` uses MEC `Timing.CallDelayed`), and `ClearAll()` kills the timers on round start, on `WaitingForPlayers`, and on disable. Don't use `Task.Delay` here - it can't be cancelled, which is what let stale lotteries resolve in the next round.
- Custom-role/effect cleanup (`GetCustomRoles()`/`RemoveRole()`/`DisableAllEffects()`) must run BEFORE `Role.Set`. EXILED's `RemoveRole` sets the player to Spectator by default (`RemovalKillsPlayer`), and `DisableAllEffects` strips the spawn protection the game grants during role init. This only covers EXILED custom roles, not UCR/SER roles.
- `Util.CanVolunteer` is the single eligibility rule, used by both the command and `Resolve`: spectators can volunteer, SCPs (except SCP-049-2) and Overwatch can't.
- `.human` and `OnLeft` both open a lottery through `ScpToReplace.Open`. `.human` is config-gated off by default (`human_forfeit_enabled`).
- SCP numbers are compared numerically via `Util.SameScp`, so `.volunteer 49` matches `049`.
- All command responses live in `Translation.cs`. Debug logging covers lottery open, volunteering, resolve and `.human`.
- `Util.cs` has the shared helpers for turning an SCP role into a colored "SCP-XXX" label (`FindScpRole`, `ColoredScpLabel`).