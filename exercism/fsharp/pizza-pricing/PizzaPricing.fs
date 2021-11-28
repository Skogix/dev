module PizzaPricing

// TODO: please define the 'Pizza' discriminated union type
type Pizza =
    | Margherita
    | Caprese
    | Formaggio
    | ExtraSauce of Pizza
    | ExtraToppings of Pizza
let rec pizzaPrice (pizza: Pizza): int =
    match pizza with
    | Margherita -> 7
    | Caprese -> 9
    | Formaggio -> 10
    | ExtraSauce p -> pizzaPrice p + 1
    | ExtraToppings p -> pizzaPrice p + 2
let orderPrice(pizzas: Pizza list): int = 
    let price = pizzas |> List.sumBy(fun pizza -> pizzaPrice pizza)
    let orderAdd = 
        match pizzas |> List.length with
        | 1 -> 3
        | 2 -> 2
        | _ -> 0
    price + orderAdd
