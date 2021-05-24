using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float projectileSpeed = 1f;
    
    void Update()
    {
        if(target == null) { return; }

        transform.LookAt(GetAimLocation());
        transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
    }

    private Vector3 GetAimLocation()
    {
        CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
        if(targetCapsule == null)
        {
            return target.position;
        }
        return target.position + Vector3.up * targetCapsule.height * 0.7f;
    }
}
