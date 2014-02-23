﻿using NeuralLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumericalExperiment.Experiments
{
    public class StepExperiment : Experiment
    {
        /// <summary>
        /// Initializes the step experiment
        /// </summary>
        /// <param name="training">The set on which the neural networks will train on.</param>
        /// <param name="testing">The set of testing data for experiments</param>
        public StepExperiment(CancerData training, CancerData testing) :
            base(training, testing)
        {
        }

        /// <summary>
        /// Runs the step experiment
        /// The experiment will consist of the following steps.
        /// 1. Train the network using a step function
        /// 2. Determine the network error while implementing a stepping function for the output
        /// 3. Determine the error over 10 networks
        /// </summary>
        public override void Run()
        {

            //Train using different step function values
            for (double sf = 0; sf < 1; sf += 0.1)
            {
                string subdirectory = sf + @"\";
                for (int i = 0; i < 10; i++)
                {
                    Network nn = new Network(false, NETWORK_SIZE);
                    Trainer trainer = new Trainer(nn, this.trainingSet, testingSet);
                    
                    trainer.Train(NETWORK_EPOCHS, NETWORK_ERROR, NETWORK_LEARNING_RATE, NETWORK_MOMENTUM, NETWORK_NUDGING, sf);
                    this.Analyze(subdirectory + i + "\\", trainer, nn, sf);
                }
            }
        }

        #region Fields
        List<string> testingError = new List<string>();
        #endregion

        /// <summary>
        /// Essentially the sub-directory in which the persistent store data will be held.
        /// </summary>
        public override string PERSIST
        {
            get { return @"LR\"; }
        }
    }
}
