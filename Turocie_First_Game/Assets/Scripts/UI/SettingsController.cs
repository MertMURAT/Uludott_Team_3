using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsController : MonoBehaviour
{
    public AudioMixer audioMixer;
    public TMPro.TMP_Dropdown _resDropDown;

    Resolution[] _resArr;
    int _currentResIndex = 0;


    private void Start()
    {
        _resArr = Screen.resolutions;
        List<string> strOptions = new List<string>();

        _resDropDown.ClearOptions();

        for(int i = 0;i < _resArr.Length; i += 1)
        {
            string option = _resArr[i].width + " x " + _resArr[i].height;
            strOptions.Add(option);

            if ((_resArr[i].width == Screen.currentResolution.width) && (_resArr[i].height == Screen.currentResolution.height)) _currentResIndex = i;
        }

        _resDropDown.AddOptions(strOptions);
        _resDropDown.value = _currentResIndex;
        _resDropDown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution res = _resArr[resolutionIndex];
        Screen.SetResolution(res.width , res.height , Screen.fullScreen);
    }


    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetGraphicQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

}
