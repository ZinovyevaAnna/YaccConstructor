module Yard.Generators.CYK

open Yard.Core
open Yard.Generators.CYKGenerator
type cykToken = 
  | EOF
  | IDENT
  | LOCALVAR
  | NUM
  | KW_PROC
  | KW_PROCEDURE
  | PLUS
  | STAR
  | KW_END
  | AS
  | FROM
  | RBR
  | COMMA
  | SELECT
  | LBR
  | SEMI
  | KW_BEGIN
  | KW_CREATE
  | KW_GO
let getTag token = 
  match token with 
  | EOF -> 0us
  | IDENT -> 1us
  | LOCALVAR -> 2us
  | NUM -> 3us
  | KW_PROC -> 4us
  | KW_PROCEDURE -> 5us
  | PLUS -> 6us
  | STAR -> 7us
  | KW_END -> 8us
  | AS -> 9us
  | FROM -> 10us
  | RBR -> 11us
  | COMMA -> 12us
  | SELECT -> 13us
  | LBR -> 14us
  | SEMI -> 15us
  | KW_BEGIN -> 16us
  | KW_CREATE -> 17us
  | KW_GO -> 18us
let rules = 
  [
  281483566841856UL; 281492156907520UL; 1407383473684480UL; 1407392063750144UL;
  562975723683840UL; 2251838469046272UL; 3096276284276736UL; 3096284874080256UL;
  4222128945627136UL; 4222133240594432UL; 4222137535561728UL; 4222193371250688UL;
  4222201961316352UL; 4222201961381888UL; 4222201961447424UL; 3377794211315712UL;
  6755463867203584UL; 6755511111778304UL; 7599828666155008UL; 5066553875759104UL;
  5066558170726400UL; 5066562465693696UL; 5066618301382656UL; 5066626891448320UL;
  7881307938029568UL; 1125908496973824UL; 8162791504478464UL; 8162795799446016UL;
  8444300841779200UL; 3940701214408704UL; 8725788704178176UL; 7318413820624896UL;
  9007225024544768UL; 9007229319512064UL; 5348161997635584UL; 4785151915655168UL;
  6474027545788416UL; 9570213634965504UL; 9851778805465088UL; 5911133426024448UL;
  2814797014237184UL; 1970449393582080UL; 10977609993682944UL;
  11259157982740480UL; 10696083474743296UL; 10414612792999936UL;
  10133142111256576UL; 9288721476091904UL; 7036925957373952UL;
  6192505322209280UL; 5629503829180416UL; 4503659756912640UL; 3659239121747968UL;
  2533343509872640UL; 1688922874707968UL; 844502239543296UL ]
  |> Array.ofList
let lblName = 
  [|
  "@bad";
  "@good";
  |]
let StartNTerm = 1
let CodeTokenStream (stream:seq<CYKToken<cykToken,_>>) = 
  stream
  |> Seq.choose (fun t ->
    let tag = getTag t.Tag
    if tag <> 0us then Some tag else None)
  |> Array.ofSeq