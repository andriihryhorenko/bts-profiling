using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace balanced_bts.BTree
{
    public class Node
    {
        public Node LeftNode { get; set; }
        public Node RightNode { get; set; }
        public int Data { get; set; }

        public void PrintPretty(string indent, bool last)
        {

            Console.Write(indent);
            if (last)
            {
                Console.Write("└─");
                indent += "  ";
            }
            else
            {
                Console.Write("├─");
                indent += "| ";
            }
            Console.WriteLine(Data);

            var children = new List<Node>();
            if (this.LeftNode != null)
                children.Add(this.LeftNode);
            if (this.RightNode != null)
                children.Add(this.RightNode);

            for (int i = 0; i < children.Count; i++)
                children[i].PrintPretty(indent, i == children.Count - 1);

        }
    }

    public class Height
    {
        public int height = 0;
    }

}
