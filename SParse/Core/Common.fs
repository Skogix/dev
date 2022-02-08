module Core.Common
open System
type ParserLabel = string
type ParserError = string
/// resultatet av en parseing
type Result<'a> =
  | ParseSuccess of 'a
  | ParseFailure of ParserLabel * ParserError
/// wrapper till alla parsers
type Parser<'T> = {
  parseFn: (string -> Result<'T * string>)
  label: ParserLabel
}
let printResult result =
  match result with
  | ParseSuccess (x, inp) -> printfn "%A" x
  | ParseFailure (label, error) -> printfn $"Error: Label %s{label}: Error: %s{error}"
/// parsear en char
let satisfy predicate label =
  let innerFn input =
    if String.IsNullOrEmpty(input) then
      ParseFailure(label, "no more input")
    else
      let first = input.[0]
      if predicate first then
        let rest = input.[1..]
        ParseSuccess (first, rest)
      else
        let error = $"Got %c{first}"
        ParseFailure (label, error)
  {parseFn=innerFn;label=label}
/// kör en parser med input
let parseChar (charToMatch:char) =
  let label = $"%c{charToMatch}"
  let predicate ch = (ch = charToMatch)
  satisfy predicate label  
let run p input =
  // deconstructar parser precis som en (x,y) skulle deconstructa en tuple
  let innerFn = p.parseFn
  innerFn input
  
let getLabel p = p.label
let setLabel p newLabel =
  let newInnerFn input =
    let result = p.parseFn input
    match result with
    | ParseSuccess s -> ParseSuccess s
    | ParseFailure (oldLabel, err) -> ParseFailure (newLabel, err)
  {parseFn=newInnerFn;label=newLabel}
let ( <?> ) = setLabel
 
/// input är diagonalt (a -> parser<b>)
/// output är horisontell (parser<a> -> parser<b>)
/// tar en f som gör en p, en p och kör
/// p och skickar output av p till f
let bindParser f p =
  let label = "binding"
  let innerFn input =
    let result1 = run p input
    match result1 with
    | ParseFailure (label, err) -> ParseFailure (label, err)
    | ParseSuccess (firstIn, restIn) ->
      // kör f för att få en ny parser
      let p2 = f firstIn
      // kör parsern med resten av input
      run p2 restIn
  {parseFn=innerFn;label=label}
/// infix av bindParser
let ( >>= ) p f = bindParser f p
/// transforma en normal value till parser, t.ex a -> parser<a>
let returnParser a =
  let label = $"%A{a}"
  let innerFn b = ParseSuccess (a, b)
  {parseFn=innerFn;label=label}
/// mappar a -> b till parser<a> -> parser<b>
let mapParse f = bindParser (f >> returnParser)
/// infix av mapParse
let ( <!> ) = mapParse
/// infix av mapParse men reversead för pipeing
let ( |>> ) x f = mapParse f x
/// transformar en parser som har en funktion, t.ex parser<a->b> -> parser<a> -> parser<b>
/// "applyar" en wrappad funktion till en wrappad value
let applyParser fP xP =
  fP >>= (fun f ->
  xP >>= (fun x ->
    returnParser (f x)))
let ( <*> ) = applyParser
// lyfter en f(a->b->c) till parsers
let lift2 f aParser bParser =
  returnParser f <*> aParser <*> bParser
let andThen p1 p2 =
  p1 >>= (fun p1Result ->
  p2 >>= (fun p2Result ->
    returnParser (p1Result, p2Result)))
/// infix för andThen
let ( .>>. ) = andThen
let orElse p1 p2 =
  let label = $"%s{getLabel p1} orElse %s{getLabel p2}"
  let innerFn input =
    let result1 = run p1 input
    match result1 with
    | ParseSuccess result -> result1
    | ParseFailure (_,err) ->
      let result2 = run p2 input
      match result2 with
      | ParseSuccess _ -> result2
      | ParseFailure (_,err) -> ParseFailure (label, err)
  {parseFn=innerFn;label=label}
/// infix för orElse
let ( <|> ) = orElse
// väljer en parser från listan
let choice ps =
  List.reduce (<|>) ps
// väljer en char från listan
let anyOf chars =
  let label = $"anyOf %A{chars}"
  chars
  |> List.map parseChar
  |> choice
  <?> label
/// tar en lista med parsers och mappar till en parser av en lista
let rec seqParsers ps =
  // todo: hemmagjort consfunktion, kolla om det går att göra snyggae
  let splitCons first rest = first::rest
  // lyfter parser<'a> till parser<'a list>
  let consParser = lift2 splitCons
  match ps with
  | [] -> returnParser []
  | first::rest -> consParser first (seqParsers rest)
/// parsea något tills fail / kör tills något hittas eller failar
let rec parseZeroOrMore p input =
  let result1 = run p input
  match result1 with
  | ParseFailure _ -> ([], input)
  // (valuen som parseas, resten av input 1)
  | ParseSuccess (x, restIn) ->
    // (resten av alla values från innan, resten av input 2)
    let (xs, restOut) =
      // kör så länge det är success
      parseZeroOrMore p restIn
    // skicka ut nya values när det kommer hit
    let values = x::xs
    // (alla values som hittades, resten efter fail)
    (values, restOut)
/// matchar 0 eller mer av en parser
let many p =
  let label = sprintf "many %s" (getLabel p)
  let rec innerFn input = ParseSuccess (parseZeroOrMore p input)
  {parseFn=innerFn;label=label}
/// matchar minst en av en parser
let many1 p =
  let label = sprintf "many1 %s" (getLabel p)
  p      >>= (fun head ->
  many p >>= (fun tail ->
    returnParser (head::tail)))
  <?> label
/// parsear något optional -> some <|> none
let opt p =
  let label = sprintf "opt %s" (getLabel p)
  let some = p |>> Some
  let none = returnParser None
  some <|> none <?> label
/// behåller vänster
let (.>>) p1 p2 = p1 .>>. p2 |> mapParse (fun (a,_) -> a)
/// behåller höger
let (>>.) p1 p2 = p1 .>>. p2 |> mapParse (fun (_,b) -> b)
/// behåller mitten
let between p1 p2 p3 = p1 >>. p2 .>> p3

/// parsear 1+ av p som separeras av sep
let separatedBy1 p sep =
  let sepThenParse = sep >>. p
  p .>>. many sepThenParse
  |>> fun (p, ps) -> p::ps
// parsear 0+ av p som separeras av sep
let separateBy p sep =
  separatedBy1 p sep <|> returnParser []
let charListToStr chars = String(List.toArray chars)
/// skapar en parser<string> av 0+ parsers 
let manyChars charParsers =
  many charParsers
  |>> charListToStr
/// skapar en parser<string> av 1+ parsers 
let manyChars1 charParsers =
  many1 charParsers
  |>> charListToStr
/// mappar string -> parser
let parseString (str:string) =
  let label = str
  
  str 
  |> List.ofSeq 
  |> List.map parseChar
  |> seqParsers
  |> mapParse charListToStr
  <?> label
/// parsear single whitespace
let parseWhitespace =
  let predicate = Char.IsWhiteSpace
  let label = "whitespace"
  satisfy predicate label
let spaces = many parseWhitespace
let spaces1 = many1 parseWhitespace
/// parsea en digit
let digitChar =
  let predicate = Char.IsDigit
  let label = "digit"
  satisfy predicate label
/// parsea en int
let parseInt =
  let label = "int"
  let resultToInt (sign, digits) =
    // todo int overflow
    let i = digits |> int
    match sign with
    | Some ch -> -i
    | None -> i
  let digits = manyChars1 digitChar
  opt (parseChar '-') .>>. digits
  |> mapParse resultToInt
  <?> label
/// parsea en float
let parseFloat =
  let label = "float"
  let resultToFloat (((sign, digits1), dot), digits2) =
    let float = sprintf "%s.%s" digits1 digits2 |> float
    match sign with
    | Some _ -> -float
    | None -> float
  let digits = manyChars1 digitChar
  opt (parseChar '-') .>>. digits .>>. parseChar '.' .>>. digits
  |> mapParse resultToFloat
  <?> label
  
