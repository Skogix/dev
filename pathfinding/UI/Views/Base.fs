module UI.Views.Base

open Avalonia.Controls
open Pathfinding.Core.State
open Pathfinding.Core
open Pathfinding.Core.Domain
open Route.Domain
open Avalonia.FuncUI.DSL

let buttons dispatch (state:State) = 
  [
  "Run once", (fun _ -> dispatch (RunBreadthFirstOnce)), (state.View = GridView && not Settings.debugState)
  "Solve", (fun _ -> dispatch (RunBreadthFirst)), (state.View = GridView && not Settings.debugState)

  "Reset", (fun _ -> dispatch (Reset)), (state.View = GridView)
  "Random Terrain", (fun _ -> dispatch (RandomTerrain)), (state.View = GridView)

  "GridView", (fun _ -> dispatch (ChangeView GridView)), (state.View = SettingsView)
  "SettingsView", (fun _ -> dispatch (ChangeView SettingsView)), (state.View = GridView)

  "Turn off debug/autorun", (fun _ -> dispatch (ToggleRunTimer)), (Settings.debugState)
  "Turn on debug/autorun", (fun _ -> dispatch (ToggleRunTimer)), (not Settings.debugState)
]
/// creates the buttons at the bottom
let createButtons state dispatch =
  StackPanel.create [
    StackPanel.dock Dock.Bottom
    StackPanel.children [
      for (text, dispatch, visible) in (buttons dispatch state) do
        Button.create [
          Button.dock Dock.Bottom
          Button.height 50.
          Button.isVisible visible
          Button.content text
          Button.onClick dispatch
        ]
    ]
  ]

///
/// the main view controller
/// input: the current state and a back-channel/dispatch to respond/send commands
/// output: avalonia iview
///
let view (state:Output) dispatch =
  DockPanel.create [
    DockPanel.children [
      createButtons state dispatch
      match state.View with
      | SettingsView -> SettingsView.view state dispatch
      | GridView -> GridView.view state dispatch
    ]
  ]