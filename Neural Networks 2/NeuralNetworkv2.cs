using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neural_Networks_2
{
    class NeuralNetworkv2
    {
        public List<NeuralNetworkLayer> Layers = new List<NeuralNetworkLayer>();

        public NeuralNetworkv2(int nOfLayers, int[] nNodesInLayers)
        {
            for(int i = 0; i < nOfLayers; i++)
            {
                Layers.Add(new NeuralNetworkLayer());
            }
            for (int i = 0; i < nOfLayers; i++)
            {
                Layers[i].NumberOfNodes = nNodesInLayers[i];
                if(i == 0)
                {
                    Layers[i].NumberOfChildNodes = nNodesInLayers[i + 1];
                    Layers[i].NumberOfParentNodes = 0;
                    Layers[i].Initialize(nNodesInLayers[i], null, Layers[i+1]);
                    Layers[i].RandomizeWeights();
                }
                else if(i == Layers.Count - 1)
                {
                    Layers[i].NumberOfChildNodes = 0;
                    Layers[i].NumberOfParentNodes = nNodesInLayers[i-1];
                    Layers[i].Initialize(nNodesInLayers[i], Layers[i-1], null);
                }
                else
                {
                    Layers[i].NumberOfChildNodes = nNodesInLayers[i+1];
                    Layers[i].NumberOfParentNodes = nNodesInLayers[i-1];
                    Layers[i].Initialize(nNodesInLayers[i], Layers[i-1], Layers[i+1]);
                    Layers[i].RandomizeWeights();
                }
            }
        }

        public void SetInput(int i, double value)
        {
            if ((i >= 0) && (i < Layers[0].NumberOfNodes))
            {
                Layers[0].NeuronValues[i] = value;
            }
        }

        public double GetOutput(int i)
        {
            if ((i >= 0) && (i < Layers[Layers.Count-1].NumberOfNodes))
            {
                return Layers[Layers.Count - 1].NeuronValues[i];
            }
            return (double)99999999; // to indicate an error
        }

        public void SetDesiredOutput(int i, double value)
        {
            if ((i >= 0) && (i < Layers[Layers.Count - 1].NumberOfNodes))
            {
                Layers[Layers.Count - 1].DesiredValues[i] = value;
            }
        }

        public void FeedForward()
        {
            for(int i = 0; i < Layers.Count; i++)
            {
                Layers[i].CalculateNeuronValues();
            }
        }

        public void BackPropagate()
        {
            Layers[Layers.Count-1].CalculateErrors();

            for (int i = 1; i < Layers.Count-1; i++)
            {
                Layers[i].CalculateErrors();
                Layers[i].AdjustWeights();
            }

            Layers[0].AdjustWeights();
        }

        public int GetMaxOutputID()
        {
            int i, id;
            double maxval;
            maxval = Layers[Layers.Count - 1].NeuronValues[0];
            id = 0;
            for (i = 1; i < Layers[Layers.Count - 1].NumberOfNodes; i++)
            {
                if (Layers[Layers.Count - 1].NeuronValues[i] > maxval)
                {
                    maxval = Layers[Layers.Count - 1].NeuronValues[i];
                    id = i;
                }
            }
            return id;
        }

        public double CalculateError()
        {
            int i;
            double error = 0;
            for (i = 0; i < Layers[Layers.Count - 1].NumberOfNodes; i++)
            {
                error += Math.Pow(Layers[Layers.Count - 1].NeuronValues[i] - Layers[Layers.Count - 1].DesiredValues[i], 2);
            }
            error = error / Layers[Layers.Count - 1].NumberOfNodes;
            return error;
        }

        public void SetLearningRate(double rate)
        {
            for(int i = 0; i < Layers.Count; i++)
            {
                Layers[i].LearningRate = rate;
            }
        }
        public void SetLinearOutput(bool useLinear)
        {
            for (int i = 0; i < Layers.Count; i++)
            {
                Layers[i].LinearOutput = useLinear;
            }
        }
        public void SetMomentum(bool useMomentum, double factor)
        {
            for (int i = 0; i < Layers.Count; i++)
            {
                Layers[i].UseMomentum = useMomentum;
                Layers[i].MomentumFactor = factor;
            }
        }
    }
}
