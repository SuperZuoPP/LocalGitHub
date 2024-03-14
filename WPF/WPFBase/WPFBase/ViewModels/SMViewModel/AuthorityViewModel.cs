using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using WPFBase.Models;
using WPFBase.Services;
using WPFBase.Shared.DTO.BM;

namespace WPFBase.ViewModels.SMViewModel
{
    public class AuthorityViewModel : NavigationViewModel
    {
        private readonly IMenuService service;
        private readonly IUserGroupService userGroupService;

        public AuthorityViewModel(IContainerProvider provider, IMenuService service, IUserGroupService userGroupService) : base(provider)
        {
            
            //GroupList = new ObservableCollection<Group>();
            //GroupList.Add(new Group { GroupId = "1", GroupName = "Group 1" });
            //GroupList.Add(new Group { GroupId = "2", GroupName = "Group 2" });
            //GroupList.Add(new Group { GroupId = "3", GroupName = "Group 3" });
            this.service = service;
            this.userGroupService = userGroupService;
            //Nodes = new List<TreeNode>()
            //{
            //    new TreeNode(){ParentID=0, NodeID=1, NodeName = "书本" },
            //    new TreeNode(){ParentID=0, NodeID=2, NodeName="课桌",IsCheck=true,IsExpand=true },
            //    new TreeNode(){ParentID=0,NodeID=3, NodeName="文具"},
            //    new TreeNode(){ParentID=1, NodeID=4, NodeName="书本名"},
            //    new TreeNode(){ParentID=1, NodeID=5, NodeName="作者"},
            //    new TreeNode(){ParentID=2, NodeID=6, NodeName="材质"},
            //    new TreeNode(){ParentID=3, NodeID=7, NodeName="品牌1"},
            //    new TreeNode(){ParentID=6, NodeID=8, NodeName="材质1"},
            //    new TreeNode(){ParentID=6, NodeID=9, NodeName="材质2"},
            //    new TreeNode(){ParentID=2, NodeID=10,NodeName="编号"},
            //    new TreeNode(){ParentID=3, NodeID=11, NodeName="品牌2"}
            //};
            //TreeNodes = getChildNodes(0, Nodes);

            SelectCommand = new DelegateCommand<object>(Select);
            CheckItemCmd = new DelegateCommand<TreeNode>(CheckItem);
            SelectedGroupCommand = new DelegateCommand<string>(SelectedComboxItem);
        }

       
        #region 属性 
        private string selectedGroup;

        public string SelectedGroup
        {
            get { return selectedGroup; }
            set { SetProperty<string>(ref selectedGroup, value); }
        }

        private List<TreeNode> treenodes = new List<TreeNode>();
        public List<TreeNode> TreeNodes
        {
            get { return treenodes; }
            set { SetProperty<List<TreeNode>>(ref treenodes, value); }
        }

        private List<TreeNode> nodes = new List<TreeNode>();
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
         

        private ObservableCollection<Group> groupList = new ObservableCollection<Group>();

        public ObservableCollection<Group> GroupList
        {
            get { return groupList; }
            set { SetProperty<ObservableCollection<Group>>(ref groupList, value); }
        }
         
        #endregion


        #region 命令
        public DelegateCommand<object> SelectCommand { get; set; }

        public DelegateCommand<TreeNode> CheckItemCmd { get; set; }

        public DelegateCommand<string> SelectedGroupCommand { get; set; }

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
        private void SelectedComboxItem(string groupid)
        {
            var groupcode = groupid;
        }

        private async void GetDataAsync() 
        {
            var result = await service.GetAllFilterAsync(new Shared.Parameters.TbWeighMenuDtoParameter()
            {
                Search = "",
                PageIndex = 0,
                PageSize=100
            }) ;

            if (result.Status)
            {
                Nodes.Clear();
               
                foreach (var item in result.Result.Items)
                {
                    Nodes.Add(new TreeNode() {
                        ParentID = string.IsNullOrEmpty(item.Attribute1) == true ? 0 : Convert.ToInt32(item.Attribute1),
                        NodeID = Convert.ToInt32(item.MenuNumber),
                        NodeName = item.MenuName 
                    }); 
                }
                TreeNodes = getChildNodes(0, Nodes);
            }

            
        }
        private async void GetGroupList()
        {
            
            var grouplists = await userGroupService.GetAllAsync(new Shared.Parameters.QueryParameter()
            {
                PageIndex = 0,
                PageSize = 100,
                Search = ""
            });

            if (grouplists.Status)
            {
                GroupList.Clear();
                foreach (var item in grouplists.Result.Items)
                {
                    GroupList.Add(new Group()
                    {
                        GroupId = item.UserGroupCode,
                        GroupName = item.UserGroupName
                    }); 
                }
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

        public override void OnNavigatedTo(NavigationContext navigationContext)
        { 
            base.OnNavigatedTo(navigationContext);
            GetGroupList();
            GetDataAsync();
            
        }
        #endregion
    }
}
