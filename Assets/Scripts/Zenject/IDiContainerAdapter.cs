using UnityEngine;

namespace Rovio.Templates.Client.Commons.Zenject
{
    public interface IDiContainerAdapter
    {
        void Inject(object @object);
        T Instantiate<T>(params object[] extraArgs);
        T InstantiatePrefabResourceForComponent<T>(string resourcePath, Transform parentTransform);

        T InstantiatePrefabForComponent<T>(GameObject prefab, Transform parentTransform, params object[] extraArgs)
            where T : Object;

        T InstantiatePrefabForComponent<T>(T prefab, Transform parentTransform, params object[] extraArgs)
            where T : Object;

        T AddComponent<T>(GameObject gameObject) where T : Component;

        T InstantiatePrefabResourceForComponent<T>(string resourcePath, Transform parentTransform,
            params object[] extraArgs);
    }
}