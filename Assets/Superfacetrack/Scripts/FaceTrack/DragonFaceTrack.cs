using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperFaceTrack.FaceTrack
{
    public class DragonFaceTrack : MonoBehaviour
    {
        FaceTrackExecuter FaceTrackExecuter { get; set; }

        [SerializeField]
        private Vector3 _spinAdjuster;

        [SerializeField]
        private float _spinRate = 1;

        [SerializeField]
        private GameObject _neck;

        [SerializeField]
        private Animator _animator;

        [SerializeField]
        private string _eye_open_str = "eye_open";

        // Start is called before the first frame update
        void Start()
        {
            FaceTrackExecuter = FaceTrackExecuter.Instance;
            FaceTrackExecuter.Execute((spin) => { NeckControl(spin); }, (isEyeOpen) => { EyeControl(isEyeOpen); }, (spin) => { });
        }

        void NeckControl(Vector3 spin)
        {
            _neck.transform.localEulerAngles = Adjuster.Instance.AdjustNeckSpin + spin * Adjuster.Instance.NeckSpinRate;
        }

        void EyeControl(bool isEyeOpen)
        {
            _animator.SetBool(_eye_open_str, isEyeOpen);
        }

        private void FixedUpdate()
        {
            transform.localScale = Vector3.one * Adjuster.Instance.AdjustSize;
            var spin = transform.eulerAngles;
            spin.y = Adjuster.Instance.AdjustAllSpin.y;
            transform.eulerAngles = spin;
           
        }

    }
}