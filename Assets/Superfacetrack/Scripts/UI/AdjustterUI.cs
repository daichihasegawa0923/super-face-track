using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperFaceTrack.FaceTrack;
using UnityEngine.UI;

namespace SuperFaceTrack.UI
{
    public class AdjustterUI : MonoBehaviour
    {
        [SerializeField]
        private Slider AllSpinSliderY;

        [SerializeField]
        private Slider NeckSpinSliderX;
        [SerializeField]
        private Slider NeckSpinSliderY;
        [SerializeField]
        private Slider NeckSpinSliderZ;

        [SerializeField]
        private Slider SizeSlider;

        [SerializeField]
        private Slider SpinRateSlider;

        [SerializeField]
        private Slider MouthSpinRateSlider;

        [SerializeField]
        private RawImage RawImage;

        public void Start()
        {
            AllSpinSliderY.onValueChanged.AddListener((value) => { Adjuster.Instance.AdjustAllSpin.y = value; });
            NeckSpinSliderX.onValueChanged.AddListener((value) => { Adjuster.Instance.AdjustNeckSpin.x = value; });
            NeckSpinSliderY.onValueChanged.AddListener((value) => { Adjuster.Instance.AdjustNeckSpin.y = value; });
            NeckSpinSliderZ.onValueChanged.AddListener((value) => { Adjuster.Instance.AdjustNeckSpin.z = value; });
            SizeSlider.onValueChanged.AddListener((value) => { Adjuster.Instance.AdjustSize = value; });
            SpinRateSlider.onValueChanged.AddListener((value) => { Adjuster.Instance.NeckSpinRate = value; });
            MouthSpinRateSlider.onValueChanged.AddListener((value) => { Adjuster.Instance.MouthSpinRate = value; });

            RawImage.texture = WebCameraPlayer.Instance.WebCamTexture;
        }
    }
}