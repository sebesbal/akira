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
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.5")]
[System.CLSCompliant(false)]
public partial class slpParser : Parser {
	public const int
		T__0=1, T__1=2, T__2=3, T__3=4, T__4=5, T__5=6, T__6=7, T__7=8, T__8=9, 
		T__9=10, T__10=11, T__11=12, TRUE=13, FALSE=14, NULL=15, STRING=16, INT=17, 
		FLOAT=18, ID=19, OP=20, NATIVE=21, COMMENT=22, LINE_COMMENT=23, SPACES=24, 
		TREE=25, OPEN_PAREN=26, CLOSE_PAREN=27, OPEN_BRACK=28, CLOSE_BRACK=29, 
		OPEN_BRACE=30, CLOSE_BRACE=31, NEWLINE=32, WS=33, INDENT=34, DEDENT=35;
	public const int
		RULE_program = 0, RULE_assoc = 1, RULE_opdef = 2, RULE_token = 3, RULE_exp = 4;
	public static readonly string[] ruleNames = {
		"program", "assoc", "opdef", "token", "exp"
	};

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
		"OPEN_BRACK", "CLOSE_BRACK", "OPEN_BRACE", "CLOSE_BRACE", "NEWLINE", "WS", 
		"INDENT", "DEDENT"
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

	public override string SerializedAtn { get { return _serializedATN; } }

	public slpParser(ITokenStream input)
		: base(input)
	{
		Interpreter = new ParserATNSimulator(this,_ATN);
	}
	public partial class ProgramContext : ParserRuleContext {
		public ExpContext exp() {
			return GetRuleContext<ExpContext>(0);
		}
		public ProgramContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_program; } }
		public override void EnterRule(IParseTreeListener listener) {
			IslpListener typedListener = listener as IslpListener;
			if (typedListener != null) typedListener.EnterProgram(this);
		}
		public override void ExitRule(IParseTreeListener listener) {
			IslpListener typedListener = listener as IslpListener;
			if (typedListener != null) typedListener.ExitProgram(this);
		}
	}

	[RuleVersion(0)]
	public ProgramContext program() {
		ProgramContext _localctx = new ProgramContext(Context, State);
		EnterRule(_localctx, 0, RULE_program);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 10; exp(0);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class AssocContext : ParserRuleContext {
		public AssocContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_assoc; } }
		public override void EnterRule(IParseTreeListener listener) {
			IslpListener typedListener = listener as IslpListener;
			if (typedListener != null) typedListener.EnterAssoc(this);
		}
		public override void ExitRule(IParseTreeListener listener) {
			IslpListener typedListener = listener as IslpListener;
			if (typedListener != null) typedListener.ExitAssoc(this);
		}
	}

	[RuleVersion(0)]
	public AssocContext assoc() {
		AssocContext _localctx = new AssocContext(Context, State);
		EnterRule(_localctx, 2, RULE_assoc);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 12;
			_la = TokenStream.La(1);
			if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << T__0) | (1L << T__1) | (1L << T__2) | (1L << T__3) | (1L << T__4) | (1L << T__5) | (1L << T__6) | (1L << T__7))) != 0)) ) {
			ErrorHandler.RecoverInline(this);
			}
			else {
			    Consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class OpdefContext : ParserRuleContext {
		public ITerminalNode INT() { return GetToken(slpParser.INT, 0); }
		public AssocContext assoc() {
			return GetRuleContext<AssocContext>(0);
		}
		public ITerminalNode OP() { return GetToken(slpParser.OP, 0); }
		public ITerminalNode NEWLINE() { return GetToken(slpParser.NEWLINE, 0); }
		public OpdefContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_opdef; } }
		public override void EnterRule(IParseTreeListener listener) {
			IslpListener typedListener = listener as IslpListener;
			if (typedListener != null) typedListener.EnterOpdef(this);
		}
		public override void ExitRule(IParseTreeListener listener) {
			IslpListener typedListener = listener as IslpListener;
			if (typedListener != null) typedListener.ExitOpdef(this);
		}
	}

	[RuleVersion(0)]
	public OpdefContext opdef() {
		OpdefContext _localctx = new OpdefContext(Context, State);
		EnterRule(_localctx, 4, RULE_opdef);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 14; Match(T__8);
			State = 15; Match(OPEN_PAREN);
			State = 16; Match(INT);
			State = 17; Match(T__9);
			State = 18; assoc();
			State = 19; Match(T__9);
			State = 20; Match(OP);
			State = 21; Match(CLOSE_PAREN);
			State = 22; Match(NEWLINE);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class TokenContext : ParserRuleContext {
		public ITerminalNode INT() { return GetToken(slpParser.INT, 0); }
		public ITerminalNode FLOAT() { return GetToken(slpParser.FLOAT, 0); }
		public ITerminalNode STRING() { return GetToken(slpParser.STRING, 0); }
		public ITerminalNode TRUE() { return GetToken(slpParser.TRUE, 0); }
		public ITerminalNode FALSE() { return GetToken(slpParser.FALSE, 0); }
		public ITerminalNode ID() { return GetToken(slpParser.ID, 0); }
		public ITerminalNode OP() { return GetToken(slpParser.OP, 0); }
		public ITerminalNode NEWLINE() { return GetToken(slpParser.NEWLINE, 0); }
		public ITerminalNode NATIVE() { return GetToken(slpParser.NATIVE, 0); }
		public TokenContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_token; } }
		public override void EnterRule(IParseTreeListener listener) {
			IslpListener typedListener = listener as IslpListener;
			if (typedListener != null) typedListener.EnterToken(this);
		}
		public override void ExitRule(IParseTreeListener listener) {
			IslpListener typedListener = listener as IslpListener;
			if (typedListener != null) typedListener.ExitToken(this);
		}
	}

	[RuleVersion(0)]
	public TokenContext token() {
		TokenContext _localctx = new TokenContext(Context, State);
		EnterRule(_localctx, 6, RULE_token);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 24;
			_la = TokenStream.La(1);
			if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << TRUE) | (1L << FALSE) | (1L << STRING) | (1L << INT) | (1L << FLOAT) | (1L << ID) | (1L << OP) | (1L << NATIVE) | (1L << NEWLINE))) != 0)) ) {
			ErrorHandler.RecoverInline(this);
			}
			else {
			    Consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class ExpContext : ParserRuleContext {
		public ExpContext[] exp() {
			return GetRuleContexts<ExpContext>();
		}
		public ExpContext exp(int i) {
			return GetRuleContext<ExpContext>(i);
		}
		public TokenContext token() {
			return GetRuleContext<TokenContext>(0);
		}
		public OpdefContext opdef() {
			return GetRuleContext<OpdefContext>(0);
		}
		public ITerminalNode TREE() { return GetToken(slpParser.TREE, 0); }
		public ExpContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_exp; } }
		public override void EnterRule(IParseTreeListener listener) {
			IslpListener typedListener = listener as IslpListener;
			if (typedListener != null) typedListener.EnterExp(this);
		}
		public override void ExitRule(IParseTreeListener listener) {
			IslpListener typedListener = listener as IslpListener;
			if (typedListener != null) typedListener.ExitExp(this);
		}
	}

	[RuleVersion(0)]
	public ExpContext exp() {
		return exp(0);
	}

	private ExpContext exp(int _p) {
		ParserRuleContext _parentctx = Context;
		int _parentState = State;
		ExpContext _localctx = new ExpContext(Context, _parentState);
		ExpContext _prevctx = _localctx;
		int _startState = 8;
		EnterRecursionRule(_localctx, 8, RULE_exp, _p);
		int _la;
		try {
			int _alt;
			EnterOuterAlt(_localctx, 1);
			{
			State = 53;
			switch (TokenStream.La(1)) {
			case OPEN_PAREN:
				{
				State = 27; Match(OPEN_PAREN);
				State = 29;
				ErrorHandler.Sync(this);
				_la = TokenStream.La(1);
				do {
					{
					{
					State = 28; exp(0);
					}
					}
					State = 31;
					ErrorHandler.Sync(this);
					_la = TokenStream.La(1);
				} while ( (((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << T__8) | (1L << T__10) | (1L << TRUE) | (1L << FALSE) | (1L << STRING) | (1L << INT) | (1L << FLOAT) | (1L << ID) | (1L << OP) | (1L << NATIVE) | (1L << OPEN_PAREN) | (1L << OPEN_BRACK) | (1L << NEWLINE))) != 0) );
				State = 33; Match(CLOSE_PAREN);
				}
				break;
			case T__10:
				{
				State = 35; Match(T__10);
				State = 37;
				ErrorHandler.Sync(this);
				_la = TokenStream.La(1);
				do {
					{
					{
					State = 36; exp(0);
					}
					}
					State = 39;
					ErrorHandler.Sync(this);
					_la = TokenStream.La(1);
				} while ( (((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << T__8) | (1L << T__10) | (1L << TRUE) | (1L << FALSE) | (1L << STRING) | (1L << INT) | (1L << FLOAT) | (1L << ID) | (1L << OP) | (1L << NATIVE) | (1L << OPEN_PAREN) | (1L << OPEN_BRACK) | (1L << NEWLINE))) != 0) );
				State = 41; Match(T__11);
				}
				break;
			case OPEN_BRACK:
				{
				State = 43; Match(OPEN_BRACK);
				State = 45;
				ErrorHandler.Sync(this);
				_la = TokenStream.La(1);
				do {
					{
					{
					State = 44; exp(0);
					}
					}
					State = 47;
					ErrorHandler.Sync(this);
					_la = TokenStream.La(1);
				} while ( (((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << T__8) | (1L << T__10) | (1L << TRUE) | (1L << FALSE) | (1L << STRING) | (1L << INT) | (1L << FLOAT) | (1L << ID) | (1L << OP) | (1L << NATIVE) | (1L << OPEN_PAREN) | (1L << OPEN_BRACK) | (1L << NEWLINE))) != 0) );
				State = 49; Match(CLOSE_BRACK);
				}
				break;
			case TRUE:
			case FALSE:
			case STRING:
			case INT:
			case FLOAT:
			case ID:
			case OP:
			case NATIVE:
			case NEWLINE:
				{
				State = 51; token();
				}
				break;
			case T__8:
				{
				State = 52; opdef();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			Context.Stop = TokenStream.Lt(-1);
			State = 62;
			ErrorHandler.Sync(this);
			_alt = Interpreter.AdaptivePredict(TokenStream,5,Context);
			while ( _alt!=2 && _alt!=global::Antlr4.Runtime.Atn.ATN.InvalidAltNumber ) {
				if ( _alt==1 ) {
					if ( ParseListeners!=null )
						TriggerExitRuleEvent();
					_prevctx = _localctx;
					{
					State = 60;
					switch ( Interpreter.AdaptivePredict(TokenStream,4,Context) ) {
					case 1:
						{
						_localctx = new ExpContext(_parentctx, _parentState);
						PushNewRecursionContext(_localctx, _startState, RULE_exp);
						State = 55;
						if (!(Precpred(Context, 4))) throw new FailedPredicateException(this, "Precpred(Context, 4)");
						State = 56; Match(TREE);
						State = 57; exp(5);
						}
						break;
					case 2:
						{
						_localctx = new ExpContext(_parentctx, _parentState);
						PushNewRecursionContext(_localctx, _startState, RULE_exp);
						State = 58;
						if (!(Precpred(Context, 1))) throw new FailedPredicateException(this, "Precpred(Context, 1)");
						State = 59; exp(2);
						}
						break;
					}
					} 
				}
				State = 64;
				ErrorHandler.Sync(this);
				_alt = Interpreter.AdaptivePredict(TokenStream,5,Context);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			UnrollRecursionContexts(_parentctx);
		}
		return _localctx;
	}

	public override bool Sempred(RuleContext _localctx, int ruleIndex, int predIndex) {
		switch (ruleIndex) {
		case 4: return exp_sempred((ExpContext)_localctx, predIndex);
		}
		return true;
	}
	private bool exp_sempred(ExpContext _localctx, int predIndex) {
		switch (predIndex) {
		case 0: return Precpred(Context, 4);
		case 1: return Precpred(Context, 1);
		}
		return true;
	}

	public static readonly string _serializedATN =
		"\x3\x430\xD6D1\x8206\xAD2D\x4417\xAEF1\x8D80\xAADD\x3%\x44\x4\x2\t\x2"+
		"\x4\x3\t\x3\x4\x4\t\x4\x4\x5\t\x5\x4\x6\t\x6\x3\x2\x3\x2\x3\x3\x3\x3\x3"+
		"\x4\x3\x4\x3\x4\x3\x4\x3\x4\x3\x4\x3\x4\x3\x4\x3\x4\x3\x4\x3\x5\x3\x5"+
		"\x3\x6\x3\x6\x3\x6\x6\x6 \n\x6\r\x6\xE\x6!\x3\x6\x3\x6\x3\x6\x3\x6\x6"+
		"\x6(\n\x6\r\x6\xE\x6)\x3\x6\x3\x6\x3\x6\x3\x6\x6\x6\x30\n\x6\r\x6\xE\x6"+
		"\x31\x3\x6\x3\x6\x3\x6\x3\x6\x5\x6\x38\n\x6\x3\x6\x3\x6\x3\x6\x3\x6\x3"+
		"\x6\a\x6?\n\x6\f\x6\xE\x6\x42\v\x6\x3\x6\x2\x3\n\a\x2\x4\x6\b\n\x2\x4"+
		"\x3\x2\x3\n\x5\x2\xF\x10\x12\x17\"\"G\x2\f\x3\x2\x2\x2\x4\xE\x3\x2\x2"+
		"\x2\x6\x10\x3\x2\x2\x2\b\x1A\x3\x2\x2\x2\n\x37\x3\x2\x2\x2\f\r\x5\n\x6"+
		"\x2\r\x3\x3\x2\x2\x2\xE\xF\t\x2\x2\x2\xF\x5\x3\x2\x2\x2\x10\x11\a\v\x2"+
		"\x2\x11\x12\a\x1C\x2\x2\x12\x13\a\x13\x2\x2\x13\x14\a\f\x2\x2\x14\x15"+
		"\x5\x4\x3\x2\x15\x16\a\f\x2\x2\x16\x17\a\x16\x2\x2\x17\x18\a\x1D\x2\x2"+
		"\x18\x19\a\"\x2\x2\x19\a\x3\x2\x2\x2\x1A\x1B\t\x3\x2\x2\x1B\t\x3\x2\x2"+
		"\x2\x1C\x1D\b\x6\x1\x2\x1D\x1F\a\x1C\x2\x2\x1E \x5\n\x6\x2\x1F\x1E\x3"+
		"\x2\x2\x2 !\x3\x2\x2\x2!\x1F\x3\x2\x2\x2!\"\x3\x2\x2\x2\"#\x3\x2\x2\x2"+
		"#$\a\x1D\x2\x2$\x38\x3\x2\x2\x2%\'\a\r\x2\x2&(\x5\n\x6\x2\'&\x3\x2\x2"+
		"\x2()\x3\x2\x2\x2)\'\x3\x2\x2\x2)*\x3\x2\x2\x2*+\x3\x2\x2\x2+,\a\xE\x2"+
		"\x2,\x38\x3\x2\x2\x2-/\a\x1E\x2\x2.\x30\x5\n\x6\x2/.\x3\x2\x2\x2\x30\x31"+
		"\x3\x2\x2\x2\x31/\x3\x2\x2\x2\x31\x32\x3\x2\x2\x2\x32\x33\x3\x2\x2\x2"+
		"\x33\x34\a\x1F\x2\x2\x34\x38\x3\x2\x2\x2\x35\x38\x5\b\x5\x2\x36\x38\x5"+
		"\x6\x4\x2\x37\x1C\x3\x2\x2\x2\x37%\x3\x2\x2\x2\x37-\x3\x2\x2\x2\x37\x35"+
		"\x3\x2\x2\x2\x37\x36\x3\x2\x2\x2\x38@\x3\x2\x2\x2\x39:\f\x6\x2\x2:;\a"+
		"\x1B\x2\x2;?\x5\n\x6\a<=\f\x3\x2\x2=?\x5\n\x6\x4>\x39\x3\x2\x2\x2><\x3"+
		"\x2\x2\x2?\x42\x3\x2\x2\x2@>\x3\x2\x2\x2@\x41\x3\x2\x2\x2\x41\v\x3\x2"+
		"\x2\x2\x42@\x3\x2\x2\x2\b!)\x31\x37>@";
	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN.ToCharArray());
}
