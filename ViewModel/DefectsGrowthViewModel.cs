using System.ComponentModel;
using System.Timers;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Collections.ObjectModel;

namespace TeamDashboard
{
    public class DefectsGrowthViewModel : INotifyPropertyChanged
    {
        string filePath = @"content\DefectsGrowth.json";

        public string Title { get; private set; }

        public ObservableCollection<DisplayValuePair> DefectsGrowth { get; private set; }

        public DefectsGrowthViewModel()
        {
            Initialize();
            
            var timer = new Timer();
            timer.Interval = 60 * 1000; // 1 minute update
            timer.Elapsed += TimerElapsed;
            timer.Start();
        }

        void Initialize()
        {
            Title = "Defects Growth (+15%)";

            DefectsGrowth = new ObservableCollection<DisplayValuePair>();
            DefectsGrowth.Add(new DisplayValuePair { Display = "1", Value = 4 });
            DefectsGrowth.Add(new DisplayValuePair { Display = "2", Value = 8 });
            DefectsGrowth.Add(new DisplayValuePair { Display = "3", Value = 21 });
            DefectsGrowth.Add(new DisplayValuePair { Display = "4", Value = 26 });
            DefectsGrowth.Add(new DisplayValuePair { Display = "5", Value = 23 });

            if (!File.Exists(filePath))
                CreateDefaultFile();

            Update();
        }

        void Update()
        {
            ReadFromFile();

            NotifyPropertyChanged("Title");
            NotifyPropertyChanged("DefectsGrowth");
        }

        void CreateDefaultFile()
        {
            JArray jArray = new JArray();
            foreach (var item in DefectsGrowth)
                jArray.Add(JsonConvert.SerializeObject(item));

            JObject jObject = new JObject();
            jObject["Title"] = JsonConvert.SerializeObject(Title);
            jObject["DefectsGrowth"] = jArray;

            PathHelper.WriteFile(filePath, jObject.ToString());
        }

        private void ReadFromFile()
        {
            var jsonString = File.ReadAllText(filePath);
            var jObject = JObject.Parse(jsonString);

            Title = (string)JsonConvert.DeserializeObject(jObject["Title"].Value<string>());

            DefectsGrowth = new ObservableCollection<DisplayValuePair>();

            var jDefectsGrowth = jObject["DefectsGrowth"].Values<string>().ToList();
            foreach (var item in jDefectsGrowth)
                DefectsGrowth.Add(JsonConvert.DeserializeObject<DisplayValuePair>(item));
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
