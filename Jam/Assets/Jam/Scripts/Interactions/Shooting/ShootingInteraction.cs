﻿using System;
using Jam.Scripts.BusEvents.BusEvents.Interactions;
using Jam.Scripts.BusEvents.Misc;
using UniRx;
using UnityEngine;

namespace Jam.Scripts.BusEvents
{
    public class ShootingInteraction : IInteraction
    {
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private float SecondsBetweenShots = 0.3f;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private float StressUpdateDelta = 5f;

        private bool _canShoot;
        private float _shootCooldown;

        private void Update()
        {
            if (!_canShoot)
                return;

            _shootCooldown -= Time.deltaTime;

            if (_shootCooldown > 0)
                return;
            _shootCooldown = SecondsBetweenShots;

            var bullet = GameObject.Instantiate(_bulletPrefab, spawnPoint.position, transform.rotation);
            bullet.transform.forward = spawnPoint.forward;
            
            MessageBroker.Default.Publish(new UpdatePointsEvent{Increase = true, Type = InteractionTypes.Shooting,Ammount = StressUpdateDelta});
        }

        public override bool IsInAction()
        {
            return enabled;
        }

        public override void InteractionStart()
        {
            _canShoot = true;
            _shootCooldown = 0;
            enabled = true;
        }

        public override void InteractionStop(InteractionTypes newType)
        {
            _canShoot = false;
            _shootCooldown = 0;
            enabled = false;
        }
    }
}