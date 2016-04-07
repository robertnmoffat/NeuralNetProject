using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NeuralNetProject
{
    public partial class GameForm : Form
    {
        Graphics g;
        Bitmap canvas = new Bitmap(722,482);
        Bitmap smiley, breakable, solid, ground, horiExp, vertExp, expLeft, expRight, expUp, expDown, bombLeft, bombRight;
        public int bestOffspring = -1;

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            bestOffspring = 2;
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            bestOffspring = 3;
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            bestOffspring = 1;
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            bestOffspring = 0;
        }

        public Bitmap currentTile;
        Point[] smileyPositions = {new Point(1,1),new Point(1,9), new Point(15,1), new Point(15,9) };
        
        int[,] map;

        public GameForm(int[,] map)
        {
            this.map = map;
            solid = (Bitmap)Image.FromFile("solid.bmp");
            ground = (Bitmap)Image.FromFile("ground.bmp");
            breakable = (Bitmap)Image.FromFile("breakable.bmp");
            smiley = (Bitmap)Image.FromFile("smiley.bmp");
            bombLeft = (Bitmap)Image.FromFile("bomb_left.bmp");
            bombRight = (Bitmap)Image.FromFile("bomb_right.bmp");
            g = Graphics.FromImage(canvas);
            InitializeComponent();
        }        
        

        public void refreshImage() {
            g.Clear(Color.White);
            
            for (int y=0; y<map.GetLength(1); y++) {
                for (int x=0; x<map.GetLength(0); x++) {
                    currentTile = translateMapTile(map[x,y]);

                    for (int i = 0; i < smileyPositions.Length; i++) {
                        if (smileyPositions[i].Y == x && smileyPositions[i].X == y)
                            currentTile = smiley;
                    }

                    g.DrawImage(currentTile, new Point(y*42,x*42));
                    
                }
            }
            pictureBox1.Image = canvas;
        }


        public Bitmap translateMapTile(int tile) {
            switch (tile) {
                case 0:
                    return ground;
                case 1:
                    return solid;
                case 2:
                    return breakable;
                case 3:
                    return bombLeft;
                default:
                    return ground;
            }
        }

        /// <summary>
        /// Update smileys position
        /// </summary>
        /// <param name="which"></param>
        /// <param name="position"></param>
        public void updateSmileyPoint(int which, Point position) {
            smileyPositions[which].X = position.X;
            smileyPositions[which].Y = position.Y;
        }
    }
}
