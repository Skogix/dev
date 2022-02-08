///
/// the shell/wrapper for the ui
/// is the interpreter between the application and the framework
/// also handles everything startup-related like timers and avalonia settings
///Statw 
module UI.Shell
open Elmish
open Avalonia.FuncUI
open Avalonia.FuncUI.Components.Hosts
open Avalonia.FuncUI.Elmish
type MainWindow() as this =
    inherit HostWindow()
    do
        base.Title <- "Pathfinding"
        base.Width <- 800.0
        base.Height <- 600.0
        base.MinWidth <- 800.0
        base.MinHeight <- 600.0

//        this.VisualRoot.VisualRoot.Renderer.DrawFps <- true
        this.VisualRoot.VisualRoot.Renderer.DrawDirtyRects <- true

        Elmish.Program.mkProgram (fun () -> Init.initState) Update.update Views.Base.view
        |> Program.withHost this
//        |> Program.withConsoleTrace
        // the place for eventual startup-functions like timers or eventlisteners
        |> Program.withSubscription (Update.timer Update.update) 
        |> Program.run