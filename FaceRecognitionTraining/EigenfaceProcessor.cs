using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FaceRecognitionTraining
{
    [DataContract]
    public class EigenfaceProcessor
    {
        private double[] meanImageVector;
        private List<List<double[]>> listOfPersonsWithStoredEigenfaceComponents; //list med personer, som har en lista av sparade egenface komponenter
        private List<double[]> listOfDataBaseImages; // default images
        public List<double[]> listOfDataPatternsZeroMean;

        private SangersAlgorithm sangersAlgorithm;


        private int nbrEigenfaces;
        private double recognitionThreshold;

        private int reshapeImageSize;
        private int nbrDataBasePersons;
        private int nbrDataBasePicturesPerPerson;

        private static object accessLockObject = new object();

        [DataMember]
        public SangersAlgorithm SangersAlgorithmObject
        {
            set { sangersAlgorithm = value; }
            get { return sangersAlgorithm; }
        }

        public double[,] EigenfaceVectors
        {
            get { return sangersAlgorithm.EigenfaceVectors; }
        }

        public int NbrStoredPersons
        {
            get { return listOfPersonsWithStoredEigenfaceComponents.Count; }
        }

        [DataMember]
        public int NbrEigenfaces
        {
            set { nbrEigenfaces = value; }
            get { return nbrEigenfaces; }
        }
        
        public List<List<double[]>> ListOfPersonsWithStoredEigenfaceComponents
        {
            set { listOfPersonsWithStoredEigenfaceComponents = value; }
            get { return listOfPersonsWithStoredEigenfaceComponents; }
        }

        [DataMember]
        public List<List<List<double>>> ListOfPersonsWithStoredEigenfaceComponentsSerializeable
        {
            set
            {
                List<List<List<double>>> listOfPersonsWithStoredEigenfaceComponents_List = value;
                listOfPersonsWithStoredEigenfaceComponents = new List<List<double[]>>();
                List<double[]> personPictures;
                int nbrStoredPictures;
                int vectorLength;
                double[] tmpVector;

                for (int iPerson=0; iPerson < listOfPersonsWithStoredEigenfaceComponents_List.Count; iPerson++)
                {
                    personPictures = new List<double[]>();
                    nbrStoredPictures = listOfPersonsWithStoredEigenfaceComponents_List[iPerson].Count;
                    for (int jStoredPicture = 0; jStoredPicture < nbrStoredPictures; jStoredPicture++)
                    {
                        vectorLength = listOfPersonsWithStoredEigenfaceComponents_List[iPerson][jStoredPicture].Count;
                        tmpVector = new double[vectorLength];
                        for (int kComponent=0; kComponent < vectorLength; kComponent++)
                        {
                            tmpVector[kComponent] = listOfPersonsWithStoredEigenfaceComponents_List[iPerson][jStoredPicture][kComponent];
                        }
                        personPictures.Add(tmpVector);
                    }
                    listOfPersonsWithStoredEigenfaceComponents.Add(personPictures);
                }
            }
            get
            {
                List<List<List<double>>> listOfPersonsWithStoredEigenfaceComponents_List = new List<List<List<double>>>();

                int nbrPersonsStored = listOfPersonsWithStoredEigenfaceComponents.Count;
                int nbrStoredPictures;
                int vectorLength;
                List<List<double>> iPersonList;
                List<double> tmpComponentList; 

                for (int iPerson = 0; iPerson < nbrPersonsStored; iPerson++)
                {
                    iPersonList = new List<List<double>>();
                    nbrStoredPictures = listOfPersonsWithStoredEigenfaceComponents[iPerson].Count;
                    for (int jStoredPicture = 0; jStoredPicture < nbrStoredPictures; jStoredPicture++)
                    {
                        tmpComponentList = new List<double>();
                        vectorLength = listOfPersonsWithStoredEigenfaceComponents[iPerson][jStoredPicture].Length;
                        for (int kComponent = 0; kComponent < vectorLength; kComponent++)
                        {
                            tmpComponentList.Add(listOfPersonsWithStoredEigenfaceComponents[iPerson][jStoredPicture][kComponent]);
                        }
                        iPersonList.Add(tmpComponentList);
                    }
                    listOfPersonsWithStoredEigenfaceComponents_List.Add(iPersonList);
                }
                return listOfPersonsWithStoredEigenfaceComponents_List;
            }
        }  

        private int ImageVectorLength
        {
            get { return reshapeImageSize * reshapeImageSize; }
        }

        [DataMember]
        public List<double> MeanImageVectorList
        {
            get
            {
                List<double> tmpList = new List<double>();
                for (int i=0; i<meanImageVector.Length; i++)
                {
                    tmpList.Add(meanImageVector[i]);
                }
                return tmpList;
            }
            set
            {
                List<double> tmpList = value;
                meanImageVector = new double[tmpList.Count];
                for (int i = 0; i < tmpList.Count; i++)
                {
                    meanImageVector[i] = tmpList[i];
                }
            }
        }

        [DataMember]
        public int ReshapeImageSize
        {
            set { reshapeImageSize = value; }
            get { return reshapeImageSize; }
        }

        [DataMember]
        public int NbrDataBasePersons
        {
            set { nbrDataBasePersons = value; }
            get { return nbrDataBasePersons; }
        }

        [DataMember]
        public int NbrDataBasePicturesPerPerson
        {
            set { nbrDataBasePicturesPerPerson = value; }
            get { return nbrDataBasePicturesPerPerson; }
        }

        [DataMember]
        public double RecognitionThreshold
        {
            set { recognitionThreshold = value; }
            get { return recognitionThreshold; }
        }


        public EigenfaceProcessor()
        {
            nbrEigenfaces = EigenfaceProcessorParameters.defaultNbrEigenfaces;
            reshapeImageSize = EigenfaceProcessorParameters.defaultReshapeImageSize;
            nbrDataBasePersons = EigenfaceProcessorParameters.defaultNbrDataBasePersons;
            nbrDataBasePicturesPerPerson = EigenfaceProcessorParameters.defaultNbrDataBasePicturesPerPerson;
            recognitionThreshold = EigenfaceProcessorParameters.defaultRecognitionThreshold;

            listOfPersonsWithStoredEigenfaceComponents = new List<List<double[]>>();
        }

        public void AddPerson(List<Bitmap> listOfBitmaps)
        {
            Bitmap tmpReshapedImage;
            IsaksImageProcessor imageProcessor;
            List<double[]> listOfRawImageArrays = new List<double[]>();
            foreach (Bitmap faceBitmap in listOfBitmaps)
            {
                tmpReshapedImage = new Bitmap(faceBitmap, new Size(reshapeImageSize, reshapeImageSize));
                imageProcessor = new IsaksImageProcessor(tmpReshapedImage);
                double[] imageArray = imageProcessor.AsGrayVector();
                listOfRawImageArrays.Add(imageArray);
            }
            AddPerson(listOfRawImageArrays);
        }

        public void AddPerson(List<double[]> listOfRawImageArrays)
        {
            List<double[]> personsEigenFaceProjection = new List<double[]>();
            double[] tmpEigenfaceProjection;
            foreach (double[] rawImageArray in listOfRawImageArrays)
            {
                tmpEigenfaceProjection = CalculateEigenfaceProjection(rawImageArray);
                personsEigenFaceProjection.Add(tmpEigenfaceProjection);
            }
            listOfPersonsWithStoredEigenfaceComponents.Add(personsEigenFaceProjection);
        }

        private double[] CalculateEigenfaceProjection(double[] rawImageArray)
        {
            double[] eigenfaceProjection = new double[nbrEigenfaces];
            double componentSum;
            for (int iEigenfaceVector = 0; iEigenfaceVector < nbrEigenfaces; iEigenfaceVector++)
            {
                componentSum = 0;
                for (int k = 0; k < ImageVectorLength; k++)
                {
                    componentSum += EigenfaceVectors[iEigenfaceVector, k] * (rawImageArray[k] - meanImageVector[k]);
                }
                eigenfaceProjection[iEigenfaceVector] = componentSum;
            }
            return eigenfaceProjection;
        }

        public List<double[]> LoadNewPerson(int personID) // from default images (test method)
        {
            List<double[]> listOfRawImageArrays = new List<double[]>();

            string tmpPicturePath;
            Bitmap tmpOriginalImage;
            Bitmap tmpReshapedImage;
            IsaksImageProcessor imageProcessor;
            double[] tmpImageArray;
            for (int iImage = nbrDataBasePicturesPerPerson + 1; iImage <= 14; iImage++)
            {
                tmpPicturePath = ChoosePersonPicturePath(personID, iImage);
                tmpOriginalImage = new Bitmap(tmpPicturePath);
                tmpReshapedImage = new Bitmap(tmpOriginalImage, new Size(reshapeImageSize, reshapeImageSize));

                imageProcessor = new IsaksImageProcessor(tmpReshapedImage);
                tmpImageArray = imageProcessor.AsGrayVector();
                listOfRawImageArrays.Add(tmpImageArray);
            }
            return listOfRawImageArrays;
        }

        public double[] LoadNewPersonTest(int personID, int imageID)
        {
            string tmpPicturePath = ChoosePersonPicturePath(personID, imageID);
            Bitmap tmpOriginalImage = new Bitmap(tmpPicturePath);
            Bitmap tmpReshapedImage = new Bitmap(tmpOriginalImage, new Size(reshapeImageSize, reshapeImageSize));

            IsaksImageProcessor imageProcessor = new IsaksImageProcessor(tmpReshapedImage);
            double[] imageArray = imageProcessor.AsGrayVector();
            return imageArray;
        }

        public int RecognizePerson(Bitmap facePicture)
        {
            Bitmap tmpReshapedImage = new Bitmap(facePicture, new Size(reshapeImageSize, reshapeImageSize));
            IsaksImageProcessor imageProcessor = new IsaksImageProcessor(tmpReshapedImage);
            double[] imageArray = imageProcessor.AsGrayVector();
            return RecognizePerson(imageArray);
        }

        public int RecognizePerson(double[] rawImageArray)
        {
            // returns index of arrival
            // returns a negative value if not recognized 
            double[] eigenfaceProjection = CalculateEigenfaceProjection(rawImageArray);
            double minEuclidianDistance2 = double.MaxValue;
            int mostLikePerson = -1;
            double tmpEuclidianDistance2;
            for (int iSignedInPerson = 0; iSignedInPerson < NbrStoredPersons; iSignedInPerson++)
            {
                foreach (double[] iPersonEigenfaceProjection in listOfPersonsWithStoredEigenfaceComponents[iSignedInPerson])
                {
                    tmpEuclidianDistance2 = EuclidianDistanceSquare(iPersonEigenfaceProjection, eigenfaceProjection);
                    if (tmpEuclidianDistance2 < minEuclidianDistance2)
                    {
                        mostLikePerson = iSignedInPerson;
                        minEuclidianDistance2 = tmpEuclidianDistance2;
                    }
                }
            }
            if (minEuclidianDistance2 > recognitionThreshold)
            {
                mostLikePerson = -1;
            } else
            {
                int debugVar = 1; 
            }

            return mostLikePerson;
        }

        private static double EuclidianDistanceSquare(double[] vector1, double[] vector2)
        {
            double euclidianDistance2 = 0;
            int vectorLength = vector1.Length;
            double componentDifference;

            for (int i = 0; i < vectorLength; i++)
            {
                componentDifference = vector1[i] - vector2[i];
                euclidianDistance2 += Math.Pow(componentDifference, 2);
            }
            return euclidianDistance2;
        }



        public void LoadDummyData()
        {
            listOfDataBaseImages = new List<double[]>();
            Random randomGenerator = new Random();
            double[] tmpArray;
            double r;
            ReshapeImageSize = 3;

            for (int iPattern=0; iPattern < NbrDataBasePersons*NbrDataBasePicturesPerPerson; iPattern++)
            {
                tmpArray = new double[ImageVectorLength];
                for (int jComponent=0; jComponent<ImageVectorLength; jComponent++)
                {
                    r = 2 * randomGenerator.NextDouble() - 1;
                    if (jComponent < 5)
                    {
                        r = r * (jComponent+2); // defines principal component
                    }
                    tmpArray[jComponent] = r;
                }
                listOfDataBaseImages.Add(tmpArray);
            }
        }

        public void LoadDefaultImages()
        {
            listOfDataBaseImages = new List<double[]>();

            string tmpPicturePath;
            Bitmap tmpOriginalImage;
            Bitmap tmpReshapedImage;
            IsaksImageProcessor imageProcessor;
            double[] tmpImageArray;
            for (int iPerson = 1; iPerson <= nbrDataBasePersons; iPerson++)
            {
                for (int iImage = 1; iImage <= nbrDataBasePicturesPerPerson; iImage++)
                {
                    tmpPicturePath = ChoosePersonPicturePath(iPerson, iImage);
                    tmpOriginalImage = new Bitmap(tmpPicturePath);
                    tmpReshapedImage = new Bitmap(tmpOriginalImage, new Size(reshapeImageSize, reshapeImageSize));

                    imageProcessor = new IsaksImageProcessor(tmpReshapedImage);
                    tmpImageArray = imageProcessor.AsGrayVector();
                    listOfDataBaseImages.Add(tmpImageArray);
                }
            }
        }

        public void CalculateMeanVector()
        {
            meanImageVector = new double[ImageVectorLength];
            double sum;
            double nbrStoredImagesDataBase = (double)listOfDataBaseImages.Count;
            for (int iComponent = 0; iComponent < ImageVectorLength; iComponent++)
            {
                sum = 0;
                for (int iPerson = 0; iPerson < nbrStoredImagesDataBase; iPerson++)
                {
                    sum += listOfDataBaseImages[iPerson][iComponent];
                }
                meanImageVector[iComponent] = sum / nbrStoredImagesDataBase;
            }
        }

        private string ChoosePersonPicturePath(int personID, int pictureID)
        {
            string personIDString = personID.ToString();
            string pictureIDString = pictureID.ToString();
            if (personID < 10)
            {
                personIDString = "0" + personIDString;
            }
            if (pictureID < 10)
            {
                pictureIDString = "0" + pictureIDString;
            }
            string fullString = EigenfaceProcessorParameters.RELATIVE_PATH_TRAINING_PICURES + "\\s" + personIDString + "_" + pictureIDString + ".jpg";
            return fullString;
        }

        public void CalculateZeroMeanData()
        {
            listOfDataPatternsZeroMean = new List<double[]>();
            double nbrStoredImagesDataBase = (double)listOfDataBaseImages.Count;
            double[] tmpImageArray;
            for (int iImage = 0; iImage < nbrStoredImagesDataBase; iImage++)
            {
                tmpImageArray = new double[ImageVectorLength];
                for (int jComponent = 0; jComponent < ImageVectorLength; jComponent++)
                {
                    tmpImageArray[jComponent] = listOfDataBaseImages[iImage][jComponent] - meanImageVector[jComponent];
                }
                listOfDataPatternsZeroMean.Add(tmpImageArray);
            }
        }

        public void InitializeEigenfaces()
        {
            sangersAlgorithm = new SangersAlgorithm(listOfDataPatternsZeroMean, nbrEigenfaces);
            sangersAlgorithm.GenerateRandomEigenfaceVectors();
        }

        public void TrainEigenfaces(int trainingEpochs)
        {
            sangersAlgorithm.Train(trainingEpochs);
        }

        public double[,] CalculateEigenfaceOrthogonality()
        {
            double[,] orthogonalityMatrix = new double[nbrEigenfaces, nbrEigenfaces];
            double tmpScalar2;

            Monitor.Enter(accessLockObject);
            for (int iEigenfaceVector=0; iEigenfaceVector < nbrEigenfaces; iEigenfaceVector++)
            {
                for (int jEigenfaceVector = 0; jEigenfaceVector <= iEigenfaceVector; jEigenfaceVector++)
                {
                    tmpScalar2 = 0;
                    for (int kComponent=0; kComponent < ImageVectorLength; kComponent++)
                    {
                        tmpScalar2 += EigenfaceVectors[iEigenfaceVector, kComponent]* EigenfaceVectors[jEigenfaceVector, kComponent];
                    }
                    orthogonalityMatrix[iEigenfaceVector, jEigenfaceVector] = tmpScalar2;
                    orthogonalityMatrix[jEigenfaceVector, iEigenfaceVector] = tmpScalar2;
                }
            }
            Monitor.Exit(accessLockObject);
            return orthogonalityMatrix;
        }
    }
}
