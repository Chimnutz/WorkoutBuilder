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

