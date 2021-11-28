module CarsAssemble

/// off 0 
/// max 10
/// 
/// lowest 1 = 221 cars
/// speed 4 = 221 * 4 = 884
/// 
/// 0 = 0%
/// 1..4 = 100%
/// 5..8 = 90%
/// 9 = 80%
/// 10.. = 77

let perHour = 221.
let successRate (speed: int): float =
    if speed <= 4 then 1.
    elif speed <= 8 then 0.9
    elif speed <= 9 then 0.8
    else 0.77
let productionRatePerHour (speed: int): float =
    let rate = successRate speed
    (perHour * rate) * (speed |> float)
let workingItemsPerMinute (speed: int): int =
    let rate = productionRatePerHour speed |> int
    rate / 60
