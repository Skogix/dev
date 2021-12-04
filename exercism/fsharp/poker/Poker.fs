module Poker



type Suit = Clubs | Spades | Hearts | Diamonds
type Category =
    | HighCard
    | OnePair
    | TwoPair
    | ThreeOfAKind
    | SmallStraight
    | Straight
    | Flush
    | FullHouse
    | FourOfAKind
    | SmallStraightFlush
    | StraightFlush
    | Yahtzee
    
type Value =
    | Two = 2
    | Three = 3
    | Four = 4
    | Five = 5
    | Six = 6
    | Seven = 7
    | Eight = 8
    | Nine = 9
    | Ten = 10
    | Jack = 11
    | Queen = 12
    | King = 13
    | Ace = 14
type SuitType = Suited | Offsuited
type Card = {value:Value;suit:Suit}
type Hand = Hand of SuitType * (Value * int) list
    
let parseCard inputCard =
    let parseSuit = function
        | 'H' -> Hearts
        | 'D' -> Diamonds
        | 'C' -> Clubs
        | 'S' -> Spades
        | _ -> failwith "Invalid suit"
    let(|Int|_|)(value:string) =
        match System.Int32.TryParse value with
        | true, v -> Some v
        | _ -> None
    let parseValue = function
        | Int x when x >= 2 && x <= 10 -> enum x
        | "J" -> Value.Jack
        | "Q" -> Value.Queen
        | "K" -> Value.King
        | "A" -> Value.Ace
        | _ -> failwith "Invalid value"
        
    let last = String.length inputCard - 1
    let valuePart, suitPart = inputCard.[last - 1].ToString(), inputCard.[last]
    {value = parseValue valuePart; suit = parseSuit suitPart }
let parseCards(str:string) =
    [ for item in str.Split ' ' -> parseCard item ]
let hand cards =
    let values =
        cards
        |>List.countBy(fun c -> c.value)
        |>List.sortByDescending(fun(r, c) -> (c, r))
    let suitType =
        cards
        |>List.countBy(fun c -> c.suit)
        |>function[_, 5] -> Suited | _ -> Offsuited
    Hand(suitType, values)
let (|Kind|) = List.map snd
let (|Rank|) = List.map fst
let (|Small|_|) = function
    | [Value.Ace;Value.Five;Value.Four;Value.Three;Value.Two] -> Some()
    | _ -> None
let (|Sequential|_|) value =
    let next rank = (rank - enum 1) % enum 13 + enum 2
    let eq (r1, r2) = r1 = next r2
    let isSequential = List.pairwise >> List.forall eq
    if value |> isSequential then Some() else None
let category(Hand(suitType, value)) =
    match suitType, value with
    | Suited,     Rank Sequential    -> StraightFlush
    | Suited,     Rank Small         -> SmallStraightFlush
    | Offsuited,  Kind [4;1]         -> FourOfAKind
    | Offsuited,  Kind [3;2]         -> ThreeOfAKind
    | Suited,     Rank _             -> Flush
    | Offsuited,  Rank Sequential    -> Straight
    | Offsuited,  Rank Small         -> SmallStraight
    | Offsuited,  Kind [3;2;1]       -> ThreeOfAKind
    | Offsuited,  Kind [2;2;1]       -> TwoPair
    | Offsuited,  Kind [2;1;1;1]     -> OnePair
    | Offsuited,  Rank _             -> HighCard
    
    
    
(*
let bestHands hands =
    let bestBy f = List.groupBy f >> List.max >> snd
    
    hands
    |>List.groupBy(parseCards >> hand)
    |>bestBy(fst >> category)
    |>bestBy fst
    |>List.collect snd
*)
let add x y = 
    let x = 0
    x + y
let bestHands hands =
    let bestBy f = List.groupBy f >> List.max >> snd

    hands
    |> List.groupBy (parseCards >> hand)
    |> bestBy (fst >> category)
    |> bestBy fst
    |> List.collect snd
