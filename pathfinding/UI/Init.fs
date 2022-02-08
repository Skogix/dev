module UI.Init

open Elmish

///
/// initial state for the UI
/// 
let initState =
  Route.Domain.initOutput, Cmd.none