#pragma warning disable 649
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Sources
{
    public class CameraBehaviour : MonoBehaviour
    {
        private void Awake()
        {
            Game.CameraBehaviour = this;
        }
    }
}