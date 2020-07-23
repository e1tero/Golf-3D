using UnityEngine;
using System;
#pragma warning disable 649

namespace Sources
{
    [CreateAssetMenu(fileName = "RemoteConfig", menuName = "Scriptable/RemoteConfig")]
    public class RemoteConfig : ScriptableObject
    {
        public string Url { get { return _url; } }
        [SerializeField] private string _url;

        public string HashKey { get { return _hashKey; } }
        [SerializeField] private string _hashKey;

        //public bool Encode { get { return _encode; } }
        //[SerializeField] private bool _encode;

        public bool Debug { get { return _debug; } }
        [SerializeField] private bool _debug;
    }
}