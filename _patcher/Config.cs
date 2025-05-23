﻿using _patcher.utils;
using System;
using System.IO;

namespace _patcher
{
    internal class Config : BaseConfig
    {
        internal static string ConfigFileName = "config.ini";
        private static readonly string ConfigPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "osuPatcher");

        public delegate void ConfigChangedHandler();
        public event ConfigChangedHandler onConfigChanged;

        #region config
        // ...
        private bool _patchRelax = true;
        public bool PatchRelax 
        { 
            get => _patchRelax; 
            set => _patchRelax = value; 
        }

        private bool _transitionTime = true;
        public bool TransitionTime
        {
            get => _transitionTime;
            set => _transitionTime = value;
        }
        #endregion

        internal static Config _load()
        {
            Directory.CreateDirectory(ConfigPath);
            string fullPath = Path.Combine(ConfigPath, ConfigFileName);
            Config config = new Config();

            if (File.Exists(fullPath))
                using (var reader = new StreamReader(fullPath))
                    config._loadConfig(reader);
            else
                config._save();

            return config;
        }

        private void _save()
        {
            string fullPath = Path.Combine(ConfigPath, ConfigFileName);
            using (var writer = new StreamWriter(fullPath, false))
                _saveConfig(writer);
        }

        public void ToggleSetting(ref bool conf)
        {
            conf = !conf;
            _save();
            onConfigChanged?.Invoke();   
        }

        public void TogglePatchRelax(object sender, EventArgs e) => ToggleSetting(ref _patchRelax);
        public void ToggleTransitionTime(object sender, EventArgs e) => ToggleSetting(ref _transitionTime);
    }
}
