using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GameComponents;
using GameStructures;
using GameLogic;
using ArrayAccessMethods;


namespace GameLogic {


  public class GameBoard {
    public Tile[,] TileMatrix { get; set; }


    public GameBoard() {
      this.TileMatrix = new Tile[144, 144];
    }


    /**
    * returns count of tiles not null in eight directions of position
    */
    public int CountMonasteryNeighbors((int, int) position) {
      int[] ldx = {-1, -1, 0, 1, 1, 1, 0, -1};
      int[] ldy = {0, 1, 1, 1, 0, -1, -1, -1};

      var x = 0;
      var y = 0;
      var count = 0;
      for (var i = 0; i < ldx.Length; ++i) {
        x = position.Item1 + ldx[i];
        y = position.Item2 + ldy[i];
        if (x < 0 || y < 0 || x > 143 || y > 143) {
          continue;
        }
        if (this.TileMatrix[x, y] != null) {
          count++;
        }
      }
      return count;
    }
  }
}