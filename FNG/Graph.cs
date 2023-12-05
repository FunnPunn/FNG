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
        public List<ScriptNode> Nodes = new();
        /// <summary>
        /// The subgraphs within this graph.
        /// </summary>
        public List<Graph> SubGraphs = new();
        /// <summary>
        /// There should be only one parentless graph. This will be the main graph.
        /// </summary>
        public Graph? Parent;
        public static void Serialize()
        {

        }
        public void Add(ScriptNode node)
        {
            Nodes.Add(node);
        }
        public void Remove(ScriptNode node)
        {
            Nodes.Remove(node);
        }
        public void Add(Graph graph)
        {
            SubGraphs.Add(graph);
        }
        public void Remove(Graph graph)
        {
            SubGraphs.Remove(graph);
        }
    }
}
