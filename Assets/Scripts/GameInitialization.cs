using System;
using Controller;
using Data;
using DefaultNamespace;
using UnityEngine;
using Utils;
using View;
using Object = UnityEngine.Object;

public class GameInitialization
{
   private PlanetView[] _planets;
   private GravityView[] _gravities;

   public GameInitialization(Controllers controllers, ScriptableData data)
   {
      var player = Object.FindObjectOfType<PlayerView>();
      var camera = Object.FindObjectOfType<Camera>();
      _planets = Object.FindObjectsOfType<PlanetView>();
      _gravities = Object.FindObjectsOfType<GravityView>();
      SortPlanetObjects();

      var inputInitialization = new InputInitialization();
      controllers.Add(new InputController(inputInitialization.GetAllInput()));
      // controllers.Add(new PlayerController(data, inputInitialization.GetTouchAll(), player, _planets, 
      //    _gravities, camera, inputInitialization.GetAxis()));
      controllers.Add(new StateContext(new AimNextPlanetState(), data, player,
         inputInitialization.GetTouchAll(),
         inputInitialization.GetAxis(), _planets, _gravities, camera));
   }

   private void SortPlanetObjects()
   {
      var sortedPlanets = new PlanetView[_planets.Length];
      var sortedGravities = new GravityView[_gravities.Length];
      
      foreach (var planet in _planets)
      {
         switch (planet.number)
         {
            case ObjectNumber.NotPlanet:
               sortedPlanets[0] = planet;
               break;
            case ObjectNumber.First:
               sortedPlanets[1] = planet;
               break;
            case ObjectNumber.Second:
               sortedPlanets[2] = planet;
               break;
            case ObjectNumber.Third:
               sortedPlanets[3] = planet;
               break;
            case ObjectNumber.Last:
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
            case ObjectNumber.NotPlanet:
               sortedGravities[0] = gravity;
               break;
            case ObjectNumber.First:
               sortedGravities[1] = gravity;
               break;
            case ObjectNumber.Second:
               sortedGravities[2] = gravity;
               break;
            case ObjectNumber.Third:
               sortedGravities[3]= gravity;
               break;
            case ObjectNumber.Last:
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