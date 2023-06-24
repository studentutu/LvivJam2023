using Jam.Scripts.BusEvents.BusEvents.Interactions;
using Jam.Scripts.BusEvents.CrateDropInteraction;
using UniRx;
using UnityEngine;

namespace Jam.Scripts.BusEvents.GrabInteraction
{
    public class DropCrate : MonoBehaviour
    {
        public CrateType CrateType;
        [SerializeField] private float PointAmmountToReceive;
        [SerializeField] public GrabItem _itemSelf;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<DropCrateZoneTrigger>(out var trigger))
                return;

            if(trigger.CrateType != CrateType)
                return;
            
            MessageBroker.Default.Publish(
                new UpdatePointsEvent { Increase = true,Type = _itemSelf.UsedInInteraction,Ammount = PointAmmountToReceive });

            if (_itemSelf.IsInUse)
            {
                _itemSelf.Release();
            }
            _itemSelf.TryDestroy();
        }
    }
}