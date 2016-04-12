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
        int[,] map;
        int[,] initialMap = {   { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 },
         {1,0,0,0,2,2,2,2,2,2,2,2,2,0,0,0,1},
         {1,0,1,2,1,2,1,2,1,2,1,2,1,2,1,0,1},
         {1,0,2,2,2,2,2,2,2,2,2,2,2,2,2,0,1},
         {1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1},
         {1,2,2,2,2,2,2,0,2,2,2,2,2,2,2,2,1},
         {1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1},
         {1,0,2,2,2,2,2,2,2,2,2,2,2,2,2,0,1},
         {1,0,1,2,1,2,1,2,1,2,1,2,1,2,1,0,1},
         {1,0,0,0,2,2,2,2,2,2,2,2,2,0,0,0,1},
         {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},};

        int[,] initialMapEmpty = {   { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 },
                                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                                {1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1},
                                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                                {1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1},
                                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                                {1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1},
                                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                                {1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1},
                                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},};

        //int[,] map = { { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 },
        //                {1,0,0,0,0,0,2,2,2,2,2,2,2,0,0,0,1},
        //                {1,0,1,2,1,0,1,2,1,2,1,2,1,2,1,0,1},
        //                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
        //                {1,2,1,2,1,2,1,0,1,2,1,2,1,2,1,2,1},
        //                {1,2,2,2,2,2,2,0,0,0,0,0,2,2,2,2,1},
        //                {1,2,1,2,1,2,1,0,1,2,1,0,1,2,1,2,1},
        //                {1,0,2,2,2,2,2,0,2,2,2,0,2,2,2,0,1},
        //                {1,0,1,2,1,2,1,0,1,2,1,0,1,2,1,0,1},
        //                {1,0,0,2,2,2,2,0,0,0,2,0,0,0,0,0,1},
        //                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},};
        //0=ground, 1=solid, 2=breakable
        Point[] initialSmileyPoints = { new Point(1, 1), new Point(15, 1), new Point(1, 9), new Point(15, 9) };

        int currentTime = 0;
        int previousBestScore = -99999;

        int speed = 1;

        Form1 parentForm;

        public Game(NNet net, Form1 form1) {
            map = new int[initialMap.GetLength(0), initialMap.GetLength(1)];
            resetMap();
            parentForm = form1;
            timer = new Timer();
            timer.Interval = speed;
            timer.Tick += new EventHandler(timerEvent);

            smileys = new Smiley[64];

            for (int i=0; i<smileys.Length; i++) {
                smileys[i] = new Smiley(net.copyNet().randomizeNet((int)(net.weightRange * 100)), initialSmileyPoints[i%4]);
            }            

            gameform = new GameForm(map);
            gameform.smileyPositions = new Point[smileys.Length];
            gameform.initializeSmileyPositions();
            for (int i = 0; i < smileys.Length; i++) {
                gameform.updateSmileyPoint(i, smileys[i].position);
            }
            gameform.Show();
            gameform.refreshImage();
            timer.Start();
            
        }

        public void chooseBestOffspring(int which) {
            NNet newNet = smileys[which].net.copyNet();
            parentForm.CurrentNet = newNet;
            parentForm.refreshImage();
            for (int i=0; i<smileys.Length; i++) {
                smileys[i].net = newNet.copyNet().randomizeNet((int)(newNet.WeightRange*100));
                //smileys[i].net = newNet.copyNet();
            }
        }

        public void mutateFromCurrentNet() {
            NNet newNet = parentForm.CurrentNet.copyNet();

            for (int i = 0; i < smileys.Length; i++)
            {
                smileys[i].net = newNet.copyNet().randomizeNet((int)(newNet.WeightRange * 100));
                //smileys[i].net = newNet.copyNet();
            }
        }

        public void resetMap() {
            if (gameform == null || gameform.isInitialMap())
            {
                for (int y = 0; y < initialMap.GetLength(1); y++)
                {
                    for (int x = 0; x < initialMap.GetLength(0); x++)
                    {
                        map[x, y] = initialMap[x, y];
                    }
                }
            }
            else {
                for (int y = 0; y < initialMapEmpty.GetLength(1); y++)
                {
                    for (int x = 0; x < initialMapEmpty.GetLength(0); x++)
                    {
                        map[x, y] = initialMapEmpty[x, y];
                    }
                }
            }
        }

        public void timerEvent(object sender, EventArgs eArgs)
        {
            if (sender == timer)
            {
                if (gameform.resetMap) {
                    resetMap();
                    gameform.resetMap = false;
                }


                //-----------setting timer speed stuff-----------
                if (gameform.speed == 1 && speed != 1) {
                    speed = 1;
                    timer.Interval = speed;
                } else if (gameform.speed==0&&speed!=1000) {
                    speed = 500;
                    timer.Interval = speed;
                }
                 
                //-----------scanning map for updates-------------
                for (int y=0; y<map.GetLength(0); y++) {
                    for (int x = 0; x < map.GetLength(1); x++) {
                        if (map[y, x] == 7)
                            map[y, x] = 0;

                        if (map[y, x] == 3)
                            createExplosion(new Point(x,y));

                        if (map[y, x] == 6 || map[y, x] == 5 || map[y, x] == 4) {
                            map[y, x]--;
                        }
                    }
                }



                double[] tiles;
                double[] output;
                int action;

                //------------------checking if best offspring has been manually selected-----------------
                if (gameform.bestOffspring >= 0)
                {
                    chooseBestOffspring(gameform.bestOffspring);
                    for (int i = 0; i < smileys.Length; i++)
                    {
                        smileys[i].position = initialSmileyPoints[i];
                        gameform.updateSmileyPoint(i, smileys[i].position);
                    }
                    gameform.bestOffspring = -1;
                }
                else {


                    //----------------------running neural net--------------------------

                    //Debug.WriteLine("");
                    for (int j = 0; j < smileys.Length; j++)
                    {
                        if (map[smileys[j].position.Y, smileys[j].position.X] == 7)
                        {
                            smileys[j].alive = false;
                            smileys[j].score -= 60;
                            smileys[j].net.giveNegativeFeedback();
                            smileys[j].net.giveNegativeFeedback();
                            smileys[j].net.giveNegativeFeedback();
                            smileys[j].net.giveNegativeFeedback();
                            smileys[j].net.giveNegativeFeedback();
                            smileys[j].net.giveNegativeFeedback();
                        }
                        //tiles = getPositionTiles(smileys[j].position);
                        //smileys[j].net.setInputs(tiles);
                        smileys[j].net.setInputs(getNetInputData(smileys[j].position));
                        smileys[j].net.runNet();
                        output = smileys[j].net.getOutputs();
                        for (int i = 0; i < output.Length; i++)
                        {
                            //Debug.Write(output[i] + ",");
                        }
                        action = smileys[j].net.getAction();
                        if (action == 4&&map[smileys[j].position.Y, smileys[j].position.X]<3)
                        {
                            Debug.WriteLine("BOMB!!");
                            if (map[smileys[j].position.Y + 1, smileys[j].position.X] == 2)
                            {
                                smileys[j].score += gameform.getWallBreakReward();
                                smileys[j].net.givePositiveFeedback();
                                smileys[j].net.givePositiveFeedback();
                                smileys[j].net.givePositiveFeedback();
                            }
                            if (map[smileys[j].position.Y - 1, smileys[j].position.X] == 2)
                            {
                                smileys[j].score += gameform.getWallBreakReward();
                                smileys[j].net.givePositiveFeedback();
                                smileys[j].net.givePositiveFeedback();
                                smileys[j].net.givePositiveFeedback();
                            }
                            if (map[smileys[j].position.Y, smileys[j].position.X+1] == 2)
                            {
                                smileys[j].net.givePositiveFeedback();
                                smileys[j].net.givePositiveFeedback();
                                smileys[j].net.givePositiveFeedback();
                            }
                            if (map[smileys[j].position.Y, smileys[j].position.X-1] == 2)
                            {
                                smileys[j].score += gameform.getWallBreakReward();
                                smileys[j].net.givePositiveFeedback();
                                smileys[j].net.givePositiveFeedback();
                                smileys[j].net.givePositiveFeedback();
                            }
                            placeBomb(smileys[j].position);
                            smileys[j].score += gameform.getBombReward();
                            smileys[j].net.givePositiveFeedback();
                            smileys[j].net.givePositiveFeedback();
                            smileys[j].net.givePositiveFeedback();
                        }
                       // Debug.Write("Action:" + action + " Smiley:" + j);
                       if(action<4)
                            moveSmiley(j, action);
                        //moveSmiley(0,1);
                       // Debug.WriteLine("");
                    }
                    //smileys[0].net.setInputs();
                    //moveSmiley(0, 1);
                }

                if (speed != 1)
                {
                    gameform.refreshImage();
                    for (int i=0; i<4; i++) {
                        gameform.smileyScores[i] = smileys[i].score;
                    }
                }

            }

            currentTime++;

            if (currentTime == 1 && gameform.isStartBombs())
            {
                map[1, 1] = 6;
                map[1, 15] = 6;
                map[9, 1] = 6;
                map[9, 15] = 6;
            }

            if (currentTime==6&&gameform.isStartBombs()) {
                map[1, 1] = 6;
                map[1, 15] = 6;
                map[9, 1] = 6;
                map[9, 15] = 6;
            }

            if (currentTime == 15 && gameform.isStartBombs())
            {
                map[1, 1] = 6;
                map[1, 15] = 6;
                map[9, 1] = 6;
                map[9, 15] = 6;
            }

            if (currentTime>40) {
                int highestScore=-9999999;
                int highestSmiley=0;
                for (int i = 0; i < smileys.Length; i++){
                    smileys[i].score += getScoreDistanceBonus(i);
                    if (smileys[i].score>highestScore&&smileys[i].alive==true) {
                        highestScore = smileys[i].score;
                        highestSmiley = i;
                    }
                }
                
                Debug.WriteLine("("+ smileys[0].score + ","+smileys[1].score+","+smileys[2].score+","+smileys[3].score+")"+highestScore+", "+previousBestScore+" gen:"+smileys[highestSmiley].net.generation);

                if (highestScore > previousBestScore)
                {
                    if (smileys[highestSmiley].alive == false)
                    {
                        mutateFromCurrentNet();
                    }
                    else {
                        previousBestScore = highestScore;
                        chooseBestOffspring(highestSmiley);
                    }
                }
                else {
                    mutateFromCurrentNet();
                    previousBestScore = highestScore;
                }


                for (int i = 0; i < smileys.Length; i++)
                {
                    smileys[i].position = initialSmileyPoints[i%4];
                    smileys[i].score = 0;
                    smileys[i].alive = true;
                    gameform.updateSmileyPoint(i, smileys[i].position);
                }

                resetMap();
                currentTime = 0;
            }
        }

        public int getScoreDistanceBonus(int which) {
            int score = 0;
            score += Math.Abs(smileys[which].position.X-initialSmileyPoints[which%4].X);
            score += Math.Abs(smileys[which].position.Y - initialSmileyPoints[which%4].Y);
            return score;
        }

        public void createExplosion(Point position) {
            map[position.Y, position.X] = 7;
            if(map[position.Y - 1, position.X]!=1)
                map[position.Y-1, position.X] = 7;
            if (map[position.Y + 1, position.X] != 1)
                map[position.Y+1, position.X] = 7;
            if (map[position.Y, position.X-1] != 1)
                map[position.Y, position.X-1] = 7;
            if (map[position.Y, position.X+1] != 1)
                map[position.Y, position.X+1] = 7;

        }

        public void placeBomb(Point position) {
            map[position.Y, position.X] = 6;
        }

        public double[] getNetInputData(Point position) {
            double[] data = new double[27];
            int pos = 0, mapValue;
            Point[] testPoints = { new Point(position.X, position.Y),
                new Point(position.X, position.Y - 1),
                new Point(position.X - 1, position.Y),
                new Point(position.X + 1, position.Y),
                new Point(position.X, position.Y + 1)
            };

            //wall data
            for (int i = 1; i < testPoints.Length; i++) {
                mapValue = map[testPoints[i].Y, testPoints[i].X];
                if (mapValue == 1)
                {
                    data[pos++] = 1;
                    data[pos++] = 0;
                }
                else if (mapValue == 2)
                {
                    data[pos++] = 0;
                    data[pos++] = 1;
                }
                else {
                    data[pos++] = 0;
                    data[pos++] = 0;
                }
            }
            
            Point[] bombtestPoints = { new Point(position.X, position.Y),
                new Point(position.X, position.Y - 1),
                new Point(position.X - 1, position.Y),
                new Point(position.X + 1, position.Y),
                new Point(position.X, position.Y + 1),
                new Point(position.X, position.Y - 2),
                new Point(position.X - 2, position.Y),
                new Point(position.X + 2, position.Y),
                new Point(position.X, position.Y + 2)
            };

            //bomb data
            for (int i = 0; i < bombtestPoints.Length; i++){
                if (!isInRangeOfMap(bombtestPoints[i])) {
                    data[pos++] = 0;
                    continue;
                }
                mapValue = map[bombtestPoints[i].Y, bombtestPoints[i].X];
                if (mapValue == 3|| mapValue == 4|| mapValue == 5 || mapValue == 6)
                {
                    data[pos++] = 1;
                }
                else {
                    data[pos++] = 0;
                }
            }

            //some random input
            data[pos++] = 0;//rand.Next(0,2);
            data[pos++] = 0;//rand.Next(0,2);

            return data;
        }

        public bool isInRangeOfMap(Point position) {
            if (position.X >= map.GetLength(1) || position.X < 0) return false;
            if (position.Y >= map.GetLength(0) || position.Y < 0) return false;
            return true;
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
                smileys[which].score += gameform.getMovementReward();
                smileys[which].net.givePositiveFeedback();

                if (smileys[which].previousDirection == 1 || smileys[which].previousDirection == 2)
                    if (direction == 3 || direction == 4)
                    {
                        smileys[which].score += gameform.getTurningReward();
                        smileys[which].net.givePositiveFeedback();
                    }

                if (smileys[which].previousDirection == 3 || smileys[which].previousDirection == 4)
                    if (direction == 1 || direction == 2)
                    {
                        smileys[which].score += gameform.getTurningReward();
                        smileys[which].net.givePositiveFeedback();
                    }

                smileys[which].moveSmiley(direction);
                smileys[which].previousDirection = direction;
                gameform.updateSmileyPoint(which, smileys[which].position);
            }
            else {
                smileys[which].net.giveNegativeFeedback();
                smileys[which].score -= gameform.getWallHitPunish();
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
