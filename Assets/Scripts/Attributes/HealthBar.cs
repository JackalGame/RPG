using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] RectTransform foreground = null;
        [SerializeField] Canvas canvas = null;
        [SerializeField] Health healthComponent = null;

        private void Awake()
        {
            canvas.enabled = false;
        }

        public void updateHealth(float healthPercentage)
        {
            canvas.enabled = true;

            if (Mathf.Approximately(healthComponent.GetFraction(), 0))
            {
                canvas.enabled = false;
            }

            foreground.localScale = new Vector3(healthComponent.GetFraction(), 1, 1);
        }
    }
}
