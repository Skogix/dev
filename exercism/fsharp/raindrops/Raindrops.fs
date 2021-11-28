module Raindrops

let sounds = [
  3,"Pling"
  5,"Plang"
  7,"Plong" ]
let convert n =
  let potentialSounds =
    sounds
    |> List.filter(fun (number, sound) -> n % number = 0)
    |> List.fold(fun startState (number, sound) -> startState + sound) ""
  match potentialSounds with
  | "" -> n |> string
  | _ -> potentialSounds

