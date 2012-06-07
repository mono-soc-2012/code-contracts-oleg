using Mono.CodeContracts.Static.Proving;
using Mono.CodeContracts.Static.Analysis;

namespace Mono.CodeContracts.Static.Inference
{
	interface IBoxedExpressionController
	{
		void Unary(UnaryOperator unaryOp,UnaryOperator arg, BoxedExpression source);
		
		void Binary(BinaryOperator binaryOp, BoxedExpression fromLeft, BoxedExpression toRight, BoxedExpression source);
		
		void SizeOf<Type>(Type type, int size, BoxedExpression source);
		
		void ArrayIndex(Type type, BoxedExpression array, BoxedExpression itemIndex, BoxedExpression source);
		
		void Result<Type>(Type type, BoxedExpression source);
		
		void ReturnedValue<Type>(Type type, BoxedExpression expression, BoxedExpression source);
		
		void Variable(object var, PathElement[] path, BoxedExpression source);
		
		void Constant<Type>(Type type, object value, BoxedExpression source);
		
	}
}

