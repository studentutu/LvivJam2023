using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Jam.Scripts.Spawner
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private float _TimeOut = 5f;
        [SerializeField] private BoxCollider BOunds;

        private void OnEnable()
        {
            StartCoroutine(SpawnPrefabWithTimeout());
        }

        private IEnumerator SpawnPrefabWithTimeout()
        {
            var timeOut = Random.Range(2, _TimeOut);
            var randomInBounds = GetRandomPointInsideCollider( BOunds);
            
            GameObject.Instantiate(_prefab, randomInBounds, transform.rotation, transform.parent);

            while (timeOut>0)
            {
                yield return null;
                timeOut -= Time.deltaTime;
            }
            
            StartCoroutine(SpawnPrefabWithTimeout());
        }
        
        public Vector3 GetRandomPointInsideCollider( BoxCollider boxCollider )
        {
            Vector3 extents = boxCollider.size / 2f;
            Vector3 point = new Vector3(
                Random.Range( -extents.x, extents.x ),
                Random.Range( -extents.y, extents.y ),
                Random.Range( -extents.z, extents.z )
            );
 
            return boxCollider.transform.TransformPoint( point );
        }
    }
}