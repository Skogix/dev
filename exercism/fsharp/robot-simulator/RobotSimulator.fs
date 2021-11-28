module RobotSimulator

type Direction = North | East | South | West
type Position = int * int
type FaceCommand = Left | Right
type Robot = { dir: Direction; pos: Position }
let create dir pos = {dir = dir; pos = pos}
let advance robot = 
  let newPos = 
    let x,y = robot.pos
    match robot.dir with
    | North -> (x,y+1)
    | South -> (x,y-1)
    | East ->  (x+1,y)
    | West ->  (x-1,y)
  {robot with pos = newPos}
let faceLeft (robot:Robot) = 
  let newDir =
    match robot.dir with
    | North -> West
    | West -> South
    | South -> East
    | East -> North
  {robot with dir = newDir}
let faceRight = faceLeft >> faceLeft >> faceLeft
let move (instructions:string) robot = 
  let applyInstruction instruction robot =
    match instruction with
    | 'L' -> robot |> faceLeft
    | 'R' -> robot |> faceRight
    | 'A' -> robot |> advance
    | _ -> robot
  let instructionList = instructions |> List.ofSeq
  let rec loop currentInstruction robot =
    match currentInstruction with
    | first::rest -> loop rest (applyInstruction first robot)
    | [] -> robot
  loop instructionList robot
