using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Tetris_Clone_Redo
{
    internal class playPoint
    {
        public Point Location;
        public bool Occupied;
        public bool OccupiedByCurrentShape;
        public playPoint(int x, int y)
        { 
            Location = new Point(75+(x*25),(y*25)-25);
            Occupied = false;

        }
    }
}
