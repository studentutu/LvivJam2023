using System;
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
        [SerializeField] private float PointForDamage = 2f;
        [SerializeField] private float PointForKill = 10f;
        
        [SerializeField] private float StressForShot = 2f;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private AudioClip _shotClip;

        private bool _canShoot;
        private float _shootCooldown;
        private CompositeDisposable _disposable = new CompositeDisposable();

        private void OnEnable()
        {
            MessageBroker.Default.Receive<TookDamageEvent>().Subscribe(x => ProcessDamageEvent(x))
                .AddTo(_disposable);
        }

        private void OnDisable()
        {
            _disposable?.Dispose();
            _disposable = new CompositeDisposable();
        }

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
            AudioSource.PlayClipAtPoint(_shotClip, spawnPoint.position);
            
            MessageBroker.Default.Publish(new UpdateStressDataDeltaEvent() { Amount = StressForShot});
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

        private void ProcessDamageEvent(TookDamageEvent data)
        {
            var newEvent = new UpdatePointsEvent();

            newEvent.Type = InteractionTypes.Shooting;
            newEvent.Increase = true;
            newEvent.Ammount = PointForDamage;

            if (data.IsDead)
                newEvent.Ammount += PointForKill;

            MessageBroker.Default.Publish(newEvent);
        }
    }
}