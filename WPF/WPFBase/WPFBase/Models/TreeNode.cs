using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFBase.Models
{
    public class TreeNode
    {
        public int NodeID { get; set; }
        public int ParentID { get; set; }
        public string NodeName { get; set; }
          
        public bool? IsCheck { get; set; }=false;

        public bool IsExpand { get; set; }

        public List<TreeNode> ChildNodes { get; set; }


        public TreeNode()
        {
            ChildNodes = new List<TreeNode>();
        }
    }
}
