using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Neural_Networks_2.settings;

namespace Neural_Networks_2
{
    class NeuralNetworkLayer
    {
       private string activ = "tanh"; // sigmoid, tanh, relu

       public int NumberOfNodes;
       public int NumberOfChildNodes;
       public int NumberOfParentNodes;
       public double[][] Weights;
       public double[][] WeightChanges;
       public double[] NeuronValues;
       public double[] DesiredValues;
       public double[] Errors;
       public double[] BiasWeights;
       public double[] BiasValues;
       public double LearningRate;
       public bool LinearOutput;
       public bool UseMomentum;
       public double MomentumFactor;
       public NeuralNetworkLayer ParentLayer;
       public NeuralNetworkLayer ChildLayer;

        public NeuralNetworkLayer()
        {
            ParentLayer = null;
            ChildLayer = null;
            LinearOutput = false;
            UseMomentum = false;
            MomentumFactor = 0.9;
        }

        public void Initialize(int NumNodes, NeuralNetworkLayer parent, NeuralNetworkLayer child)
        {
            NeuronValues = new double[NumberOfNodes];
            DesiredValues = new double[NumberOfNodes];
            Errors = new double[NumberOfNodes];

            if(parent != null)
            {
                ParentLayer = parent;
            }
            if(child != null)
            {
                ChildLayer = child;
                Weights = new double[NumberOfNodes][];
                WeightChanges = new double[NumberOfNodes][];
                for(int i = 0; i < NumberOfNodes; i++)
                {
                    Weights[i] = new double[NumberOfChildNodes];
                    WeightChanges[i] = new double[NumberOfChildNodes];
                }
                BiasValues = new double[NumberOfChildNodes];
                BiasWeights = new double[NumberOfChildNodes];
            }
            else
            {
                Weights = null;
                BiasValues = null;
                BiasWeights = null;
                WeightChanges = null;
            }
            for (int i = 0; i < NumberOfNodes; i++)
            {
                NeuronValues[i] = 0;
                DesiredValues[i] = 0;
                Errors[i] = 0;

                if (ChildLayer != null)
                    for (int j = 0; j < NumberOfChildNodes; j++)
                    {
                        Weights[i][j] = 0;
                        WeightChanges[i][j] = 0;
                    }
            }
            if (ChildLayer != null)
                for (int j = 0; j < NumberOfChildNodes; j++)
                {
                    BiasValues[j] = -1;
                    BiasWeights[j] = 0;
                }

        }
        public void RandomizeWeights()
        {
            int min = -mutationRange;
            int max = mutationRange;
            int number;
            for (int i = 0; i < NumberOfNodes; i++)
            {
                for (int j = 0; j < NumberOfChildNodes; j++)
                {
                    number = rnd.Next(min, max);
                    if (number > max)
                        number = max;
                    if (number < min)
                        number = min;
                    Weights[i][j] = number / 100.0f - 1;
                }
            }

            for (int j = 0; j < NumberOfChildNodes; j++)

            {

                number = rnd.Next(min, max);

                if (number > max)

                    number = max;

                if (number < min)

                    number = min;

                BiasWeights[j] = number / 100.0f - 1;

            }



        }
        public void CalculateNeuronValues()
        {
            double x;
            if (ParentLayer != null)
            {
                for (int j = 0; j < NumberOfNodes; j++)
                {
                    x = 0;
                    for (int i = 0; i < NumberOfParentNodes; i++)
                    {
                        x += ParentLayer.NeuronValues[i] *ParentLayer.Weights[i][j];
                    }
                    x += ParentLayer.BiasValues[j] *ParentLayer.BiasWeights[j];
                    if ((ChildLayer == null) && LinearOutput)
                    {
                        NeuronValues[j] = x;
                    }
                    else
                    {
                        //NeuronValues[j] = 1.0f / (1 + Math.Exp(-x));
                        NeuronValues[j] = ActivationFunction(activ, x);
                    }

                }
            }
        }
        public void CalculateErrors()
        {
            double sum;
            if (ChildLayer == null) // output layer
            {
                for (int i = 0; i < NumberOfNodes; i++)
                {
                    Errors[i] = (DesiredValues[i] - NeuronValues[i]) * NeuronValues[i] * (1.0f - NeuronValues[i]);
               
                }
            }
            else if (ParentLayer == null)
            { // input layer
                for (int i = 0; i < NumberOfNodes; i++)
                {
                    Errors[i] = 0.0f;
                }
            }
            else
            { // hidden layer
                for (int i = 0; i < NumberOfNodes; i++)
                {
                    sum = 0;
                    for (int j = 0; j < NumberOfChildNodes; j++)
                    {
                        sum += ChildLayer.Errors[j] * Weights[i][j];
                    }
                    Errors[i] = sum * NeuronValues[i] * (1.0f - NeuronValues[i]);
                }
            }

        }

        private double ActivationFunction(string activation, double x)
        {
            double value = 0;

            if (activation == "sigmoid")
                value = 1.0f / (1 + Math.Exp(-x));
            if (activation == "tanh")
                value = Math.Tanh(x);
            if (activation == "relu")
                value = Math.Max(0.01 * x, x);

            return value;
        }

        public void AdjustWeights()
        {
            double dw;
            if (ChildLayer != null)
            {
                for (int i = 0; i < NumberOfNodes; i++)
                {
                    for (int j = 0; j < NumberOfChildNodes; j++)
                    {
                        dw = LearningRate * ChildLayer.Errors[j] *
                              NeuronValues[i];
                        if (UseMomentum)
                        {
                            Weights[i][j] += dw + MomentumFactor *
                                                   WeightChanges[i][j];
                            WeightChanges[i][j] = dw;
                        }
                        else
                        {
                            Weights[i][j] += dw;
                        }

                    }

                }

                for (int j = 0; j < NumberOfChildNodes; j++)

                {

                    BiasWeights[j] += LearningRate *

                                           ChildLayer.Errors[j] *

                                           BiasValues[j];

                }

            }

        }
    }
}
