﻿//  RNGLRGenerator.fs contains implementation of interface Generator
//
//  Copyright 2011-2012 Avdyukhin Dmitry
//
//  This file is part of YaccConctructor.
//
//  YaccConstructor is free software:you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

namespace Yard.Generators.RNGLR

open Yard.Core
open Yard.Core.IL
open Yard.Generators.RNGLR.InitialConvert
open Yard.Generators.RNGLR.FinalGrammar
open Yard.Generators.YardPrinter
open Yard.Generators.RNGLR.States
open Yard.Generators.RNGLR.Printer
open Yard.Generators.RNGLR.TranslatorPrinter
open Option

type RNGLR() = 
    inherit Generator()
        override this.Name = "RNGLRGenerator"
        override this.Generate (definition, args) =
            let start = System.DateTime.Now
            let args = args.Split([|' ';'\t';'\n';'\r'|]) |> Array.filter ((<>) "")
            let pairs = Array.zeroCreate <| args.Length / 2
            for i = 0 to pairs.Length-1 do
                pairs.[i] <- args.[i * 2], args.[i * 2 + 1]
            let mutable moduleName = ""
            let mutable tokenType = ""
            let mutable table = LALR
            let mutable fullPath = false
            let mutable positionType = "Microsoft.FSharp.Text.Lexing.Position"
            let mutable needTranslate = true
            let mutable output = definition.info.fileName + ".fs"
            for opt, value in pairs do
                match opt with
                | "-module" -> moduleName <- value
                | "-token" -> tokenType <- value
                | "-pos" -> positionType <- value
                | "-o" -> output <- value
                | "-table" ->
                    match value with
                    | "LALR" -> table <- LALR
                    | "LR" -> table <- LR
                    | x -> failwith "Unexpected table type %s" x
                | "-fullpath" ->
                    if value = "true" then fullPath <- true
                    elif value = "false" then fullPath <- false
                    else failwith "Unexpected fullPath value %s" value
                | "-translate" ->
                    if value = "true" then needTranslate <- true
                    elif value = "false" then needTranslate <- false
                    else failwith "Unexpected translate value %s" value
                // In other cases causes error
                | _ -> failwithf "Unknown option %A" opt
            let newDefinition = initialConvert definition
            let grammar = new FinalGrammar(newDefinition.grammar);
            if grammar.EpsilonCyclicNonTerms.Length > 0 then
                eprintfn "Grammar contains non-terminals, which can infinitely infer epsilon:"
                grammar.EpsilonCyclicNonTerms
                |> List.iter (eprintf "%s ")
                box ()
            else
                let statesInterpreter = buildStates table grammar
                let tables = new Tables(grammar, statesInterpreter)
                use out = new System.IO.StreamWriter (output)
                let res = new System.Text.StringBuilder()
                let dummyPos = char 0
                let println (x : 'a) =
                    Printf.kprintf (fun s -> res.Append(s).Append "\n" |> ignore) x
                let print (x : 'a) =
                    Printf.kprintf (fun s -> res.Append(s) |> ignore) x
                println "%s" <| getPosFromSource fullPath dummyPos (defaultSource output)
                println "module %s"
                <|  match moduleName with
                    | "" -> "RNGLR.Parse"
                    | s -> s
                println "#nowarn \"64\";; // From fsyacc: turn off warnings that type variables used in production annotations are instantiated to concrete type"

                println "open Yard.Generators.RNGLR.Parser"
                println "open Yard.Generators.RNGLR"
                println "open Yard.Generators.RNGLR.AST"

                match definition.head with
                | None -> ()
                | Some (s : Source.t) ->
                    println "%s" <| getPosFromSource fullPath dummyPos s
                    println "%s" <| s.text + getPosFromSource fullPath dummyPos (defaultSource output)

                let tables = printTables grammar definition.head tables moduleName tokenType res
                let res = if not needTranslate then tables
                          else tables + printTranslator grammar newDefinition.grammar
                                            positionType fullPath output dummyPos
                let res = 
                    match definition.foot with
                    | None -> res
                    | Some (s : Source.t) ->
                        res + (getPosFromSource fullPath dummyPos s + "\n"
                                 + s.text + getPosFromSource fullPath dummyPos (defaultSource output) + "\n")
                let res =
                    let init = res.Replace("\r\n", "\n")
                    let curLine = ref 1
                    let res = new System.Text.StringBuilder(init.Length * 2)
                    for c in init do
                        match c with
                        | '\n' -> incr curLine; res.Append System.Environment.NewLine
                        | c when c = dummyPos -> res.Append (string !curLine)
                        | x -> res.Append x
                        |> ignore
                    res.ToString()
                out.WriteLine res
                out.Close()
                eprintfn "Generation time: %A" <| System.DateTime.Now - start
                //(new YardPrinter()).Generate newDefinition
                box ()
        override this.Generate definition = this.Generate (definition, "")
        override this.AcceptableProductionTypes =
            List.ofArray(Reflection.FSharpType.GetUnionCases typeof<IL.Production.t<string,string>>)
            |> List.map (fun unionCase -> unionCase.Name)