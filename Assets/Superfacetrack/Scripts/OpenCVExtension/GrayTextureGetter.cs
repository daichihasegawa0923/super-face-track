using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SuperFaceTrack.OpenCVExtension
{
    public class GrayTextureGetter
    {
        public static Mat Get(WebCamTexture texture)
        {
            var mat = OpenCvSharp.Unity.TextureToMat(texture);
            var grayMat = new Mat();
            Cv2.CvtColor(mat, grayMat, ColorConversionCodes.RGB2GRAY);
            return grayMat;
        }
    }
}
