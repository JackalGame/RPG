using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
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
            Debug.Log(healthPoints);

            if(healthPoints <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            GetComponent<NavMeshAgent>().enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<Animator>().SetTrigger("die");
            isDead = true;
        }
    }
}
