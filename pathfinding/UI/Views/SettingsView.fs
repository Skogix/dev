module UI.Views.SettingsView

open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.Layout
open Pathfinding.Core.Domain.Settings
open Pathfinding.Core.State
open Route.Domain

let toggleButtons _ dispatch = [
  "Toggle Diagonal", (fun _ -> dispatch (ChangeSetting Diagonal))
  "Toggle Arrow",    (fun _ -> dispatch (ChangeSetting Arrow))
  "Toggle Cost",     (fun _ -> dispatch (ChangeSetting Cost))
  "Height +", (fun _ -> dispatch (ChangeSetting (Height Increment)))
  "Height -", (fun _ -> dispatch (ChangeSetting (Height Decrement)))
  "Width +", (fun _ -> dispatch (ChangeSetting (Width Increment)))
  "Width -", (fun _ -> dispatch (ChangeSetting (Width Decrement))) ]
let view (state:State) dispatch =
  StackPanel.create [
    StackPanel.children [
      for text, dispatch in toggleButtons state dispatch do
        Button.create [
          Button.content text
          Button.height 40.
          Button.onClick dispatch ]
      TextBlock.create [
        let sprintBool bool = if bool then "On" else "Off"
        TextBlock.text $"
        Diagonal: {sprintBool state.Settings.Diagonal}
        Arrows:   {sprintBool state.Settings.Arrow}
        Position: {sprintBool state.Settings.Position}
        Cost:     {sprintBool state.Settings.Cost}
        Width:    {state.Settings.Width}
        Height:   {state.Settings.Height}
        " ]
    ]
  ]