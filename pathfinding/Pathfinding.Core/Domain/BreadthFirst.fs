module Pathfinding.Core.Domain.BreadthFirst

open Pathfinding.Core.Domain.Grid

type Node = {
  Position: Position
  Parent: Position option
  Cost: int }
let createNode parent cost pos = {
  Position = pos
  Parent = parent
  Cost = cost }
type OpenNodes = Node list
type ClosedNodes = Node list
type Data = {
  ClosedNodes: ClosedNodes
  OpenNodes: OpenNodes }
