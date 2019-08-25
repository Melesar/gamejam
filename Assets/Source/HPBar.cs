using System;
using Source.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Source
{
    public class HPBar : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private Text _text;
        
        private void OnHpChanged(float currentHealth, float maxHealth)
        {
            _slider.value = currentHealth / maxHealth;
            _text.text = ((int) currentHealth).ToString();
        }
        
        private void Awake()
        {
            PlayerHealth.healthChanged += OnHpChanged;
        }

        private void OnDestroy()
        {
            PlayerHealth.healthChanged -= OnHpChanged;
        }
    }
}