using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace Jam.Scripts.BusEvents
{
    public class Health : MonoBehaviour
    {
        public Animator m_animator;
        public GameObject m_bloodParticleSystem;
        public GameObject[] m_bloodSpawnPoints;

        public float InitialHealth = 100;
        public UnityEvent OnDead;
        public UnityEvent OnTakeDamage;

        void Awake()
        {
            m_animator.SetBool("Walk", true);
        }

        public void TakeDamage(float damage)
        {
            InitialHealth -= damage;
            OnTakeDamage?.Invoke();
            PlayBloodParticles();

            if (InitialHealth <= 0)
            {
                OnDead?.Invoke();
                MessageBroker.Default.Publish(new TookDamageEvent { Damage = damage, IsDead = InitialHealth <= 0 });

                GameObject.Destroy(this.gameObject);
            }
        }

        private void PlayBloodParticles()
        {
            int m_randSpawn = Random.Range(0, 3);
            GameObject blood = Instantiate(m_bloodParticleSystem, m_bloodSpawnPoints[m_randSpawn].transform);
            blood.transform.position = m_bloodSpawnPoints[m_randSpawn].transform.position;
        }
    }
}