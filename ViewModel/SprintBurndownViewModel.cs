using System.ComponentModel;
using System.Timers;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Collections.ObjectModel;

namespace TeamDashboard
{
    public class SprintBurndownViewModel : INotifyPropertyChanged
    {
        string filePath = @"content\Burndown.json";

        public string SprintTitle { get; private set; }

        public ObservableCollection<DisplayValuePair> SprintBurndown { get; private set; }

        public SprintBurndownViewModel()
        {
            Initialize();
            
            var timer = new Timer();
            timer.Interval = 60 * 1000; // 1 minute update
            timer.Elapsed += TimerElapsed;
            timer.Start();
        }

        void Initialize()
        {
            SprintTitle = "Sprint 5: Save the world!";

            SprintBurndown = new ObservableCollection<DisplayValuePair>();
            SprintBurndown.Add(new DisplayValuePair { Display = "1", Value = 100 });
            SprintBurndown.Add(new DisplayValuePair { Display = "2", Value = 90 });
            SprintBurndown.Add(new DisplayValuePair { Display = "3", Value = 80 });
            SprintBurndown.Add(new DisplayValuePair { Display = "4", Value = 70 });
            SprintBurndown.Add(new DisplayValuePair { Display = "5", Value = 60 });
            SprintBurndown.Add(new DisplayValuePair { Display = "6", Value = 50 });
            SprintBurndown.Add(new DisplayValuePair { Display = "7", Value = 40 });
            SprintBurndown.Add(new DisplayValuePair { Display = "8", Value = 30 });
            SprintBurndown.Add(new DisplayValuePair { Display = "9", Value = 20 });
            SprintBurndown.Add(new DisplayValuePair { Display = "10", Value = 10 });
            SprintBurndown.Add(new DisplayValuePair { Display = "11", Value = 0 });
            SprintBurndown.Add(new DisplayValuePair { Display = "12", Value = 0 });
            SprintBurndown.Add(new DisplayValuePair { Display = "13", Value = 0 });
            SprintBurndown.Add(new DisplayValuePair { Display = "14", Value = 0 });

            if (!File.Exists(filePath))
                CreateDefaultFile();

            Update();
        }

        void Update()
        {
            ReadFromFile();

            NotifyPropertyChanged("SprintTitle");
            NotifyPropertyChanged("SprintBurndown");
        }

        void CreateDefaultFile()
        {
            JArray jArray = new JArray();
            foreach (var item in SprintBurndown)
                jArray.Add(JsonConvert.SerializeObject(item));

            JObject jObject = new JObject();
            jObject["SprintTitle"] = JsonConvert.SerializeObject(SprintTitle);
            jObject["SprintBurndown"] = jArray;

            PathHelper.WriteFile(filePath, jObject.ToString());
        }

        private void ReadFromFile()
        {
            var jsonString = File.ReadAllText(filePath);
            var jObject = JObject.Parse(jsonString);

            SprintTitle = (string)JsonConvert.DeserializeObject(jObject["SprintTitle"].Value<string>());

            SprintBurndown = new ObservableCollection<DisplayValuePair>();

            var jSprintBurndown = jObject["SprintBurndown"].Values<string>().ToList();
            foreach (var item in jSprintBurndown)
                SprintBurndown.Add(JsonConvert.DeserializeObject<DisplayValuePair>(item));
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
