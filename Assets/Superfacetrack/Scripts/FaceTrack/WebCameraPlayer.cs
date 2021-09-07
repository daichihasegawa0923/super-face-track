using SuperFaceTrack.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperFaceTrack.FaceTrack
{
    public class WebCameraPlayer : SingletonMonoBehaviour<WebCameraPlayer>
    {
        public WebCamTexture WebCamTexture { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            WebCamTexture = new WebCamTexture();
            WebCamTexture.Play();
        }
    }
}