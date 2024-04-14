using Microsoft.CodeAnalysis.Elfie.Serialization;
using UniMagazine.Models;

namespace UniMagazine.Repository.IRepository
{
    public interface IDashRepository 
    {
        public List<Object> GetAllConInAllFaByAca(string? year);
        public List<Object> GetPerConInAllFaByAca(string? year);
        public List<object> GetContributerInFaAca(string? year);
        public List<Object> GetAllContributerInAllFaByAca();
    }
}
