//this file was generated by RACC
//source grammar:..\Tests\RACC\test_seq\\test_seq.yrd
//date:12/8/2010 16:59:47

module RACC.Actions_Seq

open Yard.Generators.RACCGenerator

let value x =
    ((x:>Lexeme<string>).value)

let s0 expr = 
    match expr with
    | RESeq [x0; _; x1] -> 
        let (l) =
            let yardElemAction expr = 
                match expr with
                | RELeaf NUMBER -> NUMBER :?> 'a
                | x -> "Unexpected type of node\nType " + x.ToString() + " is not expected in this position\nRELeaf was expected." |> failwith

            yardElemAction(x0)

        let (r) =
            let yardElemAction expr = 
                match expr with
                | RELeaf NUMBER -> NUMBER :?> 'a
                | x -> "Unexpected type of node\nType " + x.ToString() + " is not expected in this position\nRELeaf was expected." |> failwith

            yardElemAction(x1)
        box ((value l |> float) + (value r |> float))
    | x -> "Unexpected type of node\nType " + x.ToString() + " is not expected in this position\nRESeq was expected." |> failwith


let ruleToAction = dict [|("s",s0)|]


//test footer
