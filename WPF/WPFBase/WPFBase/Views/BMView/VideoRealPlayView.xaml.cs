using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UserControl = System.Windows.Controls.UserControl;

namespace WPFBase.Views.BMView
{
    /// <summary>
    /// VideoRealPlayView.xaml 的交互逻辑
    /// </summary>
    public partial class VideoRealPlayView : UserControl
    {
        private PictureBox[] picBoxArr;
        public VideoRealPlayView()
        {
            InitializeComponent();
            // 初始化图片控件
            picBoxArr = new PictureBox[6];
            picBoxArr[0] = picture1;
            picBoxArr[1] = picture2;
            picBoxArr[2] = picture3;
            picBoxArr[3] = picture4;
            picBoxArr[4] = picture5;
            picBoxArr[5] = picture6;
            //for (var i = 0; i < 6; i++)
            //{
            //    picBoxArr[i].Click += PictureBox_Click;
            //} 
        }
    }
}
