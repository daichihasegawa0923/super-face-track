using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperFaceTrack.Util;

namespace SuperFaceTrack.FaceTrack
{
    public class Adjuster : SingletonMonoBehaviour<Adjuster>
    {
        [SerializeField]
        public Vector3 AdjustNeckSpin;

        [SerializeField]
        public Vector3 AdjustAllSpin;

        [SerializeField]
        public float AdjustSize = 1;

        [SerializeField]
        public float NeckSpinRate = 1;

        [SerializeField]
        public float MouthSpinRate = 100;

    }
}