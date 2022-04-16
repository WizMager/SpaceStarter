using Controllers;
using InputClasses;
using Model;
using UnityEngine;
using UnityEngine.UI;
using View;
using Object = UnityEngine.Object;

public class GameInitialization
{
   public GameInitialization(AllControllers controllers, ScriptableData.AllData data)
   {
      var playerView = Object.FindObjectOfType<PlayerView>();
      var camera = Object.FindObjectOfType<Camera>();
      var missilePosition = camera.transform.GetChild(0).transform;
      var playerIndicatorView = Object.FindObjectOfType<PlayerIndicatorView>();
      var deadView = Object.FindObjectOfType<DeadScreenView>();
      var planetView = Object.FindObjectOfType<PlanetView>();
      var gravityView = Object.FindObjectOfType<GravityView>();
      var gravityLittleView = Object.FindObjectOfType<GravityLittleView>();
      var playerModel = new PlayerModel(data.Player.startHealth, data.Player.missileCount);
      var buildingViews = Object.FindObjectsOfType<BuildingView>();
      var rocketIndicatorViews = Object.FindObjectsOfType<RocketIndicatorView>();
      foreach (var rocketIndicatorView in rocketIndicatorViews)
      {
         rocketIndicatorView.TakeModelRef(playerModel, data.Player.missileCount);
      }
      var firstPersonView = Object.FindObjectOfType<FirstPersonView>();
      firstPersonView.gameObject.SetActive(false);
      var restartButton = Object.FindObjectOfType<RestartButtonView>().GetComponent<Button>();
      var finalScreenView = Object.FindObjectOfType<FinalScreenView>();
      finalScreenView.gameObject.SetActive(false);
      
      var buildingController = new BuildingsController(data, planetView.transform);
      buildingController.CreateBuildings(planetView.transform);

      var inputInitialization = new InputInitialization(data.Input.minimalDistanceForSwipe);
      var stateController = new StateController(planetView, playerView, data, gravityView, gravityLittleView, camera, 
         playerModel, deadView, firstPersonView, restartButton, finalScreenView);
      var playerMoveController = new PlayerMoveController(playerView, data, inputInitialization.GetAllTouch(),
         planetView, gravityLittleView, stateController);
      controllers.Add(new InputController(inputInitialization.GetAllTouch(), inputInitialization.GetSwipe()));
      controllers.Add(new CameraController(stateController, playerView.transform, camera.transform, planetView.transform, 
         data, inputInitialization.GetSwipe()));
      controllers.Add(new BonusController(stateController, playerModel, playerIndicatorView, BonusTypeValue(data),buildingViews));
      controllers.Add(new PlayerHealthController(playerModel, playerMoveController, data.Player.multiplyDamageTake,
         data.Player.startDamageTake, data.Player.endDamageTake));
      controllers.Add(new PortalController(playerView.transform, planetView.transform, data, stateController));
      controllers.Add(new ShootController(inputInitialization.GetAllTouch(), camera, data, missilePosition,
         planetView.transform, stateController, playerModel));
      controllers.Add(playerMoveController);
      controllers.Add(stateController);
   }

   private int[] BonusTypeValue(ScriptableData.AllData data)
   {
      return new [] {data.Bonus.goodBonus, data.Bonus.badBonus};
   }

}