using UnityEngine;
using UnityEngine.Events;

namespace Jam.Scripts.BusEvents
{
    public class Health : MonoBehaviour
    {
        public float InitialHealth = 100;
        public UnityEvent OnDead;

        public void TakeDamage(float damage)
        {
            InitialHealth -= damage;
            if(InitialHealth <=0)
                OnDead?.Invoke();
            
            GameObject.Destroy(this.gameObject);
        }
    }
}