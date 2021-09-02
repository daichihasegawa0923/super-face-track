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

        private List<Vector3> _rowNumbers = new List<Vector3>();
        private List<Vector3> _calclaterNumbers = new List<Vector3>();

        private WebCamTexture WebCamTexture { set; get; }

        private void Awake()
        {
            WebCamTexture = new WebCamTexture();
            _rowImage.texture = WebCamTexture;
            WebCamTexture.Play();
            _firstPosition = _face.transform.position;
            _firstSpin = _face.transform.eulerAngles;
            StartCoroutine(FaceTracking());
            StartCoroutine(EyeTracking());
        }

        IEnumerator EyeTracking()
        {
            while (true)
            {

                var texture = WebCamTexture;
                var eyes = FacePositionGetter.GetEyes(GrayTextureGetter.Get(texture));
                var faces = FacePositionGetter.GetFaces(GrayTextureGetter.Get(texture));

                if (eyes.Length == 2)
                {
                    _eyes.SetActive(true);
                    var eye01 = eyes[0];
                    var eye02 = eyes[1];

                    var leftEye = eye01.Center.X > eye02.Center.X ? eye01 : eye02;
                    var rightEye = eye01.Center.X > eye02.Center.X ? eye02 : eye01;

                    var spin = Vector3.zero;

                    var xSpin = 0;
                    var ySpin = 0; 
                    if(faces.Length > 0)
                    {
                        xSpin = -(faces[0].Center.Y - (texture.height / 2));
                        ySpin = rightEye.Center.X - faces[0].Center.X - (faces[0].Center.X - leftEye.Center.X);
                    }

                    var zSpin = leftEye.Center.Y - rightEye.Center.Y;

                    spin.x = xSpin;
                    spin.y = ySpin;
                    spin.z = zSpin;

                    _rowNumbers.Add(spin);

                    if(_rowNumbers.Count >= 10)
                    {
                        _calclaterNumbers =  CalculateList(_rowNumbers);
                        _rowNumbers.Clear();
                    }

                    if (_calclaterNumbers.Count > 0) 
                    {
                        var calculatedSpin = _calclaterNumbers[0];
                        _calclaterNumbers.RemoveAt(0);

                        _face.transform.localEulerAngles = calculatedSpin;
                    }
                }
                else if (eyes.Length == 0)
                {
                    _eyes.SetActive(false);
                }
                yield return new WaitForSeconds(0.025f);
            }
        }

        private List<Vector3> CalculateList(List<Vector3> rowList)
        {
            var calculated = new List<Vector3>();
            if (rowList.Count == 0) return calculated;

            var length = rowList.Count;
            var first = rowList[0];
            var last = rowList[length - 1];
            var dx = (last - first) / length;

            for (int i = 0; i < length; i++)
            {
                calculated.Add(first + i * dx);
            }

            return calculated;
        }

        IEnumerator FaceTracking()
        {
            while (true)
            {
                var texture = WebCamTexture;
                var faces = FacePositionGetter.GetFaces(GrayTextureGetter.Get(texture));
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

                yield return new WaitForSeconds(0.01f);
            }
        }

        private void SetTransform(Transform transform, OpenCvSharp.Rect rect)
        {
            var position = transform.position;
            position.x = rect.Center.X;
            position.y = rect.Center.Y;
            transform.position = position * 0.01f;
        }
    }
}