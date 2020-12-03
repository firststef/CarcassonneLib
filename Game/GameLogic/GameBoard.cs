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
    public List<Tile> PlacedTiles { get; set; }
    public List<(int, int)> FreePositions { get; set; }
    public int[] dx = {-1, 0, 1, 0};
    public int[] dy = {0, 1, 0, -1};


    public GameBoard() {
      this.TileMatrix = new Tile[144, 144];
      this.PlacedTiles = new List<Tile>();
      this.FreePositions = new List<(int, int)>();
      this.FreePositions.Add((72, 72));
    }


    /**
    * returns a list of coordinates of possible positions for current tile
    * or null if tile is unplaceable
    */
    public List<(int, int)> GetFreePositionsForTile(Tile tile) {
      var returnList = new List<(int, int)>();

      foreach (var freePosition in this.FreePositions) {
        if (this.CanPlaceTileInPosition(tile, freePosition)) {
          returnList.Add(freePosition);
        }
      }

      if (returnList.Count == 0) {
        return null;
      }
      return returnList;
    }


    /**
    * returns True if tile may be placed in position, False otherwise
    * checks tile to be compatible with all neighbors
    */
    public bool CanPlaceTileInPosition(Tile tile, (int, int) position) {
      var x = position.Item1;
      var y = position.Item2;
      
      if (x < 0 || x > 143 || y < 0 || y > 143) {
        throw new Exception("Position is incorrect");
      }

      for (var i = 0; i < 4; ++i) {
        if (! this.CheckNeighborInPosition(tile, (x + dx[i], y + dy[i]), i)) {
          return false;
        }
      }

      return true;
    }


    /**
    * return True if position is null or tile in position and direction is compatible
    * with current tile. False otherwise
    */
    public bool CheckNeighborInPosition(Tile tile, (int, int) position, int direction) {
      var neighborTile = this.TileMatrix[position.Item1, position.Item2];

      if (neighborTile == null) {
        return true;
      }
      
      return tile.CanBeNeighbors(neighborTile, direction);
    }


    /**
    * places tile in position and updates free positions accordingly
    */
    public void PlaceTileInPosition(Tile tile, (int, int) position) {
      if (! this.FreePositions.Contains(position)) {
        throw new Exception("position is not in free positions");
      }
      if (tile.TilePosition != (-1, -1)) {
        throw new Exception("tile-ul trebuia deja sa fie plasat undeva");
      }

      //TODO: de vazutr daca trebuie facut assert si la verificarea ca poate fi plasat aci

      tile.TilePosition = position;
      this.TileMatrix[position.Item1, position.Item2] = tile;
      this.PlacedTiles.Add(tile);

      this.UpdateFreePositions(position);
    }


    /**
    * removes position from free positions
    * updates free positions with all possible neighbors not in free positions
    */
    public void UpdateFreePositions((int, int) position) {
      this.FreePositions.Remove(position);
      var x = 0;
      var y = 0;

      for (var i = 0; i< 4; i++) {
        x = position.Item1 + dx[i];
        y = position.Item2 + dy[i];

        if (x < 0 || y < 0 || x > 143 || y > 143) {
          continue;
        }

        if (this.TileMatrix[x, y] == null && ! this.FreePositions.Contains((x, y))) {
          this.FreePositions.Add((x, y));
        }
      }
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