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
            var rand = new Random().Next(0, 1);
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
                    //Block colour
                    blockColor = Color.Black;
                    break;

            }
        }

        public void ActivateColour(Block Block)
        {
            Block.square.BackColor = blockColor;
        }
    }
}
