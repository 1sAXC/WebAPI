using WebApplication1.Models;

namespace WebApplication1.Interfaces
{
    public interface ILanguageRepository
    {
        ICollection<LanguageItem> GetLanguages();

        LanguageItem GetLanguageById(int id);
        public bool LanguageExists(int id);
        bool CreateLanguage(LanguageItem language);
        bool UpdateLanguage(LanguageItem language);
        bool DeleteLanguage(LanguageItem language);
        bool DeleteAll();
        bool Save();
    }
}
