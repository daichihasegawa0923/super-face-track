using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OpenCvSharp;
using SuperFaceTrack.OpenCVExtension;

namespace SuperFaceTrack.ForDebug
{
    public class CameraImport : MonoBehaviour
    {
        [SerializeField]
        private RawImage _rowImage;

        [SerializeField]
        private GameObject _face;

        [SerializeField]
        private GameObject _eyes;

        Vector3 _firstPosition;
        Vector3 _firstSpin;

        private WebCamTexture WebCamTexture { set; get; }

        private void Awake()
        {
            WebCamTexture = new WebCamTexture();
            _rowImage.texture = WebCamTexture;
            WebCamTexture.Play();
            StartCoroutine(FaceTracking());
            _firstPosition = _face.transform.position;
            _firstSpin = _face.transform.eulerAngles;
        }

        IEnumerator FaceTracking()
        {
            while (true)
            {
                var texture = WebCamTexture;
                var faces = FacePositionGetter.GetFaces(GrayTextureGetter.Get(texture));
                var eyes = FacePositionGetter.GetEyes(GrayTextureGetter.Get(texture));
                if (faces.Length != 0)
                {
                    var width  = texture.width;
                    var height = texture.height;
                    var rect = faces[0];
                    var fixPositionX  = (rect.Center.X - (0.5f* width))/width;
                    var fixPositionY = -(rect.Center.Y - (0.5f * height)) / height;
                    var position = _firstPosition;
                    position.x += fixPositionX;
                    position.y += fixPositionY;
                    _face.transform.position = position;
                }

                if (eyes.Length >= 2)
                {
                    _eyes.SetActive(true);
                    var eye01 = eyes[0];
                    var eye02 = eyes[1];

                    var leftEye = eye01.Center.X > eye02.Center.X ? eye01 : eye02;
                    var rightEye = eye01.Center.X > eye02.Center.X ? eye02 : eye01;

                    var dSizeX = rightEye.Size.Width - leftEye.Size.Width;
                    var dY = leftEye.Center.Y - rightEye.Center.Y;
                    var spin = _firstSpin;
                    spin.z += dY;
                    spin.y += dSizeX;
                    _face.transform.eulerAngles = spin;
                }
                else
                {
                    _eyes.SetActive(false);
                }

                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}