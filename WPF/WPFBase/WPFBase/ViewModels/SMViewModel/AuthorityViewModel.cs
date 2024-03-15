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
            this.service = service;
            this.userGroupService = userGroupService; 
            SelectCommand = new DelegateCommand<object>(Select);
            CheckItemCmd = new DelegateCommand<TreeNode>(CheckItem);
            SelectedGroupCommand = new DelegateCommand<string>(SelectedComboxItem);
            CheckAllCmd = new DelegateCommand(CheckAllAuthority);
        }




        #region 属性 

        private bool isChenkAll;

        public bool IsChenkAll
        {
            get { return isChenkAll; }
            set { SetProperty<bool>(ref isChenkAll, value); }
        }

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
        public DelegateCommand CheckAllCmd { get; set; }
        #endregion


        #region 方法

        /// <summary>
        /// 全选
        /// </summary>
        private void CheckAllAuthority()
        {
            CheckAll(IsChenkAll);  
        }

        private void CheckAll(bool chk)
        {
            foreach (var item in TreeNodes)
            {
                item.IsExpand = chk;
                item.IsCheck = chk;

                foreach (var itemsub in item.ChildNodes)
                {
                    itemsub.IsExpand = chk;
                    itemsub.IsCheck = chk;
                }
            }
        }

        private void CheckItem(TreeNode treeNode)
        {
            CurrentNode = treeNode;
            treeNode.IsExpand = true;
            List<TreeNode> CheckList = treeNode.ChildNodes;
            //CheckList.ForEach(item => item.IsCheck = treeNode.IsCheck);
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
            CheckAll(false);
            GetAuthorityCodeByGroupCode(groupid); 
        }

        //获取MenuCodes
        private async void GetAuthorityCodeByGroupCode(string groupcode) 
        { 
            var result = await userGroupService.GetGroupAuthority(new Shared.Parameters.QueryParameter() { 
                Search= groupcode,
                PageIndex = 0,
                PageSize = 100
            });

            if (result.Status)
            {
                foreach (var item in result.Result.Items) 
                { 
                    var node = Nodes.FirstOrDefault(x => x.NodeCode == item.AuthorityCode);
                    if (node != null)
                    {
                        node.IsCheck = true;
                        node.IsExpand = true;
                    }
                }
                    
            }
           

        }

        private async void GetDataAsync() 
        {
            var result = await service.GetAllFilterAsync(new Shared.Parameters.TbWeighMenuDtoParameter()
            {
                Search = "",
                PageIndex = 0,
                PageSize = 100
            }) ;

            if (result.Status)
            {
                TreeNodes.Clear();
                Nodes.Clear(); 
                foreach (var item in result.Result.Items)
                {
                    Nodes.Add(new TreeNode() {
                        ParentID = string.IsNullOrEmpty(item.Attribute1) == true ? 0 : Convert.ToInt32(item.Attribute1),
                        NodeID = Convert.ToInt32(item.MenuNumber),
                        NodeName = item.MenuName ,
                        NodeCode = item.MenuCode
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
