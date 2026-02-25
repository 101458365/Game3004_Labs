using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [Header("Slider")]
    public Slider healthSlider;

    [Header("Color Thresholds")]
    public Image healthFill;
    public Color highHealthColor = Color.green;
    public Color midHealthColor = Color.yellow;
    public Color lowHealthColor = Color.red;
    [Range(0f, 1f)] public float midThreshold = 0.5f;
    [Range(0f, 1f)] public float lowThreshold = 0.25f;

    float maxHealth;

    public void SetMaxHealth(float max)
    {
        maxHealth = max;
        healthSlider.maxValue = max;
        healthSlider.value = max;
        UpdateColor(1f);
    }

    public void SetHealth(float hp)
    {
        healthSlider.value = hp;
        UpdateColor(hp / maxHealth);
    }

    void UpdateColor(float fraction)
    {
        if (healthFill == null) return;

        if (fraction > midThreshold)
            healthFill.color = Color.Lerp(midHealthColor, highHealthColor,
                                           (fraction - midThreshold) / (1f - midThreshold));
        else if (fraction > lowThreshold)
            healthFill.color = Color.Lerp(lowHealthColor, midHealthColor,
                                           (fraction - lowThreshold) / (midThreshold - lowThreshold));
        else
            healthFill.color = lowHealthColor;
    }
}