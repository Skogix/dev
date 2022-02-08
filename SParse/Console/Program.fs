open System
open Core.Common
open Core.SParser

printSParse "true"
printSParse "false"
printSParse "null"
printSParse "42"
printSParse "-123"
printSParse "0.5"
printSParse "\"string\""
printSParse "[1,2]"
printSParse "[1,true, \"test\"]"
printSParse "{\"test\": 42}"
printSParse "command \"string\""
printSParse """{
"header":
    {"id": -42.4, "name": "Foo", "flag": true}
}"""
// https://json.org/example.html
printSParse """{
"menu":
    {"id": "file", "value": "File", "popup": {"menuitem": [
    {"value": "New", "onclick": "CreateNewDoc"},
    {"value": "Open", "onclick": "OpenDoc"},
    {"value": "Close", "onclick": "CloseDoc"}
    ]}}
}"""
while true do
    printf "Input: "
    printSParse (Console.ReadLine())