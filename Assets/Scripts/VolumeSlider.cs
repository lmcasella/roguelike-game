using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))] // El script va a estar siempre en un Slider
public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private AudioMixer mainMixer;

    private const string MIXER_PARAMETER_NAME = "MasterVolume";
    private Slider _slider;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        float currentVolume;
        if (mainMixer.GetFloat(MIXER_PARAMETER_NAME, out currentVolume))
        {
            _slider.value = DecibelToLinear(currentVolume);
        }

        _slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float linearValue)
    {
        float decibelValue = LinearToDecibel(linearValue);
        mainMixer.SetFloat(MIXER_PARAMETER_NAME, decibelValue);
    }

    // Hay que convertir decibeles a valores lineales
    private float LinearToDecibel(float linear)
    {
        return linear > 0.0001f ? 20.0f * Mathf.Log10(linear) : -80.0f;
    }

    private float DecibelToLinear(float db)
    {
        return Mathf.Pow(10.0f, db / 20.0f);
    }

    private void OnDestroy()
    {
        _slider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }
}
