module Pathfinding.Core.State

open Pathfinding.Core.Domain
open Pathfinding.Core.Domain.Grid

type Solution = {
  Start: Position
  Target: Position
  Path: Position list }
type View =
  | GridView
  | SettingsView
type State = {
  Grid: Grid
  Solutions: Solution list
  BreadthFirstData: BreadthFirst.Data
  View: View
  Settings: Settings.Settings
} with
  member this.UpdateGrid pos state = {this with Grid = this.Grid.Add(pos, state)}
  member this.GetStart = this.Grid |> Map.filter(fun pos terrain -> terrain = Start) |> List.ofSeq |> List.map(fun x -> x.Key) |> List.head
  member this.GetTarget = this.Grid |> Map.filter(fun pos terrain -> terrain = Target) |> List.ofSeq |> List.map(fun x -> x.Key) |> List.head
  member this.OpenNodesPositionList = this.BreadthFirstData.OpenNodes |> List.map(fun n -> n.Position)
  member this.ClosedNodesPositionList = this.BreadthFirstData.ClosedNodes |> List.map(fun n -> n.Position)
  member this.SolutionsContainsPos pos =
    this.Solutions
    |> List.collect(fun sol -> sol.Path)
    |> List.contains pos