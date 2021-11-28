module Leap


let leapYear(year:int):bool =
  match year%4,year%100,year%400 with
  | 0,0,0 -> true
  | 0,0,_ -> false
  | 0,_,_ -> true
  | _,_,_ -> false
