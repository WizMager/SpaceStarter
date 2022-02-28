using System.Collections.Generic;
using UnityEngine;

public class GameInitialization 
{
   public GameInitialization(Controllers controllers, Data data, IReadOnlyList<Camera> cameras, 
      IReadOnlyList<GameObject> planetsCenter, GameObject player)
   {
      var firstPersonCamera = cameras[0];
      var topDownCamera = cameras[1];

      var inputInitialization = new InputInitialization();
      controllers.Add(new InputController(inputInitialization.GetAllInput()));
      //controllers.Add(new CameraFirstPersonMoveController(inputInitialization.GetAxisInput(), firstPersonCamera,
         //planetsCenter[1].transform, data.Player.swipeSensitivity));
      //controllers.Add(new TapExplosionController(firstPersonCamera, inputInitialization.GetTouchInput(), data.Player.explosionArea,
         //data.Player.explosionForce, data.LastPlanet.explosionParticle));
      controllers.Add(new PlayerTopDownController(inputInitialization.GetTouchInput(), player,
         data.Player.gravity, data.Player.engineForce,planetsCenter[0].transform, data.Player.speedRotation));
      controllers.Add(new CameraTopDownController(topDownCamera, planetsCenter[0].transform, data.Player.speedRotation, player.transform));
   }
}