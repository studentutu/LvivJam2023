using System;
using UniRx;
using UnityEngine;

namespace Jam.Scripts.Misc
{
    public class Waypoints : MonoBehaviour
    {
        public Transform[] waypoints;                   //  All the waypoints where the enemy patrols

        private void OnEnable()
        {
            MessageBroker.Default.Buffered().Publish(this);
        }
    }
}