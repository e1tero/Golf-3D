#pragma warning disable 649
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Sources
{
    public class TeeBehaviour : MonoBehaviour
    {
        [SerializeField] private Transform[] _transforms;

        public Vector3 GetPosition(int id)
        {
            if (_transforms.Length == 0) throw new Exception("Tee position not found");

            if (id < 0) return GetPosition(id + _transforms.Length);
            if (id >= _transforms.Length) return GetPosition(id - _transforms.Length);
            return _transforms[id].position;
        }

        private void Awake()
        {
            Game.TeeBehaviour = this;
        }

        private void OnDestroy()
        {
            Game.TeeBehaviour = null;
        }
    }
}