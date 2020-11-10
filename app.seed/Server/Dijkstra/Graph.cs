﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Model;

namespace Server.Dijkstra
{
    /// <summary>
    /// A graph
    /// </summary>
    /// <typeparam name="T">type of values stored in graph</typeparam>
    public class Graph<T>
    {
        #region Fields

        List<GraphNode<T>> nodes = new List<GraphNode<T>>();

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Graph()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of nodes in the graph
        /// </summary>
        public int Count
        {
            get { return nodes.Count; }
        }

        /// <summary>
        /// Gets a read-only list of the nodes in the graph
        /// </summary>
        public IList<GraphNode<T>> Nodes
        {
            get { return nodes.AsReadOnly(); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Clears all the nodes from the graph
        /// </summary>
        public void Clear()
        {
            // remove all the neighbors from each node
            // so nodes can be garbage collected
            foreach (GraphNode<T> node in nodes)
            {
                node.RemoveAllNeighbors();
            }

            // now remove all the nodes from the graph
            for (int i = nodes.Count - 1; i >= 0; i--)
            {
                nodes.RemoveAt(i);
            }
        }

        /// <summary>
        /// Adds a node with the given value to the graph. If a node
        /// with the same value is in the graph, the value 
        /// isn't added and the method returns false
        /// </summary>
        /// <param name="value">value to add</param>
        /// <returns>true if the value is added, false otherwise</returns>
        public bool AddNode(T value)
        {
            if (Find(value) != null)
            {
                // duplicate value
                return false;
            }
            else
            {
                nodes.Add(new GraphNode<T>(value));
                return true;
            }
        }

        /// <summary>
        /// Adds a weighted edge between the nodes with the given values 
        /// in the graph. If one or both of the values don't exist 
        /// in the graph the method returns false. If an edge
        /// already exists between the nodes the edge isn't added
        /// and the method retruns false
        /// </summary>
        /// <param name="value1">first value to connect</param>
        /// <param name="value2">second value to connect</param>
        /// <param name="weight">weight of the edge</param>
        /// <returns>true if the edge is added, false otherwise</returns>
        public bool AddEdge(T value1, T value2, double weight)
        {
            GraphNode<T> node1 = Find(value1);
            GraphNode<T> node2 = Find(value2);
            if (node1 == null ||
                node2 == null)
            {
                return false;
            }
            else if (node1.Neighbors.Contains(node2))
            {
                // edge already exists
                return false;
            }
            else
            {
                // undirected graph, so add as neighbors to each other
                node1.AddNeighbor(node2, weight);
                node2.AddNeighbor(node1, weight);
                return true;
            }
        }

        /// <summary>
        /// Removes the node with the given value from the graph.
        /// If the node isn't found in the graph, the method 
        /// returns false
        /// </summary>
        /// <param name="value">value to remove</param>
        /// <returns>true if the value is removed, false otherwise</returns>
        public bool RemoveNode(T value)
        {
            GraphNode<T> removeNode = Find(value);
            if (removeNode == null)
            {
                return false;
            }
            else
            {
                // need to remove as neighor for all nodes
                // in graph
                nodes.Remove(removeNode);
                foreach (GraphNode<T> node in nodes)
                {
                    node.RemoveNeighbor(removeNode);
                }
                return true;
            }
        }

        /// <summary>
        /// Removes an edge between the nodes with the given values 
        /// from the graph. If one or both of the values don't exist 
        /// in the graph the method returns false
        /// </summary>
        /// <param name="value1">first value to disconnect</param>
        /// <param name="value2">second value to disconnect</param>
        /// <returns>true if the edge is removed, false otherwise</returns>
        public bool RemoveEdge(T value1, T value2)
        {
            GraphNode<T> node1 = Find(value1);
            GraphNode<T> node2 = Find(value2);
            if (node1 == null ||
                node2 == null)
            {
                return false;
            }
            else if (!node1.Neighbors.Contains(node2))
            {
                // edge doesn't exist
                return false;
            }
            else
            {
                // undirected graph, so remove as neighbors to each other
                node1.RemoveNeighbor(node2);
                node2.RemoveNeighbor(node1);
                return true;
            }
        }

        /// <summary>
        /// Finds the graph node with the given value
        /// </summary>
        /// <param name="value">value to find</param>
        /// <returns>graph node or null if not found</returns>
        public GraphNode<T> Find(T value)
        {
            foreach (GraphNode<T> node in nodes)
            {
                if (node.Value.Equals(value))
                {
                    return node;
                }
            }
            return null;
        }

        #endregion

    }
}
