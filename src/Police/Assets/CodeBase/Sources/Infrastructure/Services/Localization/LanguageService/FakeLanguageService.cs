namespace Infrastructure.Services.Localization.LanguageService
{
    public class FakeLanguageService : ILanguageService
    {
        public string GetLanguage()
        {
            return "ru";
        }
    }
}