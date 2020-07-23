#pragma warning disable 649
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Sources
{
    public class GameUi : MonoBehaviour
    {
        [SerializeField] private GameObject[] _screens;

        public T Show<T>() where T: MonoBehaviour
        {
            var screenName = typeof(T).Name;
            T obj = null;

            foreach (var screen in _screens)
            {
                screen.SetActive(screen.name == screenName);

                if (screen.activeSelf)
                    obj = screen.GetComponent<T>();
            }

            return obj;
        }

        private void Awake()
        {
            Game.UI = this;
        }
    }
}