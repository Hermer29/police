﻿using System.Collections;
using Infrastructure;
using UnityEngine;

namespace Services.PrefsService
{
    public class PlayerPrefsService : IPrefsService
    {
        private int _version = 1;
        
        public PlayerPrefsService(ICoroutineRunner coroutineRunner)
        {
            coroutineRunner.StartCoroutine(SaveOverTime());
        }

        private IEnumerator SaveOverTime()
        {
            int currentVersion = _version;
            while (true)
            {
                yield return new WaitForSeconds(2);
                if (_version != currentVersion)
                {
                    PlayerPrefs.Save();
                    currentVersion = _version;
                }
            }
        }
        
        public int GetInt(string key, int defaultValue = 0)
        {
            return PlayerPrefs.GetInt(key, defaultValue);
        }

        public void SetInt(string key, int value)
        {
            _version++;
            PlayerPrefs.SetInt(key, value);
        }

        public bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        public string GetString(string key, string defaultValue = "")
        {
            return PlayerPrefs.GetString(key, defaultValue);
        }

        public void SetString(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }
    }
}