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
        [HideInInspector] public GameObject m_player;

        void Awake()
        {
            m_player = GameObject.FindGameObjectWithTag("Player");
            m_animator.SetBool("Walk", true);
        }

        public void TakeDamage(float damage)
        {
            InitialHealth -= damage;
            OnTakeDamage?.Invoke();
            PlayBloodParticles();
            
            if(InitialHealth <=0)
                OnDead?.Invoke();
            
            GameObject.Destroy(this.gameObject);
        }
        
        public void PlayBloodParticles()
        {
            int m_randSpawn = Random.Range(0, 3);
            GameObject blood = Instantiate(m_bloodParticleSystem, m_bloodSpawnPoints[m_randSpawn].transform) as GameObject;
            blood.transform.position = m_bloodSpawnPoints[m_randSpawn].transform.position;
            blood.transform.parent = m_bloodSpawnPoints[m_randSpawn].transform;
        }
    }
}