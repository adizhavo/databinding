using System;
using System.Text;
using System.Collections.Generic;

namespace DataBinding
{
    public class DataBindingService
    {
        private List<Node> dataRoots = new List<Node>();

        public const char DATA_BRANCH_SEPARATOR = '.';

        /// <summary>
        /// Adds data to the data tree as a node
        /// </summary>
        /// <param name="branch">Branch.</param>
        /// <param name="defaultValue">Default value.</param>
        /// <param name="overrideData">If set to <c>true</c> overrides the data on that branch.</param>
        /// <typeparam name="T">The data type.</typeparam>
        public DataBindingService AddData<T>(string branch, T defaultValue = default(T), bool overrideData = false)
        {
            if (string.IsNullOrEmpty(branch))
                throw new ArgumentNullException(branch, "Branch is null or empty, please provide a branch");

            var data = GetData<T>(branch);
            if (data == null)
            {
                data = new Data<T>();
                data.branch = branch;
                data.value = defaultValue;

                var splittedBranch = branch.Split(DATA_BRANCH_SEPARATOR);
                data.Id = splittedBranch[splittedBranch.Length - 1];
                data.treeDepth = splittedBranch.Length - 1;

                AddOrOverrideNodeToDataTree(data, branch, splittedBranch, overrideData);
            }
            else
            {
                data.value = defaultValue;
            }

            return this;
        }

        public Data<T> GetData<T>(string branch)
        {
            return ExtractNode(branch) as Data<T>;
        }

        /// <summary>
        /// Adds and creates the branch if no data branch is defined
        /// Overrides the data if the data branch is defined
        /// </summary>
        private void AddOrOverrideNodeToDataTree(Node insertion, string branch, string[] splittedBranch, bool overrideData)
        {
            var extracted = ExtractNode(insertion.branch);
            if (extracted != null)
            {
                if (overrideData)
                {
                    insertion.parent = extracted.parent;
                    extracted = insertion;
                }
                else
                {
                    Console.WriteLine("[{0}] branch {1} found but the data will not change.", GetType(), branch);
                }
            }
            else
                AddNodeToDataTree(insertion, splittedBranch);
        }

        private void AddNodeToDataTree(Node insertion, string[] splittedBranch)
        {
            // creates the data branch piece by piece and adds empty nodes
            var branch = new StringBuilder();
            var parentBranch = string.Empty;

            // loops not to the end, this loop will fill with empty nodes 
            // the actual data will be added as a sub node at the end
            for (int treeDepth = 0; treeDepth < splittedBranch.Length - 1; treeDepth++)
            {
                branch.Append(splittedBranch[treeDepth]);

                var extracted = ExtractNode(branch.ToString());
                if (extracted == null)
                    CreateEmptyNode(parentBranch, branch.ToString(), splittedBranch, treeDepth);
                
                parentBranch = branch.ToString();

                if (treeDepth != splittedBranch.Length - 2)
                    branch.Append(DATA_BRANCH_SEPARATOR);
            }

            // Add the data as a subNode to the parent branch
            var parent = ExtractNode(branch.ToString());
            parent.AddSubNode(insertion);
        }

        private void CreateEmptyNode(string parentBranch, string branch, string[] splittedBranch, int treeDepth)
        {
            var node = new Node();
            node.branch = branch;
            node.Id = splittedBranch[treeDepth];
            node.treeDepth = treeDepth;

            // We are adding it as a root
            if (treeDepth == 0)
            {
                Console.WriteLine("[{0}] Adding an empty node {1} as root", GetType(), splittedBranch[treeDepth]);
                dataRoots.Add(node);
            }
            else
            {
                var parent = ExtractNode(parentBranch);
                Console.WriteLine("[{0}] Adding an empty node {1} with parent {2}", GetType(), splittedBranch[treeDepth], parent.branch);
                parent.AddSubNode(node);
            }
        }

        public bool ContainsNode(string branch)
        {
            return ExtractNode(branch) != null;
        }

        /// <summary>
        /// Extracts node from a specific branch
        /// </summary>
        public Node ExtractNode(string branch)
        {
            if (!string.IsNullOrEmpty(branch))
            {
                string[] branchPath = branch.Split(DATA_BRANCH_SEPARATOR);
                var nodes = ExtractNodes(dataRoots, 0, branchPath.Length - 1, branchPath);
                return nodes != null && nodes.Count > 0 ? nodes[nodes.Count - 1] : null;
            }
            Console.WriteLine("[{0}] Error, the request is null or empty, please provide a valid branch, will return null", GetType());
            return null;
        }

        /// <summary>
        /// Extracts a collections of nodes out of a tree of nodes based on the depth
        /// </summary>
        public List<Node> ExtractNodes(List<Node> parents, int currentDepth, int treeDepth, string[] branchPath)
        {
            List<Node> extractedNodes = new List<Node>();

            foreach (var node in parents)
            {
                if (node.Id == branchPath[currentDepth])
                {
                    if (node.treeDepth == treeDepth)
                    {
                        extractedNodes.Add(node);
                        break;
                    }
                    else
                    {
                        List<Node> nodes = ExtractNodes(node.subNodes, currentDepth + 1, treeDepth, branchPath);
                        extractedNodes.AddRange(nodes);
                    }
                }
            }

            return extractedNodes;
        }

        public DataBindingService Bind<T>(string branch, BindingComponent<T> component)
        {
            if (!string.IsNullOrEmpty(branch))
            {
                var data = GetData<T>(branch);
                if (data == null)
                {
                    AddData<T>(branch, default(T), true);
                    data = GetData<T>(branch);
                }

                if (!data.bindedComponents.Contains(component))
                {
                    data.bindedComponents.Add(component);
                    component.OnValueChanged(data.branch, data.value);
                }
            }

            return this;
        }
    }
}