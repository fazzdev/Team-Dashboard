using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TeamDashboard
{
    /// <summary>
    /// Interaction logic for CommitView.xaml
    /// </summary>
    public partial class CommitView : UserControl
    {
        static CommitInfo defaultCommitInfo = new CommitInfo() { Branch = "Branch", Message = "Commit Message" };

        static UIPropertyMetadata propertyMetadata = new UIPropertyMetadata(
            defaultCommitInfo, OnCommitInfoChanged);

        public static readonly DependencyProperty CommitInfoProperty =
            DependencyProperty.Register("CommitInfo", typeof(CommitInfo), typeof(CommitView),
            propertyMetadata);

        public CommitInfo CommitInfo
        {
            get { return (CommitInfo)GetValue(CommitInfoProperty); }
            set { SetValue(CommitInfoProperty, value); }
        }

        public CommitView()
        {
            InitializeComponent();
        }

        void Update(CommitInfo commitInfo)
        {
            if (commitInfo == null)
                return;

            if(!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => Update(commitInfo));
               return;
            }

            if (!string.IsNullOrEmpty(commitInfo.Branch))
                BranchText.Text = commitInfo.Branch;

            if (!string.IsNullOrEmpty(commitInfo.Message))
                CommitText.Text = commitInfo.Message;

            if (commitInfo.UserImage != null)
                UserImage.Source = commitInfo.UserImage.ToImageSource();
        }

        static void OnCommitInfoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var newCommitInfo = e.NewValue as CommitInfo;
            var commitView = d as CommitView;

            if (commitView == null || newCommitInfo == null)
                return;

            commitView.Update(newCommitInfo);
        }
    }
}
