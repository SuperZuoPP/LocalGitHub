using HandyControl.Controls;
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
using System.Xml.Linq;
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
            SaveAuthorityCmd = new DelegateCommand(SaveAuthority);
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

        private List<TreeNode> checkLists = new List<TreeNode>();
        public List<TreeNode> CheckLists
        {
            get { return checkLists; }
            set { SetProperty<List<TreeNode>>(ref checkLists, value); }
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

        public DelegateCommand SaveAuthorityCmd { get; set; }
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
                CheckListsAdd(item);
                CheckNode(item, chk);
            }
        }

        private void CheckNode(TreeNode node, bool chk) 
        {
            node.IsExpand = chk;
            node.IsCheck = chk;

            foreach (var child in node.ChildNodes)
            {
                CheckListsAdd(child);
                CheckNode(child, chk); // 递归调用，设置子节点的选中状态
            }
        }

        private void CheckItem(TreeNode treeNode)
        {
            CurrentNode = treeNode;
            treeNode.IsExpand = true;
            CheckParentNodes(treeNode);
            CheckChildNodes(treeNode);
            CheckListsAdd(treeNode);
        }

        private void CheckChildNodes(TreeNode treeNode)
        {
            foreach (var item in treeNode.ChildNodes)
            {
                item.IsCheck = treeNode.IsCheck; 
                var existingNode = CheckLists.FirstOrDefault(x => x.NodeID == item.NodeID); 
                if (existingNode != null)
                    CheckLists.Remove(existingNode);
                  
                CheckLists.Add(item);
                CheckChildNodes(item);
            }
        }

        private void CheckParentNodes(TreeNode treeNode)
        {
            var parent = TreeNodes.FirstOrDefault(x => x.NodeID == treeNode.ParentID);
            if (parent != null)
            {
                parent.IsCheck = true; 
                CheckParentNodes(parent);
            } 
            //foreach (var node in TreeNodes)
            //{
            //    if (node.ChildNodes.Any(x => x.NodeID == treeNode.ParentID))
            //    {
            //        node.IsCheck = true; 
            //        CheckParentNodes(node);
            //    }
            //}
            //if (treeNode.ParentID != 0)
            //{
            //    var parent = TreeNodes.FirstOrDefault(x => x.NodeID == treeNode.ParentID);
            //    if (parent != null)
            //        parent.IsCheck = true;
            //}

            //foreach (var child in TreeNodes)
            //{
            //    var childitem = child.ChildNodes.FirstOrDefault(x => x.NodeID == treeNode.ParentID);
            //    if (childitem != null)
            //    {
            //        childitem.IsCheck = true;
            //        if (childitem.ParentID != 0)
            //        {
            //            var parent = TreeNodes.FirstOrDefault(x => x.NodeID == childitem.ParentID);
            //            if (parent != null)
            //                parent.IsCheck = true;
            //        }
            //    }
            //}
        }

        private void CheckListsAdd(TreeNode treeNode) 
        {
            if (CheckLists.Exists(x => x.NodeID == treeNode.NodeID))
                CheckLists.Remove(treeNode);
            CheckLists.Add(treeNode);
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
                        ParentID = string.IsNullOrEmpty(item.MenuNumber) == true ? 0 : Convert.ToInt32(item.MenuNumber),
                        NodeID = item.Id,
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

        private async void SaveAuthority()
        {
            if (string.IsNullOrEmpty(SelectedGroup)) 
            {
                Growl.WarningGlobal("请先选择用户组！");
                return;
            }
                

            foreach (var item in CheckLists)
            {
                TbWeighGroupauthorityDto CurrentGroupauthority = new TbWeighGroupauthorityDto();
                CurrentGroupauthority.AuthorityCode = item.NodeCode;
                CurrentGroupauthority.UserGroupCode = SelectedGroup;
                if (item.IsCheck) 
                { 
                    await userGroupService.GroupAuthorityAdd(CurrentGroupauthority);
                }
                else
                {
                    await userGroupService.GroupAuthorityRemove(CurrentGroupauthority);
                }
            }
            GetGroupList();
            GetDataAsync();
            IsChenkAll = false; 
            CheckLists.Clear();
            Growl.SuccessGlobal("授权完成！"); 
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
