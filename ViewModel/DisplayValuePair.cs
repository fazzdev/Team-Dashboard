using System.ComponentModel;

namespace TeamDashboard
{
    // class which represent a data point in the chart
    public class DisplayValuePair : INotifyPropertyChanged
    {
        private double _value = 0;

        public string Display { get; set; }

        public double Value
        {
            get { return _value; }
            set
            {
                _value = value;
                NotifyPropertyChanged("Value");
            }
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
