using TMPro;
using UnityEngine;
using Zenject;
using UnityEngine.AI;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    [Inject]
    private SignalBus m_signalBus;

    [SerializeField]
    private Slider m_sliderRateTime;
    [SerializeField]
    TMP_Text m_currentRateTimeText;

    private void Start()
    {
        m_sliderRateTime.onValueChanged.AddListener(UpdateSpawnerRateTime);
    }

    private void UpdateSpawnerRateTime(float newValue)
    {
        m_currentRateTimeText.text = newValue.ToString("F1");
        m_signalBus.Fire(new OnSpawnerRateTimeChangeSignal(newValue));
    }
}
