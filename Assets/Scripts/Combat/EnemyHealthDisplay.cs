using RPG.Resources;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter fighter;
        Text healthValue;

        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
            healthValue = GetComponent<Text>();
        }

        private void Update()
        {
            if(fighter.GetTarget() == null)
            {
                healthValue.text = "N/A";
            }
            else
            {
                Health health = fighter.GetTarget();
                healthValue.text = String.Format("{0:0} / {1:0}", health.GetCurrentHealth(), health.GetMaxHealth()) ;
                //healthValue.text = String.Format("{0:0}%", health.GetPercentage());
            }
        }
    }
}
