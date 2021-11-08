using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using System;
using GameDevTV.Utils;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float regenerartionPercentage = 80;
        [SerializeField] HealthBar healthBar = null;
        [SerializeField] TakeDamageEvent takeDamage;
        [SerializeField] OnDieEvent onDie;
        

        [System.Serializable]        
        public class TakeDamageEvent : UnityEvent<float>
        {
        }

        [System.Serializable]
        public class OnDieEvent : UnityEvent<float>
        {
        }

        LazyValue<float> healthPoints;

        private bool isDead = false;

        private void Awake()
        {
            healthPoints = new LazyValue<float>(GetInitialHealth);
        }

        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void Start()
        {
            healthPoints.ForceInit();
        }

        private void OnEnable()
        {
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;
        }

        public bool IsDead()
        {
            return isDead;
        }


        public void TakeDamage(GameObject instigator, float damage)
        {
            if (isDead) { return; }

            Debug.Log($"{gameObject.name} took damage: {damage}");

            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);
            
            //if(healthBar != null)
            //{
            //    healthBar.updateHealth(GetFraction());
            //}

            if(healthPoints.value <= 0)
            {
                onDie.Invoke(damage);
                AwardExperience(instigator);
                Die();
            }
            else
            {
                takeDamage.Invoke(damage);
            }
        }

        public float GetCurrentHealth()
        {
            return healthPoints.value;
        }

        public float GetMaxHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public float GetPercentage()
        {
            return healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health) * 100;
        }

        public float GetFraction()
        {
            return healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;

            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        private void Die()
        {
            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
            GetComponent<NavMeshAgent>().enabled = false;
        }

        private void RegenerateHealth()
        {
            float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenerartionPercentage / 100);
            healthPoints.value = Mathf.Max(healthPoints.value, regenHealthPoints);
        }

        public object CaptureState()
        {
            return healthPoints.value;
        }

        public void RestoreState(object state)
        {
            healthPoints.value = (float)state;

            if (healthPoints.value <= 0)
            {
                Die();
            }
        }

    }
}
