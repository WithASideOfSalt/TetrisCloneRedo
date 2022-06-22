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
        BackgroundWorker worker;
        ShapeInstance Shape;
        KeyEventArgs KeyPressed;
        
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
            KeyPreview = true;
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

            #region Solo thread for movement
            worker = new BackgroundWorker();
            worker.DoWork += ShapeMovement;
            #endregion

            #region First Shape
            Shape = new ShapeInstance(playArea);
            for (int i =0; i< 4; i++)
            {
                Controls.Add(Shape.Blocks[i].square);
                playArea[Shape.Blocks[i].xpos, Shape.Blocks[i].ypos].OccupiedByCurrentShape = true;
                playArea[Shape.Blocks[i].xpos, Shape.Blocks[i].ypos].Occupied = true;
                Shape.Blocks[i].square.BringToFront();
            }
            #endregion

            this.Focus();
        }

        private void ShapeMovement(object sender, DoWorkEventArgs e)
        {
            switch (KeyPressed.KeyCode)
            {
                case Keys.Left:
                    if (canMove(2))
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            playArea[Shape.Blocks[i].xpos, Shape.Blocks[i].ypos].OccupiedByCurrentShape = false;
                            playArea[Shape.Blocks[i].xpos, Shape.Blocks[i].ypos].Occupied = false;
                            Shape.Blocks[i].xpos--;
                            Shape.Blocks[i].square.Invoke((MethodInvoker)delegate { Shape.Blocks[i].square.Location = playArea[Shape.Blocks[i].xpos, Shape.Blocks[i].ypos].Location; });
                        }
                        for (int i = 0; i < 4; i++)
                        {
                            playArea[Shape.Blocks[i].xpos, Shape.Blocks[i].ypos].OccupiedByCurrentShape = true;
                            playArea[Shape.Blocks[i].xpos, Shape.Blocks[i].ypos].Occupied = true;
                        }
                    }
                    break;
                case Keys.Right:
                    if (canMove(3))
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            playArea[Shape.Blocks[i].xpos, Shape.Blocks[i].ypos].OccupiedByCurrentShape = false;
                            playArea[Shape.Blocks[i].xpos, Shape.Blocks[i].ypos].Occupied = false;
                            Shape.Blocks[i].xpos++;
                            Shape.Blocks[i].square.Invoke((MethodInvoker)delegate { Shape.Blocks[i].square.Location = playArea[Shape.Blocks[i].xpos, Shape.Blocks[i].ypos].Location; });
                        }
                        for (int i = 0; i < 4; i++)
                        {
                            playArea[Shape.Blocks[i].xpos, Shape.Blocks[i].ypos].OccupiedByCurrentShape = true;
                            playArea[Shape.Blocks[i].xpos, Shape.Blocks[i].ypos].Occupied = true;
                        }
                    }
                    break;
                case Keys.Down:
                    MoveDown();
                    break;
            }
        }

        private void addEvents(Button button)
        {
            button.Click += (sender, e) => { Button_Click(sender, e, button); };
            time.Tick += Time_Tick;
            this.KeyDown += Form1_KeyDown;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            KeyPressed = e;
            worker.RunWorkerAsync();
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
            if (canMove(0))
            {
                for (int i = 0; i < 4; i++)
                {
                    playArea[Shape.Blocks[i].xpos, Shape.Blocks[i].ypos].OccupiedByCurrentShape = false;
                    playArea[Shape.Blocks[i].xpos, Shape.Blocks[i].ypos].Occupied = false;
                    Shape.Blocks[i].ypos++;
                    if (Shape.Blocks[i].square.InvokeRequired)
                    {
                        Shape.Blocks[i].square.Invoke((MethodInvoker)delegate { Shape.Blocks[i].square.Location = playArea[Shape.Blocks[i].xpos, Shape.Blocks[i].ypos].Location; });
                    }
                    else
                    {
                        Shape.Blocks[i].square.Location = playArea[Shape.Blocks[i].xpos, Shape.Blocks[i].ypos].Location;
                    }

                    if (Shape.Blocks[i].ypos == 4)
                    {
                        Shape.ActivateColour(Shape.Blocks[i]);
                    }
                }
                for (int i = 0; i < 4; i++)
                {
                    playArea[Shape.Blocks[i].xpos, Shape.Blocks[i].ypos].OccupiedByCurrentShape = true;
                    playArea[Shape.Blocks[i].xpos, Shape.Blocks[i].ypos].Occupied = true;
                }
            }
            else 
            {
                for (int c = 0; c< 4; c++)
                {
                    playArea[Shape.Blocks[c].xpos, Shape.Blocks[c].ypos].OccupiedByCurrentShape = false;
                }
                time.Stop();
                NextShape();
            }
        }
        private bool canMove(int identifier)
        {
            bool move = true;
            switch (identifier)
            {
                //Move shape down check
                case 0:
                    for (int i = 0; i< 4; i++)
                    {
                        int x = Shape.Blocks[i].xpos;
                        int y = Shape.Blocks[i].ypos;

                        if (y == 23)
                        {
                            move = false;
                            return move;
                        }
                        else if (!(playArea[x, y + 1].OccupiedByCurrentShape)
                                && playArea[x, y + 1].Occupied)
                        {
                            move = false;
                            return move;
                        }
                    }
                    return move;

                //Rotate Shape check
                case 1:
                    return move;
                //Shape Movement Check
                case 2:
                    for (int i = 0; i< 4; i++)
                    {
                        int x = Shape.Blocks[i].xpos;
                        int y = Shape.Blocks[i].ypos;

                        if (x == 0)
                        {
                            move = false;
                            return move;
                        }
                        else if (!(playArea[x - 1, y].OccupiedByCurrentShape)
                                && playArea[x - 1, y].Occupied)
                        {
                            move = false;
                            return move;
                        }
                    }
                    break;
                case 3:
                    for (int i = 0; i < 4; i++)
                    {
                        int x = Shape.Blocks[i].xpos;
                        int y = Shape.Blocks[i].ypos;

                        if (x == 9)
                        {
                            move = false;
                            return move;
                        }
                        else if (!(playArea[x + 1,y].OccupiedByCurrentShape)
                                && playArea[x + 1, y].Occupied)
                        {
                            move = false;
                            return move;
                        }
                    }
                    break;
            }

            return move;
        }

        private void NextShape() 
        {
            Shape = null;
            Shape = new ShapeInstance(playArea);
            for (int i = 0; i < 4; i++)
            {
                Controls.Add(Shape.Blocks[i].square);
                playArea[Shape.Blocks[i].xpos, Shape.Blocks[i].ypos].OccupiedByCurrentShape = true;
                playArea[Shape.Blocks[i].xpos, Shape.Blocks[i].ypos].Occupied = true;
                Shape.Blocks[i].square.BringToFront();
            }
            time.Start();
        }

    }
}
