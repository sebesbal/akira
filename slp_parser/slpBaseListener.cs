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


using Antlr4.Runtime.Misc;
using IErrorNode = Antlr4.Runtime.Tree.IErrorNode;
using ITerminalNode = Antlr4.Runtime.Tree.ITerminalNode;
using IToken = Antlr4.Runtime.IToken;
using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;

/// <summary>
/// This class provides an empty implementation of <see cref="IslpListener"/>,
/// which can be extended to create a listener which only needs to handle a subset
/// of the available methods.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.5")]
[System.CLSCompliant(false)]
public partial class slpBaseListener : IslpListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="slpParser.program"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterProgram([NotNull] slpParser.ProgramContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="slpParser.program"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitProgram([NotNull] slpParser.ProgramContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="slpParser.predicate"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterPredicate([NotNull] slpParser.PredicateContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="slpParser.predicate"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitPredicate([NotNull] slpParser.PredicateContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="slpParser.decList"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterDecList([NotNull] slpParser.DecListContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="slpParser.decList"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitDecList([NotNull] slpParser.DecListContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="slpParser.declaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterDeclaration([NotNull] slpParser.DeclarationContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="slpParser.declaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitDeclaration([NotNull] slpParser.DeclarationContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="slpParser.Switch"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterSwitch([NotNull] slpParser.SwitchContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="slpParser.Switch"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitSwitch([NotNull] slpParser.SwitchContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="slpParser.Sugar"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterSugar([NotNull] slpParser.SugarContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="slpParser.Sugar"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitSugar([NotNull] slpParser.SugarContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="slpParser.Associative"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterAssociative([NotNull] slpParser.AssociativeContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="slpParser.Associative"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitAssociative([NotNull] slpParser.AssociativeContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="slpParser.Member"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterMember([NotNull] slpParser.MemberContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="slpParser.Member"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitMember([NotNull] slpParser.MemberContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="slpParser.Binary"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterBinary([NotNull] slpParser.BinaryContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="slpParser.Binary"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitBinary([NotNull] slpParser.BinaryContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="slpParser.OpVar"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterOpVar([NotNull] slpParser.OpVarContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="slpParser.OpVar"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitOpVar([NotNull] slpParser.OpVarContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="slpParser.Operator"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterOperator([NotNull] slpParser.OperatorContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="slpParser.Operator"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitOperator([NotNull] slpParser.OperatorContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="slpParser.OpVarUnary"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterOpVarUnary([NotNull] slpParser.OpVarUnaryContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="slpParser.OpVarUnary"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitOpVarUnary([NotNull] slpParser.OpVarUnaryContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="slpParser.Para"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterPara([NotNull] slpParser.ParaContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="slpParser.Para"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitPara([NotNull] slpParser.ParaContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="slpParser.Not"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterNot([NotNull] slpParser.NotContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="slpParser.Not"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitNot([NotNull] slpParser.NotContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="slpParser.Sing"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterSing([NotNull] slpParser.SingContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="slpParser.Sing"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitSing([NotNull] slpParser.SingContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="slpParser.Question"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterQuestion([NotNull] slpParser.QuestionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="slpParser.Question"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitQuestion([NotNull] slpParser.QuestionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="slpParser.Attribute"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterAttribute([NotNull] slpParser.AttributeContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="slpParser.Attribute"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitAttribute([NotNull] slpParser.AttributeContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="slpParser.Min"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterMin([NotNull] slpParser.MinContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="slpParser.Min"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitMin([NotNull] slpParser.MinContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="slpParser.id"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterId([NotNull] slpParser.IdContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="slpParser.id"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitId([NotNull] slpParser.IdContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="slpParser.nat"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterNat([NotNull] slpParser.NatContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="slpParser.nat"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitNat([NotNull] slpParser.NatContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="slpParser.NormalCall"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterNormalCall([NotNull] slpParser.NormalCallContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="slpParser.NormalCall"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitNormalCall([NotNull] slpParser.NormalCallContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="slpParser.DeclareCall"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterDeclareCall([NotNull] slpParser.DeclareCallContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="slpParser.DeclareCall"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitDeclareCall([NotNull] slpParser.DeclareCallContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="slpParser.DelayedCall1"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterDelayedCall1([NotNull] slpParser.DelayedCall1Context context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="slpParser.DelayedCall1"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitDelayedCall1([NotNull] slpParser.DelayedCall1Context context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="slpParser.DelayedCall2"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterDelayedCall2([NotNull] slpParser.DelayedCall2Context context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="slpParser.DelayedCall2"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitDelayedCall2([NotNull] slpParser.DelayedCall2Context context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="slpParser.expList"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterExpList([NotNull] slpParser.ExpListContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="slpParser.expList"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitExpList([NotNull] slpParser.ExpListContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="slpParser.rul"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterRul([NotNull] slpParser.RulContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="slpParser.rul"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitRul([NotNull] slpParser.RulContext context) { }

	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void EnterEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void ExitEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitTerminal([NotNull] ITerminalNode node) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitErrorNode([NotNull] IErrorNode node) { }
}
