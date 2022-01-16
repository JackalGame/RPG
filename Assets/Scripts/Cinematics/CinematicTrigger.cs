using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using GameDevTV.Saving;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour, ISaveable
    {
        bool hasAlreadyPlayed = false;

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player" && !hasAlreadyPlayed)
            {
                hasAlreadyPlayed = true;
                GetComponent<PlayableDirector>().Play();
            }    
        }


        public object CaptureState()
        {
            return hasAlreadyPlayed;
        }

        public void RestoreState(object state)
        {
            hasAlreadyPlayed = (bool)state;
        }
    }
}
