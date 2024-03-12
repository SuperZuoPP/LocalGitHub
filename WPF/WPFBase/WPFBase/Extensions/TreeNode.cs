﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFBase.Extensions
{
    public class TreeNode
    {
        public int NodeID { get; set; }
        public int ParentID { get; set; }
        public string NodeName { get; set; }
        public List<TreeNode> ChildNodes { get; set; }
        public TreeNode()
        {
            ChildNodes = new List<TreeNode>();
        }
    }
}
