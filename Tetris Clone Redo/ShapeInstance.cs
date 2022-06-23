using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Tetris_Clone_Redo
{
    internal class ShapeInstance
    {
        public struct Block 
        {
            public Label square;
            public int xpos;
            public int ypos;
        }

        public Block[] Blocks;
        Color blockColor;
        public int CentreIndex;
        public ShapeInstance(playPoint[,] playArea)
        { 
            Blocks = new Block[4];
            blockColor = new Color();
            Parallel.For(0, 4, i =>
            {
                Blocks[i].square = new Label()
                {
                    Size = new Size(25,25),
                };
            });
            var rand = new Random().Next(0, 7);
            SetPosition(playArea, rand);
        }

        private void SetPosition(playPoint[,] playArea, int rand)
        {
            switch (rand)
            {
                case 0:
                    //xpos
                    Blocks[0].xpos = 4;
                    Blocks[1].xpos = 4;
                    Blocks[2].xpos = 4;
                    Blocks[3].xpos = 4;

                    //ypos
                    Blocks[0].ypos = 0;
                    Blocks[1].ypos = 1;
                    Blocks[2].ypos = 2;
                    Blocks[3].ypos = 3;

                    //positions
                    Parallel.For(0, 4, i =>
                    {
                        Blocks[i].square.Location = playArea[Blocks[i].xpos, Blocks[i].ypos].Location;
                    });
                    CentreIndex = 1;
                    //Block colour
                    blockColor = Color.Black;
                    break;

                case 1:
                    Blocks[0].xpos = 4;
                    Blocks[1].xpos = 5;
                    Blocks[2].xpos = 4;
                    Blocks[3].xpos = 4;

                    //ypos
                    Blocks[0].ypos = 1;
                    Blocks[1].ypos = 2;
                    Blocks[2].ypos = 2;
                    Blocks[3].ypos = 3;

                    //positions
                    Parallel.For(0, 4, i =>
                    {
                        Blocks[i].square.Location = playArea[Blocks[i].xpos, Blocks[i].ypos].Location;
                        Blocks[i].square.Tag = "Label";
                    });
                    CentreIndex = 2;
                    blockColor = Color.Red;
                    break;
                case 2:
                    Blocks[0].xpos = 4;
                    Blocks[1].xpos = 4;
                    Blocks[2].xpos = 5;
                    Blocks[3].xpos = 5;

                    //ypos
                    Blocks[0].ypos = 2;
                    Blocks[1].ypos = 3;
                    Blocks[2].ypos = 2;
                    Blocks[3].ypos = 3;

                    //positions
                    Parallel.For(0, 4, i =>
                    {
                        Blocks[i].square.Location = playArea[Blocks[i].xpos, Blocks[i].ypos].Location;
                    });
                    CentreIndex = -1;
                    blockColor = Color.Purple;
                    break;
                case 3:
                    Blocks[0].xpos = 4;
                    Blocks[1].xpos = 4;
                    Blocks[2].xpos = 4;
                    Blocks[3].xpos = 3;

                    //ypos
                    Blocks[0].ypos = 1;
                    Blocks[1].ypos = 2;
                    Blocks[2].ypos = 3;
                    Blocks[3].ypos = 3;

                    //positions
                    Parallel.For(0, 4, i =>
                    {
                        Blocks[i].square.Location = playArea[Blocks[i].xpos, Blocks[i].ypos].Location;
                    });
                    CentreIndex = 2;
                    blockColor = Color.Orange;
                    break;
                case 4:
                    Blocks[0].xpos = 4;
                    Blocks[1].xpos = 4;
                    Blocks[2].xpos = 4;
                    Blocks[3].xpos = 5;

                    //ypos
                    Blocks[0].ypos = 1;
                    Blocks[1].ypos = 2;
                    Blocks[2].ypos = 3;
                    Blocks[3].ypos = 3;

                    //positions
                    Parallel.For(0, 4, i =>
                    {
                        Blocks[i].square.Location = playArea[Blocks[i].xpos, Blocks[i].ypos].Location;
                    });
                    CentreIndex = 2;
                    blockColor = Color.Pink;
                    break;
                case 5:
                    Blocks[0].xpos = 4;
                    Blocks[1].xpos = 4;
                    Blocks[2].xpos = 5;
                    Blocks[3].xpos = 5;

                    //ypos
                    Blocks[0].ypos = 1;
                    Blocks[1].ypos = 2;
                    Blocks[2].ypos = 2;
                    Blocks[3].ypos = 3;

                    //positions
                    Parallel.For(0, 4, i =>
                    {
                        Blocks[i].square.Location = playArea[Blocks[i].xpos, Blocks[i].ypos].Location;
                    });
                    CentreIndex = 2;
                    blockColor = Color.Green;
                    break;
                case 6:
                    Blocks[0].xpos = 5;
                    Blocks[1].xpos = 5;
                    Blocks[2].xpos = 4;
                    Blocks[3].xpos = 4;

                    //ypos
                    Blocks[0].ypos = 1;
                    Blocks[1].ypos = 2;
                    Blocks[2].ypos = 2;
                    Blocks[3].ypos = 3;

                    //positions
                    Parallel.For(0, 4, i =>
                    {
                        Blocks[i].square.Location = playArea[Blocks[i].xpos, Blocks[i].ypos].Location;
                    });
                    CentreIndex = 2;
                    blockColor = Color.YellowGreen;
                    break;
            }
        }

        public void ActivateColour(Block Block)
        {
            Block.square.BackColor = blockColor;
        }
    }
}
