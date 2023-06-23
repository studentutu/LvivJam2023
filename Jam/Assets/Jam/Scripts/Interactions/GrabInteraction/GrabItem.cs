using System;
using UnityEngine;

namespace Jam.Scripts.BusEvents.GrabInteraction
{
    public class GrabItem : MonoBehaviour
    {
        public Collider collider;
        public Rigidbody _rb;

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
    }
}