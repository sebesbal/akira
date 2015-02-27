using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using akira;

namespace slp_parser
{
    class Listener: IslpListener
    {
        public Node Root { get; protected set; }

        Antlr4.Runtime.Tree.ParseTreeProperty<Node> m = new Antlr4.Runtime.Tree.ParseTreeProperty<Node>();

        public void EnterProgram(slpParser.ProgramContext context)
        {
        }

        public void ExitProgram(slpParser.ProgramContext context)
        {
            Root = m.Get(context.exp());
        }

        public void EnterPredicate(slpParser.PredicateContext context)
        {
            throw new NotImplementedException();
        }

        public void ExitPredicate(slpParser.PredicateContext context)
        {
            throw new NotImplementedException();
        }

        public void EnterDecList(slpParser.DecListContext context)
        {
            throw new NotImplementedException();
        }

        public void ExitDecList(slpParser.DecListContext context)
        {
            throw new NotImplementedException();
        }

        public void EnterDeclaration(slpParser.DeclarationContext context)
        {
            throw new NotImplementedException();
        }

        public void ExitDeclaration(slpParser.DeclarationContext context)
        {
            throw new NotImplementedException();
        }

        public void EnterSwitch(slpParser.SwitchContext context)
        {
            throw new NotImplementedException();
        }

        public void ExitSwitch(slpParser.SwitchContext context)
        {
            throw new NotImplementedException();
        }

        public void EnterSugar(slpParser.SugarContext context)
        {
            throw new NotImplementedException();
        }

        public void ExitSugar(slpParser.SugarContext context)
        {
            throw new NotImplementedException();
        }

        public void EnterAssociative(slpParser.AssociativeContext context)
        {
            throw new NotImplementedException();
        }

        public void ExitAssociative(slpParser.AssociativeContext context)
        {
            throw new NotImplementedException();
        }

        public void EnterMember(slpParser.MemberContext context)
        {
            throw new NotImplementedException();
        }

        public void ExitMember(slpParser.MemberContext context)
        {
            throw new NotImplementedException();
        }

        public void EnterBinary(slpParser.BinaryContext context)
        {
            throw new NotImplementedException();
        }

        public void ExitBinary(slpParser.BinaryContext context)
        {
            throw new NotImplementedException();
        }

        public void EnterOpVar(slpParser.OpVarContext context)
        {
            throw new NotImplementedException();
        }

        public void ExitOpVar(slpParser.OpVarContext context)
        {
            throw new NotImplementedException();
        }

        public void EnterOperator(slpParser.OperatorContext context)
        {
            throw new NotImplementedException();
        }

        public void ExitOperator(slpParser.OperatorContext context)
        {
            throw new NotImplementedException();
        }

        public void EnterOpVarUnary(slpParser.OpVarUnaryContext context)
        {
            throw new NotImplementedException();
        }

        public void ExitOpVarUnary(slpParser.OpVarUnaryContext context)
        {
            throw new NotImplementedException();
        }

        public void EnterPara(slpParser.ParaContext context)
        {
            throw new NotImplementedException();
        }

        public void ExitPara(slpParser.ParaContext context)
        {
            throw new NotImplementedException();
        }

        public void EnterNot(slpParser.NotContext context)
        {
            throw new NotImplementedException();
        }

        public void ExitNot(slpParser.NotContext context)
        {
            throw new NotImplementedException();
        }

        public void EnterSing(slpParser.SingContext context)
        {
        }

        public void ExitSing(slpParser.SingContext context)
        {
            //if (context.STRING() != null)
            //{
            //    Node n = new Node();
            //     context.STRING().GetText()
            //    m.Put(context,);
            //}
        }

        public void EnterQuestion(slpParser.QuestionContext context)
        {
            throw new NotImplementedException();
        }

        public void ExitQuestion(slpParser.QuestionContext context)
        {
            throw new NotImplementedException();
        }

        public void EnterAttribute(slpParser.AttributeContext context)
        {
            throw new NotImplementedException();
        }

        public void ExitAttribute(slpParser.AttributeContext context)
        {
            throw new NotImplementedException();
        }

        public void EnterMin(slpParser.MinContext context)
        {
            throw new NotImplementedException();
        }

        public void ExitMin(slpParser.MinContext context)
        {
            throw new NotImplementedException();
        }

        public void EnterId(slpParser.IdContext context)
        {
            throw new NotImplementedException();
        }

        public void ExitId(slpParser.IdContext context)
        {
            throw new NotImplementedException();
        }

        public void EnterNat(slpParser.NatContext context)
        {
            throw new NotImplementedException();
        }

        public void ExitNat(slpParser.NatContext context)
        {
            throw new NotImplementedException();
        }

        public void EnterNormalCall(slpParser.NormalCallContext context)
        {
            throw new NotImplementedException();
        }

        public void ExitNormalCall(slpParser.NormalCallContext context)
        {
            throw new NotImplementedException();
        }

        public void EnterDeclareCall(slpParser.DeclareCallContext context)
        {
            throw new NotImplementedException();
        }

        public void ExitDeclareCall(slpParser.DeclareCallContext context)
        {
            throw new NotImplementedException();
        }

        public void EnterDelayedCall1(slpParser.DelayedCall1Context context)
        {
            throw new NotImplementedException();
        }

        public void ExitDelayedCall1(slpParser.DelayedCall1Context context)
        {
            throw new NotImplementedException();
        }

        public void EnterDelayedCall2(slpParser.DelayedCall2Context context)
        {
            throw new NotImplementedException();
        }

        public void ExitDelayedCall2(slpParser.DelayedCall2Context context)
        {
            throw new NotImplementedException();
        }

        public void EnterExpList(slpParser.ExpListContext context)
        {
            throw new NotImplementedException();
        }

        public void ExitExpList(slpParser.ExpListContext context)
        {
            throw new NotImplementedException();
        }

        public void EnterRul(slpParser.RulContext context)
        {
            throw new NotImplementedException();
        }

        public void ExitRul(slpParser.RulContext context)
        {
            throw new NotImplementedException();
        }

        public void EnterEveryRule(Antlr4.Runtime.ParserRuleContext ctx)
        {
            throw new NotImplementedException();
        }

        public void ExitEveryRule(Antlr4.Runtime.ParserRuleContext ctx)
        {
            throw new NotImplementedException();
        }

        public void VisitErrorNode(Antlr4.Runtime.Tree.IErrorNode node)
        {
            throw new NotImplementedException();
        }

        public void VisitTerminal(Antlr4.Runtime.Tree.ITerminalNode node)
        {
            throw new NotImplementedException();
        }
    }
}
