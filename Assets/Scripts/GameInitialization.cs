using System.Collections.Generic;
using System.Linq;
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
      var playerView = Object.FindObjectOfType<ShipView>();
      var camera = Object.FindObjectOfType<Camera>();
      var missilePosition = data.MissileData.missilePosition;
      var playerIndicatorView = Object.FindObjectOfType<PlayerIndicatorView>();
      var deadView = Object.FindObjectOfType<DeadScreenView>();
      var planetView = Object.FindObjectOfType<PlanetView>();
      var gravityView = Object.FindObjectOfType<GravityView>();
      var gravityLittleView = Object.FindObjectOfType<GravityLittleView>();
      var playerModel = new PlayerModel(data.Player.startHealth, data.Player.missileCount);
      var rocketIndicatorViews = Object.FindObjectsOfType<RocketIndicatorView>();
      foreach (var rocketIndicatorView in rocketIndicatorViews)
      {
         rocketIndicatorView.TakeModelRef(playerModel, data.Player.missileCount);
      }
      var firstPersonView = Object.FindObjectOfType<FirstPersonView>();
      firstPersonView.gameObject.SetActive(false);
      var restartButtons = Object.FindObjectsOfType<RestartButtonView>();
      var finalScreenView = Object.FindObjectOfType<FinalScreenView>();
      finalScreenView.gameObject.SetActive(false);
      var positionGenerator = Object.FindObjectOfType<PositionGeneratorView>();
      
      var buildingController = new BuildingsController(data, planetView.transform, positionGenerator.transform);
      buildingController.CreateBuildings(planetView.transform);
      buildingController.GenerateBuildingsAroundPlanet();
      var buildingViews = Object.FindObjectsOfType<BuildingView>();

      var inputInitialization = new InputInitialization(data.Input.minimalDistanceForSwipe);
      var stateController = new StateController(planetView, playerView, data, gravityView, gravityLittleView, camera, 
         playerModel, deadView, firstPersonView, restartButtons, finalScreenView, rocketIndicatorViews);
      var playerMoveController = new PlayerMoveController(stateController, playerView, data, inputInitialization.GetAllTouch(),
         planetView, gravityLittleView, playerModel);
      var restartController = new RestartController(stateController);
      restartController.SaveObjects(buildingController.GetSpawnedBuildings);
      restartController.SaveObjects(planetView.GetComponentsInChildren<Transform>().ToList());
      controllers.Add(restartController);
      controllers.Add(new InputController(inputInitialization.GetAllTouch(), inputInitialization.GetSwipe()));
      controllers.Add(new CameraController(stateController, playerView.transform, camera.transform, planetView.transform, 
         data, inputInitialization.GetSwipe()));
      controllers.Add(new BonusController(stateController, playerModel, playerIndicatorView, buildingViews));
      controllers.Add(new PlayerHealthController(playerModel, playerMoveController, data.Player.multiplyDamageTake,
         data.Player.startDamageTake, data.Player.endDamageTake));
      controllers.Add(new PortalController(playerView.transform, planetView.transform, data, stateController));
      controllers.Add(new ShootController(inputInitialization.GetAllTouch(), camera, data, missilePosition,
         planetView.transform, stateController, playerModel));
      controllers.Add(playerMoveController);
      controllers.Add(stateController);
   }
}