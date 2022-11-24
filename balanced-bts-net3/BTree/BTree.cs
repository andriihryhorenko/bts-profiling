using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace balanced_bts.BTree
{
    public class BinaryTree
    {
        public Node Root { get; set; }

        public bool Add(int value)
        {
            Node before = null, after = this.Root;

            while (after != null)
            {
                before = after;
                if (value < after.Data) //Is new node in left tree? 
                    after = after.LeftNode;
                else if (value > after.Data) //Is new node in right tree?
                    after = after.RightNode;
                else
                {
                    //Exist same value
                    return false;
                }
            }

            Node newNode = new Node();
            newNode.Data = value;

            if (this.Root == null)//Tree ise empty
                this.Root = newNode;
            else
            {
                if (value < before.Data)
                    before.LeftNode = newNode;
                else
                    before.RightNode = newNode;
            }

            return true;
        }

        public Node Find(int value)
        {
            return this.Find(value, this.Root);
        }

        public void Remove(int value)
        {
            this.Root = Remove(this.Root, value);
        }

        private Node Remove(Node parent, int key)
        {
            if (parent == null) return parent;

            if (key < parent.Data) parent.LeftNode = Remove(parent.LeftNode, key);
            else if (key > parent.Data)
                parent.RightNode = Remove(parent.RightNode, key);

            // if value is same as parent's value, then this is the node to be deleted  
            else
            {
                // node with only one child or no child  
                if (parent.LeftNode == null)
                    return parent.RightNode;
                else if (parent.RightNode == null)
                    return parent.LeftNode;

                // node with two children: Get the inorder successor (smallest in the right subtree)  
                parent.Data = MinValue(parent.RightNode);

                // Delete the inorder successor  
                parent.RightNode = Remove(parent.RightNode, parent.Data);
            }

            return parent;
        }

        private int MinValue(Node node)
        {
            int minv = node.Data;

            while (node.LeftNode != null)
            {
                minv = node.LeftNode.Data;
                node = node.LeftNode;
            }

            return minv;
        }

        private Node Find(int value, Node parent)
        {
            if (parent != null)
            {
                if (value == parent.Data) return parent;
                if (value < parent.Data)
                    return Find(value, parent.LeftNode);
                else
                    return Find(value, parent.RightNode);
            }

            return null;
        }

        public int GetTreeDepth()
        {
            return this.GetTreeDepth(this.Root);
        }

        private int GetTreeDepth(Node parent)
        {
            return parent == null ? 0 : Math.Max(GetTreeDepth(parent.LeftNode), GetTreeDepth(parent.RightNode)) + 1;
        }

        public void TraversePreOrder(Node parent, ref List<Node> result)
        {
            if (parent != null)
            {
                //Console.Write(parent.Data + " ");
                result.Add(parent);
                TraversePreOrder(parent.LeftNode, ref result);
                TraversePreOrder(parent.RightNode, ref result);
            }
        }

        public void TraversePreOrder(Node parent, ref List<int> result)
        {
            if (parent != null)
            {
                //Console.Write(parent.Data + " ");
                result.Add(parent.Data);
                TraversePreOrder(parent.LeftNode, ref result);
                TraversePreOrder(parent.RightNode, ref result);
            }
        }

        public void TraverseInOrder(Node parent, ref List<Node> result)
        {
            if (parent != null)
            {
                TraverseInOrder(parent.LeftNode, ref result);
                result.Add(parent);
                //Console.Write(parent.Data + " ");
                TraverseInOrder(parent.RightNode, ref result);
            }
            
        }

        public void TraversePostOrder(Node parent, ref List<Node> result)
        {
            if (parent != null)
            {
                TraversePostOrder(parent.LeftNode, ref result);
                TraversePostOrder(parent.RightNode, ref result);
                result.Add(parent);
                //Console.Write(parent.Data + " ");
            }
        }

        public void Print()
        {
            if (Root != null)
            {
                Root.PrintPretty("", true);

                var height = new Height();
                Console.WriteLine($"Tree balanced: {CheckHeightBalance(Root, height)}");
            }
        }



        ////recursive building of bTree
        //private void StoreBSTNodes(Node root, List<Node> nodes)
        //{
        //    if (root == null)
        //    {
        //        return;
        //    }

        //    StoreBSTNodes(root.LeftNode, nodes);
        //    nodes.Add(root);
        //    StoreBSTNodes(root.RightNode, nodes);
        //}

        //private Node BuildTreeUtil(List<Node> nodes, int start, int end)
        //{
        //    // base case
        //    if (start > end)
        //    {
        //        return null;
        //    }

        //    /* Get the middle element and make it root */
        //    int mid = (start + end) / 2;
        //    Node node = nodes[mid];

        //    /* Using index in Inorder traversal, construct
        //       left and right subtress */
        //    node.LeftNode = BuildTreeUtil(nodes, start, mid - 1);
        //    node.RightNode = BuildTreeUtil(nodes, mid + 1, end);

        //    return node;
        //}



        private Node BuildBalancedTree(List<int> values, int min, int max)
        {
            if (min == max)
                return null;

            int median = min + (max - min) / 2;
            return new Node
            {
                Data = values[median],
                LeftNode = BuildBalancedTree(values, min, median),
                RightNode = BuildBalancedTree(values, median + 1, max)
            };
        }

        public void BuildBalancedTree(List<int> values)
        {
            Root =  BuildBalancedTree(
                values, 0, values.Count());
        }

        // This functions converts an unbalanced BST to
        // a balanced BST
        //public virtual Node BuildTree(Node root)
        //{

        //    List<Node> nodes = new List<Node>();
        //    StoreBSTNodes(root, nodes);

        //    // Constructs BST from nodes[]
        //    int n = nodes.Count;
        //    return BuildTreeUtil(nodes, 0, n - 1);
        //}


        //public void BuildInOrderTree(List<Node> nodes)
        //{
        //    var firstNode = nodes.FirstOrDefault();
        //    Root = new Node()
        //    {
        //        Data = firstNode.Data
        //    };

        //    var childNodes = nodes.Skip(1).ToList();
        //    var rootRef = Root;

        //    foreach(var nd in childNodes)
        //    {
        //        if (nd != null)
        //        {
        //            rootRef.LeftNode = new Node() { Data = nd.Data};
        //            rootRef = rootRef.LeftNode;
        //        }
        //    }
        //}



        private bool CheckHeightBalance(Node root, Height height)
        {
            if (root == null)
            {
                height.height = 0;
                return true;
            }

            Height leftHeighteight = new Height(), rightHeighteight = new Height();
            var l = CheckHeightBalance(root.LeftNode, leftHeighteight);
            var r = CheckHeightBalance(root.RightNode, rightHeighteight);
            int leftHeight = leftHeighteight.height, rightHeight = rightHeighteight.height;

            height.height = (leftHeight > rightHeight ? leftHeight : rightHeight) + 1;

            if ((leftHeight - rightHeight >= 2) || (rightHeight - leftHeight >= 2))
                return false;

            else
                return l && r;
        }



    }
}
