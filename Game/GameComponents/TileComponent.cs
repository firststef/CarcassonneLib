using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GameComponents;
using ArrayAccessMethods;


namespace GameComponents {


    public class TileComponent {
    public string Name { get; set; }
    public int[,] Matrix { get; set; }
    public List<ComponentType> Types { get; set; }
    

    public TileComponent(string name, int[,] matrix, List<ComponentType> types) {
      this.Name = name;
      this.Matrix = matrix;
      this.Types = types;
    }


    public override string ToString() {
      return "{\n" + $"\"name\": {this.Name}\n\"matrix\": \n{this.PrintMatrix()}\n\"types\": {this.PrintTypes()}" + "\n}";
    }


    public string PrintMatrix() {
      var x = new CustomArray<int>();
      return x.PrintArray(this.Matrix);
    }


    public string PrintTypes() {
      var returnString = "";
      foreach (var t in this.Types) {
        if (returnString == "") {
          returnString += "[\n" + t.ToString();
        } else {
          returnString += ", \n" + t.ToString();
        }
      }
      return returnString + "\n]";

    }
  }


  public class ComponentType {
    public int Id { get; set; }
    public string Type { get; set; }
    public List<int> Neighbors { get; set; }
    public List<float> Center { get; set; }


    public ComponentType(int id, string type, List<int> neighbors, List<float> center) {
      this.Id = id;
      this.Type = type;
      this.Neighbors = neighbors;
      this.Center = center;
    }


    public override string ToString() {
      return $"{{\n\"id\": {this.Id}\n\"center\": {this.PrintList<float>(this.Center)}\n\"type\": {this.Type}\n\"neighbors\": {this.PrintList<int>(this.Neighbors)}\n}}";
    }


    public string PrintList<T>(List<T> l) {
      var x = new CustomArray<T>();
      return x.PrintList(l);
    }


  }


}