using System;
using Jam.Scripts.BusEvents.ChangeTriggerZone;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Jam.Scripts.BusEvents
{
    [ExecuteBefore(typeof(ChangeTrigger))]
    public class SceneTrigger : MonoBehaviour
    {
        public AssetReference Scene;
        public ChangeTrigger Trigger;

        private void OnEnable()
        {
            Trigger.ChangeOnTriggerEnter.AddListener(OnEnter);
            Trigger.ChangeOnTriggerExit.AddListener(OnExit);
        }
        
        private void OnDisable()
        {
            Trigger.ChangeOnTriggerEnter.RemoveListener(OnEnter);
            Trigger.ChangeOnTriggerExit.RemoveListener(OnExit);
        }

        public void OnEnter()
        {
            MessageBroker.Default.Publish<LoadSceneEvent>(new LoadSceneEvent{Scene = Scene});
        }
        
        public void OnExit()
        {
            MessageBroker.Default.Publish<RemoveSceneEvent>(new RemoveSceneEvent{Scene = Scene});
        }

        private void OnDrawGizmos()
        {
            
        }
    }
}