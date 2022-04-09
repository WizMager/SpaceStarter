using Controllers;
using InputClasses;
using Model;
using UnityEngine;
using View;
using Object = UnityEngine.Object;

public class GameInitialization
{
   public GameInitialization(AllControllers controllers, ScriptableData.AllData data)
   {
      var playerView = Object.FindObjectOfType<PlayerView>();
      var camera = Object.FindObjectOfType<Camera>();
      var missilePosition = camera.GetComponentInChildren<Transform>();
      var playerIndicatorView = Object.FindObjectOfType<PlayerIndicatorView>();
      var bonusViews = Object.FindObjectsOfType<BonusView>();
      var deadView = Object.FindObjectOfType<DeadScreenView>();
      var planetView = Object.FindObjectOfType<PlanetView>();
      var gravityView = Object.FindObjectOfType<GravityView>();
      var gravityLittleView = Object.FindObjectOfType<GravityLittleView>();
      var playerModel = new PlayerModel(data.Player.startHealth, data.Player.missileCount);
      

      var inputInitialization = new InputInitialization(data.Input.minimalDistanceForSwipe);
      var stateController = new StateController(planetView, playerView, data, gravityView, gravityLittleView, inputInitialization.GetAllTouch(), camera);
      controllers.Add(new InputController(inputInitialization.GetAllTouch(), inputInitialization.GetSwipe()));
      controllers.Add(new CameraController(stateController, playerView.transform, camera.transform, planetView.transform, data, inputInitialization.GetSwipe()));
      controllers.Add(new PlayerMoveController(playerView, data, inputInitialization.GetAllTouch(), planetView,
         gravityLittleView, stateController));
      controllers.Add(new BonusController(playerModel, playerIndicatorView, bonusViews, BonusTypeValue(data)));
      controllers.Add(stateController);
   }

   private int[] BonusTypeValue(ScriptableData.AllData data)
   {
      return new [] {data.Bonus.goodBonus, data.Bonus.badBonus};
   }

}