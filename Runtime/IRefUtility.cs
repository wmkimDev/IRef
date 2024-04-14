using System.Linq;
using UnityEngine;

namespace WMK.IRef.Runtime
{
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
            public readonly bool isValid;
            public readonly string errorMessage;

            public ValidateTargetResult(bool isValid, string errorMessage)
            {
                this.isValid      = isValid;
                this.errorMessage = errorMessage;
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
}