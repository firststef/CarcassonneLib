using System;
using System.Linq;
using System.Collections.Generic;

namespace LibCarcassonne {
namespace ArrayAccessMethods {
  public class CustomArray<T>
  {


    public T[] GetColumn(T[,] matrix, int columnNumber)
    {
      return Enumerable.Range(0, matrix.GetLength(0))
              .Select(x => matrix[x, columnNumber])
              .ToArray();
    }


    public T[] GetRow(T[,] matrix, int rowNumber)
    {
      return Enumerable.Range(0, matrix.GetLength(1))
              .Select(x => matrix[rowNumber, x])
              .ToArray();
    }


    public string PrintArray(T[,] matrix) {
      var returnString = "";
      for (var i = 0; i < matrix.GetLength(0); i++) {
        for (var j = 0; j < matrix.GetLength(1); j++) {
          returnString += matrix[i, j].ToString() + " ";
        }
        returnString += "\n";
      }
      return returnString;
    }


    public string PrintList(List<T> l) {
      if (l == null) {
        return "null";
      }
      var returnString = "[";
      foreach (var el in l) {
        returnString += el.ToString() + " ";
      }
      return returnString + "]";
    }
  }

  public class Utils<T> {
    public bool DoesListContain(List<T> list, T element) {
      return list.Contains(element);
    }
  }
}
}