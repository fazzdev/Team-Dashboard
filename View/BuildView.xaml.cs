using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TeamDashboard
{
    /// <summary>
    /// Interaction logic for BuildView.xaml
    /// </summary>
    public partial class BuildView : UserControl
    {
        static BuildInfo defaultBuildInfo = new BuildInfo() { Name = "Build Name", FinishTime = "5 minutes ago", StartedBy = "User" };

        static UIPropertyMetadata propertyMetadata = new UIPropertyMetadata(
            defaultBuildInfo, OnBuildInfoChanged);

        public static readonly DependencyProperty BuildInfoProperty =
            DependencyProperty.Register("BuildInfo", typeof(BuildInfo), typeof(BuildView),
            propertyMetadata);

        public BuildInfo BuildInfo
        {
            get { return (BuildInfo)GetValue(BuildInfoProperty); }
            set { SetValue(BuildInfoProperty, value); }
        }

        public BuildView()
        {
            InitializeComponent();
        }

        void Update(BuildInfo buildInfo)
        {
            if (buildInfo == null)
                return;

            if(!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => Update(buildInfo));
               return;
            }

            if (!string.IsNullOrEmpty(buildInfo.Name))
                BuildNameText.Text = buildInfo.Name;

            if (!string.IsNullOrEmpty(buildInfo.FinishTime))
                FinishTimeText.Text = buildInfo.FinishTime;

            if (!string.IsNullOrEmpty(buildInfo.StartedBy))
                StartedByText.Text = buildInfo.StartedBy;
        }

        static void OnBuildInfoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var buildInfo = e.NewValue as BuildInfo;
            var buildView = d as BuildView;

            if (buildView == null || buildInfo == null)
                return;

            buildView.Update(buildInfo);
        }
    }
}
