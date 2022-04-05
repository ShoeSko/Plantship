using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MicInput : MonoBehaviour
{
    #region Test Variables
    [SerializeField] private GameObject TestButton;
    [SerializeField] private Slider volumeSlider;
    #endregion


    public float testSound;
    public static float MicLoudness;
    private string _device;
    private AudioClip _clipRecord;
    private int _sampleWindow = 128;
    [HideInInspector] public bool _isInitialized;

    void InitMic()
    {
        if (_device == null)
        {
            TestButton.GetComponent<Image>().color = Color.red; //Sets button colour to red.

            _device = Microphone.devices[0];
            _device = null;
            _clipRecord = Microphone.Start(_device, true, 999, 1000);
            Debug.Log(_clipRecord);
        }
    }

    void StopMicrophone()

    {
        Microphone.End(_device);

        TestButton.GetComponent<Image>().color = Color.green; //Sets button colour to Green
    }

    float LevelMax()
    {
        float levelMax = 0;
        float[] waveData = new float[_sampleWindow];
        int micPosition = Microphone.GetPosition(_device) - (_sampleWindow + 1);
        if (micPosition < 0)
        {
            return 0;
        }
        _clipRecord.GetData(waveData, micPosition);
        for (int i = 0; i < _sampleWindow; ++i)
        {
            float wavePeak = waveData[i] * waveData[i];
            if (levelMax < wavePeak)
            {
                levelMax = wavePeak;
            }
        }
        return levelMax;
    }

    void Update()
    {
        MicLoudness = LevelMax();
        testSound = MicLoudness;
        volumeSlider.value = MicLoudness;

    }

    //void OnEnable()
    //{
    //    InitMic();
    //    _isInitialized = true;
    //}

    /// <summary>
    /// Activates recording of volume.
    /// </summary>
    public void ActivateVolumeRecording()
    {
        if (!_isInitialized)
        {
            InitMic();
            _isInitialized = true;
        }
        else if (_isInitialized)
        {
            StopMicrophone();
            _isInitialized = false;
        }
    }

    //void OnDisable()
    //{
    //    StopMicrophone();
    //}

    void OnDestory()
    {
        StopMicrophone();
    }

    //void OnApplicationFocus(bool focus)
    //{
    //    if (focus)
    //    {
    //        if (!_isInitialized)
    //        {
    //            InitMic();
    //            _isInitialized = true;
    //        }
    //    }

    //    if (!focus)
    //    {
    //        StopMicrophone();
    //        _isInitialized = false;
    //    }
    //}
}
