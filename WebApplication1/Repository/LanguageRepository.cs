using WebApplication1.Data;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public class LanguageRepository : ILanguageRepository
    {
        private DataContext _context;

        public LanguageRepository(DataContext context) 
        { 
            _context = context;
        }

        public bool CreateLanguage(LanguageItem language)
        {
            _context.Add(language);
            return Save();
        }

        public bool DeleteAll()
        {
            List<LanguageItem> languages = _context.TodoItems.ToList();
            if (languages.Count != 0)
            {
                _context.TodoItems.RemoveRange(languages);
                return Save();
            }
            return true;
        }

        public bool DeleteLanguage(LanguageItem language)
        {
            _context.Remove(language);
            return Save();
        }

        public LanguageItem GetLanguageById(int id)
        {
            return _context.TodoItems.Where(p => p.Id == id).FirstOrDefault();
        }

        public ICollection<LanguageItem> GetLanguages()
        {
            return _context.TodoItems.OrderBy(p => p.Id).ToList();
        }

        public bool LanguageExists(int id)
        {
            return _context.TodoItems.Any(p => p.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateLanguage(LanguageItem language)
        {
            _context.Update(language);
            return Save();
        }
    }
}
