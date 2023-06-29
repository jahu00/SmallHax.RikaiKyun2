using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallHax.RikaiKyun2.Services
{
    public class DocumentService
    {
        public string Document { get; set; }

        public event Action DocumentLoaded;

        public async Task Open(string fileName)
        {
            Document = await File.ReadAllTextAsync(fileName);
            DocumentLoaded?.Invoke();
        }
    }
}
