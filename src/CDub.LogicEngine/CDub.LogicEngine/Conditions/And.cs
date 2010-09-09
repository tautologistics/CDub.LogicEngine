using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace CDub.LogicEngine.Conditions
{
	class And : Clause {

		public And (XmlNode node)
			: base(node) {
			_startState = true;
		}

		protected override bool Test (IEvaluator condition, State state, ref bool currentState) {
			currentState = condition.Evaluate(state) && currentState;
			return (currentState);
		}
	}
}
