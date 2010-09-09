using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace CDub.LogicEngine.Conditions
{
	class Ends : Condition {

		public Ends (XmlNode node)
			: base(node) {
		}

		protected override bool Compare (string input, string control) {
			long inputLen = input.Length;
			long controlLen = control.Length;

			return (input.ToLower().IndexOf(control.ToLower()) == (inputLen - controlLen));
		}
	}
}
