using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GameComponents;

namespace GameLogic
{
  class GameRunner {
    public GameBoard GameBoard { get; set; }

    public List<Player> PlayerList { get; set; }

    public GameRunner(List<Player> playerList) {
      this.PlayerList = playerList;
      this.GameBoard = new GameBoard(); 
    }

    /**getPossiblePositions = getPossibleTransitions(AI)
    * @Tile currentTile => tile curent pentru care trebuie luata o decizie
    * @return List<Tuple<int, List<int>>>
    *   => o lista care contine tuple care contin:
    *     indexul pozitie din PossiblePositions care e compatibil cu tile-ul currentTile
    *     o lista de rotiri care poate fi ([0-3]+) (nu poate fi nula, are dim maxima = 4)
    *   => sau null, caz in care tile-ul currentTile nu poate fi pus nicaieri
    */
    public List<Tuple<int, List<int>>> GetPossiblePositions(Tile currentTile) {
      var returnList = new List<Tuple<int, List<int>>>();
      var isCompatible = false;
      Tile testTile;
      for (var i = 0; i < this.GameBoard.PossiblePositions.Count; i++) {
        isCompatible = false;
        var rotations = new List<int>();
        for (var rotation = 0; rotation < 4; rotation++) {
          testTile = this.GetClonedRotatedTile(currentTile, rotation);
          if (this.CheckTileCompatibility(testTile, this.GameBoard.PossiblePositions[i])) {
            isCompatible = true;
            rotations.Add(rotation);
          }
          // System.Console.WriteLine(testTile.ToString());  // testing rotation
        }
        if (isCompatible) {
          returnList.Add(new Tuple<int, List<int>>(i, rotations));
        }
      }
      if (returnList.Count == 0) {
        return null;
      }
      return returnList;
    }

    public bool CheckTileCompatibility(Tile testTile, (int, int) coordiantes) {
      //TODO de testat!!
      var neighborTiles = this.GameBoard.GetNeighboringTiles(coordiantes);
      if (neighborTiles == null) {
        //firsto tile jijitsu da, eien ni
        return true;
      }
      foreach (var obj in neighborTiles) {
        var tilePosition = obj.Item1;
        var tile = obj.Item2;
        var neighborCharacteristic = tile.GetReversedTileCharacteristic(tilePosition);
        if (neighborCharacteristic != testTile.GetTileCharacteristic(tilePosition)) {
          return false;
        }
      }
      return true;
    }

    public Orientation rotatePosition(Orientation position, int rotation) {
      if (position != Orientation.C) {
        position = (Orientation) (((int) position + rotation) % 4);
      }
      return position;
    }


    public Tile GetClonedRotatedTile(Tile currentTile, int rotation) {
      Tile cloneTile = (Tile)currentTile.Clone();
      if (rotation == 0) {
        return cloneTile;
      }
      foreach (var city in cloneTile.City) {
        for (var i = 0; i < city.Position.Count; i++) {
          city.Position[i] = rotatePosition(city.Position[i], rotation);
        }
      }
      foreach (var road in cloneTile.Road) {
        for (var i = 0; i < road.Count; i++) {
          road[i] = rotatePosition(road[i], rotation);
        }
      }
      cloneTile.Name += " rotated " + rotation.ToString();
      return cloneTile;
    }

    public string PossiblePositionsToString(List<Tuple<int, List<int>>> a) {
      var returnString = "";
      foreach (var i in a) {
        returnString += "PossiblePosition: " + this.GameBoard.PossiblePositions[i.Item1].ToString() + " PossiblePositionIndex: " + i.Item1.ToString() + "\t->\trotations: " + string.Join(",", i.Item2) + '\n';
      }
      return returnString;
    }

  }
}