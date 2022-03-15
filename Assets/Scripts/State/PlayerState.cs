using Controller;
using DefaultNamespace;

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