using System.ComponentModel;
using System.Timers;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace TeamDashboard
{
    public class BuildsViewModel : INotifyPropertyChanged
    {
        string filePath = @"content\Builds.json";

        public BuildInfo BuildInfo1 { get; set; }
        public BuildInfo BuildInfo2 { get; set; }
        public BuildInfo BuildInfo3 { get; set; }
        public BuildInfo BuildInfo4 { get; set; }
        public BuildInfo BuildInfo5 { get; set; }
        public BuildInfo BuildInfo6 { get; set; }

        public BuildsViewModel()
        {
            Initialize();

            var timer = new Timer();
            timer.Interval = 60 * 1000; // 1 minute update
            timer.Elapsed += TimerElapsed;
            timer.Start();
        }

        void Initialize()
        {
            BuildInfo1 = new BuildInfo();
            BuildInfo2 = new BuildInfo();
            BuildInfo3 = new BuildInfo();
            BuildInfo4 = new BuildInfo();
            BuildInfo5 = new BuildInfo();
            BuildInfo6 = new BuildInfo();

            if (!File.Exists(filePath))
                CreateDefaultFile();

            Update();
        }

        void Update()
        {
            ReadFromFile();

            NotifyPropertyChanged("BuildInfo1");
            NotifyPropertyChanged("BuildInfo2");
            NotifyPropertyChanged("BuildInfo3");
            NotifyPropertyChanged("BuildInfo4");
            NotifyPropertyChanged("BuildInfo5");
            NotifyPropertyChanged("BuildInfo6");
        }

        void CreateDefaultFile()
        {
            JArray jArray = new JArray();
            jArray.Add(JsonConvert.SerializeObject(BuildInfo1));
            jArray.Add(JsonConvert.SerializeObject(BuildInfo2));
            jArray.Add(JsonConvert.SerializeObject(BuildInfo3));
            jArray.Add(JsonConvert.SerializeObject(BuildInfo4));
            jArray.Add(JsonConvert.SerializeObject(BuildInfo5));
            jArray.Add(JsonConvert.SerializeObject(BuildInfo6));
            
            JObject jObject = new JObject();
            jObject["Builds"] = jArray;

            string jsonString = jObject.ToString();

            PathHelper.WriteFile(filePath, jsonString);
        }

        private void ReadFromFile()
        {
            var jsonString = File.ReadAllText(filePath);
            var jObject = JObject.Parse(jsonString);

            var jBuilds = jObject["Builds"].Values<string>().ToList();
            BuildInfo1 = JsonConvert.DeserializeObject<BuildInfo>(jBuilds[0]);
            BuildInfo2 = JsonConvert.DeserializeObject<BuildInfo>(jBuilds[1]);
            BuildInfo3 = JsonConvert.DeserializeObject<BuildInfo>(jBuilds[2]);
            BuildInfo4 = JsonConvert.DeserializeObject<BuildInfo>(jBuilds[3]);
            BuildInfo5 = JsonConvert.DeserializeObject<BuildInfo>(jBuilds[4]);
            BuildInfo6 = JsonConvert.DeserializeObject<BuildInfo>(jBuilds[5]);
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
