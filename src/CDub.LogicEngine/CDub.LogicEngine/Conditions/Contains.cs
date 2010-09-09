using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace CDub.LogicEngine.Conditions
{
	class Equal : Condition {

		public Equal (XmlNode node)
			: base(node) {
		}

		protected override bool Compare (string input, string control) {
			return (control.Equals(input));
		}
	}
}
