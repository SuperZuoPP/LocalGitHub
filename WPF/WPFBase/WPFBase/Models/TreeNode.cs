using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFBase.Models
{
    public class TreeNode: BindableBase
    {
        public int NodeID { get; set; }
        public int ParentID { get; set; }
        public string NodeName { get; set; }
            
        private bool isCheck;

        public bool IsCheck
        {
            get { return isCheck; }
            set { SetProperty<bool>(ref isCheck, value); }
        }
         

        private bool isExpand;

        public bool IsExpand
        {
            get { return isExpand; }
            set { SetProperty<bool>(ref isExpand, value); }
        }

        private List<TreeNode> childNodes;

        public List<TreeNode> ChildNodes
        {
            get { return childNodes; }
            set { SetProperty<List<TreeNode>>(ref childNodes, value); }
        } 


        public TreeNode()
        {
            ChildNodes = new List<TreeNode>();
             
        }
         
    }
}
