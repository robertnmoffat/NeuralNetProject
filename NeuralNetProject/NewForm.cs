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
    public partial class NewForm : Form
    {
        Form1 parent;

        public NewForm(Form1 parent)
        {
            this.parent = parent;
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int inputs = (int)numericUpDown1.Value;
            int rows = (int)numericUpDown2.Value;
            int rowWidth = (int)numericUpDown3.Value;
            double weightRange = (double)numericUpDown4.Value;
            double weightAvg = (double)numericUpDown5.Value;
            parent.CurrentNet = new NNet(inputs,rows,rowWidth,weightRange,weightAvg);
            parent.CurrentNet.initializeNet();
            parent.CurrentNet.randomizeNet((int)(weightRange*100));
            parent.CurrentNet.randomizeNet((int)(weightRange * 100));
            parent.CurrentNet.randomizeNet((int)(weightRange * 100));
            parent.CurrentNet.randomizeNet((int)(weightRange * 100));
            parent.refreshImage();
            this.Dispose();
        }
    }
}
