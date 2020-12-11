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


        public class GameBoard
        {
            public Tile[,] TileMatrix { get; set; }
            public List<Tile> PlacedTiles { get; set; }
            public List<(int, int)> FreePositions { get; set; }
            public List<GameStructure> GameStructures { get; set; }
            public int[] dx = { -1, 0, 1, 0 };
            public int[] dy = { 0, 1, 0, -1 };


            public GameBoard()
            {
                this.TileMatrix = new Tile[144, 144];
                this.PlacedTiles = new List<Tile>();
                this.FreePositions = new List<(int, int)>();
                this.FreePositions.Add((72, 72));
                this.GameStructures = new List<GameStructure>();
            }


            /**
             * returns a list of coordinates of possible positions and corresponding rotations for current tile
             * or null if tile is unplaceable
             */
            public List<Tuple<(int, int), List<int>>> GetFreePositionsForTile(Tile tile)
            {
                var returnList = new List<Tuple<(int, int), List<int>>>();

                foreach (var freePosition in this.FreePositions)
                {
                    var possibleRotations = this.GetPossibleRotationsForTileInPosition(tile, freePosition);
                    if (possibleRotations != null)
                    {
                        // we found at least a rotation
                        returnList.Add(new Tuple<(int, int), List<int>>(freePosition, possibleRotations));
                    }
                }

                if (returnList.Count == 0)
                {
                    return null;
                }
                return returnList;
            }


            /**
             * returns a list of rotations [0-3] for current tile compatible with position
             * or null if tile is unplaceable
             */
            public List<int> GetPossibleRotationsForTileInPosition(Tile tile, (int, int) position)
            {
                var returnList = new List<int>();

                for (var rotation = 0; rotation < 4; ++rotation)
                {
                    var clonedTile = tile.Clone();
                    clonedTile.RotateTile(rotation);

                    if (this.CanPlaceTileInPosition(clonedTile, position))
                    {
                        returnList.Add(rotation);
                    }
                }

                if (returnList.Count == 0)
                {
                    return null;
                }
                return returnList;
            }


            /**
             * returns list of non null neighboring tiles for tile
             */
            public List<Tuple<Tile, int>> GetTileNeighborsAndPosition(Tile tile)
            {
                var returnList = new List<Tuple<Tile, int>>();

                for (var i = 0; i < 4; ++i)
                {
                    var x = tile.TilePosition.Item1 + dx[i];
                    var y = tile.TilePosition.Item2 + dy[i];
                    var neighbor = this.TileMatrix[x, y];
                    if (neighbor != null)
                    {
                        returnList.Add(new Tuple<Tile, int>(neighbor, i));
                    }
                }

                if (returnList.Count == 0)
                {
                    return null;
                }
                return returnList;
            }



            /**
             * returns list of non null neighboring tiles and directionfor position 
             */
            public List<Tuple<Tile, int>> GetTileNeighborsAndPosition((int, int) position)
            {
                var returnList = new List<Tuple<Tile, int>>();
                int x = 0, y = 0;

                for (var i = 0; i < 4; ++i)
                {
                    x = position.Item1 + dx[i];
                    y = position.Item2 + dy[i];
                    var neighbor = this.TileMatrix[x, y];
                    if (neighbor != null)
                    {
                        returnList.Add(new Tuple<Tile, int>(neighbor, i));
                    }
                }

                if (returnList.Count == 0)
                {
                    return null;
                }
                return returnList;
            }



            /**
             * returns True if tile may be placed in position, False otherwise
             * checks tile to be compatible with all neighbors
             */
            public bool CanPlaceTileInPosition(Tile tile, (int, int) position)
            {
                var x = position.Item1;
                var y = position.Item2;

                if (x < 0 || x > 143 || y < 0 || y > 143)
                {
                    throw new Exception("Position is incorrect");
                }

                for (var i = 0; i < 4; ++i)
                {
                    if (!this.CheckNeighborInPosition(tile, (x + dx[i], y + dy[i]), i))
                    {
                        return false;
                    }
                }

                return true;
            }


            /**
             * return True if position is null or tile in position and direction is compatible
             * with current tile. False otherwise
             */
            public bool CheckNeighborInPosition(Tile tile, (int, int) position, int direction)
            {
                var neighborTile = this.TileMatrix[position.Item1, position.Item2];

                if (neighborTile == null)
                {
                    return true;
                }

                return tile.CanBeNeighbors(neighborTile, direction);
            }


            /**
             * places tile in position, updates structures and free positions accordingly
             */
            public void PlaceTileInPosition(Tile tile, (int, int) position)
            {
                if (!this.FreePositions.Contains(position))
                {
                    throw new Exception("position is not in free positions");
                }
                if (tile.TilePosition != (-1, -1))
                {
                    throw new Exception("tile-ul trebuia deja sa fie plasat undeva");
                }

                //TODO: de vazutara daca trebuie facut assert si la verificarea ca poate fi plasat aci

                tile.TilePosition = position;
                this.TileMatrix[position.Item1, position.Item2] = tile;
                this.PlacedTiles.Add(tile);

                this.UpdateStructures(tile);
                this.UpdateFreePositions(position);
            }


            /**
             * removes position from free positions
             * updates free positions with all possible neighbors not in free positions
             */
            public void UpdateFreePositions((int, int) position)
            {
                this.FreePositions.Remove(position);
                var x = 0;
                var y = 0;

                for (var i = 0; i < 4; i++)
                {
                    x = position.Item1 + dx[i];
                    y = position.Item2 + dy[i];

                    if (x < 0 || y < 0 || x > 143 || y > 143)
                    {
                        continue;
                    }

                    if (this.TileMatrix[x, y] == null && !this.FreePositions.Contains((x, y)))
                    {
                        this.FreePositions.Add((x, y));
                    }
                }
            }


            /**
             * oughts to update game structures
             */
            public void UpdateStructures(Tile tile)
            {
                System.Console.WriteLine(tile.TilePosition);
                var tileNeighborsAndPositions = this.GetTileNeighborsAndPosition(tile);

                if (tileNeighborsAndPositions == null)
                {
                    this.InitializeGameStructures(tile);
                } 
                else
                {
                    this.CheckNeigborStructures(tile, tileNeighborsAndPositions);
                }

            }



            /**
             * searches each neigboring tile and checks structure compatibility in order to add current tile to structure or join structures.
             * next initializes new structures for current tile components not included in neighborhood
             */
            public void CheckNeigborStructures(Tile tile, List<Tuple<Tile, int>> tileNeighborsAndPositions)
            {
                // se considera ca fiecare din tile-urile deja puse au structurile initializate 
                foreach (var neighbor in tileNeighborsAndPositions)
                {
                    this.ResolveStructureCompatibilityInPosition(tile, neighbor.Item1, neighbor.Item2);
                }
                this.InitializeGameStructures(tile);
            }



            /**
             * checks border between current tile and neighbor tile in position. Adds current tile components in border to neighbor GameStructures or joins GameStructures if needed
             */
            public void ResolveStructureCompatibilityInPosition(Tile currentTile, Tile neighborTile, int neighboringPosition)
            {
                var currentBorder = currentTile.GetBorderInPosition<int>(neighboringPosition);
                var neighborBorder = neighborTile.GetBorderInReversedPosition<int>(neighboringPosition);
                var currentBorderList = new List<int>();
                var neighborBorderList = new List<int>();

                for (var i = 1; i < currentBorder.Length - 1; ++i)
                {
                    if (! currentBorderList.Contains(currentBorder[i]))
                    {
                        currentBorderList.Add(currentBorder[i]);
                        neighborBorderList.Add(neighborBorder[i]);
                    }
                }

                for (var i = 0; i < currentBorderList.Count; ++i)
                {
                    if (currentBorderList[i] < 10)
                    {
                        // current tile component was not added previously to a game structure
                        var a = this.GetGameStructureWithId(neighborBorderList[i]);
                        a.AddTile(currentTile, currentBorderList[i]);
                    }
                    else
                    {
                        var gameStructure1 = this.GetGameStructureWithId(currentBorderList[i]);
                        var gameStructure2 = this.GetGameStructureWithId(neighborBorderList[i]);
                        if (gameStructure1.CanJoin(gameStructure2))
                        {
                            System.Console.WriteLine("URMEAZA GAME STRUCTURE JOIN");
                            this.GetGameStructureWithId(currentBorderList[i]).JoinStructures(this.GetGameStructureWithId(neighborBorderList[i]));
                        }
                    }
                }

            }


            /**
             * takes each distinct structure from tile and creates new structure on tile of type Type and encompassing matrix elements with id Id
             */
            public void InitializeGameStructures(Tile tile)
            {
                var tileComponentStructures = tile.GetTileComponentStructures();
                
                foreach (var component in tileComponentStructures)
                {
                    if (component.Id < 10)
                    {
                        this.CreateNewStructure(tile, component.Id, component.Type);
                    }
                }
            }



            /**
             * Adds new game structure to GameStructures and adds current tile to it
             */
            public void CreateNewStructure(Tile tile, int id, string type)
            {
                var gameStructure = this.CreateGameStructure(type);
                gameStructure.AddTile(tile, id);
                this.GameStructures.Add(gameStructure);
            }



            /**
             * id => id of previously created structure
             * returns structure with id or null if id not found
             */
            public GameStructure GetGameStructureWithId(int id)
            {
                foreach (var gameStructure in this.GameStructures)
                {
                    if (gameStructure.StructureId == id)
                    {
                        return gameStructure;
                    }
                }

                return null;
            }


            /**
             * returns a gamestructure derived class, City, Road, Field or Monastery
             */
            public GameStructure CreateGameStructure(string type)
            {
                if (type == "city")
                {
                    return new City(this);
                }
                if (type == "road")
                {
                    return new Road(this);
                }
                if (type == "field")
                {
                    return new Field(this);
                }
                if (type == "monastery")
                {
                    return new Monastery(this);
                }

                throw new Exception("trebuia sa fie una dintre cele 4 tipuri anterioare");
            }


            /**
             * removes game structure from game structures list after join
             */
            public void RemoveStructure(GameStructure gameStructure)
            {
                if (! this.GameStructures.Contains(gameStructure))
                {
                    throw new Exception("Nu exista structura cautata in game board");
                }
                this.GameStructures.Remove(gameStructure);
            }


            /**
             * returns count of tiles not null in eight directions of position
             */
            public int CountMonasteryNeighbors((int, int) position)
            {
                int[] ldx = { -1, -1, 0, 1, 1, 1, 0, -1 };
                int[] ldy = { 0, 1, 1, 1, 0, -1, -1, -1 };

                var x = 0;
                var y = 0;
                var count = 0;
                for (var i = 0; i < ldx.Length; ++i)
                {
                    x = position.Item1 + ldx[i];
                    y = position.Item2 + ldy[i];
                    if (x < 0 || y < 0 || x > 143 || y > 143)
                    {
                        continue;
                    }
                    if (this.TileMatrix[x, y] != null)
                    {
                        count++;
                    }
                }
                return count;
            }


            public override string ToString()
            {
                var util = new CustomArray<string>();
                var rowGetter = new CustomArray<int>();
                var coordinates = this.GetExtremeCoordinates();
                var returnString = "";

                for (var x = coordinates[0]; x <= coordinates[1]; ++x)
                {
                    var lines = util.CreateList("", "", "", "", "");

                    for (var y = coordinates[2]; y <= coordinates[3]; ++y)
                    {
                        if (this.TileMatrix[x, y] != null)
                        {
                            var matrix = this.TileMatrix[x, y].TileComponent.Matrix;
                            for (var row = 0; row < 5; ++row)
                            {
                                lines[row] += string.Join(" ", rowGetter.GetRow(matrix, row)) + "\t";
                            }
                        }
                        else
                        {
                            for (var row = 0; row < 5; ++row)
                            {
                                lines[row] += "        \t"; // this should stand for each empty space
                            }
                        }
                    }

                    foreach (var line in lines)
                    {
                        returnString += line + "\n";
                    }
                }
                return returnString;
            }


            /**
             * returns a list of min x, max x, min y, max y
             */
            public List<int> GetExtremeCoordinates()
            {
                var util = new CustomArray<int>();
                var coordinates = util.CreateList(144, 0, 144, 0);

                foreach (var tile in this.PlacedTiles)
                {
                    var x = tile.TilePosition.Item1;
                    var y = tile.TilePosition.Item2;

                    if (x < coordinates[0])
                    {
                        coordinates[0] = x;
                    }
                    if (x > coordinates[1])
                    {
                        coordinates[1] = x;
                    }
                    if (y < coordinates[2])
                    {
                        coordinates[2] = y;
                    }
                    if (y > coordinates[3])
                    {
                        coordinates[3] = y;
                    }
                }

                return coordinates;
            }

        }
    }
}