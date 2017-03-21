using System;
using System.Collections.Generic;

namespace DataBinding
{
    public class Node
    {
        // considered the last part of the branch
        public string Id;
        // the depth of this node in the tree
        public int treeDepth;
        // full branch 
        public string branch;

        public Node parent;
        public List<Node> subNodes = new List<Node>();

        public void AddSubNode(Node node)
        {
            foreach (var n in subNodes)
                if (n.branch.Equals(node.branch))
                {
                    #if DEBUG
                    Console.WriteLine($"[Node] node: {Id} is already a sub node of branch: {branch}");
                    #endif
                    return;
                }

            node.parent = this;
            subNodes.Add(node);

            #if DEBUG
            Console.WriteLine($"[Node] Adding node {node.branch} to {branch}");
            #endif
        }

        public void RemoveSubNode(Node node)
        {
            for (int n = 0; n < subNodes.Count; n++)
                if (subNodes[n].branch.Equals(node.branch))
                {
                    subNodes[n].parent = null;
                    subNodes.RemoveAt(n);
                    return;
                }

            #if DEBUG
            Console.Write($"[Node] node: {Id} is not part of branch: {branch}");
            #endif
        }

        public bool Contains(Node node)
        {
            if (branch.Equals(node.branch))
                return true;

            foreach (var n in subNodes)
                if (n.Contains(node))
                    return true;

            return false;
        }
    }

    public class Data<T> : Node
    {
        private T _value;
        public T value 
        {
            set 
            {   _value = value; 
                NotifyComponents(); 
            }
            get { return _value; }
        }

        public List<BindingComponent<T>> bindedComponents = new List<BindingComponent<T>>();

        public void NotifyComponents()
        {
            foreach(var component in bindedComponents)
                component.OnValueChanged(branch, value);
        }

        public override string ToString()
        {
            return $"\n{base.ToString()} branch: {branch}, value: {value}";
        }
    }
}