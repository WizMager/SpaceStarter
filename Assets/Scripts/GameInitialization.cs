using Controller;
using UnityEngine;
using View;

public class GameInitialization
{
   private readonly PlayerView _player = Object.FindObjectOfType<PlayerView>();
   private readonly PlanetView[] _planets = Object.FindObjectsOfType<PlanetView>();
   private readonly GravityView[] _gravities = Object.FindObjectsOfType<GravityView>();
   private readonly Camera _camera = Object.FindObjectOfType<Camera>();
   
   public GameInitialization(Controllers controllers, Data data)
   {
      var inputInitialization = new InputInitialization();
      controllers.Add(new InputController(inputInitialization.GetAllInput()));
      // controllers.Add(new CameraFirstPersonMoveController(inputInitialization.GetAxisInput(), cameras[1],
      //    planets[3].transform, data.Player.swipeSensitivity));
      // controllers.Add(new TapExplosionController(cameras[1], inputInitialization.GetTouchDown(), data.Player.explosionArea,
      //    data.Player.explosionForce, data.LastPlanet.explosionParticle));
      controllers.Add(new PlayerController(data, inputInitialization.GetTouchAll(), _player, _planets, 
         _gravities, _camera));
   }
   
}