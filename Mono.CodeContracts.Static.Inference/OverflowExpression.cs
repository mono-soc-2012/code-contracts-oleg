using System;
using System.Collections.Generic;
using Mono.CodeContracts.Inference.Interface;
using Mono.CodeContracts.Static.Analysis;
using Mono.CodeContracts.Static.DataStructures;
using Mono.CodeContracts.Static.Proving;

namespace Mono.CodeContracts.Static.Inference 
{
	public class OverflowExpressionControl<Var> : IBoxedExpressionController
	{
		#region Fields
		
		private BoxedExpression LocalResult;
		private readonly Dictionary<BoxedExpression, OverflowExpressionControl<Var>.Condition> Expressions;
		private readonly IFactForOverflow<BoxedExpression> Overflow;
		private readonly APC PC;
		
		#endregion
		
		#region Current Code Condition Enum
		
		private enum Condition
	    {
	      Overflow,
	      DoesNotOverflow
	    }
		
		#endregion
		
		#region Constructor
		
		public OverflowExpressionControl (APC PC)
		{
			this.localResult = null;
			this.PC = PC; 
			
		}
		
		#endregion
		
		#region Interface Methods
		
		private void Unary(UnaryOperator unaryOp, BoxedExpression arg, BoxedExpression source)
		{
			arg.Dispatch((IBoxedExpressionController) this);
      		if (this.LocalResult != null)
        	ReturnOkResult(BoxedExpression.Unary(unaryOperator, this.PartialResult));
      		ReturnNotOkResult(original);			
		}
		
		private void Binary(BinaryOperator binaryOp, BoxedExpression left, BoxedExpression right, BoxedExpression source)
		{
			if(CheckComparison(binaryOp, left, right, original) || 
			   CheckDevidingByConstant(binaryOp, left, right, original)||
			   (CheckSubstraction(binaryOp, left, right, original) || CheckAddition(binaryOp, left, right, original)))
        	return;
      		ReturnNotOkResult(source);			
		}
		
		private void SizeOf<Type>(Type type, int size, BoxedExpression source);
		
		private void ArrayIndex(Type type, BoxedExpression array, BoxedExpression itemIndex, BoxedExpression source);
		
		private void Result<Type>(Type type, BoxedExpression source);
		
		private void ReturnedValue<Type>(Type type, BoxedExpression expression, BoxedExpression source);
		
		private void Variable(object var, PathElement[] path, BoxedExpression source);
		
		private void Constant<Type>(Type type, object value, BoxedExpression source);
		
		#endregion
		
		#region Checkers 
		
		private bool CheckComparison(BinaryOperator binaryOp, BoxedExpression left, BoxedExpression right, BoxedExpression source)
		{
			 if (BinaryOperatorExtensions.IsComparisonBinaryOperator(binaryOp))
		     {
				BoxedExpression _left;
		     	left.Dispatch((IBoxedExpressionVisitor) this);
				
				if ((left1 = this.PartialResult) != null)
				{
					BoxedExpression _right;
					right.Dispatch((IBoxedExpressionVisitor) this);
          			if ((_right = this.LocalResult) != null)
			        {
			            ReturnOkResult(BoxedExpression.Binary(binaryOp, _left, _right, source.UnderlyingVariable));
			            return true;
			        }
				}
		     }
		     this.LocalResult = null;
		     return false;
		}
		
		private bool CheckDevidingByConstant(BinaryOperator binaryOp, BoxedExpression left, BoxedExpression right, BoxedExpression source)
		{
			int i;
			
			if((BoxedExpressionExtensions.IsConstantInt(right, out i) && i != 0) && 
			  ((binaryOp == BinaryOperator.Rem || binaryOp == BinaryOpertor.Rem_Un)||
			 	binaryOp == BinaryOpertor.Div || binaryOp == BinaryOpertor.Div_Un))
			{
				BoxedExpression _left;
				left.Dispatch((IBoxedExpressionController) this);
		        if ((_left = this.LocalResult) != null)
		        {
		          ReturnOkResult(BoxedExpression.Binary(binaryOp, _left, right, source.UnderlyingVariable));
		          return true;
		        }
			}
			this.LocalResult = null;
      		return false;
		}
		
		private bool CheckSubstraction(BinaryOperator binaryOp, BoxedExpression left, BoxedExpression right, BoxedExpression source)
		{
			if(binaryOp == BinaryOperator.Sub ||binaryOp == BinaryOperator.Sub_Ovf || binaryOp == BinaryOperator.Sub_Ovf_Un)
			{
				
				this.ReturnOkResult(left);
        		this.ReturnOkResult(right);
        		this.ReturnOkResult(original);
				return true;
			} 
			else
			{
				this.LocalResult = null;
				return false;
			}
		}
		
		private bool CheckAddition(BinaryOperator binaryOp, BoxedExpression left, BoxedExpression right, BoxedExpression source)
		{
			if(binaryOp == BinaryOperator.Add ||binaryOp == BinaryOperator.Add_Ovf || binaryOp == BinaryOperator.Add_Ovf_Un)
			{
				this.ReturnOkResult(left);
        		this.ReturnOkResult(right);
        		this.ReturnOkResult(original);
        		return true;
			} 
			else
			{
				this.LocalResult = null;
				return false;
			}
		}
		
		private bool CheckMovingSubtractionOnComparison(BinaryOperator binaryOp, BoxedExpression left, BoxedExpression right, BoxedExpression source)
		{
			int i;
      		BinaryOperator _binaryOp;
	      	BoxedExpression _left;
	      	BoxedExpression _right;
			
      		if (OperatorExtensions.IsComparisonBinaryOperator(binaryOp) && 
			    BoxedExpressionExtensions.IsConstantInt(right, out i) && 
			    (i == 0 && left.IsBinaryExpression(out binaryOp, out _left, out _right)) && 
			    (bop1 == BinaryOperator.Sub || binaryOp == BinaryOperator.Sub_Ovf || binaryOp == BinaryOperator.Sub_Ovf_Un))
			{
				_left.Dispatch((IBoxedExpressionController) this);
				BoxedExpression boxedExpression_1;
				if((boxedExpression_1 = this.LocalResult) != null)
				{
					ReturnOkResult(boxedExpression_1);
					_right.Dispatch((IBoxedExpressionController) this);
					BoxedExpression boxedExpression_2;
					
					if ((boxedExpression_2 = this.LocalResult) != null)
			        {
						ReturnOkResult(boxedExpression_2);
			            ReturnOkResult(BoxedExpression.Binary(_binaryOp, boxedExpression_1, boxedExpression_2, source.UnderlyingVariable));
			            return true;
			        }
				}
			}
      
      		this.LocalResult = (BoxedExpression) null;
      		return false;
		}
		
		private bool CheckSumRewriting(BinaryOperator binaryOp, BoxedExpression left, BoxedExpression right, BoxedExpression source)
		{
			int i;
      		BinaryOperator _binaryOp;
	      	BoxedExpression _left;
	      	BoxedExpression _right;
			
			if ((binaryOp == BinaryOperator.Div || binaryOp == BinaryOperator.Div_Un) && 
			    left.IsBinaryExpression(out _binaryOp, out _left, out _right) && 
			    (_binaryOp == BinaryOperator.Add || _binaryOp == BinaryOperator.Add_Ovf || _binaryOp == BinaryOperator.Add_Ovf_Un) &&
			    (BoxedExpressionExtensions.IsConstantInt(right, out i) && i == 2))
			{
				_left.Dispatch((IBoxedExpressionController) this);
        		BoxedExpression boxedExpression_1;
				if ((boxedExpression_1 = this.LocalResult) != null)
				{
					_right.Dispatch((IBoxedExpressionController) this);
          			BoxedExpression boxedExpression_2;
					if ((boxedExpression_2 = this.LocalResult) != null)
					{
						_left = (BoxedExpression) null;
						BoxedExpression boxedExpression_3 = BoxedExpression.Binary(BinaryOperator.Sub, boxedExpression_2, boxedExpression_1, (object) null);
						if(!this.Overflow.Overflow(PC, boxedExpression_3))
						{
							
						}
					}
				}
			}
      
		}
		
		private bool CheckRemovingAddInComparison(BinaryOperator binaryOp, BoxedExpression left, BoxedExpression right, BoxedExpression source)
		{
			
		}
		
		private bool CheckMovingExpressionAroundComparison(BinaryOperator binaryOp, BoxedExpression left, BoxedExpression right, BoxedExpression source)
		{
			
		}
		
		private bool CheckAltAndSub(BinaryOperator binaryOp, BoxedExpression left, BoxedExpression right, BoxedExpression source)
		{
			
		}
		
		private bool CheckRemoveWeakInequalities(BinaryOperator binaryOp, BoxedExpression left, BoxedExpression right, BoxedExpression source)
		{
			
		}
		
		#endregion
		
		private bool ReturnOkResult(BoxedExpression expression)
	    {
	      	this.LocalResult = expression;
	      	this.Expressions[expression] = (OverflowExpressionControl<Var>.Condition) 1;
	      	return true;
	    }
		
		private bool ReturnNotOkResult(BoxedExpression expression)
	    {
	       	this.LocalResult = null;
      		this.Expressions[exp] = (OverflowExpressionControl<Var>.State) 0;
      		return false;
	    }
	}
}

