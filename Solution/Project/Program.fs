open ClassLibrary1
open System
Console.Clear()
Say.hello "Test"
type Test =
  | A
  | B
  | C
  | D
let huhu x = 
  match x with
  | A -> printfn "Test A"
  | B -> printfn "Test A"
  | C -> printfn "Test A"
  | D -> printfn "Test A"
huhu B
let a = 0
a
