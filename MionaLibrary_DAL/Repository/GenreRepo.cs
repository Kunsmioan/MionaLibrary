using MionaLibrary_DAL.DataAccess;
using MionaLibrary_DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MionaLibrary_DAL.Repository
{
    public class GenreRepo
    {
        LibraryManagerContext? _context;

        public void AddGenre(Genre genre)
        {
            _context = new LibraryManagerContext();
            _context.Add(genre);
            _context.SaveChanges();
        }

        public void DeleteGenre(Genre genre)
        {
            _context = new();
            _context.Remove(genre);
            _context.SaveChanges();
        }

        public void UpdateGenre(Genre genre)
        {
            _context = new LibraryManagerContext(); 
            _context.Update(genre);
            _context.SaveChanges();
        }

        public List<Genre> GetAllGenre() {
            _context = new();
            List<Genre> genres = _context.Genres.ToList();
            return genres;
        }
    }
}
