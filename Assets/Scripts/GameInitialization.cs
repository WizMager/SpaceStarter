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
   private GravityEnterView[] _gravityEnters;

   public GameInitialization(Controllers controllers, ScriptableData.ScriptableData data)
   {
      var player = Object.FindObjectOfType<PlayerView>();
      var camera = Object.FindObjectOfType<Camera>();
      var cameraColliderView = Object.FindObjectOfType<CameraColliderView>();
      var playerIndicatorView = Object.FindObjectOfType<PlayerIndicatorView>();
      var bonusViews = Object.FindObjectsOfType<BonusView>();
      var deadView = Object.FindObjectOfType<DeadScreenView>();
      var deadZoneViews = Object.FindObjectsOfType<DeadZoneView>();
      _planets = Object.FindObjectsOfType<PlanetView>();
      _gravities = Object.FindObjectsOfType<GravityView>();
      _gravityEnters = Object.FindObjectsOfType<GravityEnterView>();
      SortAll();
      var playerModel = new PlayerModel(data.Player.startHealth, data.Player.missileCount);
      

      var inputInitialization = new InputInitialization(data.Input.minimalDistanceForSwipe);
      controllers.Add(new InputController(inputInitialization.GetAllTouch(), inputInitialization.GetSwipe()));
      controllers.Add(new PlayerController(data, player, inputInitialization.GetAllTouch(),
         inputInitialization.GetSwipe(), _planets, _gravities, _gravityEnters, camera, cameraColliderView, playerModel, 
         deadView, deadZoneViews));
      controllers.Add(new BonusController(playerModel, playerIndicatorView, bonusViews, BonusTypeValue(data)));
   }

   private int[] BonusTypeValue(ScriptableData.ScriptableData data)
   {
      return new [] {data.Bonus.goodBonus, data.Bonus.badBonus};
   }

   private void SortAll()
   {
      SortPlanetViews();
      SortGravityViews();
      SortGravityEnterViews();
   }
   
   private void SortPlanetViews()
   {
      var sortedPlanets = new PlanetView[_planets.Length];
      
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

      _planets = sortedPlanets;
   }

   private void SortGravityViews()
   {
      var sortedGravities = new GravityView[_gravities.Length];
      
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
      
      _gravities = sortedGravities;
   }

   private void SortGravityEnterViews()
   {
      var sortedGravityEnters = new GravityEnterView[_gravityEnters.Length];
      
      foreach (var gravityEnter in _gravityEnters)
      {
         switch (gravityEnter.number)
         {
            case PlanetNumber.NotPlanet:
               sortedGravityEnters[0] = gravityEnter;
               break;
            case PlanetNumber.First:
               sortedGravityEnters[1] = gravityEnter;
               break;
            case PlanetNumber.Second:
               sortedGravityEnters[2] = gravityEnter;
               break;
            case PlanetNumber.Third:
               sortedGravityEnters[3] = gravityEnter;
               break;
            case PlanetNumber.Last:
               sortedGravityEnters[4] = gravityEnter;
               break;
            default:
               throw new ArgumentOutOfRangeException($"Too much gravityEnters!");
         }
      }
      
      _gravityEnters = sortedGravityEnters;
   }
}