module Tests.State
open Pathfinding.Core.Domain.BreadthFirst
open Pathfinding.Core.Domain.Grid
open Pathfinding.Core.Domain.Settings
open Pathfinding.Core.State

///
/// separate state for testing purposes
/// 

let initStart = {X=0;Y=0}
let initSettings: Settings = {
  Diagonal = false
  Cost = false 
  Arrow =  false
  Position =  false
  Width = 2
  Height = 2 }
let initWidth = 2
let initHeight = 2
let initStartNode = ({X=0;Y=0}, Start)
let initBlockedNode = ({X=1;Y=0}, Blocked)
let initWalkableNode = ({X=0;Y=1}, Walkable)
let initTargetNode = ({X=initWidth-1;Y=initHeight-1}, Target)
let initGrid: Grid =
  let grid =
    [ for x in [0..initWidth-1] do
      for y in [0..initHeight-1] do 
        ({X=x;Y=y}, Walkable) ]
    |> Map.ofList
  grid
    .Add(initStartNode)
    .Add(initWalkableNode)
    .Add(initBlockedNode)
    .Add(initTargetNode)
let initState: State =  {
  Grid = initGrid
  Solutions = []
  BreadthFirstData = Pathfinding.Core.Init.initData
  View = GridView
  Settings = initSettings }
let newClosedNodes = Pathfinding.BreadthFirst.run initState.BreadthFirstData initState
let newData = {initState.BreadthFirstData with ClosedNodes = newClosedNodes}
let state = {initState with BreadthFirstData = newData}