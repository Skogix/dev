module InterestIsInteresting

let interestRate (balance: decimal): single =
    match balance with
    | x when x < 0.0m -> 3.213f
    | x when x < 1000.0m -> 0.5f
    | x when x < 5000.0m-> 1.621f
    | _ -> 2.475f

let interest (balance: decimal): decimal =
   let mult = decimal (interestRate balance) / 100.0m
   balance * mult

let annualBalanceUpdate(balance: decimal): decimal =
   balance + interest balance

let amountToDonate(balance: decimal) (taxFreePercentage: float): int =
   if balance > 0.0m then
      (balance * ((taxFreePercentage / 100.0 * 2.0) |> decimal))
      |> int
   else 0
