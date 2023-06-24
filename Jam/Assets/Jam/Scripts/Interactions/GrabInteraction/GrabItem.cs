using System;
using Jam.Scripts.BusEvents.BusEvents.Interactions;
using UniRx;
using UnityEngine;

namespace Jam.Scripts.BusEvents.GrabInteraction
{
    public class GrabItem : MonoBehaviour
    {
        public InteractionTypes UsedInInteraction;
        [Tooltip("From 0 to 100")]
        public float DeltaStress = 2;
        public Collider collider;
        public Rigidbody _rb;
        public GameObject OnDestroyVFX;
        
        private CompositeDisposable _disposable = new CompositeDisposable();

        private void OnEnable()
        {
            MessageBroker.Default.Receive<ChangeInteractionEvent>().Subscribe(x =>
            {
                if (x.Interaction != UsedInInteraction)
                    TryDestroy();
            }).AddTo(_disposable);
        }

        private void OnDisable()
        {
            _disposable.Dispose();
        }

        public void AttachTo( Transform parent)
        {
            _rb.isKinematic = true;
            collider.transform.SetParent(parent, true);
            collider.transform.localPosition = Vector3.zero;
            collider.transform.localRotation = Quaternion.identity;
        }

        public void Release()
        {
            collider.transform.SetParent(null, true);
            MessageBroker.Default.Publish(new UpdateStressDataDeltaEvent() { Amount = DeltaStress});
        }

        private bool destroyed = false;
        public void TryDestroy()
        {
            if(destroyed)
               return;
            
            destroyed = true;

            if(OnDestroyVFX != null)
                GameObject.Instantiate(OnDestroyVFX, transform.position, Quaternion.identity);
            
            GameObject.Destroy(collider.gameObject);
        }
    }
}