using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris_Clone_Redo
{
    internal class Press
    {
        public Button button;

        public Press(int x, int y, int width, int height)
        {
            button = new Button()
            {
                Location = new System.Drawing.Point(x, y),
                Size = new System.Drawing.Size(width, height),
                Text = "start"
            };
        }
    }
}
