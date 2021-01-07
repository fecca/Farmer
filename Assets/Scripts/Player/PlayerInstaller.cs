using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    [SerializeField] private PlayerController _playerPrefab;

    public override void InstallBindings()
    {
        Container.Bind<PlayerController>().FromComponentInNewPrefab(_playerPrefab).AsSingle();
    }
}