//this file was generated by RACC
//source grammar:..\Tests\RACC\test_summator_1\\test_summator_1.yrd
//date:12/27/2010 15:46:43

module RACC.Actions_Summator_1

open Yard.Generators.RACCGenerator

let value x = (x:>Lexeme<string>).value

let s0 expr = 
    let inner  = 
        match expr with
        | RESeq [x0] -> 
            let (res) =
                let yardElemAction expr = 
                    match expr with
                    | RELeaf e -> (e :?> _ ) 
                    | x -> "Unexpected type of node\nType " + x.ToString() + " is not expected in this position\nRELeaf was expected." |> failwith

                yardElemAction(x0)
            (res)
        | x -> "Unexpected type of node\nType " + x.ToString() + " is not expected in this position\nRESeq was expected." |> failwith
    box (inner)
let e1 expr = 
    let inner  = 
        match expr with
        | REAlt(Some(x), None) -> 
            let yardLAltAction expr = 
                match expr with
                | RESeq [x0] -> 
                    let (n) =
                        let yardElemAction expr = 
                            match expr with
                            | RELeaf tNUMBER -> tNUMBER :?> 'a
                            | x -> "Unexpected type of node\nType " + x.ToString() + " is not expected in this position\nRELeaf was expected." |> failwith

                        yardElemAction(x0)
                    (value n |> float)
                | x -> "Unexpected type of node\nType " + x.ToString() + " is not expected in this position\nRESeq was expected." |> failwith

            yardLAltAction x 
        | REAlt(None, Some(x)) -> 
            let yardRAltAction expr = 
                match expr with
                | RESeq [x0; _; x1] -> 
                    let (e) =
                        let yardElemAction expr = 
                            match expr with
                            | RELeaf e -> (e :?> _ ) 
                            | x -> "Unexpected type of node\nType " + x.ToString() + " is not expected in this position\nRELeaf was expected." |> failwith

                        yardElemAction(x0)

                    let (n) =
                        let yardElemAction expr = 
                            match expr with
                            | RELeaf tNUMBER -> tNUMBER :?> 'a
                            | x -> "Unexpected type of node\nType " + x.ToString() + " is not expected in this position\nRELeaf was expected." |> failwith

                        yardElemAction(x1)
                    (value n |> float |> ((+) e))
                | x -> "Unexpected type of node\nType " + x.ToString() + " is not expected in this position\nRESeq was expected." |> failwith

            yardRAltAction x 
        | x -> "Unexpected type of node\nType " + x.ToString() + " is not expected in this position\nREAlt was expected." |> failwith
    box (inner)

let ruleToAction = dict [|("e",e1); ("s",s0)|]
