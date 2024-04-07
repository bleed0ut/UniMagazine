using Microsoft.CodeAnalysis.Elfie.Serialization;
using UniMagazine.Models;

namespace UniMagazine.Repository.IRepository
{
    public interface IDashRepository 
    {
        public List<Object> GetAllConInAllFaByAca();
        public List<Object> GetPerConInAllFaByAca();
        public List<Object> GetAllContributerInAllFaByAca();
    }
}
