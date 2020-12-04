using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using LibCarcassonne.GameComponents;
using LibCarcassonne.GameStructures;
using LibCarcassonne.GameLogic;
using LibCarcassonne.ArrayAccessMethods;

namespace LibCarcassonne
{
    namespace GameLogic
    {


        public class GameRunner
        {
            public GameBoard GameBoard { get; set; }
            public List<Tile> UnplayedTiles { get; set; }


            public GameRunner(List<TileComponent> tileComponents)
            {
                System.Console.WriteLine("GameRunner start");
                this.GameBoard = new GameBoard();
                this.UnplayedTiles = new List<Tile>();

                foreach (var tileComponent in tileComponents)
                {
                    this.UnplayedTiles.Add(new Tile(GameBoard, tileComponent, (-1, -1)));
                }
            }


            /**
            * Tile tile => tile to be placed for which to get possible free position
            * returns a list of possible coordinates for current tile, along with coresponding rotations 
            * or null if tile is unplaceble
            */
            public List<Tuple<(int, int), List<int>>> GetFreePositionsForTile(Tile tile)
            {
                return this.GameBoard.GetFreePositionsForTile(tile);
            }


            /**
            * returns first tile able to be played. removes it from unplayed tiles
            */
            public Tile GetCurrentRoundTile()
            {
                foreach (var tile in this.UnplayedTiles)
                {
                    if (this.GetFreePositionsForTile(tile) != null)
                    {
                        this.UnplayedTiles.Remove(tile);
                        return tile;
                    }
                }

                return null;
            }


            public void AddTileInPosition(Tile tile, (int, int) position)
            {
                this.GameBoard.PlaceTileInPosition(tile, position);
            }


            public void AddTileInPositionAndRotation(Tile tile, (int, int) position, int rotation)
            {
                var clonedTile = tile.Clone();
                clonedTile.RotateTile(rotation);
                this.AddTileInPosition(clonedTile, position);
            }



        }
    }
}