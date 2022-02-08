module Route.Domain

open Pathfinding.Core.Domain.Grid
open Pathfinding.Core.Domain.Settings
open Pathfinding.Core.State
open Pathfinding.Core.Init

type Input =
  | ToggleTerrain of Position
  | RunBreadthFirstOnce
  | RunBreadthFirst
  | Reset
  | RandomTerrain
  | ChangeSetting of ChangeSettingsCommand
  | ChangeView of View
  | ToggleRunTimer
type Output = State
let initOutput: Output = initState()
