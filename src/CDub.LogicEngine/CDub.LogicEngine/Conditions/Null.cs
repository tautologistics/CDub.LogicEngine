using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace CDub.LogicEngine.Conditions
{
	class Null : Condition, IEvaluator {

		public Null (XmlNode node) : base(node) { }

		protected override bool Compare (string input, string control) {
			return(true);
		}

	}
}
