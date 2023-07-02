using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallHax.RikaiKyun2.Models
{
    public class Node
    {
        public string Value { get; set; }
        public List<Node> Children { get; set; }
    }
}
