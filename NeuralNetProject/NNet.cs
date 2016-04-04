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
        private int inputWidth, amountOfRows, rowWidth;
        private double weightRange, weightAvg;
        private int inputs;
        private double[,] weights;

        private double[][] w;
        

        public NNet() {
            w = new double[5][];
            w[4] = new double[5];
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

            weights = new double[rowWidth, amountOfRows];
        }

        public void initializeNet() {
            
        }

        public void randomizeNet() {

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

        public void mutateNet() {

        }
    }
}
