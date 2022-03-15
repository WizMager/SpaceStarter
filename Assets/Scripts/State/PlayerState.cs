using Controller;

namespace State
{
    public abstract class PlayerState
    {
        protected PlayerController PlayerController;

        public void SetContext(PlayerController context)
        {
            PlayerController = context;
        }

        public abstract void Move(float deltaTime);
    }
}