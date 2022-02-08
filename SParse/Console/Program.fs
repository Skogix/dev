open System
open Core.Common
open Core.SParser

Console.Clear()
printSParse "true"
printSParse "false"
printSParse "null"
printSParse "42"
printSParse "0.5"
printSParse "command \"string\""
printSParse "foo"
printSParse "\"foo\""
printSParse "{\"objectString\": \"str\"}"
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
