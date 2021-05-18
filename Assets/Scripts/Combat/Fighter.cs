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
        [SerializeField] GameObject weaponPrefab = null;
        [SerializeField] Transform handTransform = null;
        [SerializeField] AnimatorOverrideController weaponOverride = null;

        private Health target;
        [SerializeField] private float timeSinceLastAttack = Mathf.Infinity;

        private Mover mover;
        private ActionScheduler actionScheduler;
        private Animator anim;

        private void Start()
        {
            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();
            anim = GetComponent<Animator>();
            EquipWeapon();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) { return; }
            if (target.IsDead()) { return; }

            if (!GetIsInRange())
            {
                mover.MoveTo(target.transform.position, 1f);
            }
            else
            {
                mover.Cancel();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);

            if (timeSinceLastAttack < timeBetweenAttacks) { return; }

            // This will trigger the Hit() event.
            TriggerAttackAnim();
            timeSinceLastAttack = 0;
        }

        // Animation Evant
        void Hit()
        {
            if(target == null) { return; }
            target.TakeDamage(weaponDamage);
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) { return false; }
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        public void Attack(GameObject combatTarget)
        {
            actionScheduler.StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            StopAttackAnim();
            target = null;
            mover.Cancel();
        }

        private void TriggerAttackAnim()
        {
            anim.ResetTrigger("cancelAttack");
            anim.SetTrigger("attack");
        }

        private void StopAttackAnim()
        {
            anim.ResetTrigger("attack");
            anim.SetTrigger("cancelAttack");
        }

        private void EquipWeapon()
        {
            Instantiate(weaponPrefab, handTransform);
            anim.runtimeAnimatorController = weaponOverride;
        }
    }
}
