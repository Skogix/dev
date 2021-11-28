module Accumulate


let accumulate (func: 'a -> 'b) (input: 'a list): 'b list =
  let rec loop a b =
    match a with
    | first::rest -> loop rest (func first::b)
    | [] -> b
  loop input [] |> List.rev
