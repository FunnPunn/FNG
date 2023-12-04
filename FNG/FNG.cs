namespace FNG
{
    /// <summary>
    /// You should only use one of these. This will be the main graph containing all nodes and subgraphs.<br/>
    /// If you want to make subgraphs, use SubGraph instead.
    /// </summary>
    public class Graph
    {
        /// <summary>
        /// The nodes that this graph contains.
        /// </summary>
        public List<Node> Nodes = new();
        /// <summary>
        /// The subgraphs within this graph.
        /// </summary>
        public List<Graph> SubGraphs = new();
        public Graph? Parent;
        public static void Serialize() {

        }
        public void Add(Node node) {
            Nodes.Add(node);
        }
        public void Remove(Node node) {
            Nodes.Remove(node);
        }

        public void Add(Graph graph) {
            SubGraphs.Add(graph);
        }
        public void Remove(Graph graph)
        {
            SubGraphs.Remove(graph);
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
            if (conn.Count == 1 && conn[0].Parent != null)
            {
                conn[0].Parent.RunExeFunction(conn[0]);
            }
        }
    }
    public abstract class Connector
    {
        public string Name = "";
        public Node? Parent;
        
        public Connector(string name)
        {
            Name = name;
        }
    }
    public abstract class InputConnector : Connector
    {
        public List<OutputConnector> Connections = new();
        public InputConnector(string name):base(name)
        {
            
        }
    }
    public abstract class OutputConnector : Connector
    {
        public List<InputConnector> Connections = new();
        public OutputConnector(string name):base(name)
        {
            
        }
    }
    public abstract class DataInputConnector : InputConnector
    {
        public readonly Type CType;
        public DataInputConnector(string name, Type type):base(name)
        {
            CType = type;
        }
    }
    public abstract class DataOutputConnector : OutputConnector
    {
        public readonly Type CType;
        public readonly object Value;

        public DataOutputConnector(string name, object value, Type? type):base(name)
        {
            Value = value;
            CType = type ?? value.GetType();
        }
    }
    public class ExecutionInputConnector : InputConnector
    {
        public ExecutionInputConnector(string name):base(name)
        {
        }
    }
    public class ExecutionOutputConnector : InputConnector
    {
        public ExecutionOutputConnector(string name):base(name)
        {
        }
    }
}
