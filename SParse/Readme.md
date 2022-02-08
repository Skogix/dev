Hemmagjord parser för att serialisera simpla text-commands.
##### Exempel
```f# script
type SValue =
  | SNull
  | SBool of bool
  | SString of string
  | SNumber of float
  | SArray of SValue list
  | SObject of Map<string, SValue>
  | SCommand of string * SValue option
  
true    -> Sbool true
false   -> Sbool false
null    -> SNull
42      -> SNumber 42.0
0.5     -> SNumber 0.5
"foo"   -> SString "foo"
[1, "str", true] -> 
    SArray [SNumber 1.0; SString "str"; SBool true]
{"objectString": "str"} -> 
    SObject (map [("objectString", SString "str")])
    
// https://json.org/example.html
{"menu":
    {"id": "file", "value": "File", "popup": {"menuitem": [
        {"value": "New", "onclick": "CreateNewDoc()"},
        {"value": "Open", "onclick": "OpenDoc()"},
        {"value": "Close", "onclick": "CloseDoc()"}
    ]}}
} -> 
SObject
  (map
     [("menu",
       SObject
         (map
            [("id", SString "file");
             ("popup",
              SObject
                (map
                   [("menuitem",
                     SCommand
                       ("",
                        Some
                          (SArray
                             [SObject
                                (map
                                   [("onclick", SString "CreateNewDoc()");
                                    ("value", SString "New")]);
                              SObject
                                (map
                                   [("onclick", SString "OpenDoc()");
                                    ("value", SString "Open")]);
                              SObject
                                (map
                                   [("onclick", SString "CloseDoc()");
                                    ("value", SString "Close")])])))]));
             ("value", SString "File")]))])
// commands är allt som inte matchar någon datatyp och en optional value
bar -> SCommand ("bar", None)
foo "string" -> 
    SCommand ("foo", Some (SString "string"))
```

##### Cheat Sheet
__parseChar__ `char -> Parser<char>`<br />
grundfunktionen som skapar en parser från en char

__run__ `Parser<'a> -> string -> Result<'a * string>`<br />
kör inre curryade funktionen hos en parser

__.>>.__ `Parser<'a> -> Parser<'b> -> Parser<'a * 'b>`<br />
__andThen__ : and-combinator

__<|>__ `Parser<'a> -> Parser<'a> -> Parser<'a>`<br />
__orElse__ : or-combinator

__<!>__ `('a -> 'b) -> Parser<'a> -> Parser<'b>`<br />
__mapParse__ : `<kör en funktion (a->b) som transformar parser<a> -> parser`

__|>>__ `Parser<'a> -> ('a -> 'b) -> Parser<'b>`<br />
__mapParse__ : reversed för enklare pipeing

__chooseOne__ `Parser<a'> list -> Parser<'a>`<br />
returnar första success som hittas

__anyOf__ `char list -> Parser<char>`<br />
__parseChar__ >> __chooseOne__

__returnParser__ `a -> Parser<'a>`<br />
höjer en value till en parser

__<*>__ `Parser<('a -> 'b)> -> Parser<'a> -> Parser<'b>`<br />
__applyParser__ : kör en (a->b)-parser på en value 

```f# script
let parseDigit = anyOf ['0'..'9']
let parseThreeDigitsAsStr =
    (parseDigit .>>. parseDigit .>>. parseDigit)
    |>> fun ((c1, c2), c3) -> String [|c1;c2;c3|]
let parseThreeDigitsAsInt = mapParse int parseThreeDigitsAsStr
// ('a -> 'b -> 'c) -> Parser<'a> -> Parser<'b> -> Parser<'c>
let lift2 f aValue bValue = returnParser f <*> aValue <*> bValue
// ('a -> 'b -> 'c -> 'd) -> Parser<'a> -> Parser<'b> -> Parser<'c> -> Parser<'d>
let lift3 f aValue bValue cValue = returnParser f <*> aValue <*> bValue <*> cValue
// Parser<int> -> Parser<int> -> Parser<int>
let addParser = lift2 (+)
// string -> char -> bool
let startWith (str:string) (prefix:char) = str.StartsWith(prefix)
// Parser<string> -> Parser<char> -> Parser<bool>
let startsWithParser = lift2 startWith
// Parser<char>
let whitespaceChar = anyOf [' '; '\t'; '\n']
// Parser<char list>
let whitespace = many whitespaceChar
```
