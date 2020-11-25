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
  }
}
