using System;
using System.Text;
using System.Collections.Generic;

namespace DataBinding
{
    public class DataBindingService
    {
        public const char DATA_BRANCH_SEPARATOR = '.';

        public List<Node> dataRoots = new List<Node>();

        /// <summary>
        /// Adds data to the data tree as a node
        /// </summary>
        /// <param name="branch">Branch.</param>
        /// <param name="defaultValue">Default value.</param>
        /// <param name="overrideData">If set to <c>true</c> overrides the data on that branch.</param>
        /// <typeparam name="T">The data type.</typeparam>
        public DataBindingService AddDataNode<T>(string branch, T defaultValue = default(T), bool overrideData = false)
        {
            if (string.IsNullOrEmpty(branch))
                throw new ArgumentNullException(branch, "Branch is null or empty, please provide a branch");

            var data = new Data<T>();
            data.branch = branch;
            data.value = defaultValue;

            var splittedBranch = branch.Split(DATA_BRANCH_SEPARATOR);
            data.Id = splittedBranch[splittedBranch.Length - 1];
            data.treeDepth = splittedBranch.Length - 1;

            AddOrOverrideNodeToDataTree(data, branch, splittedBranch, overrideData);
            return this;
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
                #if DEBUG
                else
                    Console.WriteLine($"[DataBindingService] branch {branch} found but the data will not change.");
                #endif
            }
            else
                AddNodeToDataTree(insertion, splittedBranch);
        }

        private void AddNodeToDataTree(Node insertion, string[] splittedBranch)
        {
            // creates the data branch piece by piece and adds empty nodes
            var branch = new StringBuilder();

            // loops not to the end, this loop will fill with empty nodes 
            // the actual data will be added as a sub node at the end
            for (int treeDepth = 0; treeDepth < splittedBranch.Length - 1; treeDepth++)
            {
                branch.Append(splittedBranch[treeDepth]);

                var extracted = ExtractNode(splittedBranch[treeDepth], treeDepth);
                if (extracted == null)
                    CreateEmptyNode(branch.ToString(), splittedBranch, treeDepth);

                if (treeDepth != splittedBranch.Length - 2) 
                    branch.Append(DATA_BRANCH_SEPARATOR);
            }

            // Add the data as a subNode to the parent branch
            var parent = ExtractNode(branch.ToString());
            parent.AddSubNode(insertion);
        }

        private void CreateEmptyNode(string branch, string[] splittedBranch, int treeDepth)
        {
            var node = new Node();
            node.branch = branch;
            node.Id = splittedBranch[treeDepth];
            node.treeDepth = treeDepth;

            // We are adding it as a root
            if (treeDepth == 0)
            {
                #if DEBUG
                Console.WriteLine($"[DataDindingService] Adding an empty node {splittedBranch[treeDepth]} as root");
                #endif
                dataRoots.Add(node);
            }
            else
            {
                var parent = ExtractNode(splittedBranch[treeDepth - 1], treeDepth - 1);
                #if DEBUG
                Console.WriteLine($"[DataBindingService] Adding an empty node {splittedBranch[treeDepth]} with parent {parent.branch}");
                #endif
                parent.AddSubNode(node);
            }
        }

        public bool ContainsNode(string Id, int treeDepth)
        {
            return ExtractNode(Id, treeDepth) != null;
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
                return ExtractNode(branchPath[branchPath.Length - 1], branchPath.Length - 1);
            }
            #if DEBUG
            else 
            Console.WriteLine("[DataBindingService] Error, the request is null or empty, please provide a valid branch, will return null");
            #endif

            return null;
        }

        /// <summary>
        /// Extracts nodes from its Id and depth in the data tree
        /// </summary>
        public Node ExtractNode(string Id, int treeDepth)
        {
            if (!string.IsNullOrEmpty(Id))
            {
                foreach(var node in ExtractNodes(dataRoots, treeDepth))
                    if (node.Id.Equals(Id))
                        return node;
            }
            #if DEBUG
            else 
            Console.WriteLine("[DataBindingService] Error, the request Id is null or empty, please provide a valid Id, will return null");
            #endif

            return null;
        }

        /// <summary>
        /// Extracts a collections of nodes out of a tree of nodes based on the depth
        /// </summary>
        public List<Node> ExtractNodes(List<Node> parents, int treeDepth)
        {
            List<Node> extractedNodes = new List<Node>();

            foreach (var node in parents)
            {
                if (node.treeDepth == treeDepth)
                    extractedNodes.Add(node);
                else
                {
                    List<Node> nodes = ExtractNodes(node.subNodes, treeDepth);
                    extractedNodes.AddRange(nodes);
                }
            }

            return extractedNodes;
        }
    }
}