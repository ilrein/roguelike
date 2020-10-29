using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
  public Slider slider;
  public Gradient gradient;
  public Image fill;

  public int currentVal;

  public void SetHealth(int health)
  {
    slider.value = health;
    currentVal = health;

    fill.color = gradient.Evaluate(slider.normalizedValue);
  }

  public void SetMaxHealth(int health)
  {
    slider.maxValue = health;

    fill.color = gradient.Evaluate(1f);
  }
}
