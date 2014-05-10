using System;
using System.ComponentModel;
using System.Timers;

namespace TeamDashboard
{
    public class ClockViewModel : INotifyPropertyChanged
    {
        public ClockViewModel()
        {
            var timer = new Timer();
            timer.Interval = 1000; // 1 second update
            timer.Elapsed += TimerElapsed;
            timer.Start();
        }

        public string Time
        {
            get { return DateTime.Now.ToLongTimeString(); }
        }
        public string Date
        {
            get { return DateTime.Now.ToString("dddd, dd MMM yyyy"); }
        }

        void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            NotifyPropertyChanged("Time");
            NotifyPropertyChanged("Date");
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
