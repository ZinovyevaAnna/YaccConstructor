//this file was generated by GNESCC
//source grammar:../../../Tests/GNESCC/test_2/test_2.yrd
//date:10/24/2011 11:49:21

module GNESCC.Regexp_test_2

open Yard.Generators.GNESCCGenerator
open System.Text.RegularExpressions

let buildIndexMap kvLst =
    let ks = List.map (fun (x:string,y) -> x.Length + 2,y) kvLst
    List.fold (fun (bl,blst) (l,v) -> bl+l,((bl,v)::blst)) (0,[]) ks
    |> snd
    |> dict

let buildStr kvLst =
    let sep = ";;"
    List.map fst kvLst 
    |> String.concat sep
    |> fun s -> ";" + s + ";"

let s childsLst = 
    let str = buildStr childsLst
    let idxValMap = buildIndexMap childsLst
    let re = new Regex("((;2;)(;2;))")
    let elts =
        let res = re.Match(str)
        if Seq.fold (&&) true [for g in res.Groups -> g.Success]
        then res.Groups
        else (new Regex("((;2;)(;2;))",RegexOptions.RightToLeft)).Match(str).Groups
    let e1 =
        idxValMap.[elts.[3].Captures.[0].Index] |> RELeaf
    let e0 =
        idxValMap.[elts.[2].Captures.[0].Index] |> RELeaf
    RESeq [e0; e1]
let e childsLst = 
    let str = buildStr childsLst
    let idxValMap = buildIndexMap childsLst
    let re = new Regex("((;6;))")
    let elts =
        let res = re.Match(str)
        if Seq.fold (&&) true [for g in res.Groups -> g.Success]
        then res.Groups
        else (new Regex("((;6;))",RegexOptions.RightToLeft)).Match(str).Groups
    let e0 =
        idxValMap.[elts.[2].Captures.[0].Index] |> RELeaf
    RESeq [e0]

let ruleToRegex = dict [|(2,e); (1,s)|]

