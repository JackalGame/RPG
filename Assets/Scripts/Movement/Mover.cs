using RPG.Combat;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour
    {
        private NavMeshAgent navMeshAgent;
        private Animator anim;
        private Fighter fighter;

        private void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
            fighter = GetComponent<Fighter>();
        }

        private void Update()
        {
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination)
        {
            fighter.Cancel();
            MoveTo(destination);
        }

        public void MoveTo(Vector3 destination)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.isStopped = false;
        }

        public void StopMoving()
        {
            navMeshAgent.isStopped = true;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = GetComponent<NavMeshAgent>().velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            anim.SetFloat("forwardSpeed", speed);
        }
    }
}
