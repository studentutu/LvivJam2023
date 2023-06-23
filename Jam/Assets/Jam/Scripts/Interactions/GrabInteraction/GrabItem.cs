using System;
using Jam.Scripts.BusEvents.BusEvents.Interactions;
using UniRx;
using UnityEngine;

namespace Jam.Scripts.BusEvents.GrabInteraction
{
    public class GrabItem : MonoBehaviour
    {
        public InteractionTypes UsedInInteraction;
        
        public Collider collider;
        public Rigidbody _rb;
        public GameObject OnDestroyVFX;
        
        private CompositeDisposable _disposable = new CompositeDisposable();

        private void OnEnable()
        {
            MessageBroker.Default.Receive<ChangeInteractionEvent>().Subscribe(x =>
            {
                if (x.Interaction != UsedInInteraction)
                {
                    Release();
                    TryDestroy();
                }
            }).AddTo(_disposable);
        }

        private void OnDisable()
        {
            _disposable.Dispose();
        }

        public void AttachTo( Transform parent)
        {
            collider.transform.SetParent(parent, true);
            collider.transform.localPosition = Vector3.zero;
            collider.transform.localRotation = Quaternion.identity;
            _rb.isKinematic = true;
        }

        public void Release()
        {
            collider.transform.SetParent(null, true);
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