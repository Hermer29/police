using Infrastructure.AssetManagement;
using Zenject;

namespace Infrastructure
{
    public class GameFactory : IGameFactory
    {
        private readonly DiContainer _container;
        private readonly AssetLoader _assets;

        public GameFactory(DiContainer container, AssetLoader assets)
        {
            _container = container;
            _assets = assets;
        }
        
    }
}