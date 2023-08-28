using TMPro;
using UnityEngine;
using Zenject;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    public const float DEFAULT_DELAY_TO_SPAWN = 4;

    [Inject]
    private SignalBus m_signalBus;

    [SerializeField]
    private Slider m_sliderRateTime;
    [SerializeField]
    private TMP_Text m_currentRateTimeText;

    private void Start()
    {
        m_sliderRateTime.onValueChanged.AddListener(UpdateSpawnerRateTime);
    }

    /// <summary>
    /// Updates the spawning rate time based on the value of the rate time slider.
    /// </summary>
    /// <param name="newValue">The new value of the rate time slider.</param>
    private void UpdateSpawnerRateTime(float newValue)
    {
        m_currentRateTimeText.text = newValue.ToString("F1");

        m_signalBus.Fire(new OnSpawnerRateTimeChangeSignal(newValue));
    }
}