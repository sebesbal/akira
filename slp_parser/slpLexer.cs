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
		T__9=10, T__10=11, T__11=12, T__12=13, T__13=14, T__14=15, T__15=16, T__16=17, 
		T__17=18, T__18=19, T__19=20, T__20=21, T__21=22, T__22=23, T__23=24, 
		T__24=25, T__25=26, T__26=27, T__27=28, T__28=29, T__29=30, T__30=31, 
		T__31=32, T__32=33, T__33=34, T__34=35, T__35=36, EXPORT=37, CLASS=38, 
		TRUE=39, FAIL=40, NULL=41, STRING=42, INT=43, FLOAT=44, ID=45, NATIVE=46, 
		COMMENT=47, LINE_COMMENT=48, WS=49;
	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"T__0", "T__1", "T__2", "T__3", "T__4", "T__5", "T__6", "T__7", "T__8", 
		"T__9", "T__10", "T__11", "T__12", "T__13", "T__14", "T__15", "T__16", 
		"T__17", "T__18", "T__19", "T__20", "T__21", "T__22", "T__23", "T__24", 
		"T__25", "T__26", "T__27", "T__28", "T__29", "T__30", "T__31", "T__32", 
		"T__33", "T__34", "T__35", "EXPORT", "CLASS", "TRUE", "FAIL", "NULL", 
		"STRING", "INT", "FLOAT", "ID", "NATIVE", "COMMENT", "LINE_COMMENT", "WS"
	};


	public slpLexer(ICharStream input)
		: base(input)
	{
		Interpreter = new LexerATNSimulator(this,_ATN);
	}

	private static readonly string[] _LiteralNames = {
		null, "'('", "';'", "')'", "':-'", "'.'", "','", "'<'", "'>'", "'?'", 
		"'!'", "'^'", "'/'", "'*'", "'**'", "'*?'", "'*!'", "'*-'", "'-'", "'\\+-'", 
		"'+-'", "'+'", "'-+'", "'++'", "'=='", "'='", "':='", "'<='", "'>='", 
		"'@'", "'::'", "'?->'", "'-->'", "'->'", "'||'", "'['", "']'", "'export'", 
		"'class'", "'true'", "'fail'", "'<>'"
	};
	private static readonly string[] _SymbolicNames = {
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, "EXPORT", "CLASS", "TRUE", "FAIL", "NULL", "STRING", "INT", "FLOAT", 
		"ID", "NATIVE", "COMMENT", "LINE_COMMENT", "WS"
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

	public static readonly string _serializedATN =
		"\x3\x430\xD6D1\x8206\xAD2D\x4417\xAEF1\x8D80\xAADD\x2\x33\x122\b\x1\x4"+
		"\x2\t\x2\x4\x3\t\x3\x4\x4\t\x4\x4\x5\t\x5\x4\x6\t\x6\x4\a\t\a\x4\b\t\b"+
		"\x4\t\t\t\x4\n\t\n\x4\v\t\v\x4\f\t\f\x4\r\t\r\x4\xE\t\xE\x4\xF\t\xF\x4"+
		"\x10\t\x10\x4\x11\t\x11\x4\x12\t\x12\x4\x13\t\x13\x4\x14\t\x14\x4\x15"+
		"\t\x15\x4\x16\t\x16\x4\x17\t\x17\x4\x18\t\x18\x4\x19\t\x19\x4\x1A\t\x1A"+
		"\x4\x1B\t\x1B\x4\x1C\t\x1C\x4\x1D\t\x1D\x4\x1E\t\x1E\x4\x1F\t\x1F\x4 "+
		"\t \x4!\t!\x4\"\t\"\x4#\t#\x4$\t$\x4%\t%\x4&\t&\x4\'\t\'\x4(\t(\x4)\t"+
		")\x4*\t*\x4+\t+\x4,\t,\x4-\t-\x4.\t.\x4/\t/\x4\x30\t\x30\x4\x31\t\x31"+
		"\x4\x32\t\x32\x3\x2\x3\x2\x3\x3\x3\x3\x3\x4\x3\x4\x3\x5\x3\x5\x3\x5\x3"+
		"\x6\x3\x6\x3\a\x3\a\x3\b\x3\b\x3\t\x3\t\x3\n\x3\n\x3\v\x3\v\x3\f\x3\f"+
		"\x3\r\x3\r\x3\xE\x3\xE\x3\xF\x3\xF\x3\xF\x3\x10\x3\x10\x3\x10\x3\x11\x3"+
		"\x11\x3\x11\x3\x12\x3\x12\x3\x12\x3\x13\x3\x13\x3\x14\x3\x14\x3\x14\x3"+
		"\x14\x3\x15\x3\x15\x3\x15\x3\x16\x3\x16\x3\x17\x3\x17\x3\x17\x3\x18\x3"+
		"\x18\x3\x18\x3\x19\x3\x19\x3\x19\x3\x1A\x3\x1A\x3\x1B\x3\x1B\x3\x1B\x3"+
		"\x1C\x3\x1C\x3\x1C\x3\x1D\x3\x1D\x3\x1D\x3\x1E\x3\x1E\x3\x1F\x3\x1F\x3"+
		"\x1F\x3 \x3 \x3 \x3 \x3!\x3!\x3!\x3!\x3\"\x3\"\x3\"\x3#\x3#\x3#\x3$\x3"+
		"$\x3%\x3%\x3&\x3&\x3&\x3&\x3&\x3&\x3&\x3\'\x3\'\x3\'\x3\'\x3\'\x3\'\x3"+
		"(\x3(\x3(\x3(\x3(\x3)\x3)\x3)\x3)\x3)\x3*\x3*\x3*\x3+\x3+\a+\xDF\n+\f"+
		"+\xE+\xE2\v+\x3+\x3+\x3,\x6,\xE7\n,\r,\xE,\xE8\x3-\x3-\x3-\x3-\x3.\x6"+
		".\xF0\n.\r.\xE.\xF1\x3/\x3/\x3/\a/\xF7\n/\f/\xE/\xFA\v/\x3/\x3/\x3\x30"+
		"\x3\x30\x3\x30\x3\x30\a\x30\x102\n\x30\f\x30\xE\x30\x105\v\x30\x3\x30"+
		"\x3\x30\x3\x30\x3\x30\x3\x30\x3\x31\x3\x31\x3\x31\x3\x31\a\x31\x110\n"+
		"\x31\f\x31\xE\x31\x113\v\x31\x3\x31\x5\x31\x116\n\x31\x3\x31\x3\x31\x3"+
		"\x31\x3\x31\x3\x32\x6\x32\x11D\n\x32\r\x32\xE\x32\x11E\x3\x32\x3\x32\x3"+
		"\x103\x2\x33\x3\x3\x5\x4\a\x5\t\x6\v\a\r\b\xF\t\x11\n\x13\v\x15\f\x17"+
		"\r\x19\xE\x1B\xF\x1D\x10\x1F\x11!\x12#\x13%\x14\'\x15)\x16+\x17-\x18/"+
		"\x19\x31\x1A\x33\x1B\x35\x1C\x37\x1D\x39\x1E;\x1F= ?!\x41\"\x43#\x45$"+
		"G%I&K\'M(O)Q*S+U,W-Y.[/]\x30_\x31\x61\x32\x63\x33\x3\x2\a\x3\x2$$\x6\x2"+
		"\x32;\x43\\\x61\x61\x63|\x4\x2}}\x7F\x7F\x4\x2\f\f\xF\xF\x5\x2\v\f\xE"+
		"\xF\"\"\x12A\x2\x3\x3\x2\x2\x2\x2\x5\x3\x2\x2\x2\x2\a\x3\x2\x2\x2\x2\t"+
		"\x3\x2\x2\x2\x2\v\x3\x2\x2\x2\x2\r\x3\x2\x2\x2\x2\xF\x3\x2\x2\x2\x2\x11"+
		"\x3\x2\x2\x2\x2\x13\x3\x2\x2\x2\x2\x15\x3\x2\x2\x2\x2\x17\x3\x2\x2\x2"+
		"\x2\x19\x3\x2\x2\x2\x2\x1B\x3\x2\x2\x2\x2\x1D\x3\x2\x2\x2\x2\x1F\x3\x2"+
		"\x2\x2\x2!\x3\x2\x2\x2\x2#\x3\x2\x2\x2\x2%\x3\x2\x2\x2\x2\'\x3\x2\x2\x2"+
		"\x2)\x3\x2\x2\x2\x2+\x3\x2\x2\x2\x2-\x3\x2\x2\x2\x2/\x3\x2\x2\x2\x2\x31"+
		"\x3\x2\x2\x2\x2\x33\x3\x2\x2\x2\x2\x35\x3\x2\x2\x2\x2\x37\x3\x2\x2\x2"+
		"\x2\x39\x3\x2\x2\x2\x2;\x3\x2\x2\x2\x2=\x3\x2\x2\x2\x2?\x3\x2\x2\x2\x2"+
		"\x41\x3\x2\x2\x2\x2\x43\x3\x2\x2\x2\x2\x45\x3\x2\x2\x2\x2G\x3\x2\x2\x2"+
		"\x2I\x3\x2\x2\x2\x2K\x3\x2\x2\x2\x2M\x3\x2\x2\x2\x2O\x3\x2\x2\x2\x2Q\x3"+
		"\x2\x2\x2\x2S\x3\x2\x2\x2\x2U\x3\x2\x2\x2\x2W\x3\x2\x2\x2\x2Y\x3\x2\x2"+
		"\x2\x2[\x3\x2\x2\x2\x2]\x3\x2\x2\x2\x2_\x3\x2\x2\x2\x2\x61\x3\x2\x2\x2"+
		"\x2\x63\x3\x2\x2\x2\x3\x65\x3\x2\x2\x2\x5g\x3\x2\x2\x2\ai\x3\x2\x2\x2"+
		"\tk\x3\x2\x2\x2\vn\x3\x2\x2\x2\rp\x3\x2\x2\x2\xFr\x3\x2\x2\x2\x11t\x3"+
		"\x2\x2\x2\x13v\x3\x2\x2\x2\x15x\x3\x2\x2\x2\x17z\x3\x2\x2\x2\x19|\x3\x2"+
		"\x2\x2\x1B~\x3\x2\x2\x2\x1D\x80\x3\x2\x2\x2\x1F\x83\x3\x2\x2\x2!\x86\x3"+
		"\x2\x2\x2#\x89\x3\x2\x2\x2%\x8C\x3\x2\x2\x2\'\x8E\x3\x2\x2\x2)\x92\x3"+
		"\x2\x2\x2+\x95\x3\x2\x2\x2-\x97\x3\x2\x2\x2/\x9A\x3\x2\x2\x2\x31\x9D\x3"+
		"\x2\x2\x2\x33\xA0\x3\x2\x2\x2\x35\xA2\x3\x2\x2\x2\x37\xA5\x3\x2\x2\x2"+
		"\x39\xA8\x3\x2\x2\x2;\xAB\x3\x2\x2\x2=\xAD\x3\x2\x2\x2?\xB0\x3\x2\x2\x2"+
		"\x41\xB4\x3\x2\x2\x2\x43\xB8\x3\x2\x2\x2\x45\xBB\x3\x2\x2\x2G\xBE\x3\x2"+
		"\x2\x2I\xC0\x3\x2\x2\x2K\xC2\x3\x2\x2\x2M\xC9\x3\x2\x2\x2O\xCF\x3\x2\x2"+
		"\x2Q\xD4\x3\x2\x2\x2S\xD9\x3\x2\x2\x2U\xDC\x3\x2\x2\x2W\xE6\x3\x2\x2\x2"+
		"Y\xEA\x3\x2\x2\x2[\xEF\x3\x2\x2\x2]\xF3\x3\x2\x2\x2_\xFD\x3\x2\x2\x2\x61"+
		"\x10B\x3\x2\x2\x2\x63\x11C\x3\x2\x2\x2\x65\x66\a*\x2\x2\x66\x4\x3\x2\x2"+
		"\x2gh\a=\x2\x2h\x6\x3\x2\x2\x2ij\a+\x2\x2j\b\x3\x2\x2\x2kl\a<\x2\x2lm"+
		"\a/\x2\x2m\n\x3\x2\x2\x2no\a\x30\x2\x2o\f\x3\x2\x2\x2pq\a.\x2\x2q\xE\x3"+
		"\x2\x2\x2rs\a>\x2\x2s\x10\x3\x2\x2\x2tu\a@\x2\x2u\x12\x3\x2\x2\x2vw\a"+
		"\x41\x2\x2w\x14\x3\x2\x2\x2xy\a#\x2\x2y\x16\x3\x2\x2\x2z{\a`\x2\x2{\x18"+
		"\x3\x2\x2\x2|}\a\x31\x2\x2}\x1A\x3\x2\x2\x2~\x7F\a,\x2\x2\x7F\x1C\x3\x2"+
		"\x2\x2\x80\x81\a,\x2\x2\x81\x82\a,\x2\x2\x82\x1E\x3\x2\x2\x2\x83\x84\a"+
		",\x2\x2\x84\x85\a\x41\x2\x2\x85 \x3\x2\x2\x2\x86\x87\a,\x2\x2\x87\x88"+
		"\a#\x2\x2\x88\"\x3\x2\x2\x2\x89\x8A\a,\x2\x2\x8A\x8B\a/\x2\x2\x8B$\x3"+
		"\x2\x2\x2\x8C\x8D\a/\x2\x2\x8D&\x3\x2\x2\x2\x8E\x8F\a^\x2\x2\x8F\x90\a"+
		"-\x2\x2\x90\x91\a/\x2\x2\x91(\x3\x2\x2\x2\x92\x93\a-\x2\x2\x93\x94\a/"+
		"\x2\x2\x94*\x3\x2\x2\x2\x95\x96\a-\x2\x2\x96,\x3\x2\x2\x2\x97\x98\a/\x2"+
		"\x2\x98\x99\a-\x2\x2\x99.\x3\x2\x2\x2\x9A\x9B\a-\x2\x2\x9B\x9C\a-\x2\x2"+
		"\x9C\x30\x3\x2\x2\x2\x9D\x9E\a?\x2\x2\x9E\x9F\a?\x2\x2\x9F\x32\x3\x2\x2"+
		"\x2\xA0\xA1\a?\x2\x2\xA1\x34\x3\x2\x2\x2\xA2\xA3\a<\x2\x2\xA3\xA4\a?\x2"+
		"\x2\xA4\x36\x3\x2\x2\x2\xA5\xA6\a>\x2\x2\xA6\xA7\a?\x2\x2\xA7\x38\x3\x2"+
		"\x2\x2\xA8\xA9\a@\x2\x2\xA9\xAA\a?\x2\x2\xAA:\x3\x2\x2\x2\xAB\xAC\a\x42"+
		"\x2\x2\xAC<\x3\x2\x2\x2\xAD\xAE\a<\x2\x2\xAE\xAF\a<\x2\x2\xAF>\x3\x2\x2"+
		"\x2\xB0\xB1\a\x41\x2\x2\xB1\xB2\a/\x2\x2\xB2\xB3\a@\x2\x2\xB3@\x3\x2\x2"+
		"\x2\xB4\xB5\a/\x2\x2\xB5\xB6\a/\x2\x2\xB6\xB7\a@\x2\x2\xB7\x42\x3\x2\x2"+
		"\x2\xB8\xB9\a/\x2\x2\xB9\xBA\a@\x2\x2\xBA\x44\x3\x2\x2\x2\xBB\xBC\a~\x2"+
		"\x2\xBC\xBD\a~\x2\x2\xBD\x46\x3\x2\x2\x2\xBE\xBF\a]\x2\x2\xBFH\x3\x2\x2"+
		"\x2\xC0\xC1\a_\x2\x2\xC1J\x3\x2\x2\x2\xC2\xC3\ag\x2\x2\xC3\xC4\az\x2\x2"+
		"\xC4\xC5\ar\x2\x2\xC5\xC6\aq\x2\x2\xC6\xC7\at\x2\x2\xC7\xC8\av\x2\x2\xC8"+
		"L\x3\x2\x2\x2\xC9\xCA\a\x65\x2\x2\xCA\xCB\an\x2\x2\xCB\xCC\a\x63\x2\x2"+
		"\xCC\xCD\au\x2\x2\xCD\xCE\au\x2\x2\xCEN\x3\x2\x2\x2\xCF\xD0\av\x2\x2\xD0"+
		"\xD1\at\x2\x2\xD1\xD2\aw\x2\x2\xD2\xD3\ag\x2\x2\xD3P\x3\x2\x2\x2\xD4\xD5"+
		"\ah\x2\x2\xD5\xD6\a\x63\x2\x2\xD6\xD7\ak\x2\x2\xD7\xD8\an\x2\x2\xD8R\x3"+
		"\x2\x2\x2\xD9\xDA\a>\x2\x2\xDA\xDB\a@\x2\x2\xDBT\x3\x2\x2\x2\xDC\xE0\a"+
		"$\x2\x2\xDD\xDF\n\x2\x2\x2\xDE\xDD\x3\x2\x2\x2\xDF\xE2\x3\x2\x2\x2\xE0"+
		"\xDE\x3\x2\x2\x2\xE0\xE1\x3\x2\x2\x2\xE1\xE3\x3\x2\x2\x2\xE2\xE0\x3\x2"+
		"\x2\x2\xE3\xE4\a$\x2\x2\xE4V\x3\x2\x2\x2\xE5\xE7\x4\x32;\x2\xE6\xE5\x3"+
		"\x2\x2\x2\xE7\xE8\x3\x2\x2\x2\xE8\xE6\x3\x2\x2\x2\xE8\xE9\x3\x2\x2\x2"+
		"\xE9X\x3\x2\x2\x2\xEA\xEB\x5W,\x2\xEB\xEC\a\x30\x2\x2\xEC\xED\x5W,\x2"+
		"\xEDZ\x3\x2\x2\x2\xEE\xF0\t\x3\x2\x2\xEF\xEE\x3\x2\x2\x2\xF0\xF1\x3\x2"+
		"\x2\x2\xF1\xEF\x3\x2\x2\x2\xF1\xF2\x3\x2\x2\x2\xF2\\\x3\x2\x2\x2\xF3\xF8"+
		"\a}\x2\x2\xF4\xF7\x5]/\x2\xF5\xF7\n\x4\x2\x2\xF6\xF4\x3\x2\x2\x2\xF6\xF5"+
		"\x3\x2\x2\x2\xF7\xFA\x3\x2\x2\x2\xF8\xF6\x3\x2\x2\x2\xF8\xF9\x3\x2\x2"+
		"\x2\xF9\xFB\x3\x2\x2\x2\xFA\xF8\x3\x2\x2\x2\xFB\xFC\a\x7F\x2\x2\xFC^\x3"+
		"\x2\x2\x2\xFD\xFE\a\x31\x2\x2\xFE\xFF\a,\x2\x2\xFF\x103\x3\x2\x2\x2\x100"+
		"\x102\v\x2\x2\x2\x101\x100\x3\x2\x2\x2\x102\x105\x3\x2\x2\x2\x103\x104"+
		"\x3\x2\x2\x2\x103\x101\x3\x2\x2\x2\x104\x106\x3\x2\x2\x2\x105\x103\x3"+
		"\x2\x2\x2\x106\x107\a,\x2\x2\x107\x108\a\x31\x2\x2\x108\x109\x3\x2\x2"+
		"\x2\x109\x10A\b\x30\x2\x2\x10A`\x3\x2\x2\x2\x10B\x10C\a\x31\x2\x2\x10C"+
		"\x10D\a\x31\x2\x2\x10D\x111\x3\x2\x2\x2\x10E\x110\n\x5\x2\x2\x10F\x10E"+
		"\x3\x2\x2\x2\x110\x113\x3\x2\x2\x2\x111\x10F\x3\x2\x2\x2\x111\x112\x3"+
		"\x2\x2\x2\x112\x115\x3\x2\x2\x2\x113\x111\x3\x2\x2\x2\x114\x116\a\xF\x2"+
		"\x2\x115\x114\x3\x2\x2\x2\x115\x116\x3\x2\x2\x2\x116\x117\x3\x2\x2\x2"+
		"\x117\x118\a\f\x2\x2\x118\x119\x3\x2\x2\x2\x119\x11A\b\x31\x2\x2\x11A"+
		"\x62\x3\x2\x2\x2\x11B\x11D\t\x6\x2\x2\x11C\x11B\x3\x2\x2\x2\x11D\x11E"+
		"\x3\x2\x2\x2\x11E\x11C\x3\x2\x2\x2\x11E\x11F\x3\x2\x2\x2\x11F\x120\x3"+
		"\x2\x2\x2\x120\x121\b\x32\x2\x2\x121\x64\x3\x2\x2\x2\f\x2\xE0\xE8\xF1"+
		"\xF6\xF8\x103\x111\x115\x11E\x3\x2\x3\x2";
	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN.ToCharArray());
}
