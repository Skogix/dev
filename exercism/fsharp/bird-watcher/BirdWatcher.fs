module BirdWatcher

let lastWeek: int[] = [|0;2;5;3;7;8;4|]
let yesterday(counts: int[]): int = counts[counts.Length-2]
let total(counts: int[]): int = counts |> Array.sum
let dayWithoutBirds(counts: int[]): bool = counts |> Array.contains(0)
let incrementTodaysCount(counts: int[]): int[] =
  counts.[counts.Length-1] <- counts.[counts.Length-1] + 1
  counts
let oddWeek (counts: int[]): bool =
  let odds = [for x in [0..+2..counts.Length] do counts.[x]]
  let evens = [for x in [1..+2..counts.Length-1] do counts.[x]]
  let containsXBirds birds list = list |> List.forall(fun x -> x = birds)
  containsXBirds 0 evens ||
  containsXBirds 10 evens ||
  containsXBirds 5 odds