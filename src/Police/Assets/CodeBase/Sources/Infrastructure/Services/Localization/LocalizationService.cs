using System.Collections;
using Infrastructure.Services.Localization.LanguageService;
using UnityEngine.Localization.Settings;

namespace Infrastructure.Services.Localization
{
    public class LocalizationService
    {
        private readonly ILanguageService _languageService;

        public LocalizationService(ILanguageService languageService)
        {
            _languageService = languageService;
        }
        
        public IEnumerator Initialize()
        {
            var currentLanguageCode =_languageService.GetLanguage();
            var locales = LocalizationSettings.AvailableLocales.Locales;
            LocalizationSettings.SelectedLocale = locales.Find(x => x.Identifier.Code == currentLanguageCode);
            yield return LocalizationSettings.InitializationOperation;
        }
    }
}