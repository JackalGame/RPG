using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject 
    {
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] GameObject equippedPrefab = null;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] int weaponDamage = 5;
        [SerializeField] bool isRightHanded = true;

        public float GetRange()
        {
            return weaponRange;
        }

        public int GetDamage()
        {
            return weaponDamage;
        }


        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            if(equippedPrefab != null)
            {
                Transform handTransform;
                if(isRightHanded) 
                {
                    handTransform = rightHand;
                }
                else
                {
                    handTransform = leftHand;
                }
                Instantiate(equippedPrefab, handTransform);
            }

            if(animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
        }
    }
}
