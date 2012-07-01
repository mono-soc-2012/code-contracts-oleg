using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Mono.CodeContracts.Static.DataStructures;
using Mono.CodeContracts.Static.Inference.Interface;
using Mono.CodeContracts.Static.Analysis;
using Mono.CodeContracts.Static.Proving;
using Mono.CodeContracts.Static.Analysis.Drivers;

namespace Mono.CodeContracts.Static.Inference
{
	public class PreconditionInferenceBackwardSymbolic<Local, Parameter, Method, Field, Property, Event, Type, Attribute, Assembly, Expression, Variable, LogOptions> : IPreconditionInference where Type : IEquatable<Type> where Expression : IEquatable<Expression> where Variable : IEquatable<Variable> where LogOptions : IFrameworkLogOptions
	{
		private const int TIMEOUT = 2;
	    private readonly IFactQuery<BoxedExpression, Variable> Facts;
	    private readonly IMethodDriver<Local, Parameter, Method, Field, Property, Event, Type, Attribute, Assembly, Expression, Variable, LogOptions> MDriver;
	    private readonly TimeOutChecker timeout;

		public PreconditionInferenceBackwardSymbolic(IFactQuery<BoxedExpression, Variable> facts, IMethodDriver<Local, Parameter, Method, Field, Property, Event, Type, Attribute, Assembly, Expression, Variable, LogOptions> mdriver)
	    {
	      this.Facts = facts;
	      this.MDriver = mdriver;
	      this.timeout = new TimeOutChecker(2, false);
	    }

		private void ObjectInvariant()
	    {
	    }
	}
}

