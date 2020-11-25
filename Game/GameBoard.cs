using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GameComponents;

namespace GameLogic
{
  class GameBoard {
    public Tile[,] PlacedTiles { get; set; }
    public List<(int, int)> PossiblePositions { get; set; }

    /**GameBoard = getInitialState(AI)
    * 
    */
    public GameBoard() {
      //poate nu trebuie hardcodat desi meh
      this.PlacedTiles = new Tile[144, 144];
      this.PossiblePositions = new List<(int, int)>();
      //prima pozitie accesibila e cea din mijloc
      this.PossiblePositions.Add((72, 72));
    }


    /**
    * game logic to string = placed tiles to string = "int x null [[, tile_name, int x null]]"
    * ex: ,70 x null,tile13,73 x null \n ,144 x null
    */
    public override string ToString(){
      int nullCount = 0;

      List<List<string>> tileData = new List<List<string>>();
      for (var i = 0; i < this.PlacedTiles.GetLength(0); i++) {
        tileData.Add(new List<string>());
        nullCount = 0;
        for (var j = 0; j < this.PlacedTiles.GetLength(1); j++) {
          if (this.PlacedTiles[i, j] == null) {
            nullCount++;
          }
          else {
            tileData[i].Add($"{nullCount} x null");
            tileData[i].Add(this.PlacedTiles[i,j].Name);
            nullCount = 0;
          }
        }
        tileData[i].Add($"{nullCount} x null");
      }

      string returnString = "";
      foreach (var list in tileData)
      {
        foreach (var str in list)
        {
          if (returnString == "")
          {
            returnString += str;
          }
          else
          {
            returnString += "," + str;
          }
        }
        returnString += "\n";
      }
      return returnString;

    }
    
  }
}
