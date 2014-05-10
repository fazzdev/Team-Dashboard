using System.ComponentModel;

namespace TeamDashboard
{
    public class ViewModelPackage : INotifyPropertyChanged
    {
        private object selectedItem;

        public ClockViewModel ClockViewModel { get; set; }

        public SprintBurndownViewModel SprintBurndownViewModel { get; set; }

        public SprintStoriesViewModel SprintStoriesViewModel { get; set; }

        public BuildsViewModel BuildsViewModel { get; set; }

        public CommitsViewModel CommitsViewModel { get; set; }

        public VelocityViewModel VelocityViewModel { get; set; }

        public DefectsGrowthViewModel DefectsGrowthViewModel { get; set; }

        public CodeMetricsViewModel CodeMetricsViewModel { get; set; }

        public object SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                NotifyPropertyChanged("SelectedItem");
            }
        }

        public ViewModelPackage()
        {
            ClockViewModel = new ClockViewModel();
            SprintBurndownViewModel = new SprintBurndownViewModel();
            SprintStoriesViewModel = new SprintStoriesViewModel();
            BuildsViewModel = new BuildsViewModel();
            CommitsViewModel = new CommitsViewModel();
            VelocityViewModel = new VelocityViewModel();
            DefectsGrowthViewModel = new DefectsGrowthViewModel();
            CodeMetricsViewModel = new CodeMetricsViewModel();
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
