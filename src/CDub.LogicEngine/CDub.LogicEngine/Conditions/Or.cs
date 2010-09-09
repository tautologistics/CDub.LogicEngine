using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace CDub.LogicEngine.Conditions
{
	class Or : Clause {

		public Or (XmlNode node)
			: base(node) {
			_startState = false;
		}

		protected override bool Test (IEvaluator condition, State state, ref bool currentState) {
			currentState = condition.Evaluate(state) || _startState;
			return(!currentState);
		}
	}
}
