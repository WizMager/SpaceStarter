using UnityEngine;

public class PlayerTopDownController : IExecute, IClean
{
    private Transform _player;
    private IUserInput<Vector3> _inputTouch;
    private bool _isTouched;
    private float _gravityForce;
    private float _engineForce;
    private Transform _currentPlanet;
    private float _speedRotation;

    public PlayerTopDownController(IUserInput<Vector3> inputTouch, Transform player, float gravityForce, 
        float engineForce,Transform planet, float speedRotation)
    {
        _player = player;
        _inputTouch = inputTouch;
        _inputTouch.OnChange += OnTouched;
        _gravityForce = gravityForce;
        _engineForce = engineForce;
        _currentPlanet = planet;
        _speedRotation = speedRotation;
    }

    private void OnTouched(Vector3 touchPosition)
    {
        _isTouched = true;
    }

    private void CameraMove(bool isTouched, float deltaTime)
    {
        var shipPositionAxisX = new Vector3(0, 0);
        if (isTouched)
        {
            shipPositionAxisX.x = -_engineForce;
            _player.Translate(shipPositionAxisX * deltaTime);
            _isTouched = false;
        }
        else
        {
            shipPositionAxisX.x = _gravityForce;
            _player.Translate(shipPositionAxisX * deltaTime);
        }
    }
    
    private void PlayerRotation(float deltaTime)
    {
        _player.RotateAround(_currentPlanet.position, Vector3.up, _speedRotation * deltaTime);
    }
    
    public void Execute(float deltaTime)
    {
        CameraMove(_isTouched, deltaTime);
        PlayerRotation(deltaTime);
    }

    public void Clean()
    {
        _inputTouch.OnChange -= OnTouched;
    }
}