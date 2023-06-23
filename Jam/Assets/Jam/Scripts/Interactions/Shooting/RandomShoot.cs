using System;
using System.Collections;
using Jam.Scripts.BusEvents.Misc;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Jam.Scripts.Interactions.Shooting
{
    public class RandomShoot : MonoBehaviour
    {
        [SerializeField] private GameObject bullet;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private AudioClip _shotClip;
        [SerializeField] private float _TimeOut = 2f;

        private void OnEnable()
        {
            StartCoroutine(SpawnPrefabWithTimeout());
        }

        private IEnumerator SpawnPrefabWithTimeout()
        {
            var timeOut = Random.Range(0.3f, _TimeOut);


            var newObj = GameObject.Instantiate(bullet, spawnPoint.position, transform.rotation);
            newObj.transform.forward = spawnPoint.forward;
            AudioSource.PlayClipAtPoint(_shotClip, spawnPoint.position);


            while (timeOut > 0)
            {
                yield return null;
                timeOut -= Time.deltaTime;
            }

            StartCoroutine(SpawnPrefabWithTimeout());
        }
    }
}