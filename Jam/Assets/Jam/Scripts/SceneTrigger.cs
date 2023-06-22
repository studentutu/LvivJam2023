using System;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Jam.Scripts.BusEvents
{
    public class SceneTrigger : MonoBehaviour
    {
        public AssetReference Scene;

        public void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
                MessageBroker.Default.Publish<LoadSceneEvent>(new LoadSceneEvent{Scene = Scene});
        }
        
        public void OnTriggerExit(Collider other)
        {
            if(other.CompareTag("Player"))
                MessageBroker.Default.Publish<RemoveSceneEvent>(new RemoveSceneEvent{Scene = Scene});
        }
    }
}