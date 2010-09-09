using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace CDub.LogicEngine.Conditions
{
	class Starts : Condition {

		public Starts (XmlNode node)
			: base(node) {
		}

		protected override bool Compare (string input, string control) {
			return (input.ToLower().IndexOf(control.ToLower()) == 0);
		}
	}
}
