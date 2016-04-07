using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NeuralNetProject
{
    class Game
    {
        public static Random rand = new Random();
        Timer timer;
        GameForm gameform;
        public Smiley[] smileys;
        int[,] map = { { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 },
                        {1,0,0,0,2,2,2,2,2,2,2,2,2,0,0,0,1},
                        {1,0,1,2,1,2,1,2,1,2,1,2,1,2,1,0,1},
                        {1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0,1},
                        {1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1},
                        {1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1},
                        {1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1},
                        {1,0,2,2,2,2,2,2,2,2,2,2,2,2,2,0,1},
                        {1,0,1,2,1,2,1,2,1,2,1,2,1,2,1,0,1},
                        {1,0,0,2,2,2,2,2,2,2,2,2,2,0,0,0,1},
                        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},};
        //0=ground, 1=solid, 2=breakable
        public Game(NNet net, Form1 form1) {
            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += new EventHandler(timerEvent);

            smileys = new Smiley[4];

            smileys[0] = new Smiley(net.copyNet().randomizeNet((int)(net.weightRange * 100)), new Point(1, 1));
            smileys[1] = new Smiley(net.copyNet().randomizeNet((int)(net.weightRange * 100)), new Point(15, 1));
            smileys[2] = new Smiley(net.copyNet().randomizeNet((int)(net.weightRange * 100)), new Point(1, 9));
            smileys[3] = new Smiley(net.copyNet().randomizeNet((int)(net.weightRange * 100)), new Point(15, 9));

            gameform = new GameForm(map);
            for (int i = 0; i < smileys.Length; i++) {
                gameform.updateSmileyPoint(i, smileys[i].position);
            }
            gameform.Show();
            gameform.refreshImage();
            timer.Start();
        }

        public void chooseBestOffspring(int which) {
            NNet newNet = smileys[which].net;
            for (int i=0; i<smileys.Length; i++) {
                smileys[i].net = newNet.copyNet().randomizeNet((int)(newNet.WeightRange*100));
            }
        }

        public void timerEvent(object sender, EventArgs eArgs)
        {
            if (sender == timer)
            {
                double[] tiles;
                double[] output;
                int action;

                if (gameform.bestOffspring>=0) {
                    chooseBestOffspring(gameform.bestOffspring);
                }

                Debug.WriteLine("");
                for (int j=0; j<smileys.Length; j++) {
                    //tiles = getPositionTiles(smileys[j].position);
                    //smileys[j].net.setInputs(tiles);
                    smileys[j].net.setInputs(getNetInputData(smileys[j].position));
                    smileys[j].net.runNet();
                    output = smileys[j].net.getOutputs();
                    for (int i = 0; i < output.Length; i++) {
                        Debug.Write(output[i] + ",");
                    }
                    action = smileys[j].net.getAction();
                    if (action == 4)
                        placeBomb(smileys[j].position);
                    Debug.Write("Action:" + action+" Smiley:"+j);
                    moveSmiley(j, action);
                    //moveSmiley(0,1);
                    Debug.WriteLine("");
                }
                //smileys[0].net.setInputs();
                //moveSmiley(0, 1);
                gameform.refreshImage();
            }
        }

        public void placeBomb(Point position) {
            map[position.Y, position.X] = 3;
        }

        public double[] getNetInputData(Point position) {
            double[] data = new double[21];
            int pos = 0, mapValue;
            Point[] testPoints = { new Point(position.X, position.Y), new Point(position.X, position.Y - 1), new Point(position.X - 1, position.Y), new Point(position.X + 1, position.Y), new Point(position.X, position.Y + 1) };

            //wall data
            for (int i = 1; i < testPoints.Length; i++) {
                mapValue = map[testPoints[i].Y, testPoints[i].X];
                if (mapValue == 1) {
                    data[pos++] = 1;
                    data[pos++] = 0;
                }
                else {
                    data[pos++] = 0;
                    data[pos++] = 1;
                }
            }

            //bomb data
            for (int i = 0; i < testPoints.Length; i++){
                mapValue = map[testPoints[i].Y, testPoints[i].X];
                if (mapValue == 3)
                {
                    data[pos++] = 1;
                }
                else {
                    data[pos++] = 0;
                }
            }

            return data;
        }

        public double[] getPositionTiles(Point position) {
            double[] tiles = new double[27];
            int pos = 0;
            int dist = 3;

            for (int y=0; y<dist; y++) {
                for (int x=0; x< dist; x++) {
                    int value = map[y + position.Y - 1, x + position.X - 1];
                    if (value == 0)
                        tiles[pos++] = 1;
                    else
                        tiles[pos++] = 0;
                    if(value == 1)
                        tiles[pos++] = 1;
                    else
                        tiles[pos++] = 0;
                    if(value == 2)
                        tiles[pos++] = 1;
                    else
                        tiles[pos++] = 0;

                }
            }

            return tiles;
        }

        public void moveSmiley(int which, int direction) {
            Point position = smileys[which].position;

            if (isDirectionClear(position, direction)) {
            //if (true) { 
                smileys[which].moveSmiley(direction);
                gameform.updateSmileyPoint(which, smileys[which].position);
            }
            else {
                smileys[which].giveNegativeFeedback();
            }
        }

        public void setSmileyNet(int which, NNet net) {
            smileys[which].net = net;
        }

        public bool isDirectionClear(Point position, int direction) {
            int y = position.X;
            int x = position.Y;

            //Debug.WriteLine(y+","+x);

            switch (direction) {
                case 0:
                    if (map[x+1, y] == 0) return true;//down
                    break;
                case 1:
                    if (map[x-1, y] == 0) return true;//up
                    break;
                case 2:
                    if (map[x, y-1] == 0) return true;//left
                    break;
                case 3:
                    if (map[x, y+1] == 0) return true;//actually right
                    break;
            }
            return false;
        }
    }
}
