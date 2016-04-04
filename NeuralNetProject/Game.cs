using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NeuralNetProject
{
    class Game
    {
        Timer timer;
        GameForm gameform;
        public Smiley[] smileys;

        public Game(NNet net, Form1 form1) {
            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += new EventHandler(timerEvent);

            smileys = new Smiley[4];
            smileys[0] = new Smiley(net, new Point(1,1));
            smileys[1] = new Smiley(net, new Point(1, 15));
            smileys[2] = new Smiley(net, new Point(9, 1));
            smileys[3] = new Smiley(net, new Point(9, 15));

            gameform = new GameForm();
            gameform.smileys = smileys;            
            gameform.Show();
            gameform.refreshImage();
            timer.Start();
        }

        public void timerEvent(object sender, EventArgs eArgs)
        {
            if (sender == timer)
            {
                smileys[0].moveSmiley(4);
                gameform.refreshImage();
            }
        }
    }
}
