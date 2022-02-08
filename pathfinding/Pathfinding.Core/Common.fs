module Pathfinding.Core.Common

open Pathfinding.Core.Domain.BreadthFirst
open Pathfinding.Core.Domain.Grid
open Pathfinding.Core.State

let createPosition x y = {X=x;Y=y}
let upFrom pos = { X = pos.X; Y = pos.Y - 1 }
let downFrom pos = { X = pos.X; Y = pos.Y + 1 }
let leftFrom pos = { X = pos.X + 1; Y = pos.Y }
let rightFrom pos = { X = pos.X - 1; Y = pos.Y }
let getNeighbors (pos:Position) (state:State) =
  let output =
    [ pos |> downFrom
      pos |> rightFrom
      pos |> leftFrom
      pos |> upFrom
      if state.Settings.Diagonal then
        pos |> downFrom |> rightFrom
        pos |> downFrom |> leftFrom
        pos |> upFrom |> rightFrom
        pos |> upFrom |> leftFrom ] 
  output
  |> List.filter(state.Grid.ContainsKey)
  |> List.filter(fun pos ->
    state.Grid.[pos] = Walkable ||
    state.Grid.[pos] = Target)
let filterInList (nodes:Node list) pos =
  nodes
  |> List.map(fun node -> node.Position)
  |> List.contains(pos)
  |> not
