module Grains

let board: uint64 array =
  let arr = Array.create 64 (0UL)
  arr[0] <- 1UL
  for i in [1..63] do
    arr[i] <- arr[i-1] * 2UL
  arr
let square (n: int): Result<uint64,string> = 
  match n with
  | n when n <= 0 || n > 64-> Error("square must be between 1 and 64")
  | _ -> Ok(board.[n-1])
let total: Result<uint64,string> = 
  Ok(board |> Array.sum)