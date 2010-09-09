using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace CDub.LogicEngine.Conditions
{
	class Clause : Condition {

		protected List<IEvaluator> _conditions;

		protected bool _startState;

		public Clause (XmlNode node)
			: base(node) {
			_conditions = new List<IEvaluator>();
			XmlNodeList conditions = node.SelectNodes(XPATH_CONDITION);
			foreach (XmlNode condition in conditions) {
				_conditions.Add(Condition.CreateCondition(condition));
			}
		}

		protected virtual bool Test (IEvaluator condition, State state, ref bool currentState) {
			currentState = false;
			return (currentState);
		}

		#region IEvaluator Members

		public override bool Evaluate (State state) {
			bool currentState = _startState;
			foreach (IEvaluator condition in _conditions) {
				if (!Test(condition, state, ref currentState)) {
					break;
				}
			}
			return (currentState ^ _notOp);
		}

		#endregion
	}
}
