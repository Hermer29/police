using Services;
using Zenject;

namespace Infrastructure.DependencyInjection
{
    public class GameplayInstaller : MonoInstaller
    {
        private const string InputServicePath = "Prefabs/InputService";

        public override void InstallBindings()
        {
            BindInputService();
        }

        private void BindInputService()
        {
            Container.Bind<IInputService>()
                .To<InputService>()
                .FromComponentInNewPrefabResource(InputServicePath)
                .AsSingle();
        }
    }
}