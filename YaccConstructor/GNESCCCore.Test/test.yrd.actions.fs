//this file was generated by GNESCC
//source grammar:../../../Tests/GNESCC/test.yrd
//date:10/24/2011 11:49:19

module GNESCC.Actions_test

open Yard.Generators.GNESCCGenerator

let getUnmatched x expectedType =
    "Unexpected type of node\nType " + x.ToString() + " is not expected in this position\n" + expectedType + " was expected." |> failwith
let s0 expr = 
    let inner  = 
        match expr with
        | RESeq [gnescc_x0; gnescc_x1] -> 
            let (gnescc_x0) =
                let yardElemAction expr = 
                    match expr with
                    | RELeaf e -> (e :?> _ ) 
                    | x -> getUnmatched x "RELeaf"

                yardElemAction(gnescc_x0)
            let (gnescc_x1) =
                let yardElemAction expr = 
                    match expr with
                    | REAlt(Some(x), None) -> 
                        let yardLAltAction expr = 
                            match expr with
                            | RESeq [gnescc_x0] -> 
                                let (gnescc_x0) =
                                    let yardElemAction expr = 
                                        match expr with
                                        | RELeaf a -> (a :?> _ ) 
                                        | x -> getUnmatched x "RELeaf"

                                    yardElemAction(gnescc_x0)
                                (gnescc_x0 )
                            | x -> getUnmatched x "RESeq"

                        yardLAltAction x 
                    | REAlt(None, Some(x)) -> 
                        let yardRAltAction expr = 
                            match expr with
                            | RESeq [gnescc_x0] -> 
                                let (gnescc_x0) =
                                    let yardElemAction expr = 
                                        match expr with
                                        | RELeaf b -> (b :?> _ ) 
                                        | x -> getUnmatched x "RELeaf"

                                    yardElemAction(gnescc_x0)
                                (gnescc_x0 )
                            | x -> getUnmatched x "RESeq"

                        yardRAltAction x 
                    | x -> getUnmatched x "REAlt"

                yardElemAction(gnescc_x1)
            (gnescc_x0,gnescc_x1 )
        | x -> getUnmatched x "RESeq"
    box (inner)
let e1 expr = 
    let inner  = 
        match expr with
        | RESeq [gnescc_x0] -> 
            let (gnescc_x0) =
                let yardElemAction expr = 
                    match expr with
                    | RELeaf tNUMBER -> tNUMBER :?> 'a
                    | x -> getUnmatched x "RELeaf"

                yardElemAction(gnescc_x0)
            (gnescc_x0 )
        | x -> getUnmatched x "RESeq"
    box (inner)
let a2 expr = 
    let inner  = 
        match expr with
        | RESeq [gnescc_x0] -> 
            let (gnescc_x0) =
                let yardElemAction expr = 
                    match expr with
                    | RELeaf tPLUS -> tPLUS :?> 'a
                    | x -> getUnmatched x "RELeaf"

                yardElemAction(gnescc_x0)
            (gnescc_x0 )
        | x -> getUnmatched x "RESeq"
    box (inner)
let b3 expr = 
    let inner  = 
        match expr with
        | RESeq [gnescc_x0] -> 
            let (gnescc_x0) =
                let yardElemAction expr = 
                    match expr with
                    | RELeaf tMINUS -> tMINUS :?> 'a
                    | x -> getUnmatched x "RELeaf"

                yardElemAction(gnescc_x0)
            (gnescc_x0 )
        | x -> getUnmatched x "RESeq"
    box (inner)

let ruleToAction = dict [|(4,b3); (3,a2); (2,e1); (1,s0)|]

