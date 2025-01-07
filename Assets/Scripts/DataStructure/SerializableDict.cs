using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

namespace DataStructure
{
    [Serializable]
    public class SerializableDict<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] private TKey _defaultKey;
        [Serializable]
        public class DictPair<TKey, TValue>
        {
            public TKey Key;
            public TValue Value;
        }

        [SerializeField] private List<DictPair<TKey, TValue>> _pairs = new ();


        public void OnBeforeSerialize()
        {
            _pairs.Clear();
            int i = 0;
            foreach (var pair in this)
            {
                _pairs.Add(new DictPair<TKey, TValue>() { Key = pair.Key, Value = pair.Value });
                i++;
            }
        }

        public void OnAfterDeserialize()
        {
            this.Clear();
            foreach (var pair in _pairs)
            {
                if (pair.Key == null || this.ContainsKey(pair.Key))
                {
                    pair.Key = _defaultKey;
                }
                this[pair.Key] = pair.Value;
            }
        }


    }
}