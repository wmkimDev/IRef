using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace WMK.IRef.Runtime
{
    [System.Serializable]
    public class IRefList<T> : ISerializationCallbackReceiver where T : class
    {
        [SerializeField]
        private List<Object> targets = new();

        public List<Object> Objects => targets;

        private List<T> _cachedInterfaces = new();
        public List<T> Interfaces => _cachedInterfaces ??= GetInterfaces();
        private List<T> GetInterfaces() => targets.Select(IRefUtility.GetInterface<T>).ToList();

        private void OnValidate()
        {
            for (var i = 0; i < targets.Count; i++)
            {
                if (targets[i] == null) continue;
                var result = IRefUtility.ValidateTarget<T>(targets[i]);
                if (!result.isValid)
                {
                    targets[i] = null;
                    Debug.LogWarning(result.errorMessage);
                }
            }
        }

        public void Add(Object obj)
        {
            var result = IRefUtility.ValidateTarget<T>(obj);
            if (result.isValid)
            {
                targets.Add(obj);
                _cachedInterfaces.Add(IRefUtility.GetInterface<T>(obj));
            }
            else
            {
                throw new System.Exception(result.errorMessage);
            }
        }
        
        public bool TryAdd(Object obj)
        {
            var result = IRefUtility.ValidateTarget<T>(obj);
            if (result.isValid)
            {
                targets.Add(obj);
                _cachedInterfaces.Add(IRefUtility.GetInterface<T>(obj));
                return true;
            }
            
            Debug.LogWarning(result.errorMessage);
            return false;
        }
        
        public void Remove(Object obj)
        {
            var index = targets.IndexOf(obj);
            if (index >= 0) RemoveAt(index);
        }

        public void RemoveAt(int index)
        {
            targets.RemoveAt(index);
            _cachedInterfaces.RemoveAt(index);
        }

        public T this[int index] => Interfaces[index];

        #region Operators

        public static implicit operator bool(IRefList<T> ir) => ir.targets is { Count: > 0 };
        public static implicit operator List<T>(IRefList<T> ir) => ir.Interfaces;

        #endregion

        public void OnBeforeSerialize() => OnValidate();

        public void OnAfterDeserialize()
        {
        }
    }
}