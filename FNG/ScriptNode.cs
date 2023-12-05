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
        /// Manually run the execution function for a specific input exec on the node.<br/>
        /// Please only use this when neccesary. You can now use ExecutionConnector.Run() instead.
        /// </summary>
        /// <param name="targetconnector"></param>
        /// <exception cref="UnknownFunctionException"></exception>
        public virtual void RunExeFunction(InputConnector targetconnector)
        {
            if (ExecFunctions.TryGetValue(targetconnector, out Action? value))
            {
                value();
            }
            else throw new UnknownFunctionException();
            return;
        }
        /// <summary>
        /// Manually run the data function for a specific output data connector on the node.<br/>
        /// Please only use this when neccesary. You can now use ExecutionConnector.Run() instead.
        /// </summary>
        /// <param name="targetconnector"></param>
        /// <exception cref="UnknownFunctionException"></exception>
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
