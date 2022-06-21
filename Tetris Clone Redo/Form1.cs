using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris_Clone_Redo
{
    public partial class Form1 : Form
    {
        Timer time = new Timer();
        playPoint[,] playArea;
        PictureBox PlayAreaDisplay;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            #region prepare form
            //modifies the form to correct size
            Size = new Size(700, 700);
            BackColor = Color.Gainsboro;
            CenterToScreen();
            #endregion

            #region GetPlayArea
            //self explanitory
            playArea = new playPoint[10, 24];
            for (int c = 0; c< 10; c++)
            {
                Parallel.For(0, 24, i =>
                {
                    playArea[c, i] = new playPoint(c, i);
                });
            }
            PlayAreaDisplay = new PictureBox()
            {
                Location = playArea[0, 4].Location,
                Size = new Size(250, 500),
                BackColor = Color.AliceBlue
            };
            Controls.Add(PlayAreaDisplay);
            #endregion
        }
    }
}
