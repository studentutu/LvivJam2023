using UnityEngine;
using UnityEngine.Events;

namespace Jam.Scripts.BusEvents.ChangeTriggerZone
{
    public class ChangeTrigger : MonoBehaviour
    {
        [SerializeField] public UnityEvent ChangeOnTriggerEnter;
        [SerializeField] public UnityEvent ChangeOnTriggerExit;
        
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
    }
}