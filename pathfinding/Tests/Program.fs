namespace Tests

open Expecto

///
/// run the tests, a normal console application
/// https://github.com/haf/expecto
/// 
module Main =
  [<EntryPoint>]
  let main argv =
    runTestsInAssembly defaultConfig argv |> ignore
    0