using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetProject
{
    class NNet
    {
        public int inputWidth, amountOfRows, rowWidth;
        public double weightRange, weightAvg;
        
        private double[,] weights;
        public int outputCount = 6;      
        
        private double[][] w;
        Neuron[] inputNeurons;
        public Neuron[,] hiddenNeurons;
        public Neuron[] outputNeurons;

        public int generation = 0;

        public NNet() {
        }

        /// <summary>
        /// Default constructor for a new neural net
        /// </summary>
        /// <param name="inputs"></param>
        /// <param name="rowWidth"></param>
        /// <param name="weightRange"></param>
        /// <param name="weightAvg"></param>
        public NNet(int inputs, int rows, int rowWidth, double weightRange, double weightAvg) {
            this.inputWidth = inputs;
            this.amountOfRows = rows;
            this.rowWidth = rowWidth;
            this.weightRange = weightRange;
            this.weightAvg = weightAvg;
            
        }

        public void givePositiveFeedback()
        {
            if (1 != 1)
            {
                for (int y = 0; y < amountOfRows; y++)
                {
                    for (int x = 0; x < rowWidth; x++)
                    {
                        if (hiddenNeurons[x, y].currentSum == 1)
                        {
                            for (int i = 0; i < hiddenNeurons[x, y].inputWeights.Length; i++)
                            {
                                if (hiddenNeurons[x, y].inputWeights[i] > 0)
                                {
                                    hiddenNeurons[x, y].inputWeights[i] += 0.001;
                                }
                                else {
                                    hiddenNeurons[x, y].inputWeights[i] -= 0.001;
                                }
                            }
                        }
                    }
                }

                return;

                double highest = -9999;
                int pos = 0;
                for (int i = 0; i < outputCount; i++)
                {
                    if (outputNeurons[i].currentSum > highest)
                    {
                        highest = outputNeurons[i].currentSum;
                        pos = i;
                    }
                }
                for (int i = 0; i < rowWidth; i++)
                {
                    if (outputNeurons[pos].inputWeights[i] > 0)
                        outputNeurons[pos].inputWeights[i] += 0.001;
                    else
                        outputNeurons[pos].inputWeights[i] -= 0.001;
                }
            }
        }

        public void giveNegativeFeedback()
        {
            if (1 != 1)
            {
                for (int y = 0; y < amountOfRows; y++)
                {
                    for (int x = 0; x < rowWidth; x++)
                    {
                        if (hiddenNeurons[x, y].currentSum == 1)
                        {
                            for (int i = 0; i < hiddenNeurons[x, y].inputWeights.Length; i++)
                            {
                                if (hiddenNeurons[x, y].inputWeights[i] > 0)
                                {
                                    hiddenNeurons[x, y].inputWeights[i] -= 0.01;
                                }
                                else {
                                    hiddenNeurons[x, y].inputWeights[i] += 0.01;
                                }
                            }
                        }
                    }
                }

                return;

                double highest = -9999;
                int pos = 0;
                for (int i = 0; i < outputCount; i++)
                {
                    if (outputNeurons[i].currentSum > highest)
                    {
                        highest = outputNeurons[i].currentSum;
                        pos = i;
                    }
                }
                for (int i = 0; i < rowWidth; i++)
                {
                    if (outputNeurons[pos].inputWeights[i] > 0)
                        outputNeurons[pos].inputWeights[i] -= 0.01;
                    else
                        outputNeurons[pos].inputWeights[i] += 0.01;
                }
            }
        }


        public void initializeNet() {
            inputNeurons = new Neuron[inputWidth];
            for (int i=0; i<inputNeurons.Length; i++) {
                inputNeurons[i] = new Neuron();
            }

            hiddenNeurons = new Neuron[rowWidth,amountOfRows];
            Neuron[] temp;
            for (int y=0; y<amountOfRows; y++) {
                for (int x=0; x<rowWidth; x++) {
                    if (y == 0) {
                        hiddenNeurons[x, y] = new Neuron(inputNeurons);
                        hiddenNeurons[x, y].initializeWeights(weightAvg);
                    } else {
                        temp = new Neuron[hiddenNeurons.GetLength(0)];
                        for (int i=0; i<temp.Length; i++) {
                            temp[i] = hiddenNeurons[i,y-1];
                        }
                        hiddenNeurons[x, y] = new Neuron(temp);
                        hiddenNeurons[x, y].initializeWeights(weightAvg);
                    }
                }
            }

            outputNeurons = new Neuron[outputCount];
            temp = new Neuron[hiddenNeurons.GetLength(0)];
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i] = hiddenNeurons[i, hiddenNeurons.GetLength(1)-1];
            }
            for (int i=0; i<outputCount; i++) {
                outputNeurons[i] = new Neuron(temp);
                outputNeurons[i].initializeWeights(weightAvg);
            }

        }

        public NNet randomizeNet(int randomAmount) {
            for (int y = 0; y < amountOfRows; y++)
            {
                for (int x = 0; x < rowWidth; x++)
                {
                    hiddenNeurons[x, y].mutateWeights(randomAmount);
                }
            }
            for (int i=0; i<outputCount; i++) {
                outputNeurons[i].mutateWeights(randomAmount);
            }

            return this;
        }

        public int InputWidth
        {
            get
            {
                return inputWidth;
            }

            set
            {
                inputWidth = value;
            }
        }

        public int InputWidth1
        {
            get
            {
                return inputWidth;
            }

            set
            {
                inputWidth = value;
            }
        }

        public double WeightRange
        {
            get
            {
                return weightRange;
            }

            set
            {
                weightRange = value;
            }
        }

        public double WeightRange1
        {
            get
            {
                return weightRange;
            }

            set
            {
                weightRange = value;
            }
        }

        public void setWeights(double[] weights) {
            int pos = 0;
            for (int y=0; y<hiddenNeurons.GetLength(1); y++) {
                for (int x=0; x<hiddenNeurons.GetLength(0); x++) {
                    for (int i=0; i<hiddenNeurons[x,y].inputWeights.Length; i++) {
                        hiddenNeurons[x, y].inputWeights[i] = weights[pos++];
                    }
                    hiddenNeurons[x, y].threshold = weights[pos++];
                }                
            }

            for (int i=0; i<outputCount; i++) {
                for (int j=0; j<outputNeurons[i].inputWeights.Length; j++) {
                    outputNeurons[i].inputWeights[j] = weights[pos++];
                }
                outputNeurons[i].threshold = weights[pos++];
            }
            
        }

        //int inputs, int rows, int rowWidth, double weightRange, double weightAvg
        //hiddenWeights,threshold,hiddenWeights,threshold... outputWeights,threshold
        public NNet copyNet() {
            NNet outputNet = new NNet(inputWidth,amountOfRows,rowWidth,WeightRange,weightAvg);
            outputNet.initializeNet();
            double[] allWeights = new double[(inputWidth+1)*rowWidth+(rowWidth+1)*(amountOfRows-1)*rowWidth+(outputCount+1)*rowWidth];
            //double[] allWeights = new double[amountOfRows*rowWidth+amountOfRows*rowWidth*rowWidth+];
            double[] tempWeights;
            int pos = 0;

            for(int i = 0; i < amountOfRows; i++){
                for (int j=0; j<rowWidth; j++) {
                    tempWeights = hiddenNeurons[j, i].getWeights();

                    for (int k=0; k<tempWeights.Length; k++) {
                        allWeights[pos++] = tempWeights[k];
                    }
                    allWeights[pos++] = hiddenNeurons[j,i].threshold;
                }
            }
            

            for (int i = 0; i < outputCount; i++) {
                tempWeights = outputNeurons[i].getWeights();
                for (int j=0; j<tempWeights.Length; j++) {
                    allWeights[pos++] = tempWeights[j];
                }
                allWeights[pos++] = outputNeurons[i].threshold;
            }

            outputNet.setWeights(allWeights);
            outputNet.generation = generation+1;
            
            return outputNet;
        }

        public void setInputs(double[] inputValues) {
            for (int i=0; i<inputNeurons.Length; i++) {
                inputNeurons[i].currentSum = inputValues[i];
            }
        }

        public void runNet() {
            for (int y=0; y<hiddenNeurons.GetLength(1); y++) {
                for (int x=0; x<hiddenNeurons.GetLength(0); x++) {
                    hiddenNeurons[x, y].sum();
                    if (hiddenNeurons[x, y].currentSum > 0)
                        hiddenNeurons[x, y].currentSum = 1;
                    else
                        hiddenNeurons[x, y].currentSum = 0;

                }
            }
            for (int i=0; i<outputNeurons.Length; i++) {
                outputNeurons[i].sum();
            }
        }

        public double[] getOutputs() {
            double[] output = new double[outputCount];
            for (int i = 0; i < outputCount; i++){
                output[i] = outputNeurons[i].currentSum;
            }
            return output;
        }

        public int getAction() {
            double[] outputValues = getOutputs();
            double highestValue = -999999;
            int highestPos = -1;

            for (int i=0; i<outputValues.Length; i++) {
                if (outputValues[i] > highestValue) {
                    highestValue = outputValues[i];
                    highestPos = i;
                }
            }

            return highestPos;
        }
    }
}
