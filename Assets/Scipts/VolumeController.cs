// 2025/12/27 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeController : MonoBehaviour
{
    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    [Header("Sliders")]
    public Slider masterVolumeSlider;
    public Slider bgmVolumeSlider;
    public Slider sfxVolumeSlider;

    private const string MASTER_VOLUME_PARAM = "VolumeMaster";
    private const string BGM_VOLUME_PARAM = "VolumeBGM";
    private const string SFX_VOLUME_PARAM = "VolumeSFX";

    private void Start()
    {
        InitializeSliders();
        AddSliderListeners();
    }

    /// <summary>
    /// Initializes the sliders with the current values from the audio mixer.
    /// </summary>
    private void InitializeSliders()
    {
        float masterVolume;
        float bgmVolume;
        float sfxVolume;

        audioMixer.GetFloat(MASTER_VOLUME_PARAM, out masterVolume);
        audioMixer.GetFloat(BGM_VOLUME_PARAM, out bgmVolume);
        audioMixer.GetFloat(SFX_VOLUME_PARAM, out sfxVolume);

        masterVolumeSlider.value = masterVolume;
        bgmVolumeSlider.value = bgmVolume;
        sfxVolumeSlider.value = sfxVolume;
    }

    /// <summary>
    /// Adds listeners to the sliders to update the audio mixer values when their value changes.
    /// </summary>
    private void AddSliderListeners()
    {
        masterVolumeSlider.onValueChanged.AddListener(value => SetVolume(MASTER_VOLUME_PARAM, value));
        bgmVolumeSlider.onValueChanged.AddListener(value => SetVolume(BGM_VOLUME_PARAM, value));
        sfxVolumeSlider.onValueChanged.AddListener(value => SetVolume(SFX_VOLUME_PARAM, value));
    }

    /// <summary>
    /// Sets the volume of the specified audio mixer parameter.
    /// </summary>
    /// <param name="parameter">The name of the audio mixer parameter.</param>
    /// <param name="value">The value to set the parameter to.</param>
    private void SetVolume(string parameter, float value)
    {
        audioMixer.SetFloat(parameter, value);
    }
}