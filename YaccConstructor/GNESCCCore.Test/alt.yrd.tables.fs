//this tables was generated by GNESCC
//source grammar:../../../Tests/GNESCC/regexp/simple/alt/alt.yrd
//date:10/24/2011 11:49:21

module Yard.Generators.GNESCCGenerator.Tables_alt

open Yard.Generators.GNESCCGenerator
open Yard.Generators.GNESCCGenerator.CommonTypes

type symbol =
    | T_MULT
    | T_PLUS
    | NT_s
    | NT_gnesccStart
let getTag smb =
    match smb with
    | T_MULT -> 6
    | T_PLUS -> 5
    | NT_s -> 4
    | NT_gnesccStart -> 2
let getName tag =
    match tag with
    | 6 -> T_MULT
    | 5 -> T_PLUS
    | 4 -> NT_s
    | 2 -> NT_gnesccStart
    | _ -> failwith "getName: bad tag."
let prodToNTerm = 
  [| 1; 0 |];
let symbolIdx = 
  [| 2; 3; 1; 3; 0; 1; 0 |];
let startKernelIdxs =  [0]
let isStart =
  [| [| true; true |];
     [| false; false |];
     [| false; false |];
     [| false; false |]; |]
let gotoTable =
  [| [| Some 1; None |];
     [| None; None |];
     [| None; None |];
     [| None; None |]; |]
let actionTable = 
  [| [| [Shift 3]; [Shift 2]; [Error]; [Error] |];
     [| [Accept]; [Accept]; [Accept]; [Accept] |];
     [| [Reduce 1]; [Reduce 1]; [Reduce 1]; [Reduce 1] |];
     [| [Reduce 1]; [Reduce 1]; [Reduce 1]; [Reduce 1] |]; |]
let tables = 
  {StartIdx=startKernelIdxs
   SymbolIdx=symbolIdx
   GotoTable=gotoTable
   ActionTable=actionTable
   IsStart=isStart
   ProdToNTerm=prodToNTerm}
