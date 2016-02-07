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
		T__0=1, T__1=2, T__2=3, T__3=4, T__4=5, T__5=6, T__6=7, T__7=8, T__8=9, 
		T__9=10, T__10=11, T__11=12, TRUE=13, FALSE=14, NULL=15, STRING=16, INT=17, 
		FLOAT=18, ID=19, OP=20, NATIVE=21, COMMENT=22, LINE_COMMENT=23, SPACES=24, 
		TREE=25, OPEN_PAREN=26, CLOSE_PAREN=27, OPEN_BRACK=28, CLOSE_BRACK=29, 
		OPEN_BRACE=30, CLOSE_BRACE=31, NEWLINE=32, WS=33;
	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"T__0", "T__1", "T__2", "T__3", "T__4", "T__5", "T__6", "T__7", "T__8", 
		"T__9", "T__10", "T__11", "TRUE", "FALSE", "NULL", "STRING", "INT", "FLOAT", 
		"ID", "OP", "NATIVE", "COMMENT", "LINE_COMMENT", "SPACES", "TREE", "OPEN_PAREN", 
		"CLOSE_PAREN", "OPEN_BRACK", "CLOSE_BRACK", "OPEN_BRACE", "CLOSE_BRACE", 
		"NEWLINE", "WS"
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
		null, "'fx'", "'fy'", "'xf'", "'yf'", "'xfx'", "'xfy'", "'yfx'", "'yfy'", 
		"'op'", "','", "'{'", "'}'", "'true'", "'false'", "'<>'", null, null, 
		null, null, null, null, null, null, null, "':'", "'('", "')'", "'['", 
		"']'", "'<'", "'>'"
	};
	private static readonly string[] _SymbolicNames = {
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, "TRUE", "FALSE", "NULL", "STRING", "INT", "FLOAT", "ID", "OP", "NATIVE", 
		"COMMENT", "LINE_COMMENT", "SPACES", "TREE", "OPEN_PAREN", "CLOSE_PAREN", 
		"OPEN_BRACK", "CLOSE_BRACK", "OPEN_BRACE", "CLOSE_BRACE", "NEWLINE", "WS"
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
		case 25 : OPEN_PAREN_action(_localctx, actionIndex); break;
		case 26 : CLOSE_PAREN_action(_localctx, actionIndex); break;
		case 27 : OPEN_BRACK_action(_localctx, actionIndex); break;
		case 28 : CLOSE_BRACK_action(_localctx, actionIndex); break;
		case 29 : OPEN_BRACE_action(_localctx, actionIndex); break;
		case 30 : CLOSE_BRACE_action(_localctx, actionIndex); break;
		case 31 : NEWLINE_action(_localctx, actionIndex); break;
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
				 Emit(commonToken(slpParser.TREE, ":"));
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
		case 31 : return NEWLINE_sempred(_localctx, predIndex);
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
		"\x3\x430\xD6D1\x8206\xAD2D\x4417\xAEF1\x8D80\xAADD\x2#\xEC\b\x1\x4\x2"+
		"\t\x2\x4\x3\t\x3\x4\x4\t\x4\x4\x5\t\x5\x4\x6\t\x6\x4\a\t\a\x4\b\t\b\x4"+
		"\t\t\t\x4\n\t\n\x4\v\t\v\x4\f\t\f\x4\r\t\r\x4\xE\t\xE\x4\xF\t\xF\x4\x10"+
		"\t\x10\x4\x11\t\x11\x4\x12\t\x12\x4\x13\t\x13\x4\x14\t\x14\x4\x15\t\x15"+
		"\x4\x16\t\x16\x4\x17\t\x17\x4\x18\t\x18\x4\x19\t\x19\x4\x1A\t\x1A\x4\x1B"+
		"\t\x1B\x4\x1C\t\x1C\x4\x1D\t\x1D\x4\x1E\t\x1E\x4\x1F\t\x1F\x4 \t \x4!"+
		"\t!\x4\"\t\"\x3\x2\x3\x2\x3\x2\x3\x3\x3\x3\x3\x3\x3\x4\x3\x4\x3\x4\x3"+
		"\x5\x3\x5\x3\x5\x3\x6\x3\x6\x3\x6\x3\x6\x3\a\x3\a\x3\a\x3\a\x3\b\x3\b"+
		"\x3\b\x3\b\x3\t\x3\t\x3\t\x3\t\x3\n\x3\n\x3\n\x3\v\x3\v\x3\f\x3\f\x3\r"+
		"\x3\r\x3\xE\x3\xE\x3\xE\x3\xE\x3\xE\x3\xF\x3\xF\x3\xF\x3\xF\x3\xF\x3\xF"+
		"\x3\x10\x3\x10\x3\x10\x3\x11\x3\x11\a\x11{\n\x11\f\x11\xE\x11~\v\x11\x3"+
		"\x11\x3\x11\x3\x12\x6\x12\x83\n\x12\r\x12\xE\x12\x84\x3\x13\x3\x13\x3"+
		"\x13\x3\x13\x3\x14\x6\x14\x8C\n\x14\r\x14\xE\x14\x8D\x3\x15\x6\x15\x91"+
		"\n\x15\r\x15\xE\x15\x92\x3\x16\x3\x16\x3\x16\a\x16\x98\n\x16\f\x16\xE"+
		"\x16\x9B\v\x16\x3\x16\x3\x16\x3\x17\x3\x17\x3\x17\x3\x17\a\x17\xA3\n\x17"+
		"\f\x17\xE\x17\xA6\v\x17\x3\x17\x3\x17\x3\x17\x3\x17\x3\x17\x3\x18\x3\x18"+
		"\x3\x18\x3\x18\a\x18\xB1\n\x18\f\x18\xE\x18\xB4\v\x18\x3\x18\x5\x18\xB7"+
		"\n\x18\x3\x18\x3\x18\x3\x18\x3\x18\x3\x19\x6\x19\xBE\n\x19\r\x19\xE\x19"+
		"\xBF\x3\x1A\x3\x1A\x3\x1B\x3\x1B\x3\x1B\x3\x1C\x3\x1C\x3\x1C\x3\x1D\x3"+
		"\x1D\x3\x1D\x3\x1E\x3\x1E\x3\x1E\x3\x1F\x3\x1F\x3\x1F\x3 \x3 \x3 \x3!"+
		"\x3!\x3!\x5!\xD9\n!\x3!\x3!\x5!\xDD\n!\x3!\x5!\xE0\n!\x5!\xE2\n!\x3!\x3"+
		"!\x3\"\x6\"\xE7\n\"\r\"\xE\"\xE8\x3\"\x3\"\x3\xA4\x2#\x3\x3\x5\x4\a\x5"+
		"\t\x6\v\a\r\b\xF\t\x11\n\x13\v\x15\f\x17\r\x19\xE\x1B\xF\x1D\x10\x1F\x11"+
		"!\x12#\x13%\x14\'\x15)\x16+\x17-\x18/\x19\x31\x1A\x33\x1B\x35\x1C\x37"+
		"\x1D\x39\x1E;\x1F= ?!\x41\"\x43#\x3\x2\t\x3\x2$$\x6\x2\x32;\x43\\\x61"+
		"\x61\x63|\f\x2##%%\'(,-//\x31\x31??\x41\x42~~\x80\x80\x4\x2}}\x7F\x7F"+
		"\x4\x2\f\f\xF\xF\x4\x2\v\v\"\"\x5\x2\v\f\xE\xF\"\"\xFA\x2\x3\x3\x2\x2"+
		"\x2\x2\x5\x3\x2\x2\x2\x2\a\x3\x2\x2\x2\x2\t\x3\x2\x2\x2\x2\v\x3\x2\x2"+
		"\x2\x2\r\x3\x2\x2\x2\x2\xF\x3\x2\x2\x2\x2\x11\x3\x2\x2\x2\x2\x13\x3\x2"+
		"\x2\x2\x2\x15\x3\x2\x2\x2\x2\x17\x3\x2\x2\x2\x2\x19\x3\x2\x2\x2\x2\x1B"+
		"\x3\x2\x2\x2\x2\x1D\x3\x2\x2\x2\x2\x1F\x3\x2\x2\x2\x2!\x3\x2\x2\x2\x2"+
		"#\x3\x2\x2\x2\x2%\x3\x2\x2\x2\x2\'\x3\x2\x2\x2\x2)\x3\x2\x2\x2\x2+\x3"+
		"\x2\x2\x2\x2-\x3\x2\x2\x2\x2/\x3\x2\x2\x2\x2\x31\x3\x2\x2\x2\x2\x33\x3"+
		"\x2\x2\x2\x2\x35\x3\x2\x2\x2\x2\x37\x3\x2\x2\x2\x2\x39\x3\x2\x2\x2\x2"+
		";\x3\x2\x2\x2\x2=\x3\x2\x2\x2\x2?\x3\x2\x2\x2\x2\x41\x3\x2\x2\x2\x2\x43"+
		"\x3\x2\x2\x2\x3\x45\x3\x2\x2\x2\x5H\x3\x2\x2\x2\aK\x3\x2\x2\x2\tN\x3\x2"+
		"\x2\x2\vQ\x3\x2\x2\x2\rU\x3\x2\x2\x2\xFY\x3\x2\x2\x2\x11]\x3\x2\x2\x2"+
		"\x13\x61\x3\x2\x2\x2\x15\x64\x3\x2\x2\x2\x17\x66\x3\x2\x2\x2\x19h\x3\x2"+
		"\x2\x2\x1Bj\x3\x2\x2\x2\x1Do\x3\x2\x2\x2\x1Fu\x3\x2\x2\x2!x\x3\x2\x2\x2"+
		"#\x82\x3\x2\x2\x2%\x86\x3\x2\x2\x2\'\x8B\x3\x2\x2\x2)\x90\x3\x2\x2\x2"+
		"+\x94\x3\x2\x2\x2-\x9E\x3\x2\x2\x2/\xAC\x3\x2\x2\x2\x31\xBD\x3\x2\x2\x2"+
		"\x33\xC1\x3\x2\x2\x2\x35\xC3\x3\x2\x2\x2\x37\xC6\x3\x2\x2\x2\x39\xC9\x3"+
		"\x2\x2\x2;\xCC\x3\x2\x2\x2=\xCF\x3\x2\x2\x2?\xD2\x3\x2\x2\x2\x41\xE1\x3"+
		"\x2\x2\x2\x43\xE6\x3\x2\x2\x2\x45\x46\ah\x2\x2\x46G\az\x2\x2G\x4\x3\x2"+
		"\x2\x2HI\ah\x2\x2IJ\a{\x2\x2J\x6\x3\x2\x2\x2KL\az\x2\x2LM\ah\x2\x2M\b"+
		"\x3\x2\x2\x2NO\a{\x2\x2OP\ah\x2\x2P\n\x3\x2\x2\x2QR\az\x2\x2RS\ah\x2\x2"+
		"ST\az\x2\x2T\f\x3\x2\x2\x2UV\az\x2\x2VW\ah\x2\x2WX\a{\x2\x2X\xE\x3\x2"+
		"\x2\x2YZ\a{\x2\x2Z[\ah\x2\x2[\\\az\x2\x2\\\x10\x3\x2\x2\x2]^\a{\x2\x2"+
		"^_\ah\x2\x2_`\a{\x2\x2`\x12\x3\x2\x2\x2\x61\x62\aq\x2\x2\x62\x63\ar\x2"+
		"\x2\x63\x14\x3\x2\x2\x2\x64\x65\a.\x2\x2\x65\x16\x3\x2\x2\x2\x66g\a}\x2"+
		"\x2g\x18\x3\x2\x2\x2hi\a\x7F\x2\x2i\x1A\x3\x2\x2\x2jk\av\x2\x2kl\at\x2"+
		"\x2lm\aw\x2\x2mn\ag\x2\x2n\x1C\x3\x2\x2\x2op\ah\x2\x2pq\a\x63\x2\x2qr"+
		"\an\x2\x2rs\au\x2\x2st\ag\x2\x2t\x1E\x3\x2\x2\x2uv\a>\x2\x2vw\a@\x2\x2"+
		"w \x3\x2\x2\x2x|\a$\x2\x2y{\n\x2\x2\x2zy\x3\x2\x2\x2{~\x3\x2\x2\x2|z\x3"+
		"\x2\x2\x2|}\x3\x2\x2\x2}\x7F\x3\x2\x2\x2~|\x3\x2\x2\x2\x7F\x80\a$\x2\x2"+
		"\x80\"\x3\x2\x2\x2\x81\x83\x4\x32;\x2\x82\x81\x3\x2\x2\x2\x83\x84\x3\x2"+
		"\x2\x2\x84\x82\x3\x2\x2\x2\x84\x85\x3\x2\x2\x2\x85$\x3\x2\x2\x2\x86\x87"+
		"\x5#\x12\x2\x87\x88\a\x30\x2\x2\x88\x89\x5#\x12\x2\x89&\x3\x2\x2\x2\x8A"+
		"\x8C\t\x3\x2\x2\x8B\x8A\x3\x2\x2\x2\x8C\x8D\x3\x2\x2\x2\x8D\x8B\x3\x2"+
		"\x2\x2\x8D\x8E\x3\x2\x2\x2\x8E(\x3\x2\x2\x2\x8F\x91\t\x4\x2\x2\x90\x8F"+
		"\x3\x2\x2\x2\x91\x92\x3\x2\x2\x2\x92\x90\x3\x2\x2\x2\x92\x93\x3\x2\x2"+
		"\x2\x93*\x3\x2\x2\x2\x94\x99\a}\x2\x2\x95\x98\x5+\x16\x2\x96\x98\n\x5"+
		"\x2\x2\x97\x95\x3\x2\x2\x2\x97\x96\x3\x2\x2\x2\x98\x9B\x3\x2\x2\x2\x99"+
		"\x97\x3\x2\x2\x2\x99\x9A\x3\x2\x2\x2\x9A\x9C\x3\x2\x2\x2\x9B\x99\x3\x2"+
		"\x2\x2\x9C\x9D\a\x7F\x2\x2\x9D,\x3\x2\x2\x2\x9E\x9F\a\x31\x2\x2\x9F\xA0"+
		"\a,\x2\x2\xA0\xA4\x3\x2\x2\x2\xA1\xA3\v\x2\x2\x2\xA2\xA1\x3\x2\x2\x2\xA3"+
		"\xA6\x3\x2\x2\x2\xA4\xA5\x3\x2\x2\x2\xA4\xA2\x3\x2\x2\x2\xA5\xA7\x3\x2"+
		"\x2\x2\xA6\xA4\x3\x2\x2\x2\xA7\xA8\a,\x2\x2\xA8\xA9\a\x31\x2\x2\xA9\xAA"+
		"\x3\x2\x2\x2\xAA\xAB\b\x17\x2\x2\xAB.\x3\x2\x2\x2\xAC\xAD\a\x31\x2\x2"+
		"\xAD\xAE\a\x31\x2\x2\xAE\xB2\x3\x2\x2\x2\xAF\xB1\n\x6\x2\x2\xB0\xAF\x3"+
		"\x2\x2\x2\xB1\xB4\x3\x2\x2\x2\xB2\xB0\x3\x2\x2\x2\xB2\xB3\x3\x2\x2\x2"+
		"\xB3\xB6\x3\x2\x2\x2\xB4\xB2\x3\x2\x2\x2\xB5\xB7\a\xF\x2\x2\xB6\xB5\x3"+
		"\x2\x2\x2\xB6\xB7\x3\x2\x2\x2\xB7\xB8\x3\x2\x2\x2\xB8\xB9\a\f\x2\x2\xB9"+
		"\xBA\x3\x2\x2\x2\xBA\xBB\b\x18\x2\x2\xBB\x30\x3\x2\x2\x2\xBC\xBE\t\a\x2"+
		"\x2\xBD\xBC\x3\x2\x2\x2\xBE\xBF\x3\x2\x2\x2\xBF\xBD\x3\x2\x2\x2\xBF\xC0"+
		"\x3\x2\x2\x2\xC0\x32\x3\x2\x2\x2\xC1\xC2\a<\x2\x2\xC2\x34\x3\x2\x2\x2"+
		"\xC3\xC4\a*\x2\x2\xC4\xC5\b\x1B\x3\x2\xC5\x36\x3\x2\x2\x2\xC6\xC7\a+\x2"+
		"\x2\xC7\xC8\b\x1C\x4\x2\xC8\x38\x3\x2\x2\x2\xC9\xCA\a]\x2\x2\xCA\xCB\b"+
		"\x1D\x5\x2\xCB:\x3\x2\x2\x2\xCC\xCD\a_\x2\x2\xCD\xCE\b\x1E\x6\x2\xCE<"+
		"\x3\x2\x2\x2\xCF\xD0\a>\x2\x2\xD0\xD1\b\x1F\a\x2\xD1>\x3\x2\x2\x2\xD2"+
		"\xD3\a@\x2\x2\xD3\xD4\b \b\x2\xD4@\x3\x2\x2\x2\xD5\xD6\x6!\x2\x2\xD6\xE2"+
		"\x5\x31\x19\x2\xD7\xD9\a\xF\x2\x2\xD8\xD7\x3\x2\x2\x2\xD8\xD9\x3\x2\x2"+
		"\x2\xD9\xDA\x3\x2\x2\x2\xDA\xDD\a\f\x2\x2\xDB\xDD\a\xF\x2\x2\xDC\xD8\x3"+
		"\x2\x2\x2\xDC\xDB\x3\x2\x2\x2\xDD\xDF\x3\x2\x2\x2\xDE\xE0\x5\x31\x19\x2"+
		"\xDF\xDE\x3\x2\x2\x2\xDF\xE0\x3\x2\x2\x2\xE0\xE2\x3\x2\x2\x2\xE1\xD5\x3"+
		"\x2\x2\x2\xE1\xDC\x3\x2\x2\x2\xE2\xE3\x3\x2\x2\x2\xE3\xE4\b!\t\x2\xE4"+
		"\x42\x3\x2\x2\x2\xE5\xE7\t\b\x2\x2\xE6\xE5\x3\x2\x2\x2\xE7\xE8\x3\x2\x2"+
		"\x2\xE8\xE6\x3\x2\x2\x2\xE8\xE9\x3\x2\x2\x2\xE9\xEA\x3\x2\x2\x2\xEA\xEB"+
		"\b\"\x2\x2\xEB\x44\x3\x2\x2\x2\x12\x2|\x84\x8D\x92\x97\x99\xA4\xB2\xB6"+
		"\xBF\xD8\xDC\xDF\xE1\xE8\n\x2\x3\x2\x3\x1B\x2\x3\x1C\x3\x3\x1D\x4\x3\x1E"+
		"\x5\x3\x1F\x6\x3 \a\x3!\b";
	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN.ToCharArray());
}
