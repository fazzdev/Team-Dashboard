using System.ComponentModel;
using System.Timers;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace TeamDashboard
{
    public class SprintStoriesViewModel : INotifyPropertyChanged
    {
        string filePath = @"content\BacklogItems.json";

        public BacklogItem BacklogItem1 { get; set; }
        public BacklogItem BacklogItem2 { get; set; }
        public BacklogItem BacklogItem3 { get; set; }

        public SprintStoriesViewModel()
        {
            Initialize();

            var timer = new Timer();
            timer.Interval = 60 * 1000; // 1 minute update
            timer.Elapsed += TimerElapsed;
            timer.Start();
        }

        void Initialize()
        {
            BacklogItem1 = new BacklogItem();
            BacklogItem2 = new BacklogItem();
            BacklogItem3 = new BacklogItem();

            if (!File.Exists(filePath))
                CreateDefaultFile();

            Update();
        }

        void Update()
        {
            ReadFromFile();

            NotifyPropertyChanged("BacklogItem1");
            NotifyPropertyChanged("BacklogItem2");
            NotifyPropertyChanged("BacklogItem3");
        }

        void CreateDefaultFile()
        {
            JArray jArray = new JArray();
            jArray.Add(JsonConvert.SerializeObject(BacklogItem1));
            jArray.Add(JsonConvert.SerializeObject(BacklogItem2));
            jArray.Add(JsonConvert.SerializeObject(BacklogItem3));
            
            JObject jObject = new JObject();
            jObject["BacklogItems"] = jArray;

            string jsonString = jObject.ToString();

            PathHelper.WriteFile(filePath, jsonString);
        }

        private void ReadFromFile()
        {
            var jsonString = File.ReadAllText(filePath);
            var jObject = JObject.Parse(jsonString);

            var jBacklogItems = jObject["BacklogItems"].Values<string>().ToList();
            BacklogItem1 = JsonConvert.DeserializeObject<BacklogItem>(jBacklogItems[0]);
            BacklogItem2 = JsonConvert.DeserializeObject<BacklogItem>(jBacklogItems[1]);
            BacklogItem3 = JsonConvert.DeserializeObject<BacklogItem>(jBacklogItems[2]);
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
