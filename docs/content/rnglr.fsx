(*** hide ***)
// This block of code is omitted in the generated HTML documentation. Use 
// it to define helpers that you do not want to show in the documentation.
#I "../../bin"

(**
RNGLR
======================

<h2>Module purpose</h2>
This generator creates GLR parser, corresponding to described grammar. The main their benefit is ability to produce all possible derivation trees and store them in compact form. As result, they can accept all strings, belonging to language, described with given grammar.

The generated GLR parser works according to the paper Elizabeth Scott, Adrian Johnstone, Right Nulled GLR Parsers, 2006.

<h2>Generator</h2>
<h3>Running</h3>
To run generator you must type:
<pre>
YaccConstructor -i &lt;input file&gt; &lt;conversions&gt; -g "RNGLRGenerator &lt;command line options&gt;"
</pre>

Where <code> &lt;command line options&gt;</code>  is sequence of pair: (-option value). All of them are represented as string, separated by whitespaces. The meaning of options is as follows:
<ul>
<li><tt>-module name</tt> - The name of generated module. Inserted in the beginning of generated file. default: RNGLR.Parse.
</li>
<li><tt>-token tokentype</tt> - For all tokens will be generated type union Token, each subtype A of what have signature A <tokentype>. default: tokens have no arguments and can't be used in translation.
</li>
<li><tt>-pos posType</tt> - type of Position. During translation it's useful for current nonTerminal to know it's start and end position in source file. Unlike fsyacc, working with already known fslex position, this generator supposed to work with any sequence of tokens. So, position type is unknown in advance. default: Microsoft.FSharp.Text.Lexing.Position.
</li>
<li><tt>-o output</tt> - destination of generated file. default: fileName from IL + ".fs"
</li>
<li><tt>-table (LA|LALR)</tt> - what tables to use. LALR have more ambiguities, but incomparably smaller. default: LALR
</li>
<li><tt>-fullpath (true|false)</tt> - For easier debugging some lines of resulted file are marked, from what file and line they have arised. If fullpath=true, then file name will be printed fully. It can be bad because of usual problems of absolute files. It's usual situation when source and generated files are located in one directory, causing to specify this option to false. default: false
</li>
<li><tt>-translate (true|false)</tt> - whether generate translation description in result file or not. If you are pleasured with only derivation tree, it equals to false. Otherwise, if it's supposed to perform some semantic action, set it to true value. default: true
</li>
<li><tt>-light (true|false)</tt> - whether print #light "off" directive in the beginning of generated file. Currently only 'true' option is supported. default: true
</li>
</ul>

<h3>Constraints on a grammar</h3>
<p>
    The grammar must not contain EBNF, meta-rules, Literals, embedded alternatives (only high-level are allowed). It's also recommended not to use nested rules sequences because of unknown performance effect. Following conversion can translate your grammar into appropriate form: ExpandMeta, ExpandEbnf, ReplaceLiterals, ExpandInnerAlt, ExpandBrackets.
</p>
<p>
    On the other hand, such features like resolvers, l- and s- attributes are supported.
</p>
<p>
    Epsilon-productions are allowed. Generation extracts all epsilon-trees from grammar (so in the result tree they doesn't figurate directly and just referenced by some label). For each nonterminal there is a set of epsilon-trees, it can produce.
</p>

<h2>Parser</h2>
<h3>Abstract Syntax Tree structure</h3>
<p>
Firstly, it isn't tree. In fact, it can be any (with hardly described limitations) connected digraph. It can contain cycles, some vertices can be used by several other vertices. If user doesn't want to have all possible derivation trees, it's usually a grammar defect. And it's not recommended to run translation on tree with cycles! It's not tested, but can lead to something like NullReference? exception (because of using not-evaluated value). There are ways to work with such problems, they will be described further.
</p>
<p>
Each node, if we discard optimization details, have several families of children. Family is some way to expand the nonterminal. It consists of array (also, if simplify) of children. Each children can be:
<ul>
<li>Another Node
</li>
<li>int with value >= 0. Then it represents a number of token in input sequence
</li>
<li>int with value < 0. It represents an epsilon-tree for nonterminal, what number equals to the value.
</li>
</ul>
The entire tree can be either Node or epsilon-tree.
</p>

<h3>Building AST</h3>
<p>
See RNGLRApplication for example. Project references must include RNGLRCommon and RNGLRParser. Using any tool you have to obtain a sequence of tokens. You can create AST from this sequence using command
</p>
<pre>
&lt;parserModule&gt;.buildAst tokens
</pre>
, where <code>&lt;parserModule&gt;</code> - name of generated module.

<h3>Translation</h3>
Suppose you have AST named tree. Then you can perform semantic actions on it. This is allowed by command:
<pre>
&lt;parserModule&gt;.translate args tree : &lt;result_type&gt;
</pre>
, where args instantiate a record type (TranslateArguments) with following fields:
<ul>
<li>
<pre>
tokenToRange : Token -> Position * Position
</pre>
Function, allowing for each token to count it's position in source file. Positions of all non-terminals and epsilon-trees are derived from these values.
</li>
<li>
<pre>
zeroPosition : Position
</pre>
But input sequence may be empty => there is no tokens => we don't know any position. In this case zeroPosition allows to translate resultin epsilon-tree, where both start and end positions for all nonterminals equals to zeroPosition.
</li>
<li>
<pre>
clearAST : boolean
</pre>
Memory optimization feature. If it's true, then during translation initial tree will be destroyed. In can reduce memory usage during translation.
</li>
<li>
<pre>
filterEpsilons
</pre>
If it is true, used epsilon-trees will produce only one tree for each. If you want to have all results (with all epsilon-trees values), you are to set it to false.
</li>
</ul>

<h2>Useful Features</h2>
Suppose we have AST (named ast). Then we can perform following actions:
<ul>
<li>
<pre>
<parserModule>.defaultAstToDot tree <filename>
</pre>
Print generated tree in dot format. Then it can be translated in visual format using GraphViz. After installing you can run, e.g.
</li>
<pre>
dot <dotFileName> -Tsvg -O
</pre>
Result svg-file can be viewed in browser. All nonterminals with ambiguities are red-colored and contains '!' character for easier search with Ctrl-F.
<li>
<pre>
tree.PrintAst()
</pre>
Immediately print tree in readable text format. If tree contains cycles, StackOverflowException occurs.
</li>
<li>
<pre>
tree.collectWarnings tokenToRange
</pre>
tokenToRange - previously described function (Token -> Position * Position). It traverses AST and for each ambiguity prints:
<ul>
<li>Its range.</li>
<li>All rules, it can be derived with.</li>
</ul>

</li>
<li>
<pre>
tree.ChooseSingleAst()
</pre>
Select one tree (kills all cycles and ambiguities). It's unknown, what exactly. Looks like something near longest match, but definitely isn't it in general.
</li>
<li>
<pre>
tree.ChooseLongestMatch()
</pre>
Also, select one tree. But now it's guaranted that result corresponds the longest match.
</li>
<li>
<pre>
tree.EliminateCycles()
</pre>
Delete cycles from tree. It allows to translate tree. Also used in generation to allow writing epsilon-tree structurally.
</li>
</ul>

<h2>Error recovery</h2>

<h3>Algorithm</h3>
Parser has a mechanism for error recovery. The mechanism is based on a strategy of error token (more about).

In short, when error happens then error token is inserted in front of the error detection point. The parser will pop states from the parse stack until this token becomes valid, and then skip symbols from the input until an acceptable symbol is found.

<h3>Rule with error token in a grammar</h3>
You should add error token in grammar as simple rule:
<pre>
stmt: firstRule 
    | secondRule
    | ...
    | error
</pre>
By the way, the error token is always defined.

<h3>Info about errors</h3>
When you got the tree (named ast), you can learn about all errors which occur during parsing by command

<pre>
ast.collectErrors tokenToRange
</pre>
where <tt>tokenToRange</tt> - previously described function (Token -> Position * Position).

This function traverses AST and returns the array of triples:
<ul>
<li>Start position of unparsed exression.
</li>
<li>End position of unparsed expression.
</li>
<li>Array of skipped tokens.
</li>
</ul>

<h3>Semantic for error node</h3>
You can add semantic for error node.

For instance:
<pre>
stmt: res=someRule {<some actions with res>} 
    | e=error {printfn "Error on %A token. Expected %A token(s)" e.errorOn e.expected}
</pre>
Now error node has such fields as:
<ul>
<li>
<tt>errorOn</tt>
It's token in which error occurs
</li>
<li>
<tt>expected</tt>
Parser was expecting one of these tokens. Example ariphmetic expression "1 2;" After "1" parser was expecting such symbols as "+", "-", "", "/" etc.
</li>
<li>

<tt>tokens</tt>
Skipped tokens during error recovery.
</li>
<li>

<tt>recTokens</tt>
Parser was looking for one of these tokens during recovery. Example: ariphmetic expression with rule:
<pre>
stmt -> error ;
</pre>
and "1 2 ;" as input. The recovery token in this case is ";".
</li>
</ul>

*)