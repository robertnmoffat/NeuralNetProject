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
        bool alive;
        NNet net;

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
                case 1:
                    position.Y++;
                    break;
                case 2:
                    position.Y--;
                    break;
                case 3:
                    position.X--;
                    break;
                case 4:
                    position.X++;
                    break;
            }
        }
    }
}
