using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMessageBoard.Models
{
    class UndoStack
    {
        public bool isErase { get; set; }
        public System.Windows.Ink.Stroke stroke { get; set; }
    }
}
