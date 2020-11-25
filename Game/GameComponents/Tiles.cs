using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GameComponents;

namespace GameComponents
{
  public enum Orientation
  {
    [EnumMember(Value = "N")]
    N = 0,
    [EnumMember(Value = "E")]
    E = 1,
    [EnumMember(Value = "S")] 
    S = 2,
    [EnumMember(Value = "W")]
    W = 3, 
    [EnumMember(Value = "C")]
    C = 4
  }

	public class Tile
  {
    public string Name{ get; set; }
    public List<CityComp> City { get; set; }
    public List<List<Orientation>> Road {get; set;}

    public Tile(string name, List<CityComp> city, List<List<Orientation>> road)
    {
      this.Name = name;
      this.City = city;
      this.Road = road;
    }

    /**
    * road to string = null | [S, E],[N, C] | [E, V] | ..
    */
    public string RoadToString()
    {
      var returnString = "";
      if (this.Road == null){
        return "null";
      }
      foreach (var list in this.Road)
      {
        var roadString = "[";
        foreach (var str in list)
        {
          if (roadString == "[")
          {
            roadString += str.ToString();
          }
          else
          {
            roadString += "," + str.ToString();
          }
          
        }
        if (returnString == "") {
          returnString += roadString + "]";
        }
        else {
          returnString += "," + roadString + "]";
        }
      }
      return returnString;
    }

    /**
    * city to string = null | {city1}[, {city2}]
    */
    public string CityToString() {
      if (this.City == null){
        return "null";
      }
      string returnString = "";
      foreach(var city in City){
        if (returnString == "") {
          returnString += city.ToString();
        }
        else {
          returnString += "\n," + city.ToString();
        }
      }
      return returnString;
    }

    /**
    * tile to string = {name: string, city: {city1}[,{city2}], road = [roads]}
    */
    public override string ToString()
    {
      return "{\n" + $"\"name\": \"{this.Name}\"\n\"city\": {this.CityToString()}\n\"road\": {this.RoadToString()}" + "\n}";
    }
  }
  
}