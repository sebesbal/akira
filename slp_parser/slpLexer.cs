//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.5
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from slp.g4 by ANTLR 4.5

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591


	using System.Collections.Generic;
	using System.Text.RegularExpressions;

using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.5")]
[System.CLSCompliant(false)]
public partial class slpLexer : Lexer {
	public const int
		T__0=1, T__1=2, EXPORT=3, CLASS=4, TRUE=5, FALSE=6, NULL=7, STRING=8, 
		INT=9, FLOAT=10, ID=11, OP=12, NATIVE=13, COMMENT=14, LINE_COMMENT=15, 
		SPACES=16, OPEN_PAREN=17, CLOSE_PAREN=18, OPEN_BRACK=19, CLOSE_BRACK=20, 
		OPEN_BRACE=21, CLOSE_BRACE=22, NEWLINE=23, WS=24;
	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"T__0", "T__1", "EXPORT", "CLASS", "TRUE", "FALSE", "NULL", "STRING", 
		"INT", "FLOAT", "ID", "OP", "NATIVE", "COMMENT", "LINE_COMMENT", "SPACES", 
		"OPEN_PAREN", "CLOSE_PAREN", "OPEN_BRACK", "CLOSE_BRACK", "OPEN_BRACE", 
		"CLOSE_BRACE", "NEWLINE", "WS"
	};


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
	  


	public slpLexer(ICharStream input)
		: base(input)
	{
		Interpreter = new LexerATNSimulator(this,_ATN);
	}

	private static readonly string[] _LiteralNames = {
		null, "'{'", "'}'", "'export'", "'class'", "'true'", "'false'", "'<>'", 
		null, null, null, null, null, null, null, null, null, "'('", "')'", "'['", 
		"']'", "'<'", "'>'"
	};
	private static readonly string[] _SymbolicNames = {
		null, null, null, "EXPORT", "CLASS", "TRUE", "FALSE", "NULL", "STRING", 
		"INT", "FLOAT", "ID", "OP", "NATIVE", "COMMENT", "LINE_COMMENT", "SPACES", 
		"OPEN_PAREN", "CLOSE_PAREN", "OPEN_BRACK", "CLOSE_BRACK", "OPEN_BRACE", 
		"CLOSE_BRACE", "NEWLINE", "WS"
	};
	public static readonly IVocabulary DefaultVocabulary = new Vocabulary(_LiteralNames, _SymbolicNames);

	[NotNull]
	public override IVocabulary Vocabulary
	{
		get
		{
			return DefaultVocabulary;
		}
	}

	public override string GrammarFileName { get { return "slp.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override string[] ModeNames { get { return modeNames; } }

	public override string SerializedAtn { get { return _serializedATN; } }

	public override void Action(RuleContext _localctx, int ruleIndex, int actionIndex) {
		switch (ruleIndex) {
		case 16 : OPEN_PAREN_action(_localctx, actionIndex); break;
		case 17 : CLOSE_PAREN_action(_localctx, actionIndex); break;
		case 18 : OPEN_BRACK_action(_localctx, actionIndex); break;
		case 19 : CLOSE_BRACK_action(_localctx, actionIndex); break;
		case 20 : OPEN_BRACE_action(_localctx, actionIndex); break;
		case 21 : CLOSE_BRACE_action(_localctx, actionIndex); break;
		case 22 : NEWLINE_action(_localctx, actionIndex); break;
		}
	}
	private void OPEN_PAREN_action(RuleContext _localctx, int actionIndex) {
		switch (actionIndex) {
		case 0: opened++; break;
		}
	}
	private void CLOSE_PAREN_action(RuleContext _localctx, int actionIndex) {
		switch (actionIndex) {
		case 1: opened--; break;
		}
	}
	private void OPEN_BRACK_action(RuleContext _localctx, int actionIndex) {
		switch (actionIndex) {
		case 2: opened++; break;
		}
	}
	private void CLOSE_BRACK_action(RuleContext _localctx, int actionIndex) {
		switch (actionIndex) {
		case 3: opened--; break;
		}
	}
	private void OPEN_BRACE_action(RuleContext _localctx, int actionIndex) {
		switch (actionIndex) {
		case 4: opened++; break;
		}
	}
	private void CLOSE_BRACE_action(RuleContext _localctx, int actionIndex) {
		switch (actionIndex) {
		case 5: opened--; break;
		}
	}
	private void NEWLINE_action(RuleContext _localctx, int actionIndex) {
		switch (actionIndex) {
		case 6: 
		     string newLine = Regex.Replace(Text, "[^\r\n]", "");
		     string spaces = Regex.Replace(Text, "[\r\n]", "");
		     int next = InputStream.La(1);
		     if (opened > 0 || next == '\r' || next == '\n' || next == '#') {
		       // If we're inside a list or on a blank line, ignore all indents, 
		       // dedents and line breaks.
		       Skip();
		     }
		     else {
		       Emit(commonToken(NEWLINE, newLine));
		       int indent = getIndentationCount(spaces);
		       int previous = indents.Count == 0 ? 0 : indents.Peek();
		       if (indent == previous) {
		         // skip indents of the same size as the present indent-size
		         Skip();
		       }
		       else if (indent > previous) {
		         indents.Push(indent);
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
		    break;
		}
	}
	public override bool Sempred(RuleContext _localctx, int ruleIndex, int predIndex) {
		switch (ruleIndex) {
		case 22 : return NEWLINE_sempred(_localctx, predIndex);
		}
		return true;
	}
	private bool NEWLINE_sempred(RuleContext _localctx, int predIndex) {
		switch (predIndex) {
		case 0: return atStartOfInput();
		}
		return true;
	}

	public static readonly string _serializedATN =
		"\x3\x430\xD6D1\x8206\xAD2D\x4417\xAEF1\x8D80\xAADD\x2\x1A\xC4\b\x1\x4"+
		"\x2\t\x2\x4\x3\t\x3\x4\x4\t\x4\x4\x5\t\x5\x4\x6\t\x6\x4\a\t\a\x4\b\t\b"+
		"\x4\t\t\t\x4\n\t\n\x4\v\t\v\x4\f\t\f\x4\r\t\r\x4\xE\t\xE\x4\xF\t\xF\x4"+
		"\x10\t\x10\x4\x11\t\x11\x4\x12\t\x12\x4\x13\t\x13\x4\x14\t\x14\x4\x15"+
		"\t\x15\x4\x16\t\x16\x4\x17\t\x17\x4\x18\t\x18\x4\x19\t\x19\x3\x2\x3\x2"+
		"\x3\x3\x3\x3\x3\x4\x3\x4\x3\x4\x3\x4\x3\x4\x3\x4\x3\x4\x3\x5\x3\x5\x3"+
		"\x5\x3\x5\x3\x5\x3\x5\x3\x6\x3\x6\x3\x6\x3\x6\x3\x6\x3\a\x3\a\x3\a\x3"+
		"\a\x3\a\x3\a\x3\b\x3\b\x3\b\x3\t\x3\t\a\tU\n\t\f\t\xE\tX\v\t\x3\t\x3\t"+
		"\x3\n\x6\n]\n\n\r\n\xE\n^\x3\v\x3\v\x3\v\x3\v\x3\f\x6\f\x66\n\f\r\f\xE"+
		"\fg\x3\r\x6\rk\n\r\r\r\xE\rl\x3\xE\x3\xE\x3\xE\a\xEr\n\xE\f\xE\xE\xEu"+
		"\v\xE\x3\xE\x3\xE\x3\xF\x3\xF\x3\xF\x3\xF\a\xF}\n\xF\f\xF\xE\xF\x80\v"+
		"\xF\x3\xF\x3\xF\x3\xF\x3\xF\x3\xF\x3\x10\x3\x10\x3\x10\x3\x10\a\x10\x8B"+
		"\n\x10\f\x10\xE\x10\x8E\v\x10\x3\x10\x5\x10\x91\n\x10\x3\x10\x3\x10\x3"+
		"\x10\x3\x10\x3\x11\x6\x11\x98\n\x11\r\x11\xE\x11\x99\x3\x12\x3\x12\x3"+
		"\x12\x3\x13\x3\x13\x3\x13\x3\x14\x3\x14\x3\x14\x3\x15\x3\x15\x3\x15\x3"+
		"\x16\x3\x16\x3\x16\x3\x17\x3\x17\x3\x17\x3\x18\x3\x18\x3\x18\x5\x18\xB1"+
		"\n\x18\x3\x18\x3\x18\x5\x18\xB5\n\x18\x3\x18\x5\x18\xB8\n\x18\x5\x18\xBA"+
		"\n\x18\x3\x18\x3\x18\x3\x19\x6\x19\xBF\n\x19\r\x19\xE\x19\xC0\x3\x19\x3"+
		"\x19\x3~\x2\x1A\x3\x3\x5\x4\a\x5\t\x6\v\a\r\b\xF\t\x11\n\x13\v\x15\f\x17"+
		"\r\x19\xE\x1B\xF\x1D\x10\x1F\x11!\x12#\x13%\x14\'\x15)\x16+\x17-\x18/"+
		"\x19\x31\x1A\x3\x2\t\x3\x2$$\x6\x2\x32;\x43\\\x61\x61\x63|\f\x2##%%\'"+
		"(,-//\x31\x31??\x41\x42~~\x80\x80\x4\x2}}\x7F\x7F\x4\x2\f\f\xF\xF\x4\x2"+
		"\v\v\"\"\x5\x2\v\f\xE\xF\"\"\xD2\x2\x3\x3\x2\x2\x2\x2\x5\x3\x2\x2\x2\x2"+
		"\a\x3\x2\x2\x2\x2\t\x3\x2\x2\x2\x2\v\x3\x2\x2\x2\x2\r\x3\x2\x2\x2\x2\xF"+
		"\x3\x2\x2\x2\x2\x11\x3\x2\x2\x2\x2\x13\x3\x2\x2\x2\x2\x15\x3\x2\x2\x2"+
		"\x2\x17\x3\x2\x2\x2\x2\x19\x3\x2\x2\x2\x2\x1B\x3\x2\x2\x2\x2\x1D\x3\x2"+
		"\x2\x2\x2\x1F\x3\x2\x2\x2\x2!\x3\x2\x2\x2\x2#\x3\x2\x2\x2\x2%\x3\x2\x2"+
		"\x2\x2\'\x3\x2\x2\x2\x2)\x3\x2\x2\x2\x2+\x3\x2\x2\x2\x2-\x3\x2\x2\x2\x2"+
		"/\x3\x2\x2\x2\x2\x31\x3\x2\x2\x2\x3\x33\x3\x2\x2\x2\x5\x35\x3\x2\x2\x2"+
		"\a\x37\x3\x2\x2\x2\t>\x3\x2\x2\x2\v\x44\x3\x2\x2\x2\rI\x3\x2\x2\x2\xF"+
		"O\x3\x2\x2\x2\x11R\x3\x2\x2\x2\x13\\\x3\x2\x2\x2\x15`\x3\x2\x2\x2\x17"+
		"\x65\x3\x2\x2\x2\x19j\x3\x2\x2\x2\x1Bn\x3\x2\x2\x2\x1Dx\x3\x2\x2\x2\x1F"+
		"\x86\x3\x2\x2\x2!\x97\x3\x2\x2\x2#\x9B\x3\x2\x2\x2%\x9E\x3\x2\x2\x2\'"+
		"\xA1\x3\x2\x2\x2)\xA4\x3\x2\x2\x2+\xA7\x3\x2\x2\x2-\xAA\x3\x2\x2\x2/\xB9"+
		"\x3\x2\x2\x2\x31\xBE\x3\x2\x2\x2\x33\x34\a}\x2\x2\x34\x4\x3\x2\x2\x2\x35"+
		"\x36\a\x7F\x2\x2\x36\x6\x3\x2\x2\x2\x37\x38\ag\x2\x2\x38\x39\az\x2\x2"+
		"\x39:\ar\x2\x2:;\aq\x2\x2;<\at\x2\x2<=\av\x2\x2=\b\x3\x2\x2\x2>?\a\x65"+
		"\x2\x2?@\an\x2\x2@\x41\a\x63\x2\x2\x41\x42\au\x2\x2\x42\x43\au\x2\x2\x43"+
		"\n\x3\x2\x2\x2\x44\x45\av\x2\x2\x45\x46\at\x2\x2\x46G\aw\x2\x2GH\ag\x2"+
		"\x2H\f\x3\x2\x2\x2IJ\ah\x2\x2JK\a\x63\x2\x2KL\an\x2\x2LM\au\x2\x2MN\a"+
		"g\x2\x2N\xE\x3\x2\x2\x2OP\a>\x2\x2PQ\a@\x2\x2Q\x10\x3\x2\x2\x2RV\a$\x2"+
		"\x2SU\n\x2\x2\x2TS\x3\x2\x2\x2UX\x3\x2\x2\x2VT\x3\x2\x2\x2VW\x3\x2\x2"+
		"\x2WY\x3\x2\x2\x2XV\x3\x2\x2\x2YZ\a$\x2\x2Z\x12\x3\x2\x2\x2[]\x4\x32;"+
		"\x2\\[\x3\x2\x2\x2]^\x3\x2\x2\x2^\\\x3\x2\x2\x2^_\x3\x2\x2\x2_\x14\x3"+
		"\x2\x2\x2`\x61\x5\x13\n\x2\x61\x62\a\x30\x2\x2\x62\x63\x5\x13\n\x2\x63"+
		"\x16\x3\x2\x2\x2\x64\x66\t\x3\x2\x2\x65\x64\x3\x2\x2\x2\x66g\x3\x2\x2"+
		"\x2g\x65\x3\x2\x2\x2gh\x3\x2\x2\x2h\x18\x3\x2\x2\x2ik\t\x4\x2\x2ji\x3"+
		"\x2\x2\x2kl\x3\x2\x2\x2lj\x3\x2\x2\x2lm\x3\x2\x2\x2m\x1A\x3\x2\x2\x2n"+
		"s\a}\x2\x2or\x5\x1B\xE\x2pr\n\x5\x2\x2qo\x3\x2\x2\x2qp\x3\x2\x2\x2ru\x3"+
		"\x2\x2\x2sq\x3\x2\x2\x2st\x3\x2\x2\x2tv\x3\x2\x2\x2us\x3\x2\x2\x2vw\a"+
		"\x7F\x2\x2w\x1C\x3\x2\x2\x2xy\a\x31\x2\x2yz\a,\x2\x2z~\x3\x2\x2\x2{}\v"+
		"\x2\x2\x2|{\x3\x2\x2\x2}\x80\x3\x2\x2\x2~\x7F\x3\x2\x2\x2~|\x3\x2\x2\x2"+
		"\x7F\x81\x3\x2\x2\x2\x80~\x3\x2\x2\x2\x81\x82\a,\x2\x2\x82\x83\a\x31\x2"+
		"\x2\x83\x84\x3\x2\x2\x2\x84\x85\b\xF\x2\x2\x85\x1E\x3\x2\x2\x2\x86\x87"+
		"\a\x31\x2\x2\x87\x88\a\x31\x2\x2\x88\x8C\x3\x2\x2\x2\x89\x8B\n\x6\x2\x2"+
		"\x8A\x89\x3\x2\x2\x2\x8B\x8E\x3\x2\x2\x2\x8C\x8A\x3\x2\x2\x2\x8C\x8D\x3"+
		"\x2\x2\x2\x8D\x90\x3\x2\x2\x2\x8E\x8C\x3\x2\x2\x2\x8F\x91\a\xF\x2\x2\x90"+
		"\x8F\x3\x2\x2\x2\x90\x91\x3\x2\x2\x2\x91\x92\x3\x2\x2\x2\x92\x93\a\f\x2"+
		"\x2\x93\x94\x3\x2\x2\x2\x94\x95\b\x10\x2\x2\x95 \x3\x2\x2\x2\x96\x98\t"+
		"\a\x2\x2\x97\x96\x3\x2\x2\x2\x98\x99\x3\x2\x2\x2\x99\x97\x3\x2\x2\x2\x99"+
		"\x9A\x3\x2\x2\x2\x9A\"\x3\x2\x2\x2\x9B\x9C\a*\x2\x2\x9C\x9D\b\x12\x3\x2"+
		"\x9D$\x3\x2\x2\x2\x9E\x9F\a+\x2\x2\x9F\xA0\b\x13\x4\x2\xA0&\x3\x2\x2\x2"+
		"\xA1\xA2\a]\x2\x2\xA2\xA3\b\x14\x5\x2\xA3(\x3\x2\x2\x2\xA4\xA5\a_\x2\x2"+
		"\xA5\xA6\b\x15\x6\x2\xA6*\x3\x2\x2\x2\xA7\xA8\a>\x2\x2\xA8\xA9\b\x16\a"+
		"\x2\xA9,\x3\x2\x2\x2\xAA\xAB\a@\x2\x2\xAB\xAC\b\x17\b\x2\xAC.\x3\x2\x2"+
		"\x2\xAD\xAE\x6\x18\x2\x2\xAE\xBA\x5!\x11\x2\xAF\xB1\a\xF\x2\x2\xB0\xAF"+
		"\x3\x2\x2\x2\xB0\xB1\x3\x2\x2\x2\xB1\xB2\x3\x2\x2\x2\xB2\xB5\a\f\x2\x2"+
		"\xB3\xB5\a\xF\x2\x2\xB4\xB0\x3\x2\x2\x2\xB4\xB3\x3\x2\x2\x2\xB5\xB7\x3"+
		"\x2\x2\x2\xB6\xB8\x5!\x11\x2\xB7\xB6\x3\x2\x2\x2\xB7\xB8\x3\x2\x2\x2\xB8"+
		"\xBA\x3\x2\x2\x2\xB9\xAD\x3\x2\x2\x2\xB9\xB4\x3\x2\x2\x2\xBA\xBB\x3\x2"+
		"\x2\x2\xBB\xBC\b\x18\t\x2\xBC\x30\x3\x2\x2\x2\xBD\xBF\t\b\x2\x2\xBE\xBD"+
		"\x3\x2\x2\x2\xBF\xC0\x3\x2\x2\x2\xC0\xBE\x3\x2\x2\x2\xC0\xC1\x3\x2\x2"+
		"\x2\xC1\xC2\x3\x2\x2\x2\xC2\xC3\b\x19\x2\x2\xC3\x32\x3\x2\x2\x2\x12\x2"+
		"V^glqs~\x8C\x90\x99\xB0\xB4\xB7\xB9\xC0\n\x2\x3\x2\x3\x12\x2\x3\x13\x3"+
		"\x3\x14\x4\x3\x15\x5\x3\x16\x6\x3\x17\a\x3\x18\b";
	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN.ToCharArray());
}
