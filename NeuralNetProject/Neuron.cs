using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetProject
{
    class Neuron
    {
        Neuron[] inputs;
        double[] inputWeights;
        public double currentSum;
        Random rand = new Random();

        //Use empty constructor for input neurons
        public Neuron() {

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
        }

        public double getSigmoidOutput() {
            return 1 / (1 + Math.Pow(Math.E, -1 * currentSum));
        }

        public void initializeWeights(double avg)
        {
            for (int i = 0; i < inputWeights.Length; i++){
                inputWeights[i] = avg;
            }
        }

        public void mutateWeights(double amount) {
            double mutation;
            for (int i=0; i<inputWeights.Length; i++) {
                mutation = amount - amount / 2;                
                mutation = rand.Next(0,(int)amount);
                mutation -= amount / 2;
                mutation /= 100;
                inputWeights[i] += mutation;
            }
        }

        public double[] getWeights() {
            return inputWeights;
        }
    }
}
