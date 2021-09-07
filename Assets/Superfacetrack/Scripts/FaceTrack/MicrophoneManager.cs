using System.Collections;
using System.Collections.Generic;
using SuperFaceTrack.Util;
using UnityEngine;

namespace SuperFaceTrack.FaceTrack
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(AudioClip))]
    public class MicrophoneManager : SingletonMonoBehaviour<MicrophoneManager>
    {
        AudioSource AudioSource { get; set; }

        protected override void Awake()
        {
            base.Awake();

            AudioSource = GetComponent<AudioSource>();

            int minFreq ,maxFreq;
            Microphone.GetDeviceCaps(null, out minFreq, out maxFreq);
            AudioSource.clip = Microphone.Start(null, true, 999, minFreq);
            AudioSource.loop = true;
            while (!(Microphone.GetPosition(null) > 0)) { }
            AudioSource.Play();
        }

        public float GetAudioData()
        {
            var data = new float[256];
            var a = 0.0f;
            AudioSource.GetOutputData(data, 0);
            foreach (float s in data)
            {
                a += Mathf.Abs(s);
            }
            return a / 256.0f;
        }
    }
}