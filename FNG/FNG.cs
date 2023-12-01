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

        public Dictionary<InputConnector, Action> ExecFunctions = new();
        public Dictionary<OutputConnector, Action> DataFunctions = new();

        public List<InputConnector> Inputs = new();
        public List<OutputConnector> Outputs = new();

        public Node(string name, Graph parent)
        {
            GUID = Guid.NewGuid();
            Name = name;
            Parent = parent;
        }
        /// <summary>
        /// This should be overridden.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public void RunExeFunction(InputConnector targetconnector) {
            if (targetconnector is ExecutionInputConnector)
            {
                ExecFunctions[targetconnector]();
            }
            return;
        }
        public void RunDataFunction(OutputConnector targetconnector)
        {
            if (targetconnector is DataOutputConnector)
            {
                DataFunctions[targetconnector]();
            }
            return;
        }
        public void Run() {
            // assuming port 0 is an exec here
            List<InputConnector> conn = Outputs[0].Connections;
            if (conn.Count == 1)
            {
                conn[0].Parent.RunExeFunction(conn[0]);
            }
        }
    }
    public abstract class Connector
    {
        public string Name = "";
        public Node Parent;
        
        public Connector(string name, Node parent)
        {
            Name = name;
            Parent = parent;
        }
    }
    public abstract class InputConnector : Connector
    {
        public List<OutputConnector> Connections = new();
        public InputConnector(string name, Node parent):base(name, parent)
        {
            parent.Inputs.Add(this);
        }
    }
    public abstract class OutputConnector : Connector
    {
        public List<InputConnector> Connections = new();
        public OutputConnector(string name, Node parent):base(name, parent)
        {
            parent.Outputs.Add(this);
        }
    }
    public abstract class DataInputConnector : InputConnector
    {
        public readonly Type CType;
        public DataInputConnector(string name, Node parent, Type type):base(name, parent)
        {
            CType = type;
        }
    }
    public abstract class DataOutputConnector : OutputConnector
    {
        public readonly Type CType;
        public readonly object Value;

        public DataOutputConnector(string name, Node parent, object value, Type? type):base(name, parent)
        {
            Value = value;
            CType = type ?? value.GetType();
        }
    }
    public class ExecutionInputConnector : InputConnector
    {
        public ExecutionInputConnector(string name, Node parent):base(name, parent)
        {
        }
    }
    public class ExecutionOutputConnector : InputConnector
    {
        public ExecutionOutputConnector(string name, Node parent):base(name, parent)
        {
        }
    }
}
