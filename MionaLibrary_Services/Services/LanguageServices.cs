using MionaLibrary_DAL.Entity;
using MionaLibrary_DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MionaLibrary_Services.Services
{
    public class LanguageServices
    {
        LanguageRepo _repo = new LanguageRepo();

        public void AddLanguage(Language language)
        {
            _repo.AddLanguage(language);
        }

        public void RemoveLanguage(Language language)
        {
            _repo.DeleteLanguage(language);
        }

        public void UpdateLanguage(Language language)
        {
            _repo.UpdateLanguage(language);
        }

        public List<Language> GetLanguageList()
        {
            return _repo.GetAllLanguages();
        }
    }
}
