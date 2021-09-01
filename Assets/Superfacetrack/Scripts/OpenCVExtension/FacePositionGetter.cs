using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperFaceTrack.OpenCVExtension  
{
    public class FacePositionGetter
    {
        static Size eyesSize = new Size(20, 20);

        public static OpenCvSharp.Rect[] GetFaces(Mat mat)
        {
            var haarCascade = new CascadeClassifier("Assets/OpenCV+Unity/Demo/Face_Detector/haarcascade_frontalface_default.xml");
            var faces = haarCascade.DetectMultiScale(mat);
            return faces;
        }

        public static OpenCvSharp.Rect[] GetEyes(Mat mat)
        {
            var harrCascade = new CascadeClassifier("Assets/OpenCV+Unity/Demo/Face_Detector/haarcascade_eye_tree_eyeglasses.xml");
            var eyes = harrCascade.DetectMultiScale(mat, minSize: eyesSize);
            return eyes;
        }
    }
}