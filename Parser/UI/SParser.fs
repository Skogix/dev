module UI.SParser
open System
open UI.Core
type SValue =
  | SNull
  | SBool of bool
  | SString of string
  | SNumber of float
  | SArray of SValue list
  | SObject of Map<string, SValue>
  | SCommand of string * SValue option

/// infix som kör parser, ignorerar resultatet och returnar value
let (>>%) p x = p |>> fun _ -> x
let sNull =
  parseString "null"
  >>% SNull <?> "null"
let sBool =
  let sTrue =
    parseString "true"
    >>% SBool true
  let sFalse =
    parseString "false"
    >>% SBool false
  sTrue <|> sFalse <?> "bool"
let sStringParser =
  let chars = anyOf (['a'..'z'] @ ['A'..'Z'])
  let string =
    manyChars chars 
  string
  <?> "string parser"
let sQuotedString =
  let quote = parseChar '\"' <?> "quote"
  let chars = anyOf (['a'..'z'] @ ['A'..'Z'])
  quote >>. manyChars chars .>> quote
let sString =
  sQuotedString
  |>> SString
  <?> "quoted string"
let sNumber =
  let optMinus = opt (parseChar '-')
  let zero = parseString "0"
  let digitOneToNine = satisfy (fun ch -> Char.IsDigit ch && ch <> '0') "digits 1-9"
  let digit = satisfy (fun ch -> Char.IsDigit ch) "digit"
  let dot = parseChar '.'
  let optPlusMinus = opt(parseChar '-' <|> parseChar '+')
  let nonZeroNumber =
    digitOneToNine .>>.manyChars digit
    |>> fun (first, rest) -> string first + rest
    
  let numberPart = zero <|> nonZeroNumber
  let fractionPart = dot >>. manyChars1 digit
  
  let convertToSkogixNumber ((optSign, numberPart), fractionPart) =
    let (|>?) opt f =
      match opt with
      | None -> ""
      | Some x -> f x
    let signStr =
      optSign
      |>? string // "-"
    let fractionPartStr =
      fractionPart
      |>?  (fun digits -> "." + digits) // ".123"
    (signStr + numberPart + fractionPartStr)
    |> float
    |> SNumber
  
  optMinus .>>. numberPart .>>. opt fractionPart
  |>> convertToSkogixNumber
  <?> "number"
let createParserForwardedToRef<'a>() =
  let dummyParser =
    let innerFn input : Result<'a * string> = failwith "forward parser not set"
    {parseFn=innerFn;label="forward parser"}
  let parserRef = ref dummyParser
  let innerFn input =
    run !parserRef input
  let wrapperParser = {parseFn=innerFn;label="wrapper parser"}
  wrapperParser, parserRef
let sSharpValue, sValueRef = createParserForwardedToRef<SValue>() 

let skogixArray =
  let left = parseChar '[' .>> spaces
  let right = parseChar ']' .>> spaces
  let comma = parseChar ',' .>> spaces
  let value = sSharpValue .>> spaces
  let values = separatedBy1 value comma
  between left values right
  |>> SArray
  <?> "array"
// sätter potentiella forwarden till number
sValueRef := sNumber
let sObject =
  let left = parseChar '{' .>> spaces
  let right = parseChar '}' .>> spaces
  let colon = parseChar ':' .>> spaces
  let comma = parseChar ',' .>> spaces
  let key = sQuotedString .>> spaces
  let value = sSharpValue .>> spaces
  
  let keyValue = (key .>> colon) .>>. value
  let keyValues = separatedBy1 keyValue comma
  
  between left keyValues right
  |>> Map.ofList
  |>> SObject
let sCommand =
  let command = sStringParser .>> spaces
  let argument = opt (
                       sNull
                       <|> sBool
                       <|> sNumber
                       <|> skogixArray
                       <|> sString
                       <|> sObject
                       
                       .>> spaces
                     )
  let sCommand = command .>>. argument
//  let argument = opt (sValue .>> spaces)
//  let sCommand = command .>>. argument
  sCommand
  |>>  SCommand
sValueRef := choice
  [
    sNull
    sBool
    sNumber
    sObject
    sString
//    sCommand
    skogixArray
  ]
let printSParse (input:string) = printResult (run sSharpValue input)
let getSParse (input:string) = run sSharpValue input
