using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace CDub.LogicEngine.Conditions
{
	class Contains : Condition {

		public Contains (XmlNode node)
			: base(node) {
		}

		protected override bool Compare (string input, string control) {
			return (input.ToLower().Contains(control.ToLower()));
		}
	}
}
