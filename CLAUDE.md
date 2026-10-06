# SCPReplacer

Lets players volunteer via `.volunteer <number>` to take over an SCP role within a time window after the original player disconnects, provided the SCP had enough health remaining. A separate `.human` command optionally lets an SCP forfeit their role early for a random human class.

Current version: **v1.3.0**. Independent rewrite (not a fork) of the concept from [jmoore34/ScpReplacer](https://github.com/jmoore34/ScpReplacer) — the original plugin this was based on was old and broken, rewritten from scratch rather than patched.

See `../CLAUDE.md` for shared plugin conventions and the `AutoUpdate` module design.

## Structure

- `Plugin.cs`, `Config.cs`, `Translation.cs`, `Util.cs`, `EventHandlers.cs`
- `Commands/` — `Volunteer.cs`, `Human.cs`
- `Models/` — `ScpToReplace.cs` (the lottery itself)
- `API/` — the public custom SCP API (`CustomScp`, `CustomScps`). A contract: don't rename or change anything public in it without a major version.
- `Integrations/` — built-in providers: `UcrScps.cs` (UncomplicatedCustomRoles, by reflection) and `ExiledScps.cs` (EXILED CustomRoles).
- `Modules/AutoUpdate.cs` — the shared module, see `../CLAUDE.md`

## Known history / gotchas

- The lottery timer starts when the SCP leaves (`ScpToReplace.Open` uses MEC `Timing.CallDelayed`), and `ClearAll()` kills the timers on round start, on `WaitingForPlayers`, `RoundEnded`, and on disable. Don't use `Task.Delay` here - it can't be cancelled, which is what let stale lotteries resolve in the next round.
- Custom-role/effect cleanup (`GetCustomRoles()`/`RemoveRole()`/`DisableAllEffects()`) must run BEFORE `Role.Set`. EXILED's `RemoveRole` sets the player to Spectator by default (`RemovalKillsPlayer`), and `DisableAllEffects` strips the spawn protection the game grants during role init. This only covers EXILED custom roles, not UCR/SER roles.
- `Util.CanVolunteer` is the single eligibility rule, used by both the command and `Resolve`: spectators can volunteer, SCPs (except SCP-049-2) and Overwatch can't.
- `.human` and `OnLeft` both open a lottery through `ScpToReplace.Open`. `.human` is config-gated off by default (`human_forfeit_enabled`).
- Lottery names match loosely via `Util.SameScp`: case, an "SCP" prefix and separators are ignored (`scp-939-53` = `93953`), and numeric names compare as numbers, so `.volunteer 49` matches `049`.
- All command responses live in `Translation.cs`. Debug logging covers lottery open, volunteering, resolve and `.human`.
- `ScpToReplace` stores the SCP as a `RoleTypeId` (`Role`, the base role for a custom SCP) plus `Custom`. `Name` is what players type: the SCP number for vanilla ("079"), or the custom name without "SCP-" ("939-53"), so SCP-939 and SCP-939-53 lotteries can run side by side. `Label` is the colored name for broadcasts.
- `ClearAll()` also runs on `RoundEnded`, so a lottery can't resolve on the end-of-round screen.
- `.human` shows the new role as its code name with spaces added ("Class D", "Facility Guard"), not the game's full name ("Class-D Personnel" reads badly after "became a").

## Custom SCPs (v1.3.0+)

- `CustomScps.Get(player)` asks every registered provider, in order, whether a leaving or forfeiting player is a custom SCP; provider exceptions are logged and skipped. `OnLeft` and `.human` call `CustomScps.GetReplaceable` instead, **before** anything changes the role: it drops custom SCPs on a zombie base unless their name is in `custom_zombie_scps` (matched with `Util.SameScp`, default `SCP-008`), so custom zombie variants are skipped like plain zombies. The filter covers API providers too: it's the server admin's call, not the providing plugin's.
- Timing on leave: EXILED's `Left` fires at the very start of `CustomNetworkManager.OnServerDisconnect`, before the hub is destroyed. UCR forgets the role on LabAPI's `Left` (from `ReferenceHub.OnDestroy`) and EXILED CustomRoles on `Destroying`, both later, so the custom role and position are still readable.
- The winner of a custom lottery is set to the base role (without the spawnpoint), `Apply` runs, then they're moved to `Position`, where the SCP left. UCR's `SummonedCustomRole.Summon` and EXILED's `CustomRole.AddRole` both set the role synchronously, so the final move sticks (only EXILED's inventory is delayed). A failing `Apply` is logged and leaves them as the base role.
- `UcrScps` resolves `SummonedCustomRole.TryGet(ReferenceHub, out)`, `SummonedCustomRole.Summon(LabApi Player, ICustomRole)` and `ICustomRole.Name`/`Role` once on enable; only SCP-team base roles count. Config: `uncomplicated_custom_roles_support`, `exiled_custom_roles_support`.
- Providers are registered in `OnEnabled` and unregistered in `OnDisabled` by method group; delegates from the same static method compare equal, so that removes exactly what was registered.
