namespace Infrastructure
{
    public static class AllServices
    {
        public static void Bind<TService>(TService service) 
            => ServiceContainer<TService>.Instance = service;

        public static TService Get<TService>() 
            => ServiceContainer<TService>.Instance;

        private static class ServiceContainer<TService>
        {
            public static TService Instance;
        }
    }
}