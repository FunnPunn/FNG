namespace FNG
{
    public abstract class ScriptNode
    {
        public readonly Guid GUID;
        public string Name;
        public Graph Parent;

        public Dictionary<InputConnector, Action> ExecFunctions = new();
        public Dictionary<OutputConnector, Action> DataFunctions = new();

        public List<InputConnector> Inputs = new();
        public List<OutputConnector> Outputs = new();

        public ScriptNode(string name, Graph parent)
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
        public void RunExeFunction(InputConnector targetconnector)
        {
            if (ExecFunctions.ContainsKey(targetconnector))
            {
                ExecFunctions[targetconnector]();
            }
            else throw new UnknownFunctionException();
            return;
        }
        public void RunDataFunction(OutputConnector targetconnector)
        {
            if (DataFunctions.ContainsKey(targetconnector))
            {
                DataFunctions[targetconnector]();
            }
            else throw new UnknownFunctionException();
            return;
        }
    }
}
