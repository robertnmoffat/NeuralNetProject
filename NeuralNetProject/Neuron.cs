using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetProject
{
    class Neuron
    {
        Neuron[] inputs;
        public double[] inputWeights;
        public double currentSum;
        public double threshold;

        //Use empty constructor for input neurons
        public Neuron() {
            threshold = 0;
        }

        public Neuron(Neuron[] inputs) {
            this.inputs = inputs;
            inputWeights = new double[inputs.Length];
        }

        public void sum() {
            currentSum = 0.0;
            for (int i=0; i<inputWeights.Length; i++) {
                currentSum += inputs[i].currentSum * inputWeights[i];
            }
            currentSum = getSigmoidOutput();
            currentSum += -1*threshold;
        }

        public void sigmoidSetCurrentSum(double input) {
            currentSum = 1 / (1 + Math.Pow(Math.E, -1 * input)); ;
        }

        public double getSigmoidOutput() {
            //return currentSum;
            return 1 / (1 + Math.Pow(Math.E, -1 * currentSum));
        }
        
        public void initializeWeights(double avg){
            for (int i = 0; i < inputWeights.Length; i++){
                inputWeights[i] = avg;
            }
            threshold = avg;
        }

        public void mutateWeights(double amount) {
            double mutation;
            for (int i=0; i<inputWeights.Length; i++) {
                int number = Game.rand.Next(0, 4);
                if (number==3) {
                    //Debug.Write("Weight before:"+inputWeights[i]);
                    mutation = 0;
                    mutation = Game.rand.Next(0, (int)amount+1);
                    mutation -= amount / 2;
                    mutation /= 100;
                    inputWeights[i] -= mutation;
                    //Debug.WriteLine(" Weight after:"+inputWeights[i]);
                }
            }
            if (Game.rand.Next(0, 10) == 3)
            {
                mutation = 0;
                mutation = Game.rand.Next(0, (int)amount+1);
                mutation -= amount / 2;
                mutation /= 100;
                threshold += mutation;
            }
        }

        public double[] getWeights() {
            return inputWeights;
        }
    }
}
