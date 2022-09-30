using System;
using System.IO;
using System.Reflection;
using System.Text.Json;

namespace ExecutionEngineCli.Tools
{
    public class ConfigurationService
    {
        private static ConfigurationService instance;
        public static ConfigurationService Instance
        {
            get
            {
                if (instance == null)
                {
                    string path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                    path = EngineCore.GetSolutionDir(path).ToString() + "\\WorkFlowEngine\\ExecutionEngineCli\\configuration.json";

                    if (!File.Exists(path))
                        throw new FileNotFoundException("Configuration file missing!");

                    string jsonData = File.ReadAllText(path);
                    instance = JsonSerializer.Deserialize<ConfigurationService>(jsonData);
                }

                return instance;
            }
        }

        private ConfigurationService() { }

        public string ActionRepositoryPath { get; set; }
    }
}
