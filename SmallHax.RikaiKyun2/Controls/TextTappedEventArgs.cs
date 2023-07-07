using SmallHax.RikaiKyun2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallHax.RikaiKyun2.Controls
{
    public class TextTappedEventArgs: EventArgs
    {
        public CharacterData Character { get; }
        public Node Node { get; }
        public static TextTappedEventArgs NotFound { get; } = new TextTappedEventArgs();
        
        public TextTappedEventArgs()
        {
        }

        public TextTappedEventArgs(CharacterData character, Node node)
        {
            Character = character;
            Node = node;
        }
    }
}
