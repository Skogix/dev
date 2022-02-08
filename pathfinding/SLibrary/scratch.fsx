#load "SLibrary.fs"
/// applicative vs monadic
/// applicative;
///   apply, lift, combine
/// monadic;
///   bind
///
/// do something in paralell and independent data; applicative
/// one at a time, skip the next if this fails; monadic
/// if dependendencies then you need to be monadic
///
/// static vs dynamic
/// if applicative then we can define all actions up front / statically
/// if monadic then we have more flexibility but cant do things like paralellelization or optimize

/// basic validation example
type CustomerId = CustomerId of int
type EmailAddress = EmailAddress of string
type CustomerInfo = { id: CustomerId; email: EmailAddress }

type Result<'a> =
  | Success of 'a
  | Failure of string list

let createCustomerId id =
  if id > 0 then
    Success (CustomerId id)
  else
    Failure [ "CustomerId must be positive" ]

let createEmailAddress str =
  if System.String.IsNullOrEmpty (str) then
    Failure [ "Email cant be empty" ]
  elif str.Contains ("@") then
    Success (EmailAddress str)
  else
    Failure [ "Email must contain @" ]

module Result =
  // map    (a -> b) -> result<a> -> result<b>
  // ret    a -> result<a>
  // apply  result<(a->b)> -> result<a> -> result<b>
  // bind   (a -> result<b>) -> result<a> -> result<b>

  let map f x =
    match x with
    | Success x -> Success (f x)
    | Failure errs -> Failure errs

  let ret x = Success x

  let apply f x =
    match f, x with
    | Success f, Success x -> Success (f x)
    | Failure errs, Success x -> Failure errs
    | Success x, Failure errs -> Failure errs
    | Failure errs1, Failure errs2 -> Failure (List.concat [ errs1; errs2 ])

  let bind f x =
    match x with
    | Success x -> f x
    | Failure errs -> Failure errs
let createCustomer customerId email = { id = customerId; email = email }
let (<!>) = Result.map
let (<*>) = Result.apply
let createCustomerResultApplicative id email =
  let idResult = createCustomerId id
  let emailResult = createEmailAddress email
  /// createCustomer; normal to result
  /// map;
  /// idResult apply createCustomer
  /// emailResult apply createEmailAddress
  let example2 =
    createCustomer <!> idResult <*> emailResult
  example2
//      let example1 =
//        createCustomer
//        |> Result.map
//        <| idResult
//        |> Result.apply
//        <| emailResult
//      example1

//    let f x y =
//      let xResult = createCustomerId x
//      let yResult = createEmailAddress y
//      let f =
//        Result.map createCustomer
//        <| xResult
//        |> Result.apply
//        <| yResult
//      f
//      // test with return -> apply
//      // int -> string -> result<custinfo>
//      let f1 =
//        // custid -> email -> custinfo
//        createCustomer
//        // result<(custid -> email -> custinfo)>
//        |> Result.ret
//        // result<custid> -> result<email -> custinfo>
//        |> Result.apply
//      let x1 = f1 idResult
//      let f2 =
//        x1
//        |> Result.apply
//      let x2 = f2 emailResult
//
//      // test with map
//      let test1 =
//        createCustomer
//        |> Result.map
//      let result1 = test1 idResult
//      let test2 =
//        result1
//        |> Result.apply
//      test2 emailResult
//      createCustomer <!> idResult <*> emailResult
//      let a = createCustomer |> Result.map
//      let b = a xResult |> Result.apply
//      let c = b yResult
//      let test = xResult |> Result.map createCustomer
//      let test2 = yResult |> Result.apply test

let okId = 1
let badId = 0
let okEmail = "test@test.com"
let badEmail = "test.com"

let goodCustomer1 =
  createCustomerResultApplicative okId okEmail

let badCustomer1 =
  createCustomerResultApplicative badId badEmail

let badCustomer2 =
  createCustomerResultApplicative okId badEmail


let (>>=) x f = Result.bind f x

let createCustomerResultMonadic (id: int) (email: string) =
  /// convert int into id
  /// if successful, convert string into email
  /// if successful, create customerinfo
//
//  let output =
//    createCustomerId id >>= (fun xId ->
//      createEmailAddress email >>= (fun xEmail ->
//        let customer = createCustomer xId xEmail
//        Success customer
//        )
//      )
//  output
  let a =
    createCustomerId id
    >>= (fun xId ->
      createEmailAddress email
      >>= fun xEmail -> Success (createCustomer xId xEmail))

  a

let ok1 = createCustomerResultMonadic okId okEmail

let bad1 =
  createCustomerResultMonadic badId badEmail

let bad2 =
  createCustomerResultMonadic okId badEmail

/// monadic vs applicative
/// applicative
///   all validations up front and combine results
///   pro; didnt lose any validation errors
///   con; may do work that we dont need (still validate second when first fail)
/// monadic
///   chained together
///   pro; short circuit when we get an error
///   con; only gets the first error message




/// computation expressions
/// https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/computation-expressions
/// let! M<'T> * ('T -> M<'U>) -> M<'U>
/// do! unit -> M<'T>
/// yield 'T -> M<'T>
/// yield! M<'T> -> M<'T>
/// return 'T -> M<'T>
/// return! M<'T> -> M<'T>
/// match! (let! with pattern match)
///
/// let! binds the result of a call to a name
/// let binds the value of an unrealized call
///   bind; bind(x ,f)
///
/// do! call a computation expression that return unit
///   zero; bind(x, f<*->unit>)
///
/// yield returns a whole value to ienumerable
///   yield(type)
///   most often used with ->
///
/// yield! returns a flattened collection one by one
///   yieldfrom(type)
///
/// return wraps a value to the expressions type
///   return(type)
///   let! x = ...
///   async return -> x
///   // async<x>
///
/// return! realizes the value of the expression
///   returnfrom(type)
///   return! x
///   // async<x>
///
/// match! matches a realized expression
///   match! asyncTryGet x with
///
type ResultBuilder () =
  member this.Return x = Result.ret x
  member this.Bind (x, f) = Result.bind f x

let result = new ResultBuilder ()

let createCustomerResultComputationExpression id email =
  result {
    let! customerId = createCustomerId id
    let! customerEmail = createEmailAddress email
    let customer = createCustomer customerId customerEmail
    return customer
  }



let log p = printfn "expression is %A" p

let loggedWorkFlow =
  let x = 42
  log x
  let y = 43
  log y
  let z = x + y
  log z
  z

type LoggingBuilder () =
  let log p = printfn "expression is %A" p

  member this.Bind (x, f) =
    log x
    f x

  member this.Return (x) = x

let logger = LoggingBuilder ()

let loggedWorkFlowCE =
  logger {
    let! x = 42
    let! y = 43
    let! z = x + y
    return z
  }

let divideBy bottom top =
  if bottom = 0 then
    None
  else
    Some (top / bottom)

let divideByTriangleOfDoom init x y z =
  let a = init |> divideBy x

  match a with
  | None -> None
  | Some a' ->
    let b = a' |> divideBy y

    match b with
    | None -> None
    | Some b' ->
      let c = b' |> divideBy z

      match c with
      | None -> None
      | Some c' -> Some c'

let good = divideByTriangleOfDoom 12 3 2 1
let bad = divideByTriangleOfDoom 12 3 0 1

type MaybeBuilder () =
  member this.Bind (x, f) =
    match x with
    | None -> None
    | Some a -> f a

  member this.Return (x) = Some x

let maybe = new MaybeBuilder ()

let divideByCE init x y z =
  maybe {
    let! a = init |> divideBy x
    let! b = a |> divideBy y
    let! c = b |> divideBy z
    return c
  }

// or else
type OrElseBuilder () =
  member this.ReturnFrom (x) = x

  member this.Combine (a, b) =
    match a with
    | Some _ -> a
    | None -> b

  member this.Delay (f) = f ()

let orElse = new OrElseBuilder ()

let is1 =
  function
  | 1 -> Some "1"
  | _ -> None

let is2 =
  function
  | 2 -> Some "2"
  | _ -> None

let is3 =
  function
  | 3 -> Some "3"
  | _ -> None

let multiCheck i =
  orElse {
    return! is1 i
    return! is2 i
    return! is3 i
  }

multiCheck 1
multiCheck 3
multiCheck 7


/// continuation
/// let the caller decide, almost like visitor-pattern?
///
//public T Divide<T>(int top, int bottom, Func<T> ifZero, Func<int,T> ifSuccess)
//{
//    if (bottom==0)
//    {
//        return ifZero();
//    }
//    else
//    {
//        return ifSuccess( top/bottom );
//    }
//}
//
//public T IsEven<T>(int aNumber, Func<int,T> ifOdd, Func<int,T> ifEven)
//{
//    if (aNumber % 2 == 0)
//    {
//        return ifEven(aNumber);
//    }
//    else
//    {   return ifOdd(aNumber);
//    }
//}
let example1 =
  let x = 42 in
  let y = 42 in
  let z = x + y in
  z
let example2 =
  42
  |> (fun x -> 42 |> fun y -> x + y |> fun z -> z)
let pipeInto (someExpression, lambda) = someExpression |> lambda
let example3 =
  pipeInto (42, (fun x -> pipeInto (42, (fun y -> pipeInto (x + y, (fun z -> z))))))
()
