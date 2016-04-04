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
        Bitmap smiley, breakable, solid, ground, horiExp, vertExp, expLeft, expRight, expUp, expDown;
        public Bitmap currentTile;
        Point[] smileyPositions = {new Point(1,1),new Point(1,9), new Point(15,1), new Point(15,9) };
        public Smiley[] smileys;
        int[,] map = { { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 }, 
                        {1,0,0,2,2,2,2,2,2,2,2,2,2,2,0,0,1},
                        {1,0,1,2,1,2,1,2,1,2,1,2,1,2,1,0,1},
                        {1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1},
                        {1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1},
                        {1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1},
                        {1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1},
                        {1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1},
                        {1,0,1,2,1,2,1,2,1,2,1,2,1,2,1,0,1},
                        {1,0,0,2,2,2,2,2,2,2,2,2,2,2,0,0,1},
                        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},};

        public GameForm()
        {
            solid = (Bitmap)Image.FromFile("solid.bmp");
            ground = (Bitmap)Image.FromFile("ground.bmp");
            breakable = (Bitmap)Image.FromFile("breakable.bmp");
            smiley = (Bitmap)Image.FromFile("smiley.bmp");
            g = Graphics.FromImage(canvas);
            InitializeComponent();
        }        
        

        public void refreshImage() {
            g.Clear(Color.White);
            
            for (int y=0; y<map.GetLength(1); y++) {
                for (int x=0; x<map.GetLength(0); x++) {
                    currentTile = translateMapTile(map[x,y]);

                    for (int i = 0; i < smileys.Length; i++) {
                        if (smileys[i].position.X == y && smileys[i].position.Y == x)
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
                default:
                    return ground;
            }
        }  
    }
}
