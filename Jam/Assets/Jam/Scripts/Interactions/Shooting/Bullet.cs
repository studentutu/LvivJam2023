using System;
using UnityEngine;

namespace Jam.Scripts.BusEvents.Misc
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float Speed = 0.3f;
        [SerializeField] private float BulletDamage = 0.3f;
        [SerializeField] private float Lifetime = 1f;
        
        private void FixedUpdate()
        {
            var transform1 = transform;
            transform1.position += transform1.forward* (Time.deltaTime * Speed);

            Lifetime -= Time.fixedDeltaTime;
            if(Lifetime <=0)
                GameObject.Destroy(this.gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent<Health>(out var h))
            {
                h.TakeDamage(BulletDamage);
            }

            GameObject.Destroy(this.gameObject);
        }
    }
}