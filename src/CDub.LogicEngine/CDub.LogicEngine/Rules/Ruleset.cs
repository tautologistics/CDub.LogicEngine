using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Xml;

namespace CDub.LogicEngine
{

	public class RuleSet {

		private const string XPATH_RULE = "./rule";

		private Rules _rules;

		public RuleSet (XmlNode ruleSetNode) {
			_rules = new Rules();

			XmlNodeList ruleNodes = ruleSetNode.SelectNodes(XPATH_RULE);
			foreach (XmlNode ruleNode in ruleNodes) {
				_rules.Add(new Rule(ruleNode));
			}
		}

		public void Evaluate (State state) {
			foreach (Rule rule in _rules)
				rule.Evaluate(state);
		}

		public void Evaluate (State state, string ruleId) {
			if (!_rules.Contains(ruleId))
				return;
			_rules[ruleId].Evaluate(state);
		}

	}

	class Rules : KeyedCollection<string, Rule> {
		protected override string GetKeyForItem (Rule rule) {
			return (rule.Id);
		}
	}

}
