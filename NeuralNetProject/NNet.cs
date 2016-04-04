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

        public void setWeights(int row, double[] weights) {

        }

        //int inputs, int rows, int rowWidth, double weightRange, double weightAvg
        public NNet mutateNet() {
            NNet outputNet = new NNet();
            outputNet.inputWidth = inputWidth;
            

            return outputNet;
        }
    }
}
