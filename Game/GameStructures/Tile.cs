using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GameStructures;
using GameComponents;
using ArrayAccessMethods;


namespace GameStructures {


  public class Tile {
    public TileComponent TileComponent { get; set; }
    public (int, int) TilePosition { get; set; }


    public Tile(TileComponent tileComponent) {
      this.TileComponent = tileComponent.Clone();
      this.TilePosition = (10, 12);
    }


    public Tile Clone() {
      return new Tile(this);
    }


    public Tile(Tile another) {
      this.TileComponent = another.TileComponent.Clone();
      this.TilePosition = another.TilePosition;
    }


    /**
    * TileComponent.Matrix is rotated rotation times
    * rotatedMatrix is initialized once, to save space complexity
    */
    public void RotateTile(int rotation) {
      int[,] rotatedMatrix = new int[5, 5];

      var matrix = this.TileComponent.Matrix;
      for (var i = 0; i < rotation; ++i) {
        matrix = RotateSquareMatrix<int>(matrix, 5, rotatedMatrix);
      }

      this.TileComponent.Matrix = matrix;
    }


    /**
    * returns matrix rotated 90 degrees in variable rotatedMatrix
    */
    public T[,] RotateSquareMatrix<T>(T[,] matrix, int n, T[,] rotatedMatrix) {

      for (var i = 0; i < n; ++i) {
        for (var j = 0; j < n; ++j) {
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
    public bool CanBeNeighbors(Tile another, int position) {
      
      var testParam1 = this.GetLineOrColumn(position);
      position = (position + 2) % 4;

      var testParam2 = another.GetLineOrColumn(position);
      return this.AreBordersSimilar<string>(testParam1, testParam2);
    }


    /**
    * returns first/last line or first/last column depending on position
    * position = 0 => first line
    * position = 1 => last column
    * position = 2 => last line
    * position = 3 => first column
    */
    public string[] GetLineOrColumn(int position) {
      var matrix = this.TileComponent.GetCharacteristicMatrix();

      var x = new CustomArray<string>();
      if (position == 0) {
        return x.GetRow(matrix, 0);
      }
      if (position == 1) {
        return x.GetColumn(matrix, 4);
      }
      if (position == 2) {
        return x.GetRow(matrix, 4);
      }
      if (position == 3) {
        return x.GetColumn(matrix, 0);
      }
      throw new Exception ("eroare: position nu e bun");
    }


    /**
    * checks borders except corners to be equal, returns true otherwise false
    */
    public bool AreBordersSimilar<T>(T[] param1, T[] param2) {
      if (param1.Length != param2.Length) {
        throw new Exception ("eroare: vectori nu sunt egali");
      }
      for (var i = 1; i < param1.Length - 1; ++i) {
        if (! param1[i].Equals(param2[i])) {
          return false;
        }
      }
      return true;
    }


  }


}