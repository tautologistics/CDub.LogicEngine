using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace CDub.LogicEngine.Test.Console
{
	class Program {
		static void Main (string[] args) {

			XmlDocument rulesDoc = new XmlDocument();
			
			rulesDoc.Load(System.Environment.CurrentDirectory + System.IO.Path.DirectorySeparatorChar + "ruletest01.xml");

			Engine engine = new Engine(rulesDoc);

			State state = new State();

			XmlNodeList testInput = rulesDoc.SelectNodes("/ruleset/testinput/entry");
			foreach (XmlNode node in testInput) {
				string key = node.SelectSingleNode("key").InnerText;
				string value = node.SelectSingleNode("value").InnerText;
				state.Input[key] = value;
			}

			engine.Run(state);

			foreach (string key in state.Output.Keys) {
				System.Console.WriteLine(String.Format("{0,-30}: {1}", key, state.Output[key]));
			}

			XmlNodeList testOutput = rulesDoc.SelectNodes("/ruleset/testoutput/entry");
			bool foundErrors = false;
			foreach (XmlNode node in testOutput) {
				string key = node.SelectSingleNode("key").InnerText;
				string value = node.SelectSingleNode("value").InnerText;

				if (!state.Output.ContainsKey(key)) {
					foundErrors = true;
					System.Console.WriteLine("ERROR: missing output key \"{0}\"", key);
					continue;
				}

				string result = (string)state.Output[key];
				state.Output.Remove(key);
				if (!result.Equals(value)) {
					foundErrors = true;
					System.Console.WriteLine("ERROR: incorrect output value for key \"{0}\" : expected \"{1}\" but got \"{2}\"", key, value, result);
				}

			}

			if (state.Output.Keys.Count > 0) {
				foundErrors = true;
				System.Console.WriteLine("ERROR: {0} unexpected keys found", state.Output.Keys.Count);
				foreach (System.Collections.DictionaryEntry entry in state.Output) {
					System.Console.WriteLine("    \"{0}\" = \"{1}\"", entry.Key, entry.Value);
				}
			}

			if (!foundErrors) {
				System.Console.WriteLine("All tests passed");
			}
			else {
				System.Console.WriteLine("One or more tests failed.");
			}

//			System.Console.ReadLine();

			DateTime start;
			DateTime end;

			start = DateTime.Now;
			for (int i = 0; i < 100; i++) {
				engine = new Engine(rulesDoc);
				engine.Run(state);
			}
			end = DateTime.Now;
			System.Console.WriteLine("Uncached: {0}", (end - start));


			start = DateTime.Now;
			engine = new Engine(rulesDoc);
			for (int i = 0; i < 100000; i++) {
				engine.Run(state);
			}
			end = DateTime.Now;
			System.Console.WriteLine("Cached: {0}6s", (end - start));

		}
	}
}
