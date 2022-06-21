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
        Timer time = new Timer()
        {
            Interval = 500,
        };
        playPoint[,] playArea;
        PictureBox PlayAreaDisplay;
        ShapeInstance Shape;
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

            #region Button And Labels
            Press press = new Press(400,100,100,100);
            addEvents(press.button);
            Controls.Add(press.button);
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

            #region First Shape
            Shape = new ShapeInstance(playArea);
            foreach (ShapeInstance.Block i in Shape.Blocks)
            { 
                Controls.Add(i.square);
            }
            #endregion
        }

        private void addEvents(Button button)
        {
            button.Click += (sender, e) => { Button_Click(sender, e, button); };
            time.Tick += Time_Tick;
        }

        private void Time_Tick(object sender, EventArgs e)
        {
            MoveDown();
        }

        private void Button_Click(object sender, EventArgs e, Button b)
        {
            b.Hide();
            time.Start();
        }

        private void MoveDown()
        {
            Parallel.For(0, 4, i =>
            {
                Shape.Blocks[i].square.Location = playArea[Shape.Blocks[i].xpos, Shape.Blocks[i].ypos++].Location;
                if (Shape.Blocks[i].ypos == 4)
                {
                    Shape.ActivateColour(Shape.Blocks[i]);
                }
            });

        }
    }
}
