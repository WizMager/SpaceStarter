using System;
using Controller;
using UnityEngine;
using Utils;
using View;
using Object = UnityEngine.Object;

public class GameInitialization
{
   private PlanetView[] _planets;
   private GravityView[] _gravities;

   public GameInitialization(Controllers controllers, Data data)
   {
      var player = Object.FindObjectOfType<PlayerView>();
      var camera = Object.FindObjectOfType<Camera>();
      _planets = Object.FindObjectsOfType<PlanetView>();
      _gravities = Object.FindObjectsOfType<GravityView>();
      SortPlanetObjects();
      
      var inputInitialization = new InputInitialization();
      controllers.Add(new InputController(inputInitialization.GetAllInput()));
      // controllers.Add(new CameraFirstPersonMoveController(inputInitialization.GetAxisInput(), cameras[1],
      //    planets[3].transform, data.Player.swipeSensitivity));
      // controllers.Add(new TapExplosionController(cameras[1], inputInitialization.GetTouchDown(), data.Player.explosionArea,
      //    data.Player.explosionForce, data.LastPlanet.explosionParticle));
      controllers.Add(new PlayerController(data, inputInitialization.GetTouchAll(), player, _planets, 
         _gravities, camera));
   }

   private void SortPlanetObjects()
   {
      var sortedPlanets = new PlanetView[_planets.Length];
      var sortedGravities = new GravityView[_gravities.Length];
      
      foreach (var planet in _planets)
      {
         switch (planet.number)
         {
            case ObjectNumber.First:
               sortedPlanets[0] = planet;
               break;
            case ObjectNumber.Second:
               sortedPlanets[1] = planet;
               break;
            case ObjectNumber.Third:
               sortedPlanets[2] = planet;
               break;
            default:
               throw new ArgumentOutOfRangeException($"Too much planet!");
         }
      }

      foreach (var gravity in _gravities)
      {
         switch (gravity.number)
         {
            case ObjectNumber.First:
               sortedGravities[0] = gravity;
               break;
            case ObjectNumber.Second:
               sortedGravities[1] = gravity;
               break;
            case ObjectNumber.Third:
               sortedGravities[2]= gravity;
               break;
            default:
               throw new ArgumentOutOfRangeException($"Too much gravities!");
         }
      }

      _planets = sortedPlanets;
      _gravities = sortedGravities;
   }
}