module UI.Views.GridView

open Avalonia.Controls.Primitives
open Pathfinding.Core.Domain.BreadthFirst
open Pathfinding.Core.Domain.Grid
open Pathfinding.Core.State
open Pathfinding.Core.Common
open Route.Domain
open Avalonia.Controls
open Avalonia.Layout
open Pathfinding.Core.Domain.Settings
open Avalonia.FuncUI.DSL
open Avalonia.Controls.Shapes
open SLibrary.RailWay

module Background = 
    module Colors =
        let start = "purple"
        let target = "magenta"
        let openNode = "blue"
        let solution = "orange"
        let closedNode = "gray"
        let blocked = "black"
        let walkable = "green"
        let other = "pink"
    /// sets the background for a uniformgrid node
    let background pos state =
        let isInsideGrid pos = if state.Grid.ContainsKey(pos) then Data pos else failwith "position is outside of the grid"
        let startOrTarget pos =
            match state.GetStart = pos, state.GetTarget = pos with
            | true, _ -> Result Colors.start
            | _, true -> Result Colors.target
            | _, _ -> Data pos
        let solution pos =
            match state.SolutionsContainsPos pos with
            | true -> Result Colors.solution
            | false -> Data pos
        let openClosedNodes pos =
            let isClosed = state.ClosedNodesPositionList |> List.contains (pos)
            let isOpen = state.OpenNodesPositionList |> List.contains (pos)
            match isOpen, isClosed with
            | true, _ -> Result Colors.openNode
            | _, true -> Result Colors.closedNode
            | _, _ -> Data pos
        let restOfTerrain pos =
            match state.Grid.TryFind(pos) with
            | Some terrain ->
                match terrain with
                | Blocked -> Result Colors.blocked
                | Walkable -> Result Colors.walkable
                | _ -> Data pos
            | None -> Data pos
        (Data pos)
        >>= isInsideGrid
        >>= startOrTarget
        >>= solution
        >>= openClosedNodes
        >>= restOfTerrain
        =<< Colors.other
let arrow pos state =
    let getArrow parent =
        match (parent.X - pos.X), (parent.Y - pos.Y) with
        |  1,  0 -> Some "→"
        |  0,  1 -> Some "↓"
        | -1,  0 -> Some "←"
        |  0, -1 -> Some "↑"
        |  1,  1 -> Some "↘"
        | -1, -1 -> Some "↖" 
        |  1, -1 -> Some "↗"
        | -1,  1 -> Some "↙"
        |  _,  _ -> None
    match state.Settings.Arrow with
    | false -> ""
    | true ->
        match state.BreadthFirstData.ClosedNodes |> List.tryFind (fun x -> x.Position = pos) with
        | Some node ->
            node.Parent
            |> Option.bind getArrow
            |> Option.defaultValue " "
        | None -> ""
let position pos state = if state.Settings.Position then $"Pos: {pos.X}, {pos.Y}" else ""
let cost pos state =
    match state.Settings.Cost with
    | false -> ""
    | true ->
        match state.BreadthFirstData.ClosedNodes |> List.tryFind (fun x -> x.Position = pos) with
        | Some node -> $"Cost: {node.Cost}"
        | None -> ""
/// creates the grid
let view (state: Output) dispatch =
    let settings = state.Settings
    UniformGrid.create [ 
        UniformGrid.rows settings.Height
        UniformGrid.columns settings.Width
        UniformGrid.dock Dock.Top
        UniformGrid.children [ 
            for y in [ 0 .. settings.Height - 1 ] do
            for x in [ 0 .. settings.Width - 1 ] do
                let pos = createPosition x y
                StackPanel.create [ 
                    StackPanel.background (Background.background pos state)
                    StackPanel.margin 2.
                    StackPanel.onTapped (fun _ -> dispatch (Input.ToggleTerrain pos))
                    StackPanel.children [ 
                        TextBlock.create [
                            TextBlock.text $"{position pos state}"
                            TextBlock.horizontalAlignment HorizontalAlignment.Left ]
                        TextBlock.create [ 
                            TextBlock.text $"{cost pos state}"
                            TextBlock.horizontalAlignment HorizontalAlignment.Left ]
                        TextBlock.create [ 
                            TextBlock.fontFamily "DejaVu Sans Mono"
                            TextBlock.text $"{arrow pos state}"
                            TextBlock.fontSize 40.
                            TextBlock.horizontalAlignment HorizontalAlignment.Center
                            TextBlock.verticalAlignment VerticalAlignment.Center ] 
                        ] 
                    ] 
            ] 
        ]