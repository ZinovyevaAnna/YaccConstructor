//this file was generated by RACC
//source grammar:..\Tests\RACC\test_alt\\test_alt.yrd
//date:12/8/2010 16:59:49

module RACC.Actions_Alt

open Yard.Generators.RACCGenerator

let value x =
    ((x:>Lexeme<string>).value)

let s0 expr = 
    match expr with
    | REAlt(Some(x), None) -> 
        let yardLAltAction expr = 
            match expr with
            | RESeq [x0] -> 
                let (p) =
                    let yardElemAction expr = 
                        match expr with
                        | RELeaf PLUS -> PLUS :?> 'a
                        | x -> "Unexpected type of node\nType " + x.ToString() + " is not expected in this position\nRELeaf was expected." |> failwith

                    yardElemAction(x0)
                box ("Detected: " + (value p))
            | x -> "Unexpected type of node\nType " + x.ToString() + " is not expected in this position\nRESeq was expected." |> failwith

        yardLAltAction x 
    | REAlt(None, Some(x)) -> 
        let yardRAltAction expr = 
            match expr with
            | RESeq [x0] -> 
                let (m) =
                    let yardElemAction expr = 
                        match expr with
                        | RELeaf MULT -> MULT :?> 'a
                        | x -> "Unexpected type of node\nType " + x.ToString() + " is not expected in this position\nRELeaf was expected." |> failwith

                    yardElemAction(x0)
                box ("Detected: " + (value m))
            | x -> "Unexpected type of node\nType " + x.ToString() + " is not expected in this position\nRESeq was expected." |> failwith

        yardRAltAction x 
    | x -> "Unexpected type of node\nType " + x.ToString() + " is not expected in this position\nREAlt was expected." |> failwith


let ruleToAction = dict [|("s",s0)|]
