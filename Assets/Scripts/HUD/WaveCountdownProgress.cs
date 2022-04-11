using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveCountdownProgress : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void SetMaxValue(float value)
    {
        slider.maxValue = value;
        slider.value = value;
        fill.color = gradient.Evaluate(1f);
    }

    public void SetValue(float value)
    {
        slider.value = value;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        
        float diff = 1f - (slider.value / slider.maxValue);
        slider.fillRect.anchorMin = new Vector2 (0.5f * diff, 0f);
        slider.fillRect.anchorMax = new Vector2(1f - (0.5f * diff), 1f);
    }

    private void Update() {
        SetValue(slider.value - Time.deltaTime);
    }
}
