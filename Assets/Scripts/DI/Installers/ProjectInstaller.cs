using PSG.IsleOfColors.Managers;
using UnityEngine;
using Zenject;

namespace PSG.IsleOfColors.DI.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private ApplicationManager applicationManagerPrefab;
        [SerializeField] private AudioManager audioManagerPrefab;
        [SerializeField] private AnalyticsManager analyticsManagerPrefab;

        public override void InstallBindings()
        {
            Container.Bind<ApplicationManager>().FromComponentInNewPrefab(applicationManagerPrefab).AsSingle().NonLazy();
            Container.Bind<AudioManager>().FromComponentInNewPrefab(audioManagerPrefab).AsSingle().NonLazy();
            Container.Bind<AnalyticsManager>().FromComponentInNewPrefab(analyticsManagerPrefab).AsSingle().NonLazy();
        }
    }

}
