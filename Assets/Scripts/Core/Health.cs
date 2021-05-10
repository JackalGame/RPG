using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;

namespace RPG.Core
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] int healthPoints = 100;

        private bool isDead = false;

        public bool IsDead()
        {
            return isDead;
        }



        public void TakeDamage(int damage)
        {
            if (isDead) { return; }

            healthPoints = Mathf.Max(healthPoints - damage, 0);

            if(healthPoints <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
            GetComponent<NavMeshAgent>().enabled = false;
        }



        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            healthPoints = (int)state;

            if (healthPoints <= 0)
            {
                Die();
            }
        }

    }
}
