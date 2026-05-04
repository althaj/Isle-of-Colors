using PSG.IsleOfColors.Gameplay;
using PSG.IsleOfColors.Gameplay.StateMachine;
using UnityEngine;
using Zenject;

namespace PSG.IsleOfColors.DI.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private GameManager gameManagerPrefab;
        [SerializeField] private GameStateMachine stateMachinePrefab;
        [SerializeField] private GameFactory gameFactoryPrefab;

        public override void InstallBindings()
        {
            Container.Bind<GameManager>().FromComponentInNewPrefab(gameManagerPrefab).AsSingle().NonLazy();
            Container.Bind<GameStateMachine>().FromComponentInNewPrefab(stateMachinePrefab).AsSingle().NonLazy();
            Container.Bind<GameFactory>().FromComponentInNewPrefab(gameFactoryPrefab).AsSingle().NonLazy();
        }
    }

}
