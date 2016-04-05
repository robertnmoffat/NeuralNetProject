using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetProject
{
    class NNet
    {
        public int inputWidth, amountOfRows, rowWidth;
        private double weightRange, weightAvg;
        public int outputs=6;
        private double[,] weights;
        
        private double[][] w;
        Neuron[] inputNeurons;
        public Neuron[,] hiddenNeurons;
        public Neuron[] outputNeurons; 

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

            outputNeurons = new Neuron[6];
            temp = new Neuron[hiddenNeurons.GetLength(0)];
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i] = hiddenNeurons[i, hiddenNeurons.GetLength(1)-1];
            }
            for (int i=0; i<outputs; i++) {
                outputNeurons[i] = new Neuron(temp);
                outputNeurons[i].initializeWeights(weightAvg);
            }

        }

        public void randomizeNet(int randomAmount) {
            for (int y = 0; y < amountOfRows; y++)
            {
                for (int x = 0; x < rowWidth; x++)
                {
                    hiddenNeurons[x, y].mutateWeights(randomAmount);
                }
            }
            for (int i=0; i<outputs; i++) {
                outputNeurons[i].mutateWeights(randomAmount);
            }
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
            //-------------------TODO--------------------------
        }

        //int inputs, int rows, int rowWidth, double weightRange, double weightAvg
        public NNet copyNet() {
            NNet outputNet = new NNet(inputWidth,amountOfRows,rowWidth,WeightRange,weightAvg);
            double[] allWeights = new double[inputWidth*rowWidth+rowWidth*(amountOfRows-1)+outputs*rowWidth];
            double[] tempWeights;
            int pos = 0;

            for(int i = 0; i < amountOfRows; i++){
                for (int j=0; j<rowWidth; j++) {
                    tempWeights = hiddenNeurons[i, j].getWeights();

                    for (int k=0; k<tempWeights.Length; k++) {
                        allWeights[pos++] = tempWeights[k];
                    }
                }
            }

            for (int i = 0; i < outputs; i++) {
                tempWeights = outputNeurons[i].getWeights();
                for (int j=0; j<tempWeights.Length; j++) {
                    allWeights[pos++] = tempWeights[j];
                }
            }

            outputNet.setWeights(allWeights);
            //--------------TODO-----------------------------------------------

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
                }
            }
            for (int i=0; i<outputNeurons.Length; i++) {
                outputNeurons[i].sum();
            }
        }

        public double[] getOutputs() {
            double[] output = new double[outputs];
            for (int i = 0; i < outputs; i++){
                output[i] = outputNeurons[i].getSigmoidOutput();
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
