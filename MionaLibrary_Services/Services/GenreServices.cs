using MionaLibrary_DAL.Entity;
using MionaLibrary_DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MionaLibrary_Services.Services
{
    public class GenreServices
    {
        GenreRepo? _repo;

        public void AddGenre(Genre genre)
        {
            _repo = new();
            _repo.AddGenre(genre);
        }

        public void RemoveGenre(Genre genre)
        {
            _repo = new();
            _repo.DeleteGenre(genre);
        }

        public void UpdateGenre(Genre genre) { 
            _repo = new();
            _repo.UpdateGenre(genre);
        }

        public List<Genre> GetGenreList()
        {
            _repo = new();
            return _repo.GetAllGenre();
        }
    }
}
