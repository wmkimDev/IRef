using System.Linq;
using UnityEngine;
// ReSharper disable InconsistentNaming

namespace wmk.IRef.Runtime
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
            if (!IRefUtility.ValidateTarget<T>(target).IsValid)
            {
                target = null;
                Debug.LogWarning(result.ErrorMessage);
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

public static class IRefUtility
{
    public static T GetInterface<T>(Object target) where T : class
    {
        if (target == null) return null;
        if (target is GameObject go)
        {
            go.TryGetComponent(out T component);
            return component;
        }

        return target as T;
    }

    public readonly struct ValidateTargetResult
    {
        public readonly bool IsValid;
        public readonly string ErrorMessage;

        public ValidateTargetResult(bool isValid, string errorMessage)
        {
            IsValid      = isValid;
            ErrorMessage = errorMessage;
        }
    }

    public static ValidateTargetResult ValidateTarget<T>(Object target) where T : class
    {
        string errorMessage = string.Empty;

        if (target == null)
        {
            return new ValidateTargetResult(
                false,
                "The target is null.");
        }

        if (target is GameObject go)
        {
            Component[] components = go.GetComponents<Component>().Where(c => c is T).ToArray();
            switch (components.Length)
            {
                case > 1:
                    return new ValidateTargetResult(
                        false,
                        $"There are multiple components of type {typeof(T)} on the GameObject {go.name}.");
                case 0:
                    return new ValidateTargetResult(
                        false,
                        $"There is no component of type {typeof(T)} on the GameObject {go.name}.");
                default:
                    return new ValidateTargetResult(true, errorMessage);
            }
        }

        if (!(target is T))
        {
            return new ValidateTargetResult(
                false, 
                $"The target is not of type {typeof(T)}.");
        }

        return new ValidateTargetResult(true, errorMessage);
    }
}