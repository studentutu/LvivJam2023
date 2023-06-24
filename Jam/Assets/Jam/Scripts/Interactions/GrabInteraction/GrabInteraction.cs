using Unity.VisualScripting;
using UnityEngine;

namespace Jam.Scripts.BusEvents.GrabInteraction
{
    public class GrabInteraction : IInteraction
    {
        public Transform AttachToPoint;
        public float Force = 5;

        private GameObject _CurrentInContact;
        private GrabItem _interactingWith;


        public override bool IsInAction()
        {
            if (_CurrentInContact == null)
                return false;
            if (_interactingWith == null)
                return false;

            return true;
        }

        public override void InteractionStart()
        {
            if (_CurrentInContact == null)
                return;

            _interactingWith = _CurrentInContact.GetComponent<GrabItem>();

            if (_interactingWith == null)
                return;

            _interactingWith.AttachTo(AttachToPoint);
        }

        public override void InteractionStop(InteractionTypes possibleNewZone)
        {
            if (_interactingWith == null)
                return;

            _interactingWith.Release();
            _interactingWith._rb.isKinematic = false;
            var direction = _interactingWith.ImpulseDirection;
            var force = AttachToPoint.forward * Force;

            if (_interactingWith.UseImulse)
            {
                force = (AttachToPoint.forward + direction) * Force;
            }

            _interactingWith._rb.AddForce(force, ForceMode.Impulse);

            if (possibleNewZone != Type)
                _interactingWith.TryDestroy();

            _interactingWith = null;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_interactingWith != null)
                return;

            _CurrentInContact = other.gameObject;
        }
    }
}