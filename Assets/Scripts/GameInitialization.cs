using UnityEngine;

public class GameInitialization
{
   public GameInitialization(Controllers controllers, Data data)
   {
      var camera = Camera.main;
      var inputInitialization = new InputInitialization();
      controllers.Add(new InputController(inputInitialization.GetAllInput()));
   }
}