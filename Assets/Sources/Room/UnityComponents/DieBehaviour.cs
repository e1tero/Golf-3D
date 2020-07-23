#pragma warning disable 649
using UnityEngine;
using UnityEngine.UI;
using System;
using Leopotam.Ecs;

namespace Sources
{
    public class DieBehaviour : MonoBehaviour
    {
        private float _time;

        private void OnCollisionEnter(Collision collision)
        {
            _time = Time.time;
        }

        private void OnCollisionStay(Collision collision)
        {
            if (_time + 0.35f > Time.time) return;
            if (_time < 0) return;
            
            _time = -1;

            var uEnt = collision.transform?.GetComponent<EntityBehaviour>();
            if (uEnt != null)
            {
                uEnt.entity.Set<DieEvent>();
                Debug.Log("HoleEvent");
            }
        }
    }
}