using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    public void UpdateHealthBar(float curHP, float maxHP)
    {
        slider.maxValue = maxHP;
        slider.value = curHP;
    }
}
