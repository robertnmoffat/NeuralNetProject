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
        public int speed = 1;
        public bool resetMap = false;

        private void fastToolStripMenuItem_Click(object sender, EventArgs e)
        {
            speed = 1;
        }

        private void slowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            speed = 0;
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            bestOffspring = 2;
        }

        private void resetMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            resetMap = true;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        public int getMovementReward() {
            return (int)numericUpDown1.Value;
        }

        public int getWallHitPunish() {
            return (int)numericUpDown2.Value;
        }

        public int getBombReward() {
            return (int)numericUpDown3.Value;
        }

        public int getWallBreakReward() {
            return (int)numericUpDown4.Value;
        }

        public int getTurningReward() {
            return (int)numericUpDown5.Value;
        }

        public bool isInitialMap() {
            return checkBox1.Checked;
        }

        public bool isStartBombs() {
            return checkBox2.Checked;
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
        public Point[] smileyPositions;
        public int[] smileyScores = { 0, 0, 0, 0 };
        
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
            horiExp = (Bitmap)Image.FromFile("explosion_horizontal.bmp");
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
            textBox1.Text = ""+smileyScores[0];
            textBox2.Text = "" + smileyScores[1];
            textBox3.Text = "" + smileyScores[2];
            textBox4.Text = "" + smileyScores[3];
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
                case 4:
                    return bombRight;
                case 5:
                    return bombLeft;
                case 6:
                    return bombRight;
                case 7:
                    return horiExp;
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

        public void initializeSmileyPositions() {
            for (int i=0; i<smileyPositions.Length; i++) {
                smileyPositions[i] = new Point();
            }
        }
    }
}
