using UnityEngine;
using RPG.Attributes;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float projectileSpeed = 1f;
        [SerializeField] bool isHoming = false;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] float maxDuration = 5f;
        [SerializeField] GameObject[] destroyOnHit = null;
        [SerializeField] float lifeAfterImpact = 2f;
        [SerializeField] UnityEvent onHit;

        Health target;
        GameObject instigator = null;
        float damage = 0;

        private void Start()
        {
            transform.LookAt(GetAimLocation());
        }

        void Update()
        {
            if (target == null) { return; }

            if (isHoming && !target.IsDead())
            {
                transform.LookAt(GetAimLocation());
            }
            
            transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
        }

        public void SetTarget(Health target, GameObject instigator, float damage)
        {
            this.target = target;
            this.damage = damage;
            this.instigator = instigator;

            Destroy(gameObject, maxDuration);
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null)
            {
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * targetCapsule.height * 0.7f;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.GetComponent<Health>() != target) { return; }
            if(target.IsDead()) { return; }
            target.TakeDamage(instigator, damage);

            projectileSpeed = 0f;

            onHit.Invoke();

            if(hitEffect != null)
            {
                Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            }

            foreach(GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }

            Destroy(gameObject, lifeAfterImpact);
        }
    }
}
