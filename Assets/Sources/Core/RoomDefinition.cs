#pragma warning disable 649
using UnityEngine;

namespace Sources
{
    [CreateAssetMenu(fileName = "RoomDefinition", menuName = "Scriptable/RoomDefinition")]
    public class RoomDefinition : ScriptableObject
    {
        public GameObject PlayerPrefab => _playerPrefab;
        public float BallMaxPower => _ballMaxPower;
        public Material[] ballMaterials => _ballMaterials;

        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private float _ballMaxPower;
        [SerializeField] private Material[] _ballMaterials;
    }
}