using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WorkoutAppWPF
{

    public class ApplicationSettings
    {
        // ---------------------------------------------------------------------------------------
        // Insert configurable values below:
        public List<string> targetMileage { get; set; } = new List<string>();
        public List<string> actualMileage { get; set; } = new List<string>();
        public DateTime targetRaceDay { get; set; } = new DateTime();
        public string numberOfCycles { get; set; }
        public string numDaysInCycle { get; set; }
        public string startingMileage { get; set; }
        public string endingMileage { get; set; }
        public string cycleIncrease { get; set; }
        public string cycleDelta { get; set; }
        public string numCyclesReset { get; set; }
        public List<String> dailyRunTypeList { get; set; } = new List<String>();
        public List<String> dailyWorkoutTypeList { get; set; } = new List<String>();
        public List<String> CT1TypeList { get; set; } = new List<String>();
        public List<String> CT2TypeList { get; set; } = new List<String>();

        public List<List<RunTypes>> runType { get; set; } = new List<List<RunTypes>>();

        public List<List<double>> warmupDist { get; set; } = new List<List<double>>();
        public List<List<Pace>> warmupPace { get; set; } = new List<List<Pace>>();
        public List<List<Units>> warmupUnits { get; set; } = new List<List<Units>>();

        public List<List<int>> numSets { get; set; } = new List<List<int>>();
        public List<List<List<int>>> numReps { get; set; } = new List<List<List<int>>>();
        public List<List<List<double>>> repDistance { get; set; } = new List<List<List<double>>>();
        public List<List<List<Units>>> repUnits { get; set; } = new List<List<List<Units>>>();
        public List<List<List<Pace>>> repPace { get; set; } = new List<List<List<Pace>>>();
        public List<List<List<double>>> repCoolDistance { get; set; } = new List<List<List<double>>>();
        public List<List<List<Units>>> repCoolUnits { get; set; } = new List<List<List<Units>>>();
        public List<List<List<Pace>>> repCoolPace { get; set; } = new List<List<List<Pace>>>();

        public List<List<double>> coolDownDist { get; set; } = new List<List<double>>();
        public List<List<Pace>> coolDownPace { get; set; } = new List<List<Pace>>();
        public List<List<Units>> coolDownUnits { get; set; } = new List<List<Units>>();


        // Sets the default values of all configurable values.
        private void SetDefaultValues()
        {

        }

        // ---------------------------------------------------------------------------------------

        // Name of configuration file.
        [XmlIgnore]
        public const string FileName = "C:\\Misc\\AppSettings\\config.xml";

        // Globally accessable instance of loaded configuration.
        [XmlIgnore]
        public static ApplicationSettings Instance { get; private set; }

        // Empty constructor for XmlSerializer.
        public ApplicationSettings()
        {
        }

        // Used to load the default configuration if Load() fails.
        public static void Default()
        {
            ApplicationSettings.Instance = new ApplicationSettings();
            ApplicationSettings.Instance.SetDefaultValues();
        }

        // Loads the configuration from file.
        public static void Load()
        {
            var serializer = new XmlSerializer(typeof(ApplicationSettings));

            using (var fStream = new FileStream(ApplicationSettings.FileName, FileMode.Open))
                ApplicationSettings.Instance = (ApplicationSettings)serializer.Deserialize(fStream);

        }

        // Saves the configuration to file.
        public void Save()
        {
            var serializer = new XmlSerializer(typeof(ApplicationSettings));

            using (var fStream = new FileStream(ApplicationSettings.FileName, FileMode.Create))
                serializer.Serialize(fStream, this);
        }
    }

} 

