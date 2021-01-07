using UnityEngine;
using Zenject;

namespace Rovio.Templates.Client.Commons.Zenject
{
    public class DiContainerAdapter : IDiContainerAdapter
    {
        private readonly DiContainer _container;

        public DiContainerAdapter(DiContainer container)
        {
            _container = container;
        }

        public T InstantiatePrefabResourceForComponent<T>(string resourcePath, Transform parentTransform)
        {
            return _container.InstantiatePrefabResourceForComponent<T>(resourcePath, parentTransform);
        }

        public void Inject(object @object)
        {
            _container.Inject(@object);
        }

        public T Instantiate<T>(params object[] extraArgs)
        {
            return _container.Instantiate<T>(extraArgs);
        }

        public T InstantiatePrefabForComponent<T>(GameObject prefab, Transform parentTransform,
            params object[] extraArgs) where T : Object
        {
            return _container.InstantiatePrefabForComponent<T>(prefab, parentTransform, extraArgs);
        }

        public T InstantiatePrefabForComponent<T>(T prefab, Transform parentTransform, params object[] extraArgs)
            where T : Object
        {
            return _container.InstantiatePrefabForComponent<T>(prefab, parentTransform, extraArgs);
        }

        public T InstantiatePrefabResourceForComponent<T>(string resourcePath, Transform parentTransform,
            params object[] extraArgs)
        {
            return _container.InstantiatePrefabResourceForComponent<T>(resourcePath, parentTransform, extraArgs);
        }

        public T AddComponent<T>(GameObject gameObject) where T : Component
        {
            var component = gameObject.AddComponent<T>();
            Inject(component);
            return component;
        }
    }
}