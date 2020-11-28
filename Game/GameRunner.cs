using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GameComponents;

namespace GameLogic
{
  class GameRunner {
    public GameBoard GameBoard { get; set; }

    public List<Tile> ShuffledTiles { get; set; }

    public bool EndGame { get; set; }

    public List<Player> PlayerList { get; set; }

    public GameRunner(List<Player> playerList, List<Tile> tileList) {
      var rnd = new Random();
      this.ShuffledTiles = tileList.OrderBy(a => rnd.Next()).ToList();
      this.EndGame = false;
      this.PlayerList = playerList;
      this.GameBoard = new GameBoard(); 
    }

    public void PrepareGameEnd() {
      this.EndGame = true;
    }

    /**
    * currentTile => the tile in the hand of current playerList
    * possiblePositions => the possible positions for currentTile to be played
    *                   => consists of a lists of indexes from GameBoard.PossiblePositions
    *                   => and for each index the possible rotations for the tile
    * 
    * prints all possible positions, along with rotations able to be made
    * awaits valid user input, x, y and rotation. Places tile in selected position
    */
    public void PlayRound(Tile currentTile, List<Tuple<int, List<int>>> possiblePositions) {
      var tileString = currentTile.CharactericsToString();
      var printString = $"\t\t{tileString[0]}\t\t\n\t{tileString[3]}\t{tileString[1]}\n\t\t{tileString[2]}\t\t\n";
      System.Console.WriteLine(printString);
      foreach (var tuple in possiblePositions) {
        System.Console.WriteLine($"Pozitie disponibila:{this.GameBoard.PossiblePositions[tuple.Item1]}");
        System.Console.WriteLine($"Rotatii disponibile pentru pozitie:{string.Join(",", tuple.Item2)}\n\n");
      }
      System.Console.WriteLine($"Prezumtia e ca se introduc date corecte");
      System.Console.WriteLine($"Alege o coordonata x valida");
      var x = Int32.Parse(Console.ReadLine());
      System.Console.WriteLine($"Alege o coordonata y valida pentru coordonata x aleasa");
      var y = Int32.Parse(Console.ReadLine());
      System.Console.WriteLine($"Alege o rotatie disponibila pentru coordonatele x, y alese");
      var rotation = Int32.Parse(Console.ReadLine());
      this.GameBoard.PlaceTile(x, y, this.GetClonedRotatedTile(currentTile, rotation));
      System.Console.WriteLine("GameBoard:\n" + this.GameBoard.GameBoardToString() + "\n\n\n");

      // Environment.Exit(0);
    }

    /**
    * takes the first tile which can be played, removes it and calls PlayRound 
    * if no tile is found, EndGame is triggered
    * 
    * TODO: de discutat despre designul acestei functii, nu prea is incantat de ea
    */
    public void PrepareRound() {
      
      List<Tuple<int, List<int>>> possiblePositions = null;
      Tile currentTile = null;
      var i = 0;
      for (; i < ShuffledTiles.Count; i++) {
        currentTile = ShuffledTiles[i];
        // System.Console.WriteLine(currentTile.Name);
        possiblePositions = this.GetPossiblePositions(currentTile);
        if (possiblePositions != null) {
          break;
        }
      }
      if (possiblePositions == null) {
        this.PrepareGameEnd();
      } else {
        this.ShuffledTiles.RemoveAt(i);
        this.PlayRound(currentTile, possiblePositions);
      }
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

    /**
    * checks that tile 1 is compatible in position with tile 2
    */
    public bool AreCompatible(Tile tile1, Tile tile2, int position) {
      return (tile1.GetReversedTileCharacteristic(position) == 
      tile2.GetTileCharacteristic(position));
    }

    /**
    * testTile => tile to be tested
    * coordiantes => (x, y), position for tile to be placed
    * returns true, if tile may me added to coordinates, false otherwise
    */
    public bool CheckTileCompatibility(Tile testTile, (int, int) coordiantes) {
      var neighborTiles = this.GameBoard.GetNeighboringTiles(coordiantes);
      if (neighborTiles == null) {
        //firsto tile jijitsu da, eien ni
        return true;
      }
      foreach (var obj in neighborTiles) {
        var tilePosition = obj.Item1;
        var tile = obj.Item2;
        if (! this.AreCompatible(tile, testTile, tilePosition)) {
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
      if (cloneTile.City != null) {
        foreach (var city in cloneTile.City) {
          for (var i = 0; i < city.Position.Count; i++) {
            city.Position[i] = rotatePosition(city.Position[i], rotation);
          }
        }
      }
      if (cloneTile.Road != null) {
        foreach (var road in cloneTile.Road) {
          for (var i = 0; i < road.Count; i++) {
            road[i] = rotatePosition(road[i], rotation);
          }
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