﻿// NLFAToDLFA.fs
//
// Copyright 2009-2010 Semen Grigorev
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation.
//
//Implementation of NLFA to DLFA convertion

module Yard.Generators._RACCGenerator.NLFAToDLFA


//see Dragon book p129
let NLFAToDLFA (nlfa:NLFA<_,_,_>) eLineFilter =
    let symbols = Set.filter ((<>) Epsilon) (Set.map (fun rule -> rule.Symbol) nlfa.NRules)
    let stateIDs = Set.ofSeq nlfa.NIDToStateMap.Keys
    let eLinesSet = ref Set.empty
        
    let globalVisitedStates = ref []
    let localVisitedStates = ref []

    let rec eLines state =         
        globalVisitedStates := state :: !globalVisitedStates
        localVisitedStates := state :: !localVisitedStates
        let eSteps = Set.fold 
                        (fun buf rule -> if rule.FromStateID = state && rule.Symbol = Epsilon
                                         then Set.add (rule.ToStateID, rule.Label) buf
                                         else buf)
                        Set.empty
                        nlfa.NRules
        Set.map 
            (fun (_to,l) ->                
                if not (List.exists ((=) _to) !localVisitedStates) 
                then
                    let eL = eLines _to
                    if Set.isEmpty eL
                    then Set.singleton [(_to,l)]
                    else Set.map (fun tl -> (_to,l)::tl) eL
                else Set.singleton [(_to,l)]) 
            eSteps 
        |> Set.unionMany           

                     
    let buldELines = 
        Set.iter 
            (fun stateID ->
                if not (List.exists ((=) stateID) !globalVisitedStates) 
                then
                    localVisitedStates := []
                    let el = eLines stateID
                    eLinesSet := Set.union (!eLinesSet) (Set.map (fun x -> stateID,x)el))
            stateIDs
        Set.map 
            (fun eLine -> 
                let _to = fst (List.head(List.rev (snd eLine)))
                let from = fst eLine
                let line = List.map (fun (_, lbl) -> lbl) (snd eLine)
                from,_to,line)
            !eLinesSet        

    let move stateSet symbol = 
        Set.map 
            (fun state -> Set.map 
                               (fun rule -> rule.ToStateID)
                               (Set.filter 
                                     (fun rule -> rule.FromStateID = state && rule.Symbol = symbol) 
                                     nlfa.NRules))
            stateSet
            |> Set.unionMany

    let eClosure statesSet = 
        let stack = ref (List.ofSeq statesSet)
        let eCls = ref statesSet
        while not (List.isEmpty !stack) do            
            let t = (!stack).Head
            stack := (!stack).Tail   
            Set.iter 
                 (fun state -> 
                      if not (Set.exists ((=)state) !eCls)
                      then 
                        eCls := Set.add state !eCls
                        stack := state::!stack)
                 (Set.map 
                       (fun rule -> rule.ToStateID)
                       (Set.filter (fun rule -> rule.Symbol = Epsilon && rule.FromStateID = t) nlfa.NRules))
        done
        !eCls

    let visitedNewStates = ref []
    let notVisitedNewStates = ref [eClosure (Set.singleton nlfa.NStartState)]
    let newRules = ref []

    while not (List.isEmpty !notVisitedNewStates) do
        let T = (!notVisitedNewStates).Head
        visitedNewStates :=  T :: !visitedNewStates
        notVisitedNewStates := (!notVisitedNewStates).Tail
        Set.iter
                (fun symbol ->
                let U = eClosure (move T symbol)
                if not (Set.isEmpty U)
                    then
                        if  not ((List.exists ((=)U) !notVisitedNewStates)  || (List.exists ((=)U) !visitedNewStates))
                        then 
                            notVisitedNewStates := U :: !notVisitedNewStates
                        newRules := (T,symbol,U)::!newRules)
                symbols
    done

    let newAutomata = 
        let states = List.ofSeq (List.fold (fun buf (_from,_,_to) ->  (Set.add _from buf) |> Set.add _to) Set.empty (!newRules))

        let alterNames = dict (List.zip states [0..(List.length states)-1])
        let getAlterName s = alterNames.[s]            

        let startState = getAlterName (List.find (Set.exists ((=)nlfa.NStartState)) states)
        let finaleStates = (List.filter (Set.exists (fun x -> (=) x  nlfa.NFinaleState)) states)
        let alterFinaleStates = List.map getAlterName finaleStates
        let dummyStates = 
            let l = List.length states
            dict (List.zip finaleStates [l .. l+finaleStates.Length-1])

        let rules = 
            let createLabel state =
                Set.map                                 
                    (fun x -> 
                        Set.filter (fun (from,_to,l) -> Set.exists ((=)from) state && Set.exists ((=)_to) state) buldELines
                        |> Set.map (fun (_,_,line) -> eLineFilter line))
                    state
                |> Set.unionMany
                |> List.ofSeq
                |> fun lst ->
                    List.fold
                        (fun buf elt ->
                            let checker =
                                List.fold 
                                    (fun buf l -> 
                                        match l with
                                        | hd::tl -> buf || tl=elt
                                        | [] -> buf || false)
                                    false
                                    lst
                            if checker
                            then buf
                            else elt::buf)
                        []
                        lst
                |> Set.ofList

            let _rules =
                List.map 
                    (fun (_from, _smb, _to) -> 
                        {
                            FromStateID = getAlterName _from
                            Symbol = 
                                match _smb with
                                | NSymbol(s) -> DSymbol(s)
                                | _          -> failwith "NLFA to DLFA convertion fail. Seems that DLFA contains epsilon transition."
                            Label = createLabel _from
                            ToStateID = getAlterName _to
                        }
                    )
                    (!newRules)
            let dummyRules = 
                List.map 
                    (fun state -> 
                        {
                            FromStateID = getAlterName state
                            Symbol      = Dummy
                            Label       = createLabel state                                 
                            ToStateID   = dummyStates.[state]
                        }
                    )
                    finaleStates
            _rules @ dummyRules
        {
            DIDToStateMap = 
                  (List.zip (List.ofSeq alterNames.Values) (List.init alterNames.Count (fun x -> State(x)))) 
                @ (List.zip (List.ofSeq dummyStates.Values) (List.init dummyStates.Count (fun _ -> DummyState)))
                |> dict
                
            DStartState   = startState
            DFinaleStates = Set.ofList alterFinaleStates
            DRules        = Set.ofList rules
        }

    newAutomata