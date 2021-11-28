module AnnalynsInfiltration
let canFastAttack (knightIsAwake: bool): bool =  not knightIsAwake
let canSpy(knightIsAwake: bool) (archerIsAwake: bool) (prisonerIsAwake: bool): bool =
  [knightIsAwake;archerIsAwake;prisonerIsAwake] |> List.contains(true)
let canSignalPrisoner(archerIsAwake: bool) (prisonerIsAwake: bool): bool =
  match archerIsAwake, prisonerIsAwake with
  | false, true -> true
  | _ -> false
let canFreePrisoner(knightIsAwake: bool) (archerIsAwake: bool) (prisonerIsAwake: bool) (petDogIsPresent: bool): bool =
  (petDogIsPresent && not archerIsAwake) || (prisonerIsAwake && not knightIsAwake && not archerIsAwake)