using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetProject
{
    class NNet
    {
        private int inputWidth, rowWidth;
        private double weightRange, weightAvg;
        private int inputs;

        public NNet() {

        }

        /// <summary>
        /// Default constructor for a new neural net
        /// </summary>
        /// <param name="inputs"></param>
        /// <param name="rowWidth"></param>
        /// <param name="weightRange"></param>
        /// <param name="weightAvg"></param>
        public NNet(int inputs, int rowWidth, double weightRange, double weightAvg) {
            this.inputWidth = inputs;
            this.rowWidth = rowWidth;
            this.weightRange = weightRange;
            this.weightAvg = weightAvg;
        }

        public void initializeNet() {

        }

        public void randomizeNet() { }

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
    }
}
