namespace UI

open Avalonia
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.FuncUI
///
/// todo; learn about avalonia: https://docs.avaloniaui.net/
/// 
type App() =
    inherit Application()

    override this.Initialize() =
        this.Styles.Load "avares://Avalonia.Themes.Default/DefaultTheme.xaml"
        this.Styles.Load "avares://Avalonia.Themes.Default/Accents/BaseDark.xaml"
        this.Styles.Load "avares://UI/Styles.xaml"

    override this.OnFrameworkInitializationCompleted() =
        match this.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime as desktopLifetime ->
            desktopLifetime.MainWindow <- Shell.MainWindow()
        | _ -> ()

module Program =
    [<EntryPoint>]
    let main (args: string []) =
        AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .UseSkia()
            .StartWithClassicDesktopLifetime(args)