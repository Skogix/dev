module ArmstrongNumbers
open System

// let getNumbersList (i: int) = 
//   // dat fulhack #1 
//   i
//   |> string 
//   |> List.ofSeq 
//   |> List.map(fun i -> i |> string |> Int32.Parse)
//   // dat fulhack #2
//   // i
//   // |> string
//   // |> List.ofSeq
//   // |> List.map(fun i -> ((i |> int) - ('0' |> int)))
// let isArmstrongNumber (number: int) = 
//   let numbers = getNumbersList number
//   let raise (i:int) = Math.Pow(i, numbers.Length)
//   numbers
//   |> List.map raise
//   |> List.sum
//   |> fun i -> i = number
let isArmstrongNumber (number: int) =
  let numbers = number |> string |> Seq.map(string >> int) 

  numbers
  |> Seq.sumBy(fun i -> Math.Pow(i, Seq.length(numbers)))
  |> (=) number