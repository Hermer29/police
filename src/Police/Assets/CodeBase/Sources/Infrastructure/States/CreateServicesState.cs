using Hermer29.Almasury;

namespace Infrastructure.States
{
    public class CreateServicesState : State
    {
        [Transition(typeof(LoadLevelState))]
        public bool Transit() => true;
    }
}