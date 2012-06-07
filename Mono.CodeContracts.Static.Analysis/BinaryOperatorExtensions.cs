using System;

namespace Mono.CodeContracts.Static.Analysis
{
	public class BinaryOperatorExtensions
	{
		public static bool IsComparisonBinaryOperator(BinaryOperator binOp)
	    {
	      switch (binOp)
	      {
	        case BinaryOperator.Ceq:
	        case BinaryOperator.Cobjeq:
	        case BinaryOperator.Cne_Un:
	        case BinaryOperator.Cge:
	        case BinaryOperator.Cge_Un:
	        case BinaryOperator.Cgt:
	        case BinaryOperator.Cgt_Un:
	        case BinaryOperator.Cle:
	        case BinaryOperator.Cle_Un:
	        case BinaryOperator.Clt:
	        case BinaryOperator.Clt_Un:
	          return true;
	        default:
	          return false;
	      }
	    }
	}
}

