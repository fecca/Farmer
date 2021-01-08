using UnityEngine;
using Zenject;

public class LevelInstaller : MonoInstaller
{
    [SerializeField] private LevelConfig _levelConfig;
    [SerializeField] private NodeBehaviour _nodeBehaviour;

    public override void InstallBindings()
    {
        Container.BindFactory<Transform, int, int, float, float, NodeBehaviour, NodeBehaviour.Factory>().FromComponentInNewPrefab(_nodeBehaviour).AsSingle();

        Container.Bind<ILevelService>().To<LevelService>().AsSingle();
        Container.Bind<ILevelGenerator>().To<GridLevelGenerator>().AsSingle();
        Container.Bind<ILevelRepresentation>().To<MeshLevel>().AsSingle();
        Container.Bind<LevelConfig>().FromNewScriptableObject(_levelConfig).AsSingle();

        Container.DeclareSignal<NodeBehaviourClickedSignal>();
        Container.DeclareSignal<EnteredNodeSignal>();
    }
}