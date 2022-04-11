using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View
{


    public class PortalView : MonoBehaviour
    {
        [SerializeField] private ParticleSystem launchParticleSystem;
        
        private void Start()
        {
            gameObject.SetActive(false);
        }
        
        public void SetPosition(Vector3 position, Quaternion rotation)
        {
            transform.position = position;
            transform.rotation = rotation;
        }
        
        public void OpenPortal()
        {
            gameObject.SetActive(true);
            launchParticleSystem.Stop();
        }
        
        public void LaunchTeleport()
        {
            launchParticleSystem.Play();
        }

    }
}