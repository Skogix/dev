module Pathfinding.Core.Domain.Settings
let mutable debugState = true
type GridSizeChange = Increment | Decrement
type ChangeSettingsCommand =
  | Diagonal
  | Position
  | Cost
  | Arrow
  | RunTimer
  | Width of GridSizeChange
  | Height of GridSizeChange
type Settings = {
  Diagonal: bool
  Cost: bool
  Arrow: bool
  Position: bool
  Width: int
  Height: int }
let updateSettings state command =
  let gridSizeChange (command:GridSizeChange) int =
    match command with
    | Increment -> int + 1
    | Decrement -> int - 1
  match command with
  | Diagonal -> {state with Diagonal = not state.Diagonal}
  | Cost -> {state with Cost = not state.Cost}
  | Arrow -> {state with Arrow = not state.Arrow}
  | Position -> {state with Position = not state.Position}
  | Width cmd -> {state with Width = (gridSizeChange cmd state.Width) }
  | Height cmd -> {state with Height = (gridSizeChange cmd state.Height) }
  | _ -> state