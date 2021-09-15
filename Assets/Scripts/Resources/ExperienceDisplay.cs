using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Resources
{
    public class ExperienceDisplay : MonoBehaviour
    {
        Experience experience;
        Text xp;

        private void Awake()
        {
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
            xp = GetComponent<Text>();
        }

        private void Update()
        {
            xp.text = experience.GetXP().ToString();
        }
    }
}
