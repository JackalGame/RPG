using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        BaseStats baseStats;
        Text currentLevelText;

        private void Awake()
        {
            baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
            currentLevelText = GetComponent<Text>();
        }

        private void Update()
        {
            currentLevelText.text = baseStats.GetLevel().ToString();
        }
    }
}


