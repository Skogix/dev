module Pathfinding.Core.Domain.Grid

type Position = {X:int;Y:int} 
type Terrain =
  | Walkable
  | Blocked
  | Start
  | Target
type Grid = Map<Position, Terrain>
