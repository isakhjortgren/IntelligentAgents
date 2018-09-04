using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using ObjectSerializerLibrary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FaceRecognitionTraining
{
    public partial class FaceRecognitionMainForm : Form
    {
        private EigenfaceProcessor eigenfaceProcessor;
        private Thread loadDatabaseThread;
        private Thread traingEigenfaceThread;

        public FaceRecognitionMainForm()
        {
            InitializeComponent();
            eigenfaceProcessor = new EigenfaceProcessor();
            textBox1.Text = "10000";
        }

        private void LoadTrainingDataset(object sender, EventArgs e)
        {
            richTextBox1.Text += "Loading database images\n";
            loadDatabaseThread = new Thread(new ThreadStart(() => LoadTrainingDatasetThreadMethod()));
            loadDatabaseThread.Start();
        }

        private void LoadTrainingDatasetThreadMethod()
        {
            eigenfaceProcessor.LoadDefaultImages();
            eigenfaceProcessor.CalculateMeanVector();
            eigenfaceProcessor.CalculateZeroMeanData();
            ThreadSafePrintDone();
        }

        private void ThreadSafePrintDone()
        {
            if (InvokeRequired) { BeginInvoke(new MethodInvoker(() => PrintDone())); }
            else { PrintDone(); }
        }

        private void PrintDone()
        {
            richTextBox1.Text += "Done!\n";
        }

        private void InitializeEigenfaces(object sender, EventArgs e)
        {
            if (loadDatabaseThread != null && !loadDatabaseThread.IsAlive)
            {
                eigenfaceProcessor.InitializeEigenfaces();
                richTextBox1.Text += "Eigenfaces initialized\n";
                textBox2.Text = eigenfaceProcessor.SangersAlgorithmObject.LearningRate.ToString();
                nbrOfEigenfacesTextBox.Text = eigenfaceProcessor.NbrEigenfaces.ToString();
            }
            else
            {
                richTextBox1.Text += "Loading of images not completed...\n";
            }
        }

        private void TrainEigenfacesOnClick(object sender, EventArgs e)
        {
            if (eigenfaceProcessor.SangersAlgorithmObject != null)
            {
                Boolean parseOk = Int32.TryParse(textBox1.Text, out int nbrIterations);
                if (parseOk)
                {
                    if (loadDatabaseThread != null && !loadDatabaseThread.IsAlive)
                    {
                        richTextBox1.Text += "Starting training\n";
                        traingEigenfaceThread = new Thread(new ThreadStart(() => ThreadTrainEigenfaces(nbrIterations)));
                        traingEigenfaceThread.Start();
                    }
                    else
                    {
                        richTextBox1.Text += "Loading of images not completed...\n";
                    }
                } else
                {
                    richTextBox1.Text += "Please enter a valid integer for the number of iterations\n";
                }
                
            }
            else
            {
                richTextBox1.Text += "Loading of images not completed...\n";
            }

        }

        private void ThreadTrainEigenfaces(int nbrIterations)
        {
            eigenfaceProcessor.TrainEigenfaces(nbrIterations);
            ThreadSafePrintDone();
        }

        private void CalculateEigenfaceOrthogonalityOnClick(object sender, EventArgs e)
        {
            if (eigenfaceProcessor.SangersAlgorithmObject != null)
            {
                double[,] eigenfaceOrthogonalityMatrix = eigenfaceProcessor.CalculateEigenfaceOrthogonality();
                string matrixToString = "\n";
                string doubleString;
                for (int i = 0; i < eigenfaceProcessor.NbrEigenfaces; i++)
                {
                    for (int j = 0; j < eigenfaceProcessor.NbrEigenfaces; j++)
                    {
                        doubleString = eigenfaceOrthogonalityMatrix[i, j].ToString();
                        matrixToString += doubleString + "  ";
                    }
                    matrixToString += "\n";
                }
                matrixToString += "\n";
                richTextBox1.Text += matrixToString;
            } else
            {
                richTextBox1.Text += "please initialize eigenfaces first!\n";
            }
            
        }

        private void UpdateLearningRule(object sender, EventArgs e)
        {
            if (eigenfaceProcessor.SangersAlgorithmObject != null)
            {
                if (traingEigenfaceThread == null || (traingEigenfaceThread != null && !traingEigenfaceThread.IsAlive))
                {
                    Boolean parseOK = double.TryParse(textBox2.Text, out double learningRate);
                    if (parseOK)
                    {
                        eigenfaceProcessor.SangersAlgorithmObject.LearningRate = learningRate;
                        richTextBox1.Text += "learning rates updated\n";
                    } else
                    {
                        richTextBox1.Text += "Please enter a valid learning rate\n";
                    }
                }
                else
                {
                    richTextBox1.Text += "Trainging in progress. Please pause training\n";
                }
            } else
            {
                richTextBox1.Text += "Please initialize eigenfaces first\n";
            }
            
        }

        private void PauseTrainingOnClick(object sender, EventArgs e)
        {
            if (eigenfaceProcessor.SangersAlgorithmObject != null)
            {
                if (eigenfaceProcessor.SangersAlgorithmObject.TrainingAllowed)
                {
                    eigenfaceProcessor.SangersAlgorithmObject.TrainingAllowed = false;
                    button8.Text = "Allow training";
                    richTextBox1.Text += "training paused\n";
                } else
                {
                    eigenfaceProcessor.SangersAlgorithmObject.TrainingAllowed = true;
                    button8.Text = "Pause training";
                    richTextBox1.Text += "training can be resumed now\n";
                }
            } else
            {
                richTextBox1.Text += "Please initialize eigenfaces first\n";
            }
        }


        private void SaveTrainingData(object sender, EventArgs e)
        {
            string trainingDataXMLPath = EigenfaceProcessorParameters.RELATIVE_PATH_TRAINING_DATA_XML;
            ObjectXmlSerializer.SerializeObject(trainingDataXMLPath, eigenfaceProcessor, new List<Type>() { typeof(SangersAlgorithm)});
            richTextBox1.Text += "Eigenface vectors saved\n";
        }

        private void LoadDataTest(object sender, EventArgs e)
        {
            string trainingDataXMLPath = EigenfaceProcessorParameters.RELATIVE_PATH_TRAINING_DATA_XML;
            EigenfaceProcessor tmpEFP = (EigenfaceProcessor)ObjectXmlSerializer.ObtainSerializedObject(trainingDataXMLPath, typeof(EigenfaceProcessor), new List<Type>() { typeof(SangersAlgorithm)});
            eigenfaceProcessor.SangersAlgorithmObject.EigenfaceVectors = tmpEFP.SangersAlgorithmObject.EigenfaceVectors;
            eigenfaceProcessor.NbrEigenfaces = tmpEFP.NbrEigenfaces;
            eigenfaceProcessor.ReshapeImageSize = tmpEFP.ReshapeImageSize;
            eigenfaceProcessor.NbrDataBasePersons = tmpEFP.NbrDataBasePersons;
            eigenfaceProcessor.NbrDataBasePicturesPerPerson = tmpEFP.NbrDataBasePicturesPerPerson;
            nbrOfEigenfacesTextBox.Text = eigenfaceProcessor.NbrEigenfaces.ToString();

            richTextBox1.Text += "Eigenface vectors loaded\n";
        } 

        private void InitializeDummyData(object sender, EventArgs e)
        {
            eigenfaceProcessor.LoadDummyData();
            eigenfaceProcessor.CalculateMeanVector();
            eigenfaceProcessor.CalculateZeroMeanData();
            eigenfaceProcessor.InitializeEigenfaces();
            textBox2.Text = eigenfaceProcessor.SangersAlgorithmObject.LearningRate.ToString();
            nbrOfEigenfacesTextBox.Text = eigenfaceProcessor.NbrEigenfaces.ToString();
        }

        
        private void ExitProgramOnClick(object sender, EventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

        private void UpdatePrincipalComponentsButton_Click(object sender, EventArgs e)
        {
            if (loadDatabaseThread != null && !loadDatabaseThread.IsAlive)
            {
                if (traingEigenfaceThread == null || (traingEigenfaceThread != null && !traingEigenfaceThread.IsAlive))
                {
                    Boolean parseOk = int.TryParse(nbrOfEigenfacesTextBox.Text, out int nbrEigenfaces);
                    if (parseOk)
                    {
                        eigenfaceProcessor.NbrEigenfaces = nbrEigenfaces;
                        richTextBox1.Text += "Number principal components updated\n";
                        eigenfaceProcessor.InitializeEigenfaces();
                        textBox2.Text = eigenfaceProcessor.SangersAlgorithmObject.LearningRate.ToString();
                        richTextBox1.Text += "Eigenfaces initialized\n";
                    } else
                    {
                        richTextBox1.Text += "Please enter a integer for number of eigenfaces\n";
                    }
                    
                }
                else
                {
                    richTextBox1.Text += "Trainging in progress. Please pause training before changing the number of principal components\n";
                }
            }
            else
            {
                richTextBox1.Text += "Loading of images not completed...\n";
            }
        }
    }
}
