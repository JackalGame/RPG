using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        Health health;
        Text healthValue;

        private void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
            healthValue = GetComponent<Text>();
        }

        private void Update()
        {
            healthValue.text = string.Format("{0:0} / {1:0}", health.GetCurrentHealth(), health.GetMaxHealth());
            //healthValue.text = String.Format("{0:0}%", health.GetPercentage());
        }
    }
}
 