using System;
using System.Collections.Generic;
//using System.Collections.ObjectModel;
//http://www.pluralsight.com/blogs/craig/archive/2005/10/11/15424.aspx
using System.Collections;
using System.Text;

namespace CDub.LogicEngine
{
	public class State {

		private Hashtable _input;
		private Hashtable _output;

		public State () {
			_input = new Hashtable();
			_output = new Hashtable();
		}

		public Hashtable Input {
			get { return _input; }
		}
		public Hashtable Output {
			get { return _output; }
		}

	}

}
