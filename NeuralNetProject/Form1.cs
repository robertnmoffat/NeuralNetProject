using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NeuralNetProject
{
    public partial class Form1 : Form
    {
        private NNet currentNet;
        private Game game;
        private Bitmap netDisplay;
        private Graphics g;
        private Rectangle r = new Rectangle(0,0,20,20);
        private Pen p;

        public Form1()
        {
            netDisplay = new Bitmap(688, 404);
            g = Graphics.FromImage(netDisplay);
            InitializeComponent();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewForm creator = new NewForm(this);
            creator.Show();
        }

        internal NNet CurrentNet
        {
            get
            {
                return currentNet;
            }

            set
            {
                currentNet = value;
            }
        }

        private void bomberDudeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game = new Game(currentNet, this);
        }

        public void refreshImage() {
            Point[] inputPoints = new Point[currentNet.InputWidth];
            Point[,] hiddenInputPoints = new Point[currentNet.rowWidth, currentNet.amountOfRows];
            int horiSpacing = 45;
            int vertSpacing = 100;
            g.Clear(Color.White);
            p = new Pen(Color.Black);
            int centeringOffset = currentNet.InputWidth * horiSpacing / 2;
            for (int i=0; i<CurrentNet.InputWidth; i++) {
                r.X = pictureBox1.Width/2 - centeringOffset + i * horiSpacing;
                inputPoints[i] = new Point(r.X+10,r.Y+10);
                g.DrawEllipse(p, r);
            }

            centeringOffset = currentNet.rowWidth * horiSpacing / 2;
            for (int y=0; y<currentNet.amountOfRows; y++) {
                for (int x=0; x<currentNet.rowWidth; x++) {
                    r.Y = vertSpacing * (y+1);
                    r.X = pictureBox1.Width / 2 - centeringOffset + x * horiSpacing;
                    hiddenInputPoints[x, y] = new Point(r.X + 10, r.Y + 10);
                    g.DrawEllipse(p, r);

                    double[] currentWeights = currentNet.hiddenNeurons[x, y].getWeights();
                    for (int i=0; i<inputPoints.Length; i++) {
                        p.Width = (float)currentWeights[i]+1;
                        //int darkness = (int)Math.Abs(255/Math.Round(currentWeights[i] * 100));
                        Debug.WriteLine(currentWeights[i]);
                        g.DrawLine(p, inputPoints[i], new Point(r.X+10, r.Y+10));
                        p.Width = 1;
                    }
                }
            }

            centeringOffset = currentNet.outputCount * horiSpacing / 2;
            r.Y = vertSpacing * (currentNet.amountOfRows + 1);
            for (int i=0; i<currentNet.outputCount; i++) {                
                r.X = pictureBox1.Width / 2 - centeringOffset + i * horiSpacing;
                g.DrawEllipse(p, r);

                double[] currentWeights = currentNet.outputNeurons[i].getWeights();
                for (int j = 0; j < currentNet.rowWidth; j++)
                {
                    p.Width = (float)currentWeights[j] + 1;
                    //int darkness = (int)Math.Abs(255/Math.Round(currentWeights[i] * 100));
                    Debug.WriteLine(currentWeights[j]);
                    g.DrawLine(p, hiddenInputPoints[j, currentNet.amountOfRows-1], new Point(r.X + 10, r.Y + 10));
                    p.Width = 1;
                }
            }

            pictureBox1.Image = netDisplay;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NNet net = currentNet.copyNet();
            net.randomizeNet((int)(net.weightRange*100));
            currentNet = net;
            refreshImage();
            game.setSmileyNet(0, currentNet);
        }
    }
}
