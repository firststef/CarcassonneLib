using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

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



	public class CityComp
	{
		public bool Shield { get; set; }
		public List<Orientation> Position { get; set; }

    public CityComp(bool shield, List<Orientation> position)
    {
      this.Shield = shield;
      this.Position = position;
    }

    public override string ToString()
    {
      return "{\n" + $"\"shield\": {this.Shield}\n\"position\": {string.Join(",", this.Position)}" +"\n}";
    }
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

    public string RoadToString()
    {
      var returnString = "";
      if (this.Road == null){
        return "null";
      }
      foreach (var list in this.Road)
      {
        foreach (var str in list)
        {
          if (returnString == "")
          {
            returnString += str.ToString();
          }
          else
          {
            returnString += "," + str.ToString();
          }
          
        }
      }
      return returnString;
    }

    public string CityToString(){
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

    public override string ToString()
    {
      return "{\n" + $"\"name\": \"{this.Name}\"\n\"city\": {this.CityToString()}\n\"road\": {this.RoadToString()}" + "\n}";
    }
  }
  
}