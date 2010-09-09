using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace CDub.LogicEngine
{

	public class Engine {

		private const string XPATH_RULESET = "/ruleset";

		private XmlDocument _rulesDoc;
		private RuleSet _ruleSet;

		public Engine (XmlDocument rulesDoc) {
			_rulesDoc = rulesDoc;
			_ruleSet = ParseRules(_rulesDoc);
		}

		public void Run (State state) {
			_ruleSet.Evaluate(state);
		}

		public void Run (State state, string ruleId) {
			_ruleSet.Evaluate(state, ruleId);
		}

		static protected RuleSet ParseRules (XmlDocument rulesDoc) {
			XmlNode ruleSetNode = rulesDoc.SelectSingleNode(XPATH_RULESET);
			if (ruleSetNode == null)
				throw new Exception("Unable to locate ruleset node");

			return(new RuleSet(ruleSetNode));
		}

	}

}
