module Route.Update

open Route.Domain
open Pathfinding.Core
open Pathfinding.Core.Domain.BreadthFirst
open Pathfinding.Core.Domain.Grid
open Pathfinding.Core.Domain.Settings
open Pathfinding.Core.State
///
/// MVU-version of a viewmodel, right now we only send back a new state but can return a tuple of state and a function/command.
/// 
let update (command:Input) (state:Output) =
  // todo; separate into modules/smaller functions, this will get messy if we have more than 5+ modules
  match command with
  | ChangeView view -> {state with View = view}, None
  | ChangeSetting command ->
    {state with Settings = (updateSettings state.Settings command)}, Some Reset
  | ToggleTerrain pos ->
    let toggle =
      match state.Grid.[pos] with
      | Walkable -> Blocked
      | Blocked -> Walkable
      | x -> x
    {state with Grid = state.Grid.Add(pos, toggle)}, None
  | RandomTerrain ->
    let newGrid = Init.initRandomGrid state
    {state with Grid = newGrid}, None
  | Reset ->
    {state with BreadthFirstData = Init.initData;Solutions = [];Grid = Init.initGrid state.Settings}, None
  | RunBreadthFirst ->
    let closedNodes = Pathfinding.BreadthFirst.run Init.initData state
    let newData = {state.BreadthFirstData with ClosedNodes = closedNodes}
    let newSolutions = Pathfinding.BreadthFirst.getSolutions state.GetStart state.GetTarget closedNodes
    {state with BreadthFirstData = newData;Solutions = newSolutions}, None
  | RunBreadthFirstOnce ->
    let openNodes, closedNodes = Pathfinding.BreadthFirst.runOnce state.BreadthFirstData.OpenNodes state.BreadthFirstData.ClosedNodes state
    {state with BreadthFirstData = { OpenNodes = openNodes;ClosedNodes = closedNodes }}, None
  | ToggleRunTimer ->
    debugState <- not debugState
    state, Some Reset
