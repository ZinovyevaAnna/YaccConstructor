//this tables was generated by RACC
//source grammar:..\Tests\RACC\test_cls_with_head\\test_cls_with_head.yrd
//date:12/17/2010 10:47:39

#light "off"
module Yard.Generators.RACCGenerator.Tables_Cls_head

open Yard.Generators.RACCGenerator

let autumataDict = 
dict [|("raccStart",{ 
   DIDToStateMap = dict [|(0,(State 0));(1,(State 1));(2,DummyState)|];
   DStartState   = 0;
   DFinaleStates = Set.ofArray [|1|];
   DRules        = Set.ofArray [|{ 
   FromStateID = 0;
   Symbol      = (DSymbol "s");
   Label       = Set.ofArray [|List.ofArray [|(FATrace (TSmbS 0))|]|];
   ToStateID   = 1;
}
;{ 
   FromStateID = 1;
   Symbol      = Dummy;
   Label       = Set.ofArray [|List.ofArray [|(FATrace (TSmbE 0))|]|];
   ToStateID   = 2;
}
|];
}
);("s",{ 
   DIDToStateMap = dict [|(0,(State 0));(1,(State 1));(2,(State 2));(3,DummyState);(4,DummyState)|];
   DStartState   = 0;
   DFinaleStates = Set.ofArray [|1;2|];
   DRules        = Set.ofArray [|{ 
   FromStateID = 0;
   Symbol      = (DSymbol "MINUS");
   Label       = Set.ofArray [|List.ofArray [|(FATrace (TSeqS 5));(FATrace (TSmbS 1))|]|];
   ToStateID   = 1;
}
;{ 
   FromStateID = 1;
   Symbol      = (DSymbol "MINUS");
   Label       = Set.ofArray [|List.ofArray [|(FATrace (TSmbE 1));(FATrace (TClsS 2));(FATrace (TClsE 2));(FATrace (TSeqE 5))|];List.ofArray [|(FATrace (TSmbE 1));(FATrace (TClsS 2));(FATrace (TSeqS 4));(FATrace (TSmbS 3))|];List.ofArray [|(FATrace (TSmbE 1));(FATrace (TClsS 2))|]|];
   ToStateID   = 2;
}
;{ 
   FromStateID = 1;
   Symbol      = Dummy;
   Label       = Set.ofArray [|List.ofArray [|(FATrace (TSmbE 1));(FATrace (TClsS 2));(FATrace (TClsE 2));(FATrace (TSeqE 5))|];List.ofArray [|(FATrace (TSmbE 1));(FATrace (TClsS 2));(FATrace (TSeqS 4));(FATrace (TSmbS 3))|];List.ofArray [|(FATrace (TSmbE 1));(FATrace (TClsS 2))|]|];
   ToStateID   = 3;
}
;{ 
   FromStateID = 2;
   Symbol      = (DSymbol "MINUS");
   Label       = Set.ofArray [|List.ofArray [|(FATrace (TSmbE 3));(FATrace (TSeqE 4));(FATrace (TClsE 2));(FATrace (TSeqE 5))|];List.ofArray [|(FATrace (TSmbE 3));(FATrace (TSeqE 4));(FATrace (TSeqS 4));(FATrace (TSmbS 3))|];List.ofArray [|(FATrace (TSmbE 3));(FATrace (TSeqE 4))|]|];
   ToStateID   = 2;
}
;{ 
   FromStateID = 2;
   Symbol      = Dummy;
   Label       = Set.ofArray [|List.ofArray [|(FATrace (TSmbE 3));(FATrace (TSeqE 4));(FATrace (TClsE 2));(FATrace (TSeqE 5))|];List.ofArray [|(FATrace (TSmbE 3));(FATrace (TSeqE 4));(FATrace (TSeqS 4));(FATrace (TSmbS 3))|];List.ofArray [|(FATrace (TSmbE 3));(FATrace (TSeqE 4))|]|];
   ToStateID   = 4;
}
|];
}
)|]

let items = 
List.ofArray [|("raccStart",0);("raccStart",1);("raccStart",2);("s",0);("s",1);("s",2);("s",3);("s",4)|]

let gotoSet = 
    Set.ofArray [|(-1144263691,Set.ofArray [|("s",2)|]);(-1144264106,Set.ofArray [|("s",2)|]);(-1144264172,Set.ofArray [|("s",1)|]);(-1239003080,Set.ofArray [|("raccStart",2)|]);(-635149922,Set.ofArray [|("raccStart",1)|]);(1723491585,Set.ofArray [|("s",1)|]);(1800920813,Set.ofArray [|("s",3)|]);(1800920910,Set.ofArray [|("s",4)|])|]
    |> dict
