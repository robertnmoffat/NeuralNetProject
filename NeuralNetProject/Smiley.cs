using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetProject
{
    class Smiley
    {
        public Point position;
        public bool alive=true;
        public NNet net;
        double[] memFlags = new double[4];
        public int score = 0;
        public int previousDirection = -1;

        public Smiley(NNet net, Point position) {
            this.net = net;
            this.position = position;
            alive = true;
        }

        /// <summary>
        /// Moves the smiley in the direction specified
        /// </summary>
        /// <param name="direction"></param>
        public void moveSmiley(int direction)
        {
            switch (direction)
            {
                case 0:
                    position.Y++;
                    break;
                case 1:
                    position.Y--;
                    break;
                case 2:
                    position.X--;
                    break;
                case 3:
                    position.X++;
                    break;
            }
        }
                
    }
}
