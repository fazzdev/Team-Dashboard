using System.ComponentModel;
using System.Timers;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Collections.ObjectModel;

namespace TeamDashboard
{
    public class VelocityViewModel : INotifyPropertyChanged
    {
        string filePath = @"content\Velocity.json";

        public string Title { get; private set; }

        public ObservableCollection<DisplayValuePair> Velocity { get; private set; }

        public VelocityViewModel()
        {
            Initialize();
            
            var timer = new Timer();
            timer.Interval = 60 * 1000; // 1 minute update
            timer.Elapsed += TimerElapsed;
            timer.Start();
        }

        void Initialize()
        {
            Title = "Velocity (42 pts)";

            Velocity = new ObservableCollection<DisplayValuePair>();
            Velocity.Add(new DisplayValuePair { Display = "1", Value = 38 });
            Velocity.Add(new DisplayValuePair { Display = "2", Value = 45 });
            Velocity.Add(new DisplayValuePair { Display = "3", Value = 40 });
            Velocity.Add(new DisplayValuePair { Display = "4", Value = 60 });
            Velocity.Add(new DisplayValuePair { Display = "5", Value = 35 });

            if (!File.Exists(filePath))
                CreateDefaultFile();

            Update();
        }

        void Update()
        {
            ReadFromFile();

            NotifyPropertyChanged("Title");
            NotifyPropertyChanged("Velocity");
        }

        void CreateDefaultFile()
        {
            JArray jArray = new JArray();
            foreach (var item in Velocity)
                jArray.Add(JsonConvert.SerializeObject(item));

            JObject jObject = new JObject();
            jObject["Title"] = JsonConvert.SerializeObject(Title);
            jObject["Velocity"] = jArray;

            PathHelper.WriteFile(filePath, jObject.ToString());
        }

        private void ReadFromFile()
        {
            var jsonString = File.ReadAllText(filePath);
            var jObject = JObject.Parse(jsonString);

            Title = (string)JsonConvert.DeserializeObject(jObject["Title"].Value<string>());

            Velocity = new ObservableCollection<DisplayValuePair>();

            var jVelocity = jObject["Velocity"].Values<string>().ToList();
            foreach (var item in jVelocity)
                Velocity.Add(JsonConvert.DeserializeObject<DisplayValuePair>(item));
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            Update();
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
