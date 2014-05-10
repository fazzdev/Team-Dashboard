using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TeamDashboard
{
    /// <summary>
    /// Interaction logic for BacklogItemView.xaml
    /// </summary>
    public partial class BacklogItemView : UserControl
    {
        static BacklogItem defaultBacklogItem = new BacklogItem() { Title = "Your story here", Status = "Running", StatusColor = ColorHelper.StatusToStringColor("Running") };

        static UIPropertyMetadata propertyMetadata = new UIPropertyMetadata(
            defaultBacklogItem, OnBacklogItemChanged);

        public static readonly DependencyProperty BacklogItemProperty =
            DependencyProperty.Register("BacklogItem", typeof(BacklogItem), typeof(BacklogItemView),
            propertyMetadata);

        public BacklogItem BacklogItem
        {
            get { return (BacklogItem)GetValue(BacklogItemProperty); }
            set { SetValue(BacklogItemProperty, value); }
        }

        public BacklogItemView()
        {
            InitializeComponent();
        }

        void Update(BacklogItem backlogItem)
        {
            if (backlogItem == null)
                return;

            if(!Dispatcher.CheckAccess())
            {
               Dispatcher.Invoke(() => Update(backlogItem));
               return;
            }

            if (!string.IsNullOrEmpty(backlogItem.Title))
                UserStoryText.Text = backlogItem.Title;

            if (!string.IsNullOrEmpty(backlogItem.Status))
                StatusText.Text = backlogItem.Status;

            if (!string.IsNullOrEmpty(backlogItem.StatusColor))
                StatusText.Background = new BrushConverter().ConvertFrom(backlogItem.StatusColor) as SolidColorBrush;
        }

        static void OnBacklogItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var newBacklogItem = e.NewValue as BacklogItem;
            var backlogItemView = d as BacklogItemView;

            if (backlogItemView == null || newBacklogItem == null)
                return;

            backlogItemView.Update(newBacklogItem);
        }
    }
}
