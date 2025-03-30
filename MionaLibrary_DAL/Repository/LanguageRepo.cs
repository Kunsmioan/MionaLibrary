using MionaLibrary_DAL.DataAccess;
using MionaLibrary_DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MionaLibrary_DAL.Repository
{

    public class LanguageRepo
    {
        LibraryManagerContext? _context;

        // Thêm một ngôn ngữ mới vào cơ sở dữ liệu
        public void AddLanguage(Language language)
        {
            _context = new LibraryManagerContext();
            _context.Add(language);
            _context.SaveChanges();
        }

        // Xóa một ngôn ngữ khỏi cơ sở dữ liệu
        public void DeleteLanguage(Language language)
        {
            _context = new LibraryManagerContext();
            _context.Remove(language);
            _context.SaveChanges();
        }

        // Cập nhật thông tin của một ngôn ngữ trong cơ sở dữ liệu
        public void UpdateLanguage(Language language)
        {
            _context = new LibraryManagerContext();
            _context.Update(language);
            _context.SaveChanges();
        }

        // Lấy danh sách tất cả các ngôn ngữ từ cơ sở dữ liệu
        public List<Language> GetAllLanguages()
        {
            _context = new LibraryManagerContext();
            List<Language> languages = _context.Languages.ToList();
            return languages;
        }
    }
}
