#pragma warning disable 649
using UnityEngine;
using System.Collections.Generic;
using Leopotam.Ecs;

namespace Sources
{
    public class PoolInstance: MonoBehaviour
    {
        private Dictionary<int, Stack<GameObject>> _pools = new Dictionary<int, Stack<GameObject>>(32);
        private Dictionary<int, int> _instances = new Dictionary<int, int>(128);

        public GameObject Spawn(GameObject prefab, Transform parent = null)
        {
            var key = prefab.GetInstanceID();

            Stack<GameObject> objs;
            var stacked = _pools.TryGetValue(key, out objs);

            if (stacked && objs.Count > 0)
            {
                var obj = objs.Pop();
                var transform = obj.transform;
                if (transform.parent != parent)
                    transform.SetParent(parent);

                transform.gameObject.SetActive(true);

                return obj;
            }

            if (!stacked)
            {
                _pools.Add(key, new Stack<GameObject>(32));
            }

            var createdPrefab = Object.Instantiate(prefab, parent);

            var k = createdPrefab.GetInstanceID();

            _instances.Add(k, key);
            return createdPrefab;
        }

        public void Release(GameObject go)
        {
            go.SetActive(false);
            go.transform.SetParent(transform);

            int id = 0;
            if (!_instances.TryGetValue(go.GetInstanceID(), out id))
            {
                Debug.LogWarning("Pool instance not found");
                return;
            }

            _pools[id].Push(go);
        }
    }

    public static class Pool
    {
        private static PoolInstance _instance;

        private static PoolInstance Scene()
        {
            if (_instance == null)
            {
                var obj = new GameObject("pool instance");
                _instance = obj.AddComponent<PoolInstance>();
            }
            return _instance;
        }

        public static GameObject Spawn(GameObject prefab, Transform parent = null)
        {
            return Scene().Spawn(prefab, parent);
        }

        public static void Release(GameObject go)
        {
            Scene().Release(go);
        }
    }
}