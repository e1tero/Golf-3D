#pragma warning disable 649
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Sources
{
    public class ScreenMain : MonoBehaviour
    {
        [SerializeField] private Button _buttonPlay;

        private void Start()
        {
            _buttonPlay.onClick.AddListener(() =>
            {
                Game.UI.Show<ScreenLoading>().LoadRoom(1);
            });
        }
    }
}