using PSG.IsleOfColors.Gameplay;
using PSG.IsleOfColors.Gameplay.StateMachine;
using UnityEngine;
using Zenject;

namespace PSG.IsleOfColors.DI.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private GameManager gameManagerInstance;

        public override void InstallBindings()
        {
            Container.Bind<GameManager>().FromInstance(gameManagerInstance).AsSingle().NonLazy();
            Container.Bind<GameStateMachine>().FromComponentInHierarchy().AsSingle().NonLazy();
        }
    }

}
