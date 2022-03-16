using System;
using Controller;
using InputClasses;
using Model;
using UnityEngine;
using Utils;
using View;
using Object = UnityEngine.Object;

public class GameInitialization
{
   private PlanetView[] _planets;
   private GravityView[] _gravities;

   public GameInitialization(Controllers controllers, ScriptableData.ScriptableData data)
   {
      var player = Object.FindObjectOfType<PlayerView>();
      var camera = Object.FindObjectOfType<Camera>();
      var cameraColliderView = Object.FindObjectOfType<CameraColliderView>();
      var playerHealthView = Object.FindObjectOfType<PlayerHealthView>();
      var playerBonusView = Object.FindObjectOfType<PlayerBonusView>();
      var bonusViews = Object.FindObjectsOfType<BonusView>();
      var deadView = Object.FindObjectOfType<DeadScreenView>();
      _planets = Object.FindObjectsOfType<PlanetView>();
      _gravities = Object.FindObjectsOfType<GravityView>();
      SortPlanetObjects();
      var playerModel = new PlayerModel(data.Player.startHealth, data.Player.missileCount,
         BonusTypeValue(data));
      

      var inputInitialization = new InputInitialization(data.Input.minimalDistanceForSwipe);
      controllers.Add(new InputController(inputInitialization.GetAllTouch(), inputInitialization.GetSwipe()));
      controllers.Add(new PlayerController(data, player, inputInitialization.GetAllTouch(),
         inputInitialization.GetSwipe(), _planets, _gravities, camera, cameraColliderView, playerModel, deadView));
      controllers.Add(new BonusController(playerModel, playerHealthView, playerBonusView, bonusViews));
   }

   private int[] BonusTypeValue(ScriptableData.ScriptableData data)
   {
      return new [] {data.Bonus.goodBonus, data.Bonus.badBonus};
   }
   
   private void SortPlanetObjects()
   {
      var sortedPlanets = new PlanetView[_planets.Length];
      var sortedGravities = new GravityView[_gravities.Length];
      
      foreach (var planet in _planets)
      {
         switch (planet.number)
         {
            case PlanetNumber.NotPlanet:
               sortedPlanets[0] = planet;
               break;
            case PlanetNumber.First:
               sortedPlanets[1] = planet;
               break;
            case PlanetNumber.Second:
               sortedPlanets[2] = planet;
               break;
            case PlanetNumber.Third:
               sortedPlanets[3] = planet;
               break;
            case PlanetNumber.Last:
               sortedPlanets[4] = planet;
               break;
            default:
               throw new ArgumentOutOfRangeException($"Too much planet!");
         }
      }

      foreach (var gravity in _gravities)
      {
         switch (gravity.number)
         {
            case PlanetNumber.NotPlanet:
               sortedGravities[0] = gravity;
               break;
            case PlanetNumber.First:
               sortedGravities[1] = gravity;
               break;
            case PlanetNumber.Second:
               sortedGravities[2] = gravity;
               break;
            case PlanetNumber.Third:
               sortedGravities[3]= gravity;
               break;
            case PlanetNumber.Last:
               sortedGravities[4] = gravity;
               break;
            default:
               throw new ArgumentOutOfRangeException($"Too much gravities!");
         }
      }

      _planets = sortedPlanets;
      _gravities = sortedGravities;
   }
}