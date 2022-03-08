namespace DefaultNamespace
{
    public abstract class State
    {
        protected StateContext _stateContext;

        public void SetContext(StateContext context)
        {
            _stateContext = context;
        }

        public abstract void Move(float deltaTime);
    }
}