open System

Console.Clear()

type Data = {i:int; label:string option}
type Rule = {label:string;condition:int -> bool}
let fizz = 
  {label="Fizz";condition=fun x -> x%3=0}
let carbonate rule data =
  let {i=i; label=labelSoFar} = data
  if rule.condition i then
    let newLabel =
      match labelSoFar with
      | Some s -> s + rule.condition
      | None -> rule.condition
    {data with label=Some newLabel}
  else
    data
let labelOrDefault data =
  let {i=i; label=labelSoFar} = data
  match labelSoFar with
  | Some s -> s
  | None -> sprintf "%i" i
let fizzbuzz i =
  {i=i; label=None}
  |> carbonate fizz
  |> labelOrDefault
[1..100]
|> List.map fizzbuzz
|> List.iter (printfn "%s")
