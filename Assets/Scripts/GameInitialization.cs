using UnityEngine;

public class GameInitialization
{
   public GameInitialization(Controllers controllers, Data data)
   {
      var camera = Camera.main;
      var inputInitialization = new InputInitialization();
      controllers.Add(new InputController(inputInitialization.GetAllInput()));
      controllers.Add(new CameraFirstPersonMoveController(inputInitialization.GetAxisInput(), camera,
         data.Planet.planetCenter, data.Player.swipeSensitivity));
      controllers.Add(new TapExplosionController(camera, inputInitialization.GetTouchInput(), data.Player.explosionArea,
         data.Player.explosionForce));
   }
}