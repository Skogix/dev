module Clock
type Hour = int
type Minute = int
type STime = int
let MINUTE = 1
let HOUR = 60
let INIT_TIME: STime = 0
let TEST_TIME: STime = 61
let addHours (hours:Hour) (time:STime) = 
    time + (hours*HOUR)
let addMinutes (minutes:Minute) (time:STime) = 
    time + (minutes*MINUTE)
let getHours (time:STime): Hour =
    (time / 60) % 24
let getMinutes (time:STime): Minute =
    time % 60 
let create (hours) (minutes): STime = 
    INIT_TIME
    |> addHours hours
    |> addMinutes minutes
let add (minutes) (clock) = 
    clock
let subtract (minutes) (clock) = 
    clock
let formatDisplay (time:STime): string =
    let add0 i = if i < 10 then $"0{i}" else $"{i}"
    let hours = 
        getHours time 
        |> add0
    let minutes = getMinutes time |> add0
    $"{hours}:{minutes}"
let display (clock) =
    formatDisplay clock
let foo = create -1 15



let clock = create -1 15
display clock 
// Expected: Equals "23:15"
// Actual:   "00:45"