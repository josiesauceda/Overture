using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Image fill;
    public Color fullColor = Color.green;
    public Color emptyColor = Color.red;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        fill.color = fullColor;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
        fill.color = Color.Lerp(emptyColor, fullColor, (float)health / slider.maxValue);
    }
}