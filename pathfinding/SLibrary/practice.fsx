#load "SLibrary.fs"

module Map =
    /// Wrapper type, Monadic type
    /// map, lift, select
    /// <$> <!>
    /// lifts a function
    /// (a-b) -> X<a> -> X<b>
    let mapOption f opt =
        match opt with
        | None -> None
        | Some x -> Some(f x)

    let rec mapList f list =
        match list with
        | [] -> []
        | first :: rest -> (f first) :: (mapList f rest)

module Return =
    /// return, pure, unit, yield, point
    /// lifts a single value
    /// a -> X<a>
    let returnOption x = Some x
    let returnList x = [ x ]

module Apply =
    /// Applicative Functor
    /// apply, ap
    /// <*>
    /// unpacks a lifted function from a lifted value to a lifted function
    /// X<(a->b> -> X<a> -> X<b>
    let applyOption fOpt xOpt =
        match fOpt, xOpt with
        | Some f, Some x -> Some(f x)
        | _ -> None

    let applyList (fList: ('a -> 'b) list) (xList: 'a list) =
        [ for f in fList do
              for x in xList do
                  yield f x ]

    let add x y = x + y

    let applyListTest1 =
        let (<*>) = applyList
        [ add ] <*> [ 1; 2 ] <*> [ 10; 20 ]
    // [11;21;12;22]
    let applyResultOption1 =
        let (<*>) = applyOption
        (Some add) <*> (Some 2) <*> (Some 3)
    // Some 5
    let applyResultOption2 =
        let (<!>) = Map.mapOption
        let (<*>) = applyOption
        add <!> (Some 2) <*> (Some 3)

    let applyListTest2 =
        let (<!>) = Map.mapList
        let (<*>) = applyList
        add <!> [ 1; 2 ] <*> [ 10; 20 ]

    let testStringConcat =
        let (<!>) = Map.mapList
        let (<*>) = applyList
        (+) <!> [ "foo"; "bar"; "baz" ] <*> [ "!"; "!?!" ]
// foo!; foo!?"; bar!; bar!?!; baz!; baz!?!
module Lift =
    /// lift2, lift2... (lift1 = map)
    /// combine x lifter values using a function
    /// lift2: (a->b->c) -> X<a> -> X<b> -> X<c>
    let (<*>) = Apply.applyList
    let (<!>) = List.map
    let lift2 f x y = f <!> x <*> y
    let lift3 f x y z = f <!> x <*> y <*> z
    let lift4 f x y z w = f <!> x <*> y <*> z <*> w

    let add x y = x + y
    let addPairList = lift2 add
    //  addPairOption (Some 1) (Some 2) |> ignore
    // Some 3


    let lift2FromScratch f xOpt yOpt =
        match xOpt, yOpt with
        | Some x, Some y -> Some(f x y)
        | _ -> None

    let applyFromLift2 fOpt xOpt = lift2FromScratch id fOpt xOpt
    let (<*) x y = lift2 (fun left right -> left) x y
    let ( *> ) x y = lift2 (fun left right -> left) x y
    [ 1; 2 ] <* [ 3; 4; 5 ] |> ignore
    // [1;1;1;2;2;2]
    [ 1; 2 ] *> [ 3; 4; 5 ] |> ignore
    // [3;4;5;3;4;5]
    let repeat n pattern = [ 1 .. n ] *> pattern
    let replicate n x = [ 1 .. n ] *> [ x ]

module Zip =
    /// zip,zip3 map2
    /// <*>
    /// combine two enumerables with a function
    /// E<(a->b->c)> -> E<a> -> E<b> -> E<c> // when E is enumerable
    /// E<a> -> E<b> -> E<a,b> // when E is a tuple
    let rec zipList fList xList =
        match fList, xList with
        | [], _ -> []
        | _, [] -> []
        | (f :: fTail), (x :: xTail) -> (f x) (zipList fTail xTail)
//  let test =
//    let add10 x = x + 10
//    let add20 x = x + 20
//    let add30 x = x + 30
//    let (<*>) = zipList
//    [add10;add20;add30] <*> [1;2;3]
module Bind =
    /// like a bind but monadic instead of applicative
    /// bind, flatMap, andThen, collect, selectMany
    /// >>= (left to right) =<< (right to left)
    /// normal to monadic functions
    /// (a -> X<b>) -> X<a> -> X<B>
    let optionBind f xOpt =
        match xOpt with
        | Some x -> f x
        | _ -> None

    let listBind (f: 'a -> 'b list) (xList: 'a list) =
        [ for x in xList do
              for y in f x do
                  yield y ]

// applicative vs monadic
// applicative;
//   apply, lift, combine
// monadic;
//   bind
//
// do something in paralell and independent data; applicative
// one at a time, skip the next if this fails; monadic
// if dependendencies then you need to be monadic
//
// static vs dynamic
// if applicative then we can define all actions up front / statically
// if monadic then we have more flexibility but cant do things like paralellelization or optimize

// basic validation example
type CustomerId = CustomerId of int
type EmailAddress = EmailAddress of string
type CustomerInfo = { Id: CustomerId; Email: EmailAddress }
type TestRecord = { X: int; Y: int }

type Result<'a> =
    | Success of 'a
    | Failure of string list

let createCustomerId id =
    if id > 0 then
        Success(CustomerId id)
    else
        Failure [ "CustomerId must be positive" ]

let createEmailAddress str =
    if System.String.IsNullOrEmpty(str) then
        Failure [ "Email cant be empty" ]
    elif str.Contains("@") then
        Success(EmailAddress str)
    else
        Failure [ "Email must contain @" ]

module Result =
    // map    (a -> b) -> result<a> -> result<b>
    // ret    a -> result<a>
    // apply  result<(a->b)> -> result<a> -> result<b>
    // bind   (a -> result<b>) -> result<a> -> result<b>

    let map f x =
        match x with
        | Success x -> Success(f x)
        | Failure errs -> Failure errs

    let ret x = Success x

    let apply f x =
        match f, x with
        | Success f, Success x -> Success(f x)
        | Failure errs, Success x -> Failure errs
        | Success x, Failure errs -> Failure errs
        | Failure errs1, Failure errs2 -> Failure(List.concat [ errs1; errs2 ])

    let bind f x =
        match x with
        | Success x -> f x
        | Failure errs -> Failure errs

let createCustomer customerId email = { Id = customerId; Email = email }
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
            >>= fun xEmail -> Success(createCustomer xId xEmail))

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
type ResultBuilder() =
    member this.Return x = Result.ret x
    member this.Bind(x, f) = Result.bind f x

let result = ResultBuilder()

let createCustomerResultComputationExpression id email =
    result {
        let! customerId = createCustomerId id
        let! customerEmail = createEmailAddress email
        let customer = createCustomer customerId customerEmail
        return customer
    }



let log p = printfn $"expression is {p}"

let loggedWorkFlow =
    let x = 42
    log x
    let y = 43
    log y
    let z = x + y
    log z
    z

type LoggingBuilder() =
    let log p = printfn $"expression is {p}"

    member this.Bind(x, f) =
        log x
        f x

    member this.Return(x) = x

let logger = LoggingBuilder()

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
        Some(top / bottom)

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

type MaybeBuilder() =
    member this.Bind(x, f) =
        match x with
        | None -> None
        | Some a -> f a

    member this.Return(x) = Some x

let maybe = MaybeBuilder()

let divideByCE init x y z =
    maybe {
        let! a = init |> divideBy x
        let! b = a |> divideBy y
        let! c = b |> divideBy z
        return c
    }

// or else
type OrElseBuilder() =
    member this.ReturnFrom(x) = x

    member this.Combine(a, b) =
        match a with
        | Some _ -> a
        | None -> b

    member this.Delay(f) = f ()

let orElse = OrElseBuilder()

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

let example2 = 42 |> (fun x -> 42 |> fun y -> x)
let pipeInto (someExpression, lambda) = someExpression |> lambda

let example3 str =
    (*pipeInto (42, (fun x -> pipeInto (42, (fun y -> pipeInto (x + y, (fun z -> z))))))*)
    if System.String.IsNullOrEmpty(str) then
        Failure [ "Email cant be empty" ]
    elif str.Contains("@") then
        Success(EmailAddress str)
    else
        Failure [ "Email must contain @" ]
// test
// test 2