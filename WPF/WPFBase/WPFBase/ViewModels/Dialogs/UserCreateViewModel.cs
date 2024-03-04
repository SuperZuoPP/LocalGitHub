using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFBase.Common;

namespace WPFBase.ViewModels.Dialogs
{
    public class UserCreateViewModel : BindableBase, IDialogHostAware
    {
        public UserCreateViewModel()
        {
            CancelCommand = new DelegateCommand(Cancel);
            SaveCommand = new DelegateCommand(Save);
        }

        private void Save()
        {
            DialogParameters keys = new DialogParameters();
            keys.Add("Value", "Hello");//返回的结果，名字为Value 值为Hello
            DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.OK, keys));
        }

        private void Cancel()
        {
            if (DialogHost.IsDialogOpen(DialogHostName))
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.No)); //取消返回NO告诉操作结束
        }

        public string DialogHostName { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; } 
        public string Title { get; set; } 
         

        //接收数据
        public void OnDialogOpened(IDialogParameters parameters)
        {
            Title = parameters.GetValue<string>("Title");
        }

        
    }
}
