using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;

namespace Game.State.Data.Configuration.Editor.Nodes
{
    public static class Ext
    {
        public static List<int> GetConnectedOutputNodes(this Port port, GraphView graphView)
        {
            if (port.direction != Direction.Output) return new List<int>();
            var allNodes = graphView.nodes.ToList();

            return port.connections
                .Where(edge => edge.output == port)
                .Select(edge => edge.input.node)
                .Select(node => allNodes.IndexOf(node))
                .ToList();
        }

        public static List<int> GetConnectedInputNodes(this Port port, GraphView graphView)
        {
            if (port.direction != Direction.Input) return new List<int>();
            var allNodes = graphView.nodes.ToList();

            return port.connections
                .Where(edge => edge.input == port)
                .Select(edge => edge.output.node)
                .Select(node => allNodes.IndexOf(node))
                .ToList();
        }
    }
}