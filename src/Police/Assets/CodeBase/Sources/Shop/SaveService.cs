using System.Collections;
using System.Collections.Generic;
using Infrastructure;
using Services.PrefsService;
using SpecialPlatforms;
using UnityEngine;

namespace Shop
{
    public class SaveService
    {
        private const float SaveFrequency = .5f;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IPrefsService _prefsService;

        private readonly List<ISavableData> _data = new();
        
        public SaveService(ICoroutineRunner coroutineRunner, IPrefsService prefsService)
        {
            _coroutineRunner = coroutineRunner;
            _prefsService = prefsService;
            
            _coroutineRunner.StartCoroutine(SaveOverTime());
        }

        public void Bind(ISavableData savableData)
        {
            if (RecordExists(savableData))
                PopulateData(savableData);
            
            _data.Add(savableData);
        }

        public bool RecordExists(ISavableData savableData)
        {
            return _prefsService.HasKey(ConstructKey(savableData));
        }

        private void PopulateData(ISavableData savable)
        {
            savable.Populate(_prefsService.GetString(ConstructKey(savable)));
        }

        private IEnumerator SaveOverTime()
        {
            while (true)
            {
                foreach (ISavableData data in _data)
                {
                    _prefsService.SetString(ConstructKey(data), data.GetData());
                }
                yield return new WaitForSeconds(SaveFrequency);
            }
        }

        private string ConstructKey(ISavableData savableData)
        {
            const string KeySuffix = "_SaveService";
            return savableData.Key() + KeySuffix;
        }
    }
}