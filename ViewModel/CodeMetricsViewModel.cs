using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Timers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TeamDashboard
{
    // bind this view model to your page or window (DataContext)
    public class CodeMetricsViewModel : INotifyPropertyChanged
    {
        string filePath = @"content\CodeMetrics.json";

        public ObservableCollection<DisplayValuePair> CodeMetrics { get; private set; }

        public CodeMetricsViewModel()
        {
            Initialize();

            var timer = new Timer();
            timer.Interval = 60 * 1000; // 1 minute update
            timer.Elapsed += TimerElapsed;
            timer.Start();
        }

        void Initialize()
        {
            CodeMetrics = new ObservableCollection<DisplayValuePair>();
            CodeMetrics.Add(new DisplayValuePair { Display = "Code Coverage", Value = 32 });
            CodeMetrics.Add(new DisplayValuePair { Display = "Lowest Maintenance Index", Value = 67 });
            CodeMetrics.Add(new DisplayValuePair { Display = "Highest Code Complexity", Value = 17 });
            CodeMetrics.Add(new DisplayValuePair { Display = "Highest Class Coupling", Value = 14 });
            CodeMetrics.Add(new DisplayValuePair { Display = "Deepest Object Inheritance", Value = 9 });
            CodeMetrics.Add(new DisplayValuePair { Display = "Worst Method Performance", Value = 9 });

            if (!File.Exists(filePath))
                CreateDefaultFile();

            Update();
        }
        void Update()
        {
            ReadFromFile();

            NotifyPropertyChanged("CodeMetrics");
        }

        void CreateDefaultFile()
        {
            JArray jArray = new JArray();
            foreach (var item in CodeMetrics)
                jArray.Add(JsonConvert.SerializeObject(item));

            JObject jObject = new JObject();
            jObject["CodeMetrics"] = jArray;

            PathHelper.WriteFile(filePath, jObject.ToString());
        }

        private void ReadFromFile()
        {
            var jsonString = File.ReadAllText(filePath);
            var jObject = JObject.Parse(jsonString);

            CodeMetrics = new ObservableCollection<DisplayValuePair>();

            var jCodeMetrics = jObject["CodeMetrics"].Values<string>().ToList();
            foreach (var item in jCodeMetrics)
                CodeMetrics.Add(JsonConvert.DeserializeObject<DisplayValuePair>(item));
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
