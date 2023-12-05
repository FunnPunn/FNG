namespace FNG
{
    static class ConnectionHandler
    {
        public static bool DoTypesMatch(InputConnector a, OutputConnector b) {
            if (a is ExecutionInputConnector && b is ExecutionOutputConnector) return true;
            else
            {
                DataInputConnector aD = (DataInputConnector) a;
                DataOutputConnector bD = (DataOutputConnector) b;
                if (aD.CType == bD.CType) return true;
            }
            return false;
        }
        public static void Connect(InputConnector mainConnector, OutputConnector targetConnector)
        {
            if (DoTypesMatch(mainConnector, targetConnector) && !(mainConnector.Connections.Contains(targetConnector) || targetConnector.Connections.Contains(mainConnector)))
            {
                if (mainConnector.Connections.Count + 1 > mainConnector.MaxConnections) mainConnector.DisconnectAll();
                if (targetConnector.Connections.Count + 1 > targetConnector.MaxConnections) targetConnector.DisconnectAll();

                mainConnector.Connections.Add(targetConnector);
                targetConnector.Connections.Add(mainConnector);
            }
        }
        public static void Disconnect(InputConnector mainConnector, OutputConnector targetConnector) {
            if (mainConnector.Connections.Contains(targetConnector))
            {
                mainConnector.Connections.Remove(targetConnector);
                targetConnector.Connections.Remove(mainConnector);
            }
        }
        public static void DisconnectAll(InputConnector mainConnector)
        {
            foreach (OutputConnector conn in mainConnector.Connections)
            {
                conn.Connections.Remove(mainConnector);
                mainConnector.Connections.Remove(conn);
            }
        }
        public static void DisconnectAll(OutputConnector mainConnector)
        {
            foreach (InputConnector conn in mainConnector.Connections)
            {
                conn.Connections.Remove(mainConnector);
                mainConnector.Connections.Remove(conn);
            }
        }
    }
}
