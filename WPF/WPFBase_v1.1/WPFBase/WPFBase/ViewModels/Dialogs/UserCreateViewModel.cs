using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFBase.ViewModels.Dialogs
{
    public class UserCreateViewModel : IDialogAware
    {
        public UserCreateViewModel()
        {
            CancelCommand = new DelegateCommand(Cancel);
            SaveCommand = new DelegateCommand(Save);
        }

        private void Save()
        {
            OnDialogClosed();
        }

        private void Cancel()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.No));
        }

        public DelegateCommand CancelCommand { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public string Title { get; set; }

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            DialogParameters keys = new DialogParameters();
            keys.Add("Value", "Hello");//返回的结果，名字为Value 值为Hello
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, keys));
        }

        //接收数据
        public void OnDialogOpened(IDialogParameters parameters)
        {
            Title = parameters.GetValue<string>("Title");
        }
    }
}
