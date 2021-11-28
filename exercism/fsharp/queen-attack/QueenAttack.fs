module QueenAttack
let between min max value = value >= min && value <= max  
let create(x:int,y:int) = 
  let isOnBoard = between 0 7
  isOnBoard x && isOnBoard y
let canAttack posA posB = 
  let ax, ay = posA
  let bx, by = posB
  let canAttackStraight = ax=bx || ay=by
  // true  2,4 | 5,7 
  //       2-5=3
  //       4-7=3
  // false 2,4 | 5,8
  //       2-5=3
  //       4-8=4
  let canAttackDiagonal = 
    let firstDiagonal = (ax - bx) |> abs 
    let secondDiagonal = (ay - by) |> abs 
    firstDiagonal = secondDiagonal
  canAttackStraight || canAttackDiagonal
