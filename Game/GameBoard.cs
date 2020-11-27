using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GameComponents;
using ArrayAccessMethods;

namespace GameLogic
{

  class GameBoard {
    public Tile[,] PlacedTiles { get; set; }
    public List<(int, int)> PossiblePositions { get; set; }
    public int[] dx = new int[] {-1, 0, 1, 0};
    public int[] dy = new int[] {0, 1, 0, -1};

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
    * should tile be placed in (x, y), all (x, y)'s neighbors that are null and not in 
    * PossiblePositions are added to PossiblePositions
    */
    public void AppendPossiblePositions(int x, int y) {
      for (var i = 0; i < this.dx.Length; i++) {
        if (PlacedTiles[x + dx[i], y + dy[i]] == null && ! (PossiblePositions.Contains((x + dx[i], y + dy[i])))) {
          PossiblePositions.Add((x + dx[i], y + dy[i]));
        }
      }
    }

    /**
    * coordinates = (x, y)
    * returns all tiles neaby coordinates in format: (position, tile), or null
    * where position is [0-3] standing for [N-W] and tile is not null
    */
    public List<Tuple<int, Tile>> GetNeighboringTiles((int, int) coordinates) {
      var returnList = new List<Tuple<int, Tile>>();
      var x = coordinates.Item1;
      var y = coordinates.Item2;
      for (var i = 0; i < this.dx.Length; i++) {
        if (PlacedTiles[x + this.dx[i], y+ this.dy[i]] != null) {
          returnList.Add(new Tuple<int, Tile>(i, PlacedTiles[x + this.dx[i], y+ this.dy[i]]));
        }
      }
      if (returnList.Count == 0) {
        return null;
      }
      return returnList;
    }


    public string GameBoardToString() {
      var tileArray = new CustomArray<Tile>();
      var tilesOnBoard = new List<List<Tile>>();
      //oof
      int minRow = 144, maxRow = 0, minCol = 144, maxCol = 0;
      for (var i = 0; i < this.PlacedTiles.GetLength(0); i++) {
        for (var j = 0; j < this.PlacedTiles.GetLength(1); j++) {
          if (this.PlacedTiles[i, j] != null) {
            if (i < minRow) {
              minRow = i;
            }
            if (i > maxRow) {
              maxRow = i;
            }
            if (j < minCol) {
              minCol = j;
            }
            if (j > maxCol) {
              maxCol = j;
            }
          }
        }
      }
      for (var i = minRow; i <= maxRow; i++) {
        var row = new List<Tile>();
        for (var j = minCol; j <= maxCol; j++) {
          row.Add(this.PlacedTiles[i, j]);
        }
        tilesOnBoard.Add(row);
      }
      var returnString = "";
      foreach (var row in tilesOnBoard) {
        var rowString1 = "";
        var rowString2 = "";
        var rowString3 = "";
        foreach (var tile in row) {
          if (tile == null) {
            rowString1 += "\t\tnull\t\t";
            rowString2 += "\tnull\tnull\t";
            rowString3 += "\t\tnull\t\t";
          }
          else {
            var tileCharacteristics = tile.CharactericsToString();
            rowString1 += "\t\t" + tileCharacteristics[0] + "\t\t";
            rowString2 += "\t" + tileCharacteristics[1] + "\t" + tileCharacteristics[3] + "\t";
            rowString3 += "\t\t" + tileCharacteristics[2] + "\t\t";
          }
          
        }
        returnString += rowString1 + "\n" + rowString2 + '\n' + rowString3 + '\n';
      }
      return returnString;
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
