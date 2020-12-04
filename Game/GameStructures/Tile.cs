using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using LibCarcassonne.GameStructures;
using LibCarcassonne.GameComponents;
using LibCarcassonne.GameLogic;
using LibCarcassonne.ArrayAccessMethods;

namespace LibCarcassonne
{
    namespace GameStructures
    {


        public class Tile
        {
            public TileComponent TileComponent { get; set; }
            public (int, int) TilePosition { get; set; }
            public GameBoard GameBoard { get; set; }


            public Tile(GameBoard gameBoard, TileComponent tileComponent, (int, int) tilePosition)
            {
                this.TileComponent = tileComponent.Clone();
                this.TilePosition = tilePosition;
                this.GameBoard = gameBoard;
            }


            public Tile Clone()
            {
                return new Tile(this);
            }


            public Tile(Tile another)
            {
                this.TileComponent = another.TileComponent.Clone();
                this.TilePosition = another.TilePosition;
                this.GameBoard = another.GameBoard;
            }


            /**
            * TileComponent.Matrix is rotated rotation times
            * rotatedMatrix is initialized once, to save space complexity
            */
            public void RotateTile(int rotation)
            {
                if (rotation == 0)
                {
                    return;
                }

                int[,] rotatedMatrix = new int[5, 5];

                var matrix = this.TileComponent.Matrix;
                for (var i = 0; i < rotation; ++i)
                {
                    rotatedMatrix = RotateSquareMatrix<int>(matrix, 5);
                    matrix = rotatedMatrix;
                }

                this.TileComponent.Matrix = rotatedMatrix;
            }


            /**
            * returns matrix rotated 90 degrees in variable rotatedMatrix
            */
            public T[,] RotateSquareMatrix<T>(T[,] matrix, int n)
            {
                T[,] rotatedMatrix = new T[n, n];

                for (var i = 0; i < n; ++i)
                {
                    for (var j = 0; j < n; ++j)
                    {
                        rotatedMatrix[i, j] = matrix[n - j - 1, i];
                    }
                }

                return rotatedMatrix;
            }


            /**
            * returns True if able to be neighbors, False otherwise
            * position = 0 => another's last line == this first line
            * position = 1 => another's first column == this last column
            * position = 2 => another's first line == this last line
            * position = 3 => another's last column == this first column
            */
            public bool CanBeNeighbors(Tile another, int position)
            {

                var testParam1 = this.GetLineOrColumn<string>(position);
                position = (position + 2) % 4;

                var testParam2 = another.GetLineOrColumn<string>(position);
                return this.AreBordersSimilar<string>(testParam1, testParam2);
            }


            /**
            * returns first/last line or first/last column depending on position
            * position = 0 => first line
            * position = 1 => last column
            * position = 2 => last line
            * position = 3 => first column
            */
            public T[] GetLineOrColumn<T>(int position)
            {
                var matrix = this.TileComponent.GetCharacteristicMatrix<T>();

                var x = new CustomArray<T>();
                if (position == 0)
                {
                    return x.GetRow(matrix, 0);
                }
                if (position == 1)
                {
                    return x.GetColumn(matrix, 4);
                }
                if (position == 2)
                {
                    return x.GetRow(matrix, 4);
                }
                if (position == 3)
                {
                    return x.GetColumn(matrix, 0);
                }
                throw new Exception("eroare: position nu e bun");
            }


            /**
            * checks borders except corners to be equal, returns true otherwise false
            */
            public bool AreBordersSimilar<T>(T[] param1, T[] param2)
            {
                if (param1.Length != param2.Length)
                {
                    throw new Exception("eroare: vectori nu sunt egali");
                }
                for (var i = 1; i < param1.Length - 1; ++i)
                {
                    if (!param1[i].Equals(param2[i]))
                    {
                        return false;
                    }
                }
                return true;
            }


            /**
             * created new function to get line or column with more suggestive meaning
             */
            public T[] GetBorderInPosition<T>(int position)
            {
                return this.GetLineOrColumn<T>(position);
            }


            /**
             * same as up but reversed position
             */
            public T[] GetBorderInReversedPosition<T>(int position)
            {
                position = (position + 2) % 4;
                return this.GetLineOrColumn<T>(position);
            }


            /**
            * method for counting neighbors in all eight directions
            */
            public int CountMonasteryNeighbors()
            {
                return this.GameBoard.CountMonasteryNeighbors(this.TilePosition);
            }


            /**
            * returns True if component with given Id is city and has shield, False otherwise
            */
            public bool ComponentHasShield(int componentId)
            {
                return this.TileComponent.ComponentHasShield(componentId);
            }


            /**
            * method for replacing ids for components when added to different game structures
            */
            public void ReplaceStructureIds(int idToSearch, int idToReplace)
            {
                this.TileComponent.ChangeComponentIds(idToSearch, idToReplace);
            }


            /**
            * method for a field to get a list of current ids of neighboring city structures
            */
            public List<int> GetNeighborCities(int fieldId)
            {
                return this.TileComponent.GetNeighborCities(fieldId);
            }



            /**
             * returns the list of tipes of tile component
             */
            public List<ComponentType> GetTileComponentStructures()
            {
                return this.TileComponent.Types;
            }


            public override string ToString()
            {
                return this.TileComponent.ToString();
            }


            public string PrintMatrix()
            {
                return this.TileComponent.PrintMatrix();
            }


        }


    }
}