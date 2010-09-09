using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace CDub.LogicEngine
{
	public class Rule : IEvaluator {
		private const string NODENAME_RULE = "rule";
		private const string NODENAME_ACTION = "output";

		private const string XPATH_RULEID = "id";
		private const string XPATH_CONDITIONS = "./conditions";
		private const string XPATH_SUCCESS = "./success/output|./success/rule";
		private const string XPATH_FAILURE = "./failure/output|./failure/rule";

		private IEvaluator _conditions;
		private List<IEvaluator> _successActions;
		private List<IEvaluator> _failureActions;

		public Rule (XmlNode node) {
			_successActions = new List<IEvaluator>();
			_failureActions = new List<IEvaluator>();

			XmlAttribute ruleIdAttr = node.Attributes[XPATH_RULEID];
			if (ruleIdAttr != null)
				this.Id = ruleIdAttr.InnerText;

			XmlNode conditionsNode = node.SelectSingleNode(XPATH_CONDITIONS);
			_conditions = Conditions.Condition.CreateCondition(conditionsNode);

			_conditions = Conditions.Condition.CreateCondition(conditionsNode);

			XmlNodeList actions;

			actions = node.SelectNodes(XPATH_SUCCESS);
			ReadActions(actions, _successActions);

			actions = node.SelectNodes(XPATH_FAILURE);
			ReadActions(actions, _failureActions);
		}

		private static void ReadActions (XmlNodeList actionsNode, List<IEvaluator> actions) {
			foreach (XmlNode actionNode in actionsNode) {
				switch (actionNode.Name) {
					case NODENAME_RULE:
						actions.Add(new Rule(actionNode));
						break;

					case NODENAME_ACTION:
					default:
						actions.Add(new Action(actionNode));
						break;
				}
			}
		}

		private string _id;
		public string Id {
			get { return _id; }
			set { _id = value; }
		}

		private void RunActions (List<IEvaluator> actions, State state) {
			foreach (IEvaluator action in actions)
				action.Evaluate(state);
		}

		#region IEvaluator Members

		public bool Evaluate (State state) {
			bool result = _conditions.Evaluate(state);
			
			if (result)
				RunActions(_successActions, state);
			else
				RunActions(_failureActions, state);

			return (result);
		}

		#endregion
	}

}
