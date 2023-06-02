using Hermer29.Almasury;

namespace Infrastructure.States
{
    public class SdkInitializationState : State
    {
        private bool _isSdkInitialized;
        
        protected override void OnEnter()
        {
            _isSdkInitialized = true;
        }

        protected override void OnExit()
        {
            
        }

        [Transition(typeof(CreateServicesState))]
        public bool IsInitializedSdk() => _isSdkInitialized;
    }
}