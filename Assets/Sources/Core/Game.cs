#pragma warning disable 649
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Sources
{
    public static class Game
    {
        public static Room room;
        public static int roomScene;

        public static GameUi UI;
        public static TeeBehaviour TeeBehaviour { get; set; }
        public static JoystickBehaviour JoystickBehaviour { get; set; }
        public static HoleBehaviour HoleBehaviour { get; set; }
        public static CameraBehaviour CameraBehaviour { get; set; }
    }
}