using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace CDub.LogicEngine.Conditions
{

	public enum DataSource {
		Input,
		Output,
		Static
	}

	public class Condition : IEvaluator {

		protected const string NODENAME_CONDITION = "condition";
		protected const string NODENAME_CONDITIONCONTAINER = "conditions";
		protected const string NODENAME_INPUT = "input";
		protected const string NODENAME_CONTROL = "control";

		protected const string XPATH_CONDITION = "./condition|./conditions";
		protected const string XPATH_OPERATOR = "op";
		protected const string XPATH_NOT = "not";
		protected const string XPATH_SOURCE = "source";

		protected const string SOURCE_INPUT = "input";
		protected const string SOURCE_OUTPUT = "output";
		protected const string SOURCE_STATIC = "static";

		protected const string OP_AND = "and";
		protected const string OP_OR = "or";

		protected const string OP_EMPTY = "empty";
		protected const string OP_EQUAL = "eq";
		protected const string OP_LESSTHAN = "lt";
		protected const string OP_GREATERTHAN = "gt";
		protected const string OP_CONTAINS = "ct";
		protected const string OP_STARTSWITH = "sw";
		protected const string OP_ENDSWITH = "ew";

		protected bool _notOp;
		protected string _input;
		protected DataSource _inputSource;
		protected string _control;
		protected DataSource _controlSource;

		public Condition (XmlNode node) {
			LoadNotSetting(node);
			LoadOperands(node);
		}

		protected virtual bool Compare (string input, string control) {
			return (true);
		}

		public static IEvaluator CreateCondition (XmlNode conditionNode) {
			if (conditionNode == null)
				return (new Null(null));

			string nodeName = conditionNode.Name;
			if (nodeName.Equals(NODENAME_CONDITION)) {
				XmlAttribute opType = conditionNode.Attributes[XPATH_OPERATOR];
				string opName = (opType != null) ? opType.Value : OP_EMPTY;
				switch (opName) {
					case OP_EQUAL:
						return (new Equal(conditionNode));

					case OP_LESSTHAN:
						return (new Less(conditionNode));

					case OP_GREATERTHAN:
						return (new Greater(conditionNode));

					case OP_CONTAINS:
						return (new Contains(conditionNode));

					case OP_STARTSWITH:
						return (new Starts(conditionNode));

					case OP_ENDSWITH:
						return (new Ends(conditionNode));

					default:
						// TODO: log warning about empty/unknown op type (OP_EMPTY)
						return (new Null(null));
				}
			}
			else if (nodeName.Equals(NODENAME_CONDITIONCONTAINER) || nodeName.Equals(NODENAME_CONDITIONCONTAINER)) {
				XmlAttribute opType = conditionNode.Attributes[XPATH_OPERATOR];
				string opName = (opType != null) ? opType.Value : OP_EMPTY;
				switch (opName) {
					case OP_EMPTY:
					case OP_AND:
						return (new And(conditionNode));

					case OP_OR:
						return (new Or(conditionNode));

					default:
						// TODO: log warning about empty/unknown op type (OP_EMPTY)
						return (new Null(null));
				}
			}

			return (new Null(null));
		}

		protected void ReadOperand (XmlNode node, out string _value, out DataSource source) {
			_value = (node.InnerText == null) ? "" : node.InnerText;
			XmlAttribute sourceAttr = node.Attributes[XPATH_SOURCE];
			string sourceName = (sourceAttr == null) ? "" : sourceAttr.Value;
			switch (sourceName) {
				case SOURCE_INPUT:
					source = DataSource.Input;
					break;

				case SOURCE_OUTPUT:
					source = DataSource.Output;
					break;

				case SOURCE_STATIC:
				default:
					source = DataSource.Static;
					break;
			}
		}

		protected virtual void LoadOperands (XmlNode node) {
			if (node == null)
				return;

			XmlNode opNode;

			opNode = node.SelectSingleNode(NODENAME_INPUT);
			if (opNode != null)
				ReadOperand(opNode, out _input, out _inputSource);

			opNode = node.SelectSingleNode(NODENAME_CONTROL);
			if (opNode != null)
				ReadOperand(opNode, out _control, out _controlSource);
		}

		protected void LoadNotSetting (XmlNode node) {
			if (node == null)
				return;

			XmlNode notNode = node.Attributes[XPATH_NOT];
			_notOp = false;

			if (notNode == null)
				return;

			string notValue = notNode.Value;

			_notOp = notValue.Equals("y") || notValue.Equals("1");
		}

		protected string LoadValue (string name, DataSource source, State state) {
			if (name == null || name == "")
				return ("");
			if (source == DataSource.Static)
				return (name);
			if (source == DataSource.Input)
				return (state.Input.Contains(name) ? (string)state.Input[name] : "");
			if (source == DataSource.Output)
				return (state.Output.Contains(name) ? (string)state.Output[name] : "");
			return(""); // TODO: log something here
		}

		#region IEvaluator Members

		public virtual bool Evaluate (State state) {
			return (
				Compare(
					LoadValue(_input, _inputSource, state),
					LoadValue(_control, _controlSource, state)
					)
				^ _notOp
				);
		}

		#endregion
	}
}
