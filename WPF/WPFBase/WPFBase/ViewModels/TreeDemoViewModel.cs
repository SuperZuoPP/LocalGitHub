using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Xml.Linq;
using WPFBase.Models;

namespace WPFBase.ViewModels
{
    public class TreeDemoViewModel : BindableBase
    {
        private List<TreeNode> treenodes = new List<TreeNode>();
        public List<TreeNode> TreeNodes
        {
            get => treenodes;
            set => SetProperty(ref treenodes, value);
        }

        private List<TreeNode> nodes;
        public List<TreeNode> Nodes
        {
            get => nodes;
            set => SetProperty(ref nodes, value);
        }

        private TreeNode currentNode;
        public TreeNode CurrentNode
        {
            get => currentNode;
            set => SetProperty(ref currentNode, value);
        }
        public DelegateCommand<object> SelectCommand { get; set; }
        public TreeDemoViewModel()
        {
            Nodes = new List<TreeNode>()
            {
                new TreeNode(){ParentID=0, NodeID=1, NodeName = "书本" },
                new TreeNode(){ParentID=0, NodeID=2, NodeName="课桌"},
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
    }
}
