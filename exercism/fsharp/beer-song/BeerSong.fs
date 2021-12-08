module BeerSong
let bottlesOfBeer n = $"{n} bottles of beer on the wall, {n} bottles of beer."
let takeOnDown = function
    | x when x > 1 -> 
        $"Take one down and pass it around, {x-1} bottles of beer on the wall."
    | x when x = 1 -> 
        $"Take one down and pass it around, no more bottles of beer on the wall"
    | _ -> ""
type Test =
    | A of int
    | B
    | C
let test x =
    match x with
    | A n -> ()
let recite (startBottles: int) (takeDown: int): string list = 
    []
