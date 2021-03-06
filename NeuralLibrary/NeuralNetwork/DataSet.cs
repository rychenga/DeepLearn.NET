﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace NeuralLibrary.NeuralNetwork
{
    /// <summary>
    /// Holds an abstract dataset.
    /// </summary>
    public abstract class DataSet : List<DataPoint>
    {
        /// <summary>
        /// Load dataset into the list.
        /// </summary>
        public abstract void Load();

        /// <summary>
        /// Random used for the dataset extension (shuffle)
        /// </summary>
        public static Random r = new Random();

        /// <summary>
        /// An extension for all ILists and dataset which shuffles the order.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public DataSet Shuffle()
        {
            int n = this.Count;
            while (n > 1)
            {
                n--;
                int k = r.Next(n + 1);
                DataPoint value = this[k];
                this[k] = this[n];
                this[n] = value;
            }

            return this;
        }

        /// <summary>
        /// Calculates the errors based on the datapoints in a given dataset.
        /// </summary>
        /// <param name="nn">The network from which to calculate the errors</param>
        /// <param name="step">The step size.</param>
        /// <returns></returns>
        public double[] CalculateErrors(Network nn, double step = -1)
        {
            return this.Select((d) =>
                {
                    nn.FeedForward(d.Input);
                    return 0.5 * d.Desired.Select(
                        (x, i) => Math.Pow(Gaussian.Step(nn.Output[i], step) - x, 2)).Sum();
                }).ToArray();
        }

        /// <summary>
        /// Calculates the total error on a dataset using a default step.
        /// </summary>
        /// <param name="nn">The neural network to process the dataset.</param>
        /// <param name="step">If -1, no stepping occurs, else use a step point equivalent to this value.</param>
        /// <returns></returns>
        public double CalculateError(Network nn, double step = -1)
        {
            return CalculateErrors(nn, step).Sum();
        }


        /// <summary>
        /// Print the entire dataset
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            //Join all of the data points.
            return string.Join("\n",
                this.Select(x => "["
                + string.Join(",", x.Input)
                + "]---> ["
                + string.Join(",", x.Desired)
                + "]"));
        }
    }
}