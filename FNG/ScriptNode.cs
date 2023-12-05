using System.Diagnostics;

namespace FNG
{
    public abstract class ScriptNode
    {
        public readonly Guid GUID;
        public string Name;
        public Graph Parent;

        public Dictionary<InputConnector, Action> ExecFunctions = new();
        public Dictionary<OutputConnector, Func<object?, object>> DataFunctions = new();

        public List<InputConnector> Inputs = new();
        public List<OutputConnector> Outputs = new();

        public ScriptNode(string name, Graph parent)
        {
            GUID = Guid.NewGuid();
            Name = name;
            Parent = parent;
            parent.Nodes.Add(GUID, this);
        }
        /// <summary>
        /// This should be overridden.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual void RunExeFunction(InputConnector targetconnector)
        {
            if (ExecFunctions.TryGetValue(targetconnector, out Action? value))
            {
                value();
            }
            else throw new UnknownFunctionException();
            return;
        }
        public virtual object RunDataFunction(OutputConnector targetconnector)
        {
            if (DataFunctions.TryGetValue(targetconnector, out Func<object?, object>? value))
            {
                return value(null);
            }
            else throw new UnknownFunctionException();
        }
    }
    public class InputDefaultConnector : ScriptNode
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="targetPort"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        public InputDefaultConnector(Graph parent, DataInputConnector targetPort, object value, Type type) : base("", parent)
        {
            DataOutputConnector output = new("", value, type, this);
            Name = "DefaultInputValue<"+type.ToString()+">";
            Outputs.Add(output);
        }
        public override object RunDataFunction(OutputConnector targetConnector)
        {
            return ((DataOutputConnector) Outputs[0]).Value;
        }
        public override void RunExeFunction(InputConnector targetconnector) {}
    }
}
