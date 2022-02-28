using System.Collections.Generic;
using UnityEngine;

public class GameInitialization 
{
   public GameInitialization(Controllers controllers, Data data, Camera[] cameras, 
      GameObject[] planets, GameObject player, GameObject[] gravityFields)
   {
      GameObject[] firstStagePlanets = {planets[0], planets[1], planets[2]};
      Transform[] firstStagePlanetsTransform =
      {
         firstStagePlanets[0].transform, firstStagePlanets[1].transform, firstStagePlanets[2].transform
      };
      var inputInitialization = new InputInitialization();
      controllers.Add(new InputController(inputInitialization.GetAllInput()));
      // controllers.Add(new CameraFirstPersonMoveController(inputInitialization.GetAxisInput(), firstPersonCamera,
      //    planets[3].transform, data.Player.swipeSensitivity));
      // controllers.Add(new TapExplosionController(firstPersonCamera, inputInitialization.GetTouchInput(), data.Player.explosionArea,
      //    data.Player.explosionForce, data.LastPlanet.explosionParticle));
      controllers.Add(new PlayerTopDownController(inputInitialization.GetTouchAllInput(), player,
         data.Player.gravity, data.Player.engineForce,firstStagePlanets, data.Player.speedRotation, 
         gravityFields, data.Player.playerFlyingAngle));
      controllers.Add(new CameraTopDownController(cameras, firstStagePlanetsTransform, player.transform));
   }
}