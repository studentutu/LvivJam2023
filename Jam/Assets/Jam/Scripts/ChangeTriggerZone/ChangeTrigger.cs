using System;
using UnityEngine;
using UnityEngine.Events;

namespace Jam.Scripts.BusEvents.ChangeTriggerZone
{
    public class ChangeTrigger : MonoBehaviour
    {
        [SerializeField] private Collider _bounds;
        
        [SerializeField] public UnityEvent ChangeOnTriggerEnter;
        [SerializeField] public UnityEvent ChangeOnTriggerExit;

        private void OnValidate()
        {
            if (_bounds == null)
                _bounds = GetComponent<Collider>();
        }

        public void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
                ChangeOnTriggerEnter?.Invoke();
        }
        
        public void OnTriggerExit(Collider other)
        {
            if(other.CompareTag("Player"))
                ChangeOnTriggerExit?.Invoke();
        }

        public void OnDrawGizmos()
        {
            if (_bounds == null)
                return;
            
            var bounds = _bounds.bounds;
            Gizmos.DrawWireCube(_bounds.transform.position, bounds.size);
        }
    }
}