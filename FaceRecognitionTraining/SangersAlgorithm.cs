using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FaceRecognitionTraining
{
    [DataContract]
    public class SangersAlgorithm
    {
        private List<double[]> listOfDataPatterns;
        private int nbrPrincipalComponents;
        private int nbrDataPatterns;
        private int nbrInputCoord;
        private int nbrTrainedEpochs;
        private double[,] eigenfaceVectors; // rowVectors are principal components
        private double learningRate;
        private Boolean trainingAllowed;

        private static object accessLockObject = new object();


        public Boolean TrainingAllowed
        {
            set { trainingAllowed = value; }
            get { return trainingAllowed; }
        }

        [DataMember]
        public double LearningRate
        {
            set { learningRate = value; }
            get { return learningRate; }
        }

        [DataMember]
        public int NbrTrainedEpochs
        {
            set { nbrTrainedEpochs = value; }
            get { return nbrTrainedEpochs; }
        }
        
        public double[,] EigenfaceVectors
        {
            set { eigenfaceVectors = value; }
            get { return eigenfaceVectors; }
        }

        [DataMember]
        public List<List<double>> EigenfaceVectorsSerializeable
        {
            set
            {
                List<List<double>> listEigenfaceVectors = value;
                int nbrEigenfaceVectors = listEigenfaceVectors.Count;
                int vectorLength = listEigenfaceVectors[0].Count;
                eigenfaceVectors = new double[nbrEigenfaceVectors, vectorLength];
                for (int i=0; i<nbrEigenfaceVectors; i++)
                {
                    for (int j=0; j<vectorLength; j++)
                    {
                        eigenfaceVectors[i, j] = listEigenfaceVectors[i][j];
                    }
                }
            }
            get
            {
                List<List<double>> listEigenfaceVectors = new List<List<double>>();
                int nbrEigenfaceVectors = eigenfaceVectors.GetLength(0);
                int vectorLength = eigenfaceVectors.GetLength(1);
                for (int i = 0; i < nbrEigenfaceVectors; i++)
                {
                    listEigenfaceVectors.Add(new List<double>());
                    for (int j = 0; j < vectorLength; j++)
                    {
                        listEigenfaceVectors[i].Add(eigenfaceVectors[i, j]);
                    }
                }
                return listEigenfaceVectors;
            }
        }


        public SangersAlgorithm(List<double[]> listOfDataPatterns, int nbrPrincipalComponents)
        {
            this.nbrPrincipalComponents = nbrPrincipalComponents;
            this.listOfDataPatterns = listOfDataPatterns;
            nbrDataPatterns = listOfDataPatterns.Count;
            nbrInputCoord = listOfDataPatterns[0].Length;
            nbrTrainedEpochs = 0;
            learningRate = EigenfaceProcessorParameters.defaultLearningRate;
            trainingAllowed = true;
        }

        public void GenerateRandomEigenfaceVectors()
        {
            eigenfaceVectors = new double[nbrPrincipalComponents, nbrInputCoord];
            nbrTrainedEpochs = 0;
            Random randomGenerator = new Random();
            double normalisationFactor = 0.5 * Math.Sqrt((double)nbrInputCoord);
            for (int i = 0; i < nbrPrincipalComponents; i++)
            {
                for (int j = 0; j < nbrInputCoord; j++)
                {
                    double r = 2 * randomGenerator.NextDouble() - 1;
                    eigenfaceVectors[i, j] = r / normalisationFactor;
                }
            }
        }

        public double[,] Train()
        {
            return Train((int)Math.Pow(10, nbrPrincipalComponents));
        }

        public double[,] Train(int nbrEpochs)
        {
            Random randomGenerator = new Random();

            int randomPatternIndex;
            double[] inputPattern;
            double[] outputPattern;
            double[,] updateMatrix = new double[nbrPrincipalComponents, nbrInputCoord];
            double iSum;

            for (int iEpoch = 0; iEpoch < nbrEpochs; iEpoch++)
            {
                randomPatternIndex = randomGenerator.Next(nbrDataPatterns);
                inputPattern = listOfDataPatterns[randomPatternIndex];
                outputPattern = MatrixVectorMultiplication(eigenfaceVectors, inputPattern);

                if (trainingAllowed)
                {
                    // calculate update matrix
                    Parallel.For(0, nbrPrincipalComponents, i =>
                    {
                        for (int j = 0; j < nbrInputCoord; j++)
                        {
                            iSum = 0;
                            for (int k = 0; k <= i; k++)
                            {
                                iSum += outputPattern[k] * eigenfaceVectors[k, j];
                            }
                            updateMatrix[i, j] = learningRate * outputPattern[i] * (inputPattern[j] - iSum);
                        }
                    });
                } else
                {
                    break;
                }
                
                Monitor.Enter(accessLockObject);
                eigenfaceVectors = MatrixAddition(eigenfaceVectors, updateMatrix);
                Monitor.Exit(accessLockObject);
                nbrTrainedEpochs += 1;
            }
            return eigenfaceVectors;
        }

        public static double[] MatrixVectorMultiplication(double[,] matrix, double[] vector)
        {
            int outputLength = matrix.GetLength(0);
            int inputLength = matrix.GetLength(1);
            double[] outputPattern = new double[outputLength];
            Parallel.For(0, outputLength, iRow =>
            {
                double sum = 0;
                for (int jCol = 0; jCol < inputLength; jCol++)
                {
                    sum += matrix[iRow, jCol] * vector[jCol];
                }
                outputPattern[iRow] = sum;
            });

            return outputPattern;
        }

        public static double[,] MatrixAddition(double[,] matrix1, double[,] matrix2)
        {
            int nbrRows = matrix1.GetLength(0);
            int nbrCols = matrix1.GetLength(1);
            double[,] matrixSum = new double[nbrRows, nbrCols];
            Parallel.For(0, nbrRows, iRow =>
            {
                for (int jCol = 0; jCol < nbrCols; jCol++)
                {
                    matrixSum[iRow, jCol] = matrix1[iRow, jCol] + matrix2[iRow, jCol];
                }
            });

            return matrixSum;
        }

        public static double[] RowVecNormSquare(double[,] matrix)
        {
            int nbrRows = matrix.GetLength(0);
            int nbrCols = matrix.GetLength(1);
            double[] norm2Vector = new double[nbrRows];
            double norm2;
            for (int iRow = 0; iRow < nbrRows; iRow++)
            {
                norm2 = 0;
                for (int jCol = 0; jCol < nbrCols; jCol++)
                {
                    norm2 += matrix[iRow, jCol] * matrix[iRow, jCol];
                }

                norm2Vector[iRow] = norm2;
            }
            return norm2Vector;
        }

    }
}
