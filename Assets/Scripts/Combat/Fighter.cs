using RPG.Core;
using RPG.Movement;
using GameDevTV.Saving;
using RPG.Attributes;
using UnityEngine;
using RPG.Stats;
using System.Collections.Generic;
using GameDevTV.Utils;
using System;
using GameDevTV.Inventories;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {

        [SerializeField] float timeBetweenAttacks = 1.5f;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] WeaponConfig defaultWeapon = null;

        private float timeSinceLastAttack = Mathf.Infinity;

        private Health target;
        private Equipment equipment;
        private Mover mover;
        private ActionScheduler actionScheduler;
        private Animator anim;
        WeaponConfig currentWeaponConfig;
        LazyValue<Weapon> currentWeapon;

        private float weaponRange;
        private float weaponDamage;

        private void Awake()
        {
            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();
            anim = GetComponent<Animator>();
            currentWeaponConfig = defaultWeapon;
            currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
            equipment = GetComponent<Equipment>();
            if (equipment)
            {
                equipment.equipmentUpdated += UpdateWeapon;
            }
        }

        private Weapon SetupDefaultWeapon()
        {
            return AttachWeapon(defaultWeapon);
        }

        private void Start()
        {
            currentWeapon.ForceInit();
        }


        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) { return; }
            if (target.IsDead()) { return; }

            if (!GetIsInRange(target.transform))
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

        // Animation Event
        void Hit()
        {
            if(target == null) { return; }

            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);

            if(currentWeapon.value != null)
            {
                currentWeapon.value.OnHit();
            }

            if (currentWeaponConfig.HasProjectile())
            {
                currentWeaponConfig.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage);
            }
            else
            {
                target.TakeDamage(gameObject, damage);
            }
        }

        //Animation Event
        void Shoot()
        {
            Hit();
        }

        private bool GetIsInRange(Transform targetTransform)
        {
            return Vector3.Distance(transform.position, targetTransform.position) < weaponRange;
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) { return false; }
            if (!mover.CanMoveTo(combatTarget.transform.position) && !GetIsInRange(combatTarget.transform)) 
            { 
                return false; 
            }
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

        public void EquipWeapon(WeaponConfig weapon)
        {
            currentWeaponConfig = weapon;
            currentWeapon.value = AttachWeapon(weapon);
        }

        private void UpdateWeapon()
        {
            WeaponConfig weapon = equipment.GetItemInSlot(EquipLocation.Weapon) as WeaponConfig;
            if(weapon != null)
            {
                EquipWeapon(weapon);
            }
            else
            {
                EquipWeapon(defaultWeapon);
            }
        }

        private Weapon AttachWeapon(WeaponConfig weapon)
        {
            weaponRange = weapon.GetRange();
            weaponDamage = weapon.GetDamage();
            return weapon.Spawn(rightHandTransform, leftHandTransform, anim);
        }

        public Health GetTarget()
        {
            return target;
        }

        public object CaptureState()
        {
            return currentWeaponConfig.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            WeaponConfig weapon = UnityEngine.Resources.Load<WeaponConfig>(weaponName);
            EquipWeapon(weapon);
        }
    }
}
