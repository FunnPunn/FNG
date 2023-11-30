using System.Security.Cryptography.X509Certificates;

namespace FNG
{
    /// <summary>
    /// You should only use one of these. This will be the main graph containing all nodes and subgraphs.<br/>
    /// If you want to make subgraphs, use SubGraph instead.
    /// </summary>
    public abstract class Graph
    {
        /// <summary>
        /// The nodes that this graph contains.
        /// </summary>
        public List<Node> Nodes = new();
        /// <summary>
        /// The subgraphs within this graph.
        /// </summary>
        public List<SubGraph> SubGraphs = new();
        public static void Serialize() {

        }
        public void Add(Node node) {
            Nodes.Add(node);
        }
        public void Remove(Node node) {
            Nodes.Remove(node);
        }

        public void Add(SubGraph graph) {
            SubGraphs.Add(graph);
        }
        public void Remove(SubGraph graph)
        {
            SubGraphs.Remove(graph);
        }
    }
    public class SubGraph : Graph
    {
        public Graph Parent;
        public SubGraph(Graph parent)
        {
            Parent = parent;
        }
    }
    public abstract class Node
    {
        public readonly Guid GUID;
        public string Name;
        public Graph Parent;

        public List<InputConnector> Inputs;
        public List<OutputConnector> Outputs;

        public Node(string name, Graph parent, List<InputConnector> inputs, List<OutputConnector> outputs)
        {
            GUID = Guid.NewGuid();
            Name = name;
            Parent = parent;
            Inputs = inputs;
            Outputs = outputs;
        }
        /// <summary>
        /// This should be overridden.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public void Run() {
            return;
        }
    }
    public abstract class Connector
    {
        public string Name = "";
        public Node Parent;

        public Connector(Node parent)
        {
            Parent = parent;
        }
    }
    public abstract class InputConnector : Connector
    {
        public InputConnector(Node parent):base(parent)
        {
            parent.Inputs.Add(this);
        }
    }
    public abstract class OutputConnector : Connector
    {
        public OutputConnector(Node parent):base(parent)
        {
            parent.Outputs.Add(this);
        }
    }
    public abstract class DataInputConnector : InputConnector
    {
        public readonly Type? CType;
        public DataOutputConnector? Connection;
        public DataInputConnector(Node parent, Type type) : base(parent)
        {
            CType = type;
        }
    }
    public abstract class DataOutputConnector : OutputConnector
    {
        public readonly Type? CType;
        public readonly object? Value;
        public List<DataOutputConnector> Connections = new();

        public DataOutputConnector(Node parent, object value, Type? type) : base(parent)
        {
            Value = value;
            CType = type != null ? type : value.GetType();
        }
    }
    public class ExecutionInputConnector : InputConnector
    {
        public List<ExecutionOutputConnector> Connections = new();
        public string TargetFunction = "";
        public ExecutionInputConnector(Node parent) : base(parent)
        {
        }
    }
    public class ExecutionOutputConnector : InputConnector
    {
        public ExecutionInputConnector? Connection;
        public ExecutionOutputConnector(Node parent) : base(parent)
        {
        }
    }
}
