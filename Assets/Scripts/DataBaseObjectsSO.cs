using System.Collections.Generic;
using UnityEngine;

namespace ErfanDeveloper
{
    [CreateAssetMenu]
    public class DataBaseObjectsSO : ScriptableObject
    {
        public List<Data> objectsData;
    }

    [System.Serializable]
    public class Data
    {
        [field: SerializeField]
        public string Name { get; private set; }
        [field: SerializeField]
        public int ID { get; private set; }
        [field: SerializeField]
        public Vector2 Size { get; set; } = Vector2Int.one;
        [field: SerializeField]
        public GameObject Perfab { get; private set; }
    }
}