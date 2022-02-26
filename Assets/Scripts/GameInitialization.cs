using UnityEngine;

public class GameInitialization
{
   public GameInitialization(Controllers controllers, Data data)
   {
      var camera = Camera.main;
      var inputInitialization = new InputInitialization();
      controllers.Add(new InputController(inputInitialization.GetAllInput()));
      controllers.Add(new CameraFirstPersonMoveController(inputInitialization.GetAxisInput(), camera,
         data.Planet.planetCenter, data.Planet.swipeSensitivity));
   }
}