// Learn more about F# at http://fsharp.net

let rec fib x =
    match x with
    | 0 -> 0
    | 1 -> 1
    | _ -> fib(x - 1) + fib(x - 2);;

let result=fib 10;;
printfn "%d" result;;