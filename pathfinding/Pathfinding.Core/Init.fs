module Pathfinding.Core.Init

open System
open Pathfinding.Core.Domain.Grid
open Pathfinding.Core.State
open Pathfinding.Core.Domain.Settings

open Domain.BreadthFirst
let initSettings = {
  Diagonal = false
  Width = 10
  Height = 10
  Cost = false
  Position = false
  Arrow = false }
let initStart = {X=0;Y=0}
let initTarget = {X=initSettings.Width-1;Y=initSettings.Height-1}
let initRandomGrid state: Grid =
  let rand = Random()
  let grid =
    [ for x in [0..state.Settings.Width-1] do
      for y in [0..state.Settings.Height-1] do
        let terrain =
          match rand.Next(0, 3) with
          | 0 -> Blocked
          | _ -> Walkable
        ({X=x;Y=y}, terrain) ]
    |> Map.ofList
  grid
    .Add(state.GetStart, Start)
    .Add(state.GetTarget, Target)
let initGrid settings: Grid =
  let grid =
    [ for x in [0..settings.Width-1] do
      for y in [0..settings.Height-1] do 
        ({X=x;Y=y}, Walkable) ]
    |> Map.ofList
  grid
    .Add(initStart, Start)
    .Add({X=settings.Width-1;Y=settings.Height-1}, Target)
let initData: Domain.BreadthFirst.Data = {
  ClosedNodes = []
  OpenNodes = [createNode None 0 initStart] }
let initState(): State = {
    Grid = initGrid initSettings
    Solutions = []
    BreadthFirstData = initData
    View = GridView
    Settings = initSettings }
