namespace DefaultNamespace
{
    public class LastPlanetState : State
    {
        public LastPlanetState(CameraController cameraController)
        {
            cameraController.FirstPersonActivation();
        }
        public override void Move(float deltaTime)
        {
            
        }
    }
}