using System;
using System.Collections;
using System.Collections.Generic;
using SuperFaceTrack.OpenCVExtension;
using UnityEngine;
using SuperFaceTrack.Util;

namespace SuperFaceTrack.FaceTrack
{
    public class FaceTrackExecuter : SingletonMonoBehaviour<FaceTrackExecuter>
    {
        [SerializeField]
        private int _lateCount = 5;

        private WebCamTexture WebCamTexture { set; get; }

        [SerializeField]
        private List<Vector3> _rowVector3 = new List<Vector3>();

        [SerializeField]
        private List<Vector3> _calclaterVector3 = new List<Vector3>();

        public void Execute(Action<Vector3> spinFaceAction, Action<bool> eyeOpenAction, Action<Vector3> mouthOpenAction)
        {
            WebCamTexture = new WebCamTexture();
            WebCamTexture.Play();
            StartCoroutine(FaceSpinTrack(spinFaceAction));
            StartCoroutine(EyeOpenClose(eyeOpenAction));
            StartCoroutine(MouthOpenClose(mouthOpenAction));
        }

        private OpenCvSharp.Rect[] _eyes;
        private OpenCvSharp.Rect[] _faces;

        IEnumerator FaceSpinTrack(Action<Vector3> spinFaceAction)
        {
            while (true)
            {
                var texture = WebCamTexture;
                _eyes = (_rowVector3.Count == 0 || _rowVector3.Count >= _lateCount - 1) ? FacePositionGetter.GetEyes(GrayTextureGetter.Get(texture)) : _eyes;
                _faces = (_rowVector3.Count == 0 || _rowVector3.Count >= _lateCount - 1) ? FacePositionGetter.GetFaces(GrayTextureGetter.Get(texture)) : _faces;

                if (_eyes.Length == 2)
                {
                    var eye01 = _eyes[0];
                    var eye02 = _eyes[1];

                    var leftEye = eye01.Center.X > eye02.Center.X ? eye01 : eye02;
                    var rightEye = eye01.Center.X > eye02.Center.X ? eye02 : eye01;

                    var spin = Vector3.zero;

                    var xSpin = 0;
                    var ySpin = 0;
                    if (_faces.Length > 0)
                    {
                        xSpin = -(_faces[0].Center.Y - (texture.height / 2));
                        ySpin = rightEye.Center.X - _faces[0].Center.X - (_faces[0].Center.X - leftEye.Center.X);
                    }

                    var zSpin = leftEye.Center.Y - rightEye.Center.Y;

                    spin.x = xSpin;
                    spin.y = ySpin;
                    spin.z = zSpin;

                    _rowVector3.Add(spin);

                    if (_rowVector3.Count >= _lateCount)
                    {
                        _calclaterVector3 = CalculateList(_rowVector3);
                        _rowVector3.Clear();
                    }

                    if (_calclaterVector3.Count > 0)
                    {
                        spinFaceAction(_calclaterVector3[0]);
                        _calclaterVector3.RemoveAt(0);
                    }
                }


                yield return new WaitForSeconds(0.05f);
            }
        }

        IEnumerator EyeOpenClose(Action<bool> eyeOpenAction)
        {
            while (true)
            {
                var texture = WebCamTexture;
                var eyes = FacePositionGetter.GetEyes(GrayTextureGetter.Get(texture));
                eyeOpenAction(eyes.Length == 2);
                yield return new WaitForSeconds(0.1f);
            }
        }

        IEnumerator MouthOpenClose(Action<Vector3> mouthOpenAction)
        {
            while(true)
            {
                var texture = WebCamTexture;
                var mouthes = FacePositionGetter.GetMouthes(GrayTextureGetter.Get(texture));

                if (mouthes.Length == 1)
                {
                    var mouthSize = Vector3.zero;

                    mouthSize.x = mouthes[0].Size.Width;
                    mouthSize.y = mouthes[0].Size.Height;
                    mouthSize.z = 1;

                    Debug.Log(mouthes[0].Size);

                    mouthOpenAction(mouthSize);
                }

                yield return new WaitForSeconds(0.01f);
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
    }
}