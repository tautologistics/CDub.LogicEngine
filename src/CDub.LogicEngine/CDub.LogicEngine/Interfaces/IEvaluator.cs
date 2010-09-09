using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace CDub.LogicEngine
{

	public interface IEvaluator {
		bool Evaluate (State state);
	}

}
