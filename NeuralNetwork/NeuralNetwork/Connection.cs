﻿namespace NeuralNetwork
{
    /// <summary>
    /// The connection held between two neurons with a given weight.
    /// </summary>
    public class Connection
    {
        /// <summary>
        /// Initializes the connection.
        /// </summary>
        /// <param name="weightInitial">The initial weight of the connection. Bound from -1, 1</param>
        /// <param name="anteriorNeuron">The neuron on the anterior side of the connection.</param>
        /// <param name="posteriorNeuron">The neuron on the posterior side of the connection.</param>
        public Connection(double weightInitial, Neuron anteriorNeuron, Neuron posteriorNeuron)
        {
            this.Weight = weightInitial;
            this.AnteriorNeuron = anteriorNeuron;
            this.PosteriorNeuron = posteriorNeuron;
        }

        /// <summary>
        /// Initializes the connection with a random weight between -1 and 1
        /// </summary>
        /// <param name="anteriorNeuron">The neuron on the anterior side of the connection.</param>
        /// <param name="posteriorNeuron">The neuron on the posterior side of the connection.</param>
        public Connection(Neuron anteriorNeuron, Neuron posteriorNeuron)
            : this(Gaussian.GetRandomGaussian(), anteriorNeuron, posteriorNeuron)
        {
        }

        /// <summary>
        /// Feeds the product of output from the anterior neuron  and the weight of the connection forward to the anterior neuron.
        /// </summary>
        public void FeedForward()
        {
            PosteriorNeuron.Net += AnteriorNeuron.Output * Weight;
        }

        #region Properties

        /// <summary>
        /// The anterior neuron within the connection.
        /// </summary>
        public Neuron AnteriorNeuron { private set; get; }

        /// <summary>
        /// The posterior neuron within the connection.
        /// </summary>
        public Neuron PosteriorNeuron { private set; get; }

        /// <summary>
        /// Updates the weight of the connection using the weight update rule. dW = ERROR_posterior * OUTPUT_anterior
        /// </summary>
        public void UpdateWeight(double learningRate) //todo add momentum
        {
            Weight -= PosteriorNeuron.Error * AnteriorNeuron.Output * learningRate;
        }

        /// <summary>
        /// The weight associated with a connection.
        /// </summary>
        public double Weight { private set; get; }

        #endregion Properties
    }
}