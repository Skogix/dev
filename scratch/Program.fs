open System

Console.Clear()

module MonadCombinator =
  let (|Yay|Nay|) = function
    | Choice1Of2 y -> Yay y
    | Choice2Of2 n -> Nay n
  let yay x = Choice1Of2 x
  let nay x = Choice2Of2 x 
  let either yayFunc nayFunc input =
    match input with
    | Yay y -> yayFunc y
    | Nay n -> nayFunc n
  let bind f = either f nay
open MonadCombinator
let carbonate factor label i =
  if i % factor = 0 then
    yay label
  else
    nay i
let connect f = function
  | Yay y -> yay y
  | Nay n -> f n
