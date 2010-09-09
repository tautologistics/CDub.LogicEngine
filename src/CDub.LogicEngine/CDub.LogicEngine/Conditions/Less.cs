using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace CDub.LogicEngine.Conditions
{
	class Less : Condition {

		public Less (XmlNode node)
			: base(node) {
		}

		protected override bool Compare (string input, string control) {
			Decimal dInput;
			Decimal dControl;

			if (
				!System.Decimal.TryParse(input, out dInput)
				||
				!System.Decimal.TryParse(control, out dControl)
				) {
				dInput = input.CompareTo(control);
				dControl = 0;
			}
			
			return(dInput < dControl);
		}
	}
}
