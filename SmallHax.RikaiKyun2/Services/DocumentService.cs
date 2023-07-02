using SmallHax.RikaiKyun2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallHax.RikaiKyun2.Services
{
    public class DocumentService
    {
        public Document Document { get; set; }

        public event Action DocumentLoaded;

        public async Task Open(string fileName)
        {
            Document = await Document.FromTxt(fileName);
            DocumentLoaded?.Invoke();
        }
    }
}
