using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace CDub.LogicEngine
{
	public class Action : Conditions.Condition, IEvaluator {

		protected const string NODENAME_KEY = "key";
		protected const string NODENAME_VALUE = "value";
		
		public Action (XmlNode node)
			: base(node) {
		}

		protected override void LoadOperands (XmlNode node) {
			if (node == null)
				return;

			XmlNode opNode;

			opNode = node.SelectSingleNode(NODENAME_VALUE);
			if (opNode != null)
				ReadOperand(opNode, out _input, out _inputSource);

			opNode = node.SelectSingleNode(NODENAME_KEY);
			if (opNode != null)
				ReadOperand(opNode, out _control, out _controlSource);
		}

		#region IEvaluator Members

		public override bool Evaluate (State state) {
			string value = LoadValue(_input, _inputSource, state);
			string key = LoadValue(_control, _controlSource, state);

			state.Output[key] = value;
			
			return (true);
		}

		#endregion

	}

}
