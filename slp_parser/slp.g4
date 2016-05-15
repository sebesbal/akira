grammar slp;

@lexer::header {
	using System.Collections.Generic;
	using System.Text.RegularExpressions;
}

tokens { INDENT, DEDENT }

@lexer::members {
	// The amount of opened braces, brackets and parenthesis.
	private int opened = 0;
	private Stack<int> indents = new Stack<int>();
	private IToken lastToken = null;
	private List<IToken> tokens = new List<IToken>();
	
	static int getIndentationCount(String spaces) {
    int count = 0;
    foreach (char ch in spaces) {
      switch (ch) {
        case '\t':
          count += 8 - (count % 8);
          break;
        default:
          // A normal space char.
          count++;
		  break;
      }
    }

    return count;
  }
  
  private CommonToken commonToken(int type, String text) {
    int stop = this.CharIndex - 1;
    int start = string.IsNullOrEmpty(text) ? stop : stop - text.Length + 1;
    return new CommonToken(this._tokenFactorySourcePair, type, DefaultTokenChannel, start, stop);
  }
  
  private IToken createDedent() {
    //CommonToken dedent = commonToken(slpParser.DEDENT, "");
	CommonToken dedent = commonToken(slpParser.CLOSE_PAREN, "");
    dedent.Line = this.lastToken.Line;
    return dedent;
  }
  
  bool atStartOfInput() {
    return Column == 0 && Line == 1;
  }
  
  override public void Emit(IToken t) {
    Token = t;
    tokens.Add(t);
  }
  
  override public IToken NextToken() {
    // Check if the end-of-file is ahead and there are still some DEDENTS expected.
    if (InputStream.La(1) == Eof && this.indents.Count > 0) {
      // Remove any trailing EOF tokens from our buffer.
      for (int i = tokens.Count - 1; i >= 0; i--) {
        if (tokens[i].Type == Eof) {
          tokens.RemoveAt(i);
        }
      }

      // First emit an extra line break that serves as the end of the statement.
      this.Emit(commonToken(slpParser.NEWLINE, "\n"));

      // Now emit as much DEDENT tokens as needed.
      while (indents.Count > 0) {
        this.Emit(createDedent());
        indents.Pop();
      }

      // Put the EOF back on the token stream.
      this.Emit(commonToken(slpParser.Eof, "<EOF>"));
    }

    IToken next = base.NextToken();

    if (next.Channel == DefaultTokenChannel) {
      // Keep track of the last token on the default channel.
      this.lastToken = next;
	}

	if (tokens.Count == 0) return next;
	var first = tokens[0];
	tokens.RemoveAt(0);
    return first;
  }
  
}

TRUE
    :   'true'
    ;

FALSE
    :   'false'
    ;

NULL
    :   '<>'
    ;

STRING
    :   '"' ( ~["] )* '"'
    ;

INT
    :   '0'..'9'+
    ;

FLOAT
    :   INT '.' INT
    ;

ID  
    :   ('a'..'z'|'A'..'Z'|'0'..'9'|'_')+
    ;
	
OP
	:   (':'|'+'|'*'|'/'|'='|'-'|'!'|'?'|'|'|'~'|'%'|'&'|'#'|'@')+
	;

NATIVE
    :   '{' ( NATIVE | ~[{}] )* '}'
    ;

COMMENT
    :   '/*' .*? '*/'    -> channel(HIDDEN) // match anything between /* and */
    ;

LINE_COMMENT
    : '//' ~[\r\n]* '\r'? '\n' -> channel(HIDDEN)
    ;

SPACES
 : [ \t]+  -> channel(HIDDEN)
 ;

// TREE : ':';
OPEN_PAREN : '(' {opened++;};
CLOSE_PAREN : ')' {opened--;};
OPEN_BRACK : '[' {opened++;};
CLOSE_BRACK : ']' {opened--;};
OPEN_BRACE : '<' {opened++;};
CLOSE_BRACE : '>' {opened--;};

NEWLINE
 : ( {atStartOfInput()}?   SPACES
   | ( '\r'? '\n' | '\r' )+ SPACES?
   )
   {
     string newLine = Regex.Replace(Text, "[^\r\n]", "");
     string spaces = Regex.Replace(Text, "[\r\n]", "");
     int next = InputStream.La(1);
     if (opened > 0 || next == '\r' || next == '\n' ||next == '#') {
       // If we're inside a list or on a blank line, ignore all indents, 
       // dedents and line breaks.
       Skip();
     }
     else {
       // Emit(commonToken(NEWLINE, newLine));
       int indent = getIndentationCount(spaces);
       int previous = indents.Count == 0 ? 0 : indents.Peek();
       if (indent == previous) {
         // skip indents of the same size as the present indent-size
         Skip();
       }
       else if (indent > previous) {
         indents.Push(indent);
		 Emit(new CommonToken(slpParser.OP, ":"));
		 Emit(commonToken(slpParser.OPEN_PAREN, spaces));
       }
       else {
         // Possibly emit more than 1 DEDENT token.
         while(indents.Count > 0 && indents.Peek() > indent) {
           Emit(createDedent());
           indents.Pop();
         }
       }
     }
   };

WS  :   [ \r\t\u000C\n]+ -> channel(HIDDEN)
	;

program
	:	exp
	;
   
assoc
	:	'fx' | 'fy' | 'xf' | 'yf' | 'xfx' | 'xfy' | 'yfx' | 'yfy'
	;
   
opdef
	// :	'op' '(' INT ',' assoc ',' OP ')' // NEWLINE
	:	'op' INT assoc OP // NEWLINE
	;
   
token
    :   INT       
    |   FLOAT
	|   STRING
	|   TRUE
	|   FALSE
	|   ID
	|	OP
	|	NEWLINE
	|	NATIVE
	;
	
exp
	: (block | token | opdef | NEWLINE)+
	;

block
	:	'(' exp ')'
	|	'{' exp '}'
	|	'[' exp ']'
    ;
