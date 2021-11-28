module TisburyTreasureHunt

let getCoordinate (line: string * string): string = line |> snd

let convertCoordinate (coordinate: string): int * char = 
    let chars = coordinate |> List.ofSeq
    let a = (chars.[0] |> int) - ('0' |> int)
    let b = chars.[1]
    (a,b)
let compareRecords (azarasData: string * string) (ruisData: string * (int * char) * string) = 
    let aCord = azarasData |> snd |> convertCoordinate
    let _, bCord, _ = ruisData
    aCord = bCord
let createRecord azara rui = 
    match compareRecords azara rui with
    | true -> 
        let d,a = azara 
        let b,_,c = rui
        (a,b,c,d)
    | false -> 
        ("","","","")
