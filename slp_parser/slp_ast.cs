using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using slp_parser;

namespace akira
{
    public class AkiraParser
    {
        public static Node Parse(string code)
        {
            slpLexer lexer;
            slpParser parser;
            Listener listener;

            AntlrInputStream input = new AntlrInputStream(code);
            lexer = new slpLexer(input);
            CommonTokenStream tokens = new CommonTokenStream(lexer);

            parser = new slpParser(tokens);
            IParseTree tree = parser.program();

            ParseTreeWalker walker = new ParseTreeWalker();
            listener = new Listener();
            walker.Walk(listener, tree);

            //Node elem = TreeToNode(null, tree);
            Node elem = listener.program;
            
            return elem;
        }
    }
}
