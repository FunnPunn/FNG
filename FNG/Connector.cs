﻿namespace FNG
{
    public abstract class Connector
    {
        public string Name = "";
        public ScriptNode? Parent;
        public int MaxConnections = 0;
        public Connector(string name, ScriptNode parent)
        {
            Name = name;
        }
    }
    public abstract class InputConnector : Connector
    {
        public List<OutputConnector> Connections = new();
        public InputConnector(string name, ScriptNode parent) : base(name, parent) { }
        public void Connect(OutputConnector targetConnector)
        {

        }
        public void DisconnectAll()
        {
            ConnectionHandler.DisconnectAll(this);
        }
    }
    public abstract class OutputConnector : Connector
    {
        public List<InputConnector> Connections = new();
        public OutputConnector(string name, ScriptNode parent) : base(name, parent) { }
        public void Connect(OutputConnector targetConnector)
        {

        }
        public void DisconnectAll()
        {
            ConnectionHandler.DisconnectAll(this);
        }
    }
    public class DataInputConnector : InputConnector
    {
        public readonly Type CType;
        public DataInputConnector(string name, Type type, ScriptNode parent) : base(name, parent)
        {
            CType = type;
            MaxConnections = 1;
        }
    }
    public class DataOutputConnector : OutputConnector
    {
        public readonly Type CType;
        public readonly object Value;

        public DataOutputConnector(string name, object value, Type? type, ScriptNode parent) : base(name, parent)
        {
            Value = value;
            CType = type ?? value.GetType();
            MaxConnections = int.MaxValue;
        }
    }
    public class ExecutionInputConnector : InputConnector
    {
        public ExecutionInputConnector(string name, ScriptNode parent) : base(name, parent)
        {
            MaxConnections = int.MaxValue;
        }
    }
    public class ExecutionOutputConnector : OutputConnector
    {
        public ExecutionOutputConnector(string name, ScriptNode parent) : base(name, parent)
        {
            MaxConnections = 1;
        }
    }
}