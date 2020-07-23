#pragma warning disable 649
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Sources
{
    public class ScreenLoading : MonoBehaviour
    {
        public void LoadRoom(int scene)
        {
            Game.roomScene = scene;
            StartCoroutine(LoadRoomScene());
        }

        public void ExitRoom()
        {
            StartCoroutine(UnloadRoomScene());
        }

        private IEnumerator LoadRoomScene()
        {
            yield return SceneManager.LoadSceneAsync(Game.roomScene, LoadSceneMode.Additive);
            Game.UI.Show<ScreenGame>();
        }

        private IEnumerator UnloadRoomScene()
        {
            yield return SceneManager.UnloadSceneAsync(Game.roomScene);
            Game.UI.Show<ScreenMain>();
        }
    }
}