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
            g.Clear(Color.White);
            p = new Pen(Color.Black);
            for (int i=0; i<CurrentNet.InputWidth; i++) {
                g.DrawEllipse(p, r);
            }
            pictureBox1.Image = netDisplay;
        }
    }
}
