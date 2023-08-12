using SmallHax.SimpleLexicon.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmallHax.SimpleLexicon.Service
{
    public interface IDictionaryService
    {
        Task<List<SearchResult>> Search(List<string> lookups);
    }
}
