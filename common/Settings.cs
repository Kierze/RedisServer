using log4net;
using System;
using System.Collections.Generic;
using System.IO;

namespace common
{
    public class Settings : IDisposable
    {
        private static ILog Logger = LogManager.GetLogger(nameof(Settings));

        private Dictionary<string, string> values { get; set; }
        private string id { get; set; }
        private string cfgFile { get; set; }

        public Settings(string id)
        {
            Logger.Info($"Loading settings for \"{id}\"...");

            values = new Dictionary<string, string>();
            this.id = id;
            cfgFile = Path.Combine(Environment.CurrentDirectory, id + ".cfg");
            if (File.Exists(cfgFile))
                using (var rdr = new StreamReader(File.OpenRead(cfgFile)))
                {
                    string line;
                    int lineNum = 1;
                    while ((line = rdr.ReadLine()) != null)
                    {
                        int i = line.IndexOf(":");
                        if (i == -1)
                        {
                            Logger.Info($"Invalid settings at line {lineNum}.");
                            throw new ArgumentException("Invalid settings.");
                        }
                        string val = line.Substring(i + 1);

                        values.Add(line.Substring(0, i),
                            val.Equals("null", StringComparison.InvariantCultureIgnoreCase) ? null : val);
                        lineNum++;
                    }
                    Logger.Info("Settings loaded.");
                }
            else
                Logger.Info("Settings not found.");
        }

        public void Dispose()
        {
            try
            {
                Logger.Info($"Saving settings for \"{id}\"...");
                using (var writer = new StreamWriter(File.OpenWrite(cfgFile)))
                    foreach (var i in values)
                        writer.WriteLine($"{i.Key}:{(i.Value == null ? "null" : i.Value)}");
            }
            catch (Exception ex)
            {
                Logger.Error("Error when saving settings.", ex);
            }
        }

        public string GetValue(string key, string def = null)
        {
            string ret;
            if (!values.TryGetValue(key, out ret))
            {
                if (def == null)
                {
                    Logger.Error($"Attempt to access nonexistant settings \"{key}\".");
                    throw new ArgumentException($"\"{key}\" does not exist in settings.");
                }
                ret = values[key] = def;
            }
            return ret;
        }

        public T GetValue<T>(string key, string def = null)
        {
            string ret;
            if (!values.TryGetValue(key, out ret))
            {
                if (def == null)
                {
                    Logger.Error($"Attempt to access nonexistant settings \"{key}\".");
                    throw new ArgumentException($"\"{key}\" does not exist in settings.");
                }
                ret = values[key] = def;
            }
            return (T)Convert.ChangeType(ret, typeof(T));
        }

        public void SetValue(string key, string val)
        {
            values[key] = val;
        }
    }
}
