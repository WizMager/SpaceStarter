using System.Collections.Generic;

namespace Controller
{
    public class Controllers: IInitialization, IExecute, IClean
    {
        private readonly List<IInitialization> _initializationControllers;
        private readonly List<IExecute> _executeControllers;
        private readonly List<IFixedExecute> _fixedExecuteControllers;
        private readonly List<IClean> _cleanControllers;

        public Controllers()
        {
            _initializationControllers = new List<IInitialization>();
            _executeControllers = new List<IExecute>();
            _fixedExecuteControllers = new List<IFixedExecute>();
            _cleanControllers = new List<IClean>();
        }

        public Controllers Add(IController controller)
        {
            if (controller is IInitialization initializationController)
            {
                _initializationControllers.Add(initializationController);
            }

            if (controller is IExecute executeController)
            {
                _executeControllers.Add(executeController);
            }

            if (controller is IFixedExecute fixedExecuteController)
            {
                _fixedExecuteControllers.Add(fixedExecuteController);
            }
        
            if (controller is IClean cleanController)
            {
                _cleanControllers.Add(cleanController);
            }

            return this;
        }
    
        public void Initialization()
        {
            for (int index = 0; index < _initializationControllers.Count; index++)
            {
                _initializationControllers[index].Initialization();
            }
        }

        public void Execute(float deltaTime)
        {
            for (int index = 0; index < _executeControllers.Count; index++)
            {
                _executeControllers[index].Execute(deltaTime);
            }
        }

        public void FixedExecute(float fixedDeltaTime)
        {
            for (int index = 0; index < _fixedExecuteControllers.Count; index++)
            {
                _fixedExecuteControllers[index].FixedExecute(fixedDeltaTime);
            }
        }
    
        public void Clean()
        {
            for (int index = 0; index < _cleanControllers.Count; index++)
            {
                _cleanControllers[index].Clean();
            } 
        }
    }
}