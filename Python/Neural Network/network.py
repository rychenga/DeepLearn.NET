from neurons import *
from connections import *
from neuron import Neuron
from connection import Connection
class Network:
    def __init__(self, layers, connectionType = None, activations = None):
        
        if( connectionType == None ):
            connectionType = BPROPConnection 
        #TODO
        if( activations == None):
            pass
        if (len(layers) < 2):
            raise AttributeError("Not enough layers specified")
        if(len(activations) != len(layers)):
            raise AttributeError("Activation functions do not line up to layers")
        self.activations = activations
        self.bias = BiasNeuron()
        layerCount = len(layers)
        
        self.neurons = list(layerCount)
        for layer in range(0, layerCount):
            count = layers[layer]
            
            self.neurons[layer] = Neuron[count]
            
            for k in range(0, count):
                if(layer == 0):
                    self.neurons[layer][k] = InputNeuron()
                elif(layer == layerCount -1):
                    self.neurons[layer][k] = OutputNeuron()
                else:
                    self.neurons[layer][k] = Neuron()
        self.connections = list(layerCount-1)
        for layer in range(0, layerCount-1):
            count = (len(self.neurons[layer])+1)*len(self.neurons[layer+1])
            self.connections[layer] = Connection[count]
            for i in range(0, len(self.neurons[layer])+1):
                for j in range(0, len(self.neurons[layer+1])):
                    con = j + i*len(self.neurons[layer+1])
                    if i == 0:
                        self.connections[layer][con] = connectionType(self.bias, self.neurons[layer+1][j])
                    else: 
                        self.connections[layer][con] = connectionType(self.neurons[layer][i-1], self.neurons[layer+1][j])
                    self.connections[layer][con].network = self
    def train(self, ds, learningParameters):
        setError=0
        for dp in ds:
            self.feedForward(dp.input)
            self.backPropagate(dp.desired)
            setError+= self.globalError
        self.globalError = setError
        print(self.globalError)
        self.updateWeights(learningParameters)
        return setError
    #TODO
    def save(self, filename):
         pass
    def load(self, filename, type):
         pass
    def feedForward(self, inputs):
        if(len(inputs)!=len(self.neurons[0])):
            raise AttributeError("Input is not the same length as the neural input")
        
        for layer in self.neurons:
            for n in layer:
                n.reset()
        for i in range(0, len(inputs)):
            self.neurons[0][i].net = inputs[i]
        self.bias.updateOutput(Sigmoid.hyperbolicTangent())
        for layer in range(0, len(self.neurons)):
            for neuron in self.neurons[layer]:
                neuron.updateOutput(self.activations[layer])
            if layer != len(self.neurons) - 1:
                for connection in self.connections[layer]:
                    connection.feedForward()    
    def backPropagate(self, desired):
        if(len(desired)!=len(self.neurons[len(self.neurons)-1])):
            raise AttributeError("Desired is not the same length as the neural output")
        self.globalError = 0
        for i in range(0, len(desired)):
            self.neurons[len(self.neurons)][i].updateError(self.activations[len(self.neurons)-1], desired[i])
            self.globalError += 0.5* (self.neurons[len(self.neurons)-1][i].output-desired[i])**2
        #could throw a null pointer exception depending on the second parameter
        for layer in range(len(self.neurons)-2, -1, -1):
            for neuron in self.neurons[layer]:
                errorCoefficient = 0
                for connection in self.connections[layer]:
                    if connection.anterior.equal(self.bias):
                        connection.updateGradient()
                for connection in self.connections[layer]:
                    if connection.anterior.equals(neuron):
                        errorCoefficient+=connection.posterior.error * connection.weight
                        connection.updateGradient()
                neuron.updateError(self.activations[layer], errorCoefficient)
    def updateWeights(self, learningParameters):
        for layer in self.connections:
            for connection in layer:
                connection.finalizeGradient()
                connection.tryUpdateWeight(learningParameters)
    def generateSigmoids(self, layers):
        funcs = Sigmoid[len(layers)]
        funcs[0] = Sigmoid.none
        for i in range(1, len(funcs)-1):
            funcs[i] = Sigmoid.hyperbolicTangent
        return funcs
    def getInput(self):
        inputL = list(self.neurons[0].length)
        for i in range(0, len(inputL)):
            inputL[i] = self.neurons[0][i].net
        return inputL
    def getOutput(self):
        outputL = list(self.neurons[len(self.neurons)-1].length)
        for i in range(0, len(outputL)):
            outputL[i] = self.neurons[len(self.neurons)-1][i].output
        return outputL
    def getConnection(self, layer, anteriorI, posteriorI):
        for connection in self.connections[layer]:
            if(connection.anterior.getID(self) == anteriorI):
                if(connection.posterior.getID(self) == posteriorI):
                    return connection
        return None
    def getWeights(self):
        weights = list()
        for layer in self.connections:
            for c in layer:
                weights.add(c.weight)
        return weights