module UI.Update
open System
open Avalonia.Threading
open Pathfinding.Core.Domain
open Route.Domain
open Elmish

/// the real "fake" viewmodel
/// input: a command that the user wants to call and the current state
/// output: the new state and a potential "late response" via a function or a command
let update (command:Input) (outputState:Output) =
  let state, commandOption = Route.Update.update command outputState
  match commandOption with
  | Some command -> state, Cmd.ofMsg command
  | None -> state, Cmd.none
/// simple timer that sends a command
let timer _ _ =
  let sub dispatch =
    let invoke () =
      if Settings.debugState then dispatch RunBreadthFirst
      true
    DispatcherTimer.Run (Func<bool> invoke, TimeSpan.FromMilliseconds 50.0)
    |> ignore
  Cmd.ofSub sub