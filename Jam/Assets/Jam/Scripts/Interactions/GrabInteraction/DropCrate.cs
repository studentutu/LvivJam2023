﻿using Jam.Scripts.BusEvents.BusEvents.Interactions;
using Jam.Scripts.BusEvents.CrateDropInteraction;
using UniRx;
using UnityEngine;

namespace Jam.Scripts.BusEvents.GrabInteraction
{
    public class DropCrate : MonoBehaviour
    {
        public CrateType CrateType;
        [SerializeField] private float PointAmmountToReceive;
        [SerializeField] private GrabItem _itemSelf;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<DropCrateZoneTrigger>(out var trigger))
                return;

            var increase = trigger.CrateType == CrateType;
            MessageBroker.Default.Publish(
                new UpdatePointsEvent { Increase = increase, Ammount = PointAmmountToReceive });

            _itemSelf.Release();
            _itemSelf.TryDestroy();
        }
    }
}