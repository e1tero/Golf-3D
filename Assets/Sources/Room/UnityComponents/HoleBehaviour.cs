#pragma warning disable 649
using UnityEngine;
using UnityEngine.UI;
using System;
using Leopotam.Ecs;

namespace Sources
{
    public class HoleBehaviour : MonoBehaviour
    {
        private void Awake()
        {
            Game.HoleBehaviour = this;
        }

        private void OnDestroy()
        {
            Game.HoleBehaviour = null;
        }

        private void OnCollisionEnter(Collision collision)
        {
            var uEnt = collision.transform?.GetComponent<EntityBehaviour>();
            if (uEnt != null)
            {
                uEnt.entity.Set<HoleComponent>();
                Debug.Log("HoleEvent");
            }
        }
    }
}