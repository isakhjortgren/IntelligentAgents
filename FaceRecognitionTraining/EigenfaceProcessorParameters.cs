using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceRecognitionTraining
{
    public class EigenfaceProcessorParameters
    {
        public static double defaultLearningRate = 0.000001;
        public static int defaultNbrEigenfaces = 7;
        public static int defaultReshapeImageSize = 100;
        public static int defaultNbrDataBasePersons = 50;
        public static int defaultNbrDataBasePicturesPerPerson = 15;
        public static double defaultRecognitionThreshold = 4E+3;

        public static string RELATIVE_PATH_TRAINING_PICURES = "..\\..\\..\\Data\\FaceImagesTrainingDatabase";
        public static string RELATIVE_PATH_TRAINING_DATA_XML = "..\\..\\..\\Data\\trainingdata.xml";
    }
}
