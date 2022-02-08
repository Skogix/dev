namespace SLibrary

open System.Runtime.InteropServices

///
/// generic functions that i want to include in other projects later
///

module RailWay =
    type ResultMonad<'result, 'data> =
        | Result of 'result
        | Data of 'data
    let bind func monad =
        match monad with
        | Result result -> Result result
        | Data data -> func data
    let (>>=) monad func =
        match monad with
        | Result result -> Result result
        | Data data -> func data
    let (=<<) monad string =
        match monad with
        | Result result -> result
        | Data _ -> string
