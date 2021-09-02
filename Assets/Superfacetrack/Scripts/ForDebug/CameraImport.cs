using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OpenCvSharp;
using SuperFaceTrack.OpenCVExtension;
using SuperFaceTrack.FaceTrack;

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

        private List<Vector3> _rowNumbers = new List<Vector3>();
        private List<Vector3> _calclaterNumbers = new List<Vector3>();

        private void Start()
        {
            FaceTrackExecuter.Instance.Execute((spin) =>
            {
                _face.transform.eulerAngles = spin;
            },
            (isEyeOpen) =>
            {
                _eyes.SetActive(isEyeOpen);
            }, null);
        }
    }
}