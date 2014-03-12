﻿using NeuralLibrary;
using NumericalExperiment.Experiments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumericalExperiment
{
    class Program
    {
        static void Main(string[] args)
        {
            CancerData testing = new CancerData("testing.dat");
            CancerData training = new CancerData("training.dat");

            new StepExperiment(training, testing).Run();

         


            Console.ReadKey();
            

        }
    }
}