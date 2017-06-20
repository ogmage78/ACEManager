﻿using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;

namespace ACEManager
{
    
    public struct Config
    {
        [DefaultValue("%SYSTEMROOT%//ACEmulator//")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public string AceServerPath { get; set; }

        [DefaultValue("ACE.exe")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public string AceServerExecutable { get; set; }

        [DefaultValue("")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public string AceServerArguments { get; set; }

        [DefaultValue(true)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool SaveLogFile { get; set; }

        [DefaultValue(@"ACEManagerLog_")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public string LocalLogPath { get; set; }

        [DefaultValue("yyyy-M-dd_HH-mm-ss")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public string LogFilenameFormat { get; set; }

        [DefaultValue("yyyy-M-dd_HH-mm-ss.ffff")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public string LogDataFormat { get; set; }

        [DefaultValue(false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool EnableAutoRestart { get; set; }

        [DefaultValue(false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool AnnounceEvents { get; set; }
    }

    public static class ConfigManager
    {
        public static bool ConfigurationLoaded { get; set; } = false;

        public static Config StartingConfiguration { get; set; }

        public static string ConfigFile { get; private set; }

        public static void Initialize()
        {
            ConfigFile = @"Config.json";
            
            if (!File.Exists(ConfigFile))
            {
                ApplyDefaults();
                Save(StartingConfiguration);
            }
            else
            {
                try
                {
                    StartingConfiguration = JsonConvert.DeserializeObject<Config>(File.ReadAllText(ConfigFile));
                }
                //catch (System.IO.IOException exception)
                //{
                //    MessageBox.Show("Please verify that your Config.json file is in the proper format (json) and is placed in the same folder, as this application. You may use the Config.json.example file, too create a new config.\n\nError Details:\n\n" + exception.ToString(), "Configuration missing error", MessageBoxButtons.OK);
                //}
                catch (Exception exception)
                {
                    if (Program.Log != null)
                    {
                        Program.Log.AddLogLine($"Config Load Error: { exception.Message}");
                    }
                    Console.WriteLine("An exception occured while loading the configuration file!");
                    Console.WriteLine($"Exception: {exception.Message}");
                    throw;
                }
                finally
                {
                    ConfigurationLoaded = true;
                }
            }
        }

        public static void ApplyDefaults()
        {
            StartingConfiguration = new Config() { AceServerPath = "%SYSTEMROOT%//ACEmulator//", AceServerExecutable = "ACE.exe", AceServerArguments = "", EnableAutoRestart = false, AnnounceEvents = false, SaveLogFile = true, LocalLogPath = @"ACEManagerLog_", LogDataFormat = "yyyy-M-dd_HH-mm-ss.ffff", LogFilenameFormat = "yyyy-M-dd_HH-mm-ss" };
            ConfigurationLoaded = true;
        }

        public static void Save(Config newConfig)
        {
            try
            {
                File.WriteAllText(ConfigFile, JsonConvert.SerializeObject(newConfig, Formatting.Indented));
            }
            catch (Exception exception)
            {
                if (Program.Log != null)
                {
                    Program.Log.AddLogLine($"Config Load Error: { exception.Message}");
                }
                Console.WriteLine("An exception occured while saving the configuration file!");
                Console.WriteLine($"Exception: {exception.Message}");
                throw;
            }
        }
    }
}