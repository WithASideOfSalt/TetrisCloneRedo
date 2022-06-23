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
            Press press = new Press(400, 100, 100, 100);
            addEvents(press.button);
            Controls.Add(press.button);
            #endregion


            #region GetPlayArea
            //self explanitory
            playArea = new playPoint[10, 24];
            for (int c = 0; c < 10; c++)
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
            for (int i = 0; i < 4; i++)
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
            List<int> notNeeded = new List<int>();
            switch (KeyPressed.KeyCode)
            {
                case Keys.Left:
                    if (canMove(2, ref notNeeded))
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
                    if (canMove(3, ref notNeeded))
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
                    bool check = true;
                    for (int i = 0; i < 4; i++)
                    {
                        if (Shape.Blocks[i].ypos >= 22)
                        {
                            check = false;
                            break;
                        }
                        else if ((playArea[Shape.Blocks[i].xpos, Shape.Blocks[i].ypos + 2].Occupied
                            && !playArea[Shape.Blocks[i].xpos, Shape.Blocks[i].ypos + 2].OccupiedByCurrentShape)
                            || (playArea[Shape.Blocks[i].xpos, Shape.Blocks[i].ypos + 1].Occupied
                            && !playArea[Shape.Blocks[i].xpos, Shape.Blocks[i].ypos + 1].OccupiedByCurrentShape))
                        {
                            check = false;
                            break;
                        }
                    }
                    if (check)
                    {
                        MoveDown();
                    }
                    break;
                case Keys.Up:
                    RotateShape();
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
            if (!worker.IsBusy)
            {
                worker.RunWorkerAsync();
            }
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
            List<int> addedPositions = new List<int>();
            if (canMove(0, ref addedPositions))
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

                    if (Shape.Blocks[i].ypos >= 4)
                    {
                        Shape.ActivateColour(Shape.Blocks[i]);
                    }

                    if (Shape.Blocks[i].ypos < 4)
                    {
                        Shape.Blocks[i].square.BackColor = Color.Gainsboro;
                    }
                }
                for (int i = 0; i< 4; i++)
                {
                    playArea[Shape.Blocks[i].xpos, Shape.Blocks[i].ypos].OccupiedByCurrentShape = true;
                    playArea[Shape.Blocks[i].xpos, Shape.Blocks[i].ypos].Occupied = true;
                }
            }
            else
            {
                if (addedPositions.Count > 0)
                {
                    bool[] removeCheck = canRemove(addedPositions);
                    for (int i = 0; i < 4; i++)
                    {
                        if (removeCheck[i])
                        {
                            removeRow(addedPositions, removeCheck);
                            break;
                        }
                    }
                }
                for (int c = 0; c < 4; c++)
                {
                    playArea[Shape.Blocks[c].xpos, Shape.Blocks[c].ypos].OccupiedByCurrentShape = false;
                }
                time.Stop();
                NextShape();
            }
        }
        private bool canMove(int identifier,ref List<int> addedPositions)
        {
            bool move = true;
            switch (identifier)
            {
                //Move shape down check
                case 0:
                    for (int i = 0; i < 4; i++)
                    {
                        int x = Shape.Blocks[i].xpos;
                        int y = Shape.Blocks[i].ypos;

                        if (y == 23)
                        {
                            addedPositions.Add(y);
                            move = false;
                            return move;
                        }
                        else if (!(playArea[x, y + 1].OccupiedByCurrentShape)
                                && playArea[x, y + 1].Occupied)
                        {
                            addedPositions.Add(y);
                            move = false;
                            return move;
                        }
                    }
                    return move;
                //Shape Movement Check
                case 2:
                    for (int i = 0; i < 4; i++)
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
                        else if (!(playArea[x + 1, y].OccupiedByCurrentShape)
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

        private bool[] canRemove(List<int> addedPositions)
        {
            bool[] check = new bool[4];

            for (int i = 0; i < addedPositions.Count; i++)
            {
                for (int b = 0; b < 4; b++)
                {
                    check[b] = true;
                    for (int x = 0; x < 10; x++)
                    {
                        if (!playArea[x, addedPositions[i]].Occupied)
                        {
                            check[b] = false;
                            break;
                        }
                    }
                }
            }
            return check;
        }

        private bool canRotate(ref int[] UpdatedXPos, ref int[] UpdatedYPos)
        {
            bool check = true;
            var temp = 0;
            int xdifference;
            int ydifference;

            for (int i = 0; i < 4; i++)
            {
                if (Shape.CentreIndex == -1)
                {
                    break;
                }
                xdifference = Shape.Blocks[Shape.CentreIndex].xpos - Shape.Blocks[i].xpos;
                ydifference = Shape.Blocks[Shape.CentreIndex].ypos - Shape.Blocks[i].ypos;

                UpdatedXPos[i] = Shape.Blocks[Shape.CentreIndex].xpos + ydifference;
                UpdatedYPos[i] = Shape.Blocks[Shape.CentreIndex].ypos - xdifference;

                if (UpdatedYPos[i] > 23)
                {
                    check = false;
                    return check;
                }
                if (UpdatedXPos[i] < 0)
                {
                    temp = UpdatedXPos[i];
                    for (int c = 0; c < 4; c++)
                    {
                        UpdatedXPos[c] -= temp;
                    }
                }
                else if (UpdatedXPos[i] > 9)
                {
                    temp = UpdatedXPos[i] - 9;
                    for (int c = 0; c < 4; c++)
                    {
                        UpdatedXPos[c] -= temp;
                    }
                }

                else if (playArea[UpdatedXPos[i], UpdatedYPos[i]].Occupied && !playArea[UpdatedXPos[i], UpdatedYPos[i]].OccupiedByCurrentShape)
                {
                    check = false;
                    return check;
                }

            }

            for (int i =0; i < 4; i++)
            {
                UpdatedXPos[i] -= temp;
            }
            
            return check;
        }

        private void removeRow(List<int> row, bool[] check)
        {
            for (int i =0; i < row.Count; i++)
            {
                if (check[i])
                {
                    int[] remove = new int[10];
                    for (int c = 0; c < Controls.Count; c++)
                    {
                        for (int x = 0; x < 10; x++)
                        {
                            if (Controls[c].Location == playArea[x, row[i]].Location)
                            {
                                playArea[x, row[i]].OccupiedByCurrentShape = false; 
                                playArea[x, row[i]].Occupied = false;

                                Controls[c].Invoke((MethodInvoker)delegate { Controls.RemoveAt(c);});
                            }
                        }
                    }
                    List<int> xpos = new List<int>();
                    List<int> ypos = new List<int>();
                    for (int c = 0; c < Controls.Count; c++)
                    {
                        var t = typeof(Label);
                        if (t == Controls[c].GetType())
                        {
                            
                            Controls[c].Invoke((MethodInvoker)delegate { Controls[c].Location = new Point(Controls[c].Location.X, Controls[c].Location.Y + Controls[c].Height); });
                        }
                        for (int x = 0; x < 9; x++)
                        {
                            for (int y = 4; y < 24; y++)
                            {
                                if (Controls[c].Location == playArea[x, y].Location)
                                {
                                    xpos.Add(x);
                                    ypos.Add(y);
                                    playArea[x, y].Occupied = false;
                                    break;
                                }
                            }
                        }
                        for (int e = 0; e < xpos.Count; e++)
                        {
                            if (ypos[e] + 1 == 24)
                            {
                                break;
                            }
                            playArea[xpos[e], ypos[e] + 1].Occupied = true;
                        }
                    }
                }
            }
        }

        private void RotateShape()
        {
            int[] UpdatedXPos = new int[4];
            int[] UpdatedYPos = new int[4];

            if (canRotate(ref UpdatedXPos, ref UpdatedYPos) && Shape.CentreIndex != -1)
            {
                for (int i = 0; i< 4; i++)
                {
                    if (Shape.Blocks[i].square.InvokeRequired)
                    {
                        playArea[Shape.Blocks[i].xpos, Shape.Blocks[i].ypos].OccupiedByCurrentShape = false;
                        playArea[Shape.Blocks[i].xpos, Shape.Blocks[i].ypos].Occupied = false;

                        Shape.Blocks[i].xpos = UpdatedXPos[i];
                        Shape.Blocks[i].ypos = UpdatedYPos[i];

                        Shape.Blocks[i].square.Invoke((MethodInvoker)delegate { Shape.Blocks[i].square.Location = playArea[UpdatedXPos[i], UpdatedYPos[i]].Location;});
                    }
                }
                for (int i = 0; i< 4; i++)
                {
                    playArea[Shape.Blocks[i].xpos, Shape.Blocks[i].ypos].OccupiedByCurrentShape = true;
                    playArea[Shape.Blocks[i].xpos, Shape.Blocks[i].ypos].Occupied = true;
                }
            }
        }

        private void NextShape() 
        {
            Shape = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
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
