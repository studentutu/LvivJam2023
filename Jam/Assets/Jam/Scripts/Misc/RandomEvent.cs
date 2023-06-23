using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Jam.Scripts.Misc
{
    public class RandomEvent : MonoBehaviour
    {
        public UnityEvent OnRandomEvent;
        [SerializeField] private float _TimeOut = 5f;

        private void OnEnable()
        {
            StartCoroutine(RandomEventWithDelay());
        }
        
        private IEnumerator RandomEventWithDelay()
        {
            var timeOut = Random.Range(2, _TimeOut);
            OnRandomEvent?.Invoke();

            while (timeOut>0)
            {
                yield return null;
                timeOut -= Time.deltaTime;
            }
            
            StartCoroutine(RandomEventWithDelay());
        }
    }
}