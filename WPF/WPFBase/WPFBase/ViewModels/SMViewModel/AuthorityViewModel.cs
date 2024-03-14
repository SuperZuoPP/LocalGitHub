using Prism.Commands;
using Prism.Ioc;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using WPFBase.Models;

namespace WPFBase.ViewModels.SMViewModel
{
    public class AuthorityViewModel : NavigationViewModel
    {

        public AuthorityViewModel(IContainerProvider provider) : base(provider)
        {
            Nodes = new List<TreeNode>()
            {
                new TreeNode(){ParentID=0, NodeID=1, NodeName = "书本" },
                new TreeNode(){ParentID=0, NodeID=2, NodeName="课桌",IsCheck=true,IsExpand=true },
                new TreeNode(){ParentID=0,NodeID=3, NodeName="文具"},
                new TreeNode(){ParentID=1, NodeID=4, NodeName="书本名"},
                new TreeNode(){ParentID=1, NodeID=5, NodeName="作者"},
                new TreeNode(){ParentID=2, NodeID=6, NodeName="材质"},
                new TreeNode(){ParentID=3, NodeID=7, NodeName="品牌1"},
                new TreeNode(){ParentID=6, NodeID=8, NodeName="材质1"},
                new TreeNode(){ParentID=6, NodeID=9, NodeName="材质2"},
                new TreeNode(){ParentID=2, NodeID=10,NodeName="编号"},
                new TreeNode(){ParentID=3, NodeID=11, NodeName="品牌2"}
            };
            TreeNodes = getChildNodes(0, Nodes);

            SelectCommand = new DelegateCommand<object>(Select);
            CheckItemCmd = new DelegateCommand<TreeNode>(CheckItem);
        }

        #region 属性
        private List<TreeNode> treenodes = new List<TreeNode>();
        public List<TreeNode> TreeNodes
        {
            get { return treenodes; }
            set { SetProperty<List<TreeNode>>(ref treenodes, value); }
        }

        private List<TreeNode> nodes;
        public List<TreeNode> Nodes
        {
            get { return nodes; }
            set { SetProperty<List<TreeNode>>(ref nodes, value); }
        }

        private TreeNode currentNode;
        public TreeNode CurrentNode
        {
            get { return currentNode; }
            set { SetProperty<TreeNode>(ref currentNode, value); }
        }
        #endregion


        #region 命令
        public DelegateCommand<object> SelectCommand { get; set; }

        public DelegateCommand<TreeNode> CheckItemCmd { get; set; }

        #endregion


        #region 方法
        private void CheckItem(TreeNode treeNode)
        {
            CurrentNode = treeNode;
            treeNode.IsExpand = true;
            List<TreeNode> CheckList = treeNode.ChildNodes; 
            foreach (var item in CheckList)
            {
                if (treeNode.IsCheck == true)
                    item.IsCheck = true;
                else
                    item.IsCheck = false;
            }
        }

        private void Select(object obj)
        {
            var res = obj as TreeNode;
            if (res != null && res.ChildNodes.Count == 0)
            {
                CurrentNode = res;
            }
        }

        private List<TreeNode> getChildNodes(int parentID, List<TreeNode> nodes)
        {
            List<TreeNode> mainNodes = nodes.Where(x => x.ParentID == parentID).ToList();
            List<TreeNode> otherNodes = nodes.Where(x => x.ParentID != parentID).ToList();
            foreach (TreeNode node in mainNodes)
                node.ChildNodes = getChildNodes(node.NodeID, otherNodes);
            return mainNodes;
        }

        #endregion
    }
}
