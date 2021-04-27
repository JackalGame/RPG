using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1.5f;
        [SerializeField] int weaponDamage = 5;

        private Transform target;
        private Health healthComponent;
        [SerializeField] private float timeSinceLastAttack = 0;

        private Mover mover;
        private ActionScheduler actionScheduler;
        private Animator anim;

        private void Start()
        {
            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();
            anim = GetComponent<Animator>();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            
            if (target == null) return;

            if (!GetIsInRange())
            {
                mover.MoveTo(target.position);
            }
            else
            {
                mover.Cancel();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            if (timeSinceLastAttack < timeBetweenAttacks) { return; }
            
            // This will trigger the Hit() event.
            anim.SetTrigger("attack");
            timeSinceLastAttack = 0;
        }

        // Animation Evant
        void Hit()
        {
            healthComponent.TakeDamage(weaponDamage);
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            actionScheduler.StartAction(this);
            target = combatTarget.transform;
            healthComponent = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            target = null;
        }
    }
}
