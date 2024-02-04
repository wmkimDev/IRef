using UnityEngine;
using UnityEngine.Serialization;

namespace WMK.IRef.Runtime
{
    [System.Serializable]
    public class IRef<T> : ISerializationCallbackReceiver where T : class
    {
        [SerializeField]
        private Object target;
        public Object Object => target;

        private T _cachedInterface;
        public T Interface => _cachedInterface ??= IRefUtility.GetInterface<T>(target);
        
        private void OnValidate()
        {
            if (target == null) return;
            var result = IRefUtility.ValidateTarget<T>(target);
            if (!IRefUtility.ValidateTarget<T>(target).isValid)
            {
                target = null;
                Debug.LogWarning(result.errorMessage);
            }
        }

        #region Operators

        public static implicit operator bool(IRef<T> ir) => ir.target != null;
        public static implicit operator T(IRef<T> ir) => ir.Interface;

        #endregion

        public void OnBeforeSerialize() => OnValidate();

        public void OnAfterDeserialize() { }
    }
}