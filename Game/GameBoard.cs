using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GameComponents;

namespace GameLogic
{
  class GameBoard {
    public Tile[,] PlacedTiles { get; set; }

    public GameBoard() {
      this.PlacedTiles = new Tile[144, 144];
    }

    public override string ToString(){
      int nullCount = 0;

      List<List<string>> tileData = new List<List<string>>();
      for (var i = 0; i < this.PlacedTiles.GetLength(0); i++) {
        tileData.Add(new List<string>());
        nullCount = 0;
        for (var j = 0; j < this.PlacedTiles.GetLength(1); j++) {
          if (this.PlacedTiles[i, j] == null){
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
