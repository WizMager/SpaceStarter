using UnityEngine;

public class GameInitialization 
{
   public GameInitialization(Controllers controllers, Data data, Camera[] cameras, 
      GameObject[] planets, GameObject player, GameObject[] gravityFields)
   {
      GameObject[] firstStagePlanets = {planets[0], planets[1], planets[2]};
      var inputInitialization = new InputInitialization();
      controllers.Add(new InputController(inputInitialization.GetAllInput()));
      // controllers.Add(new CameraFirstPersonMoveController(inputInitialization.GetAxisInput(), cameras[1],
      //    planets[3].transform, data.Player.swipeSensitivity));
      // controllers.Add(new TapExplosionController(cameras[1], inputInitialization.GetTouchDown(), data.Player.explosionArea,
      //    data.Player.explosionForce, data.LastPlanet.explosionParticle));
      controllers.Add(new PlayerTopDownController(inputInitialization.GetTouchAllInput(), player,
         data.Player.gravity, data.Player.engineForce,firstStagePlanets, data.Player.speedRotation, 
         gravityFields, data.Player.playerFlyingAngle, cameras));
   }
}