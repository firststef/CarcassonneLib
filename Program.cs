using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;

					
public class Program
{
  static List<Tile> tilesList = new List<Tile>();

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
		[JsonProperty("shield")]
		public bool Shield { get; set; }
		
		[JsonProperty("position")]
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
    [JsonProperty("name")]
    public string Name{ get; set; }

    [JsonProperty("city")]
    public List<CityComp> City { get; set; }
        
    [JsonProperty("road")]
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
    {//not working for now
      return "{\n" + $"\"name\": \"{this.Name}\"\n\"city\": {this.CityToString()}\n\"road\": {this.RoadToString()}" + "\n}";
    }
  }

  public static List<Tile> parseTilesJson()
  {
    List<Tile> tilesList = new List<Tile>();
    string tilesJson = System.IO.File.ReadAllText("tiles_map.json");
    JArray jA = JArray.Parse(tilesJson);
    string name;
    JArray auxJA;
    bool shield;
    foreach (var tile in jA){
      name = (string)tile["name"];
      auxJA = (JArray)tile["city"];
      List<CityComp> cityCompList = null;
      if (auxJA != null){
        cityCompList = new List<CityComp>();
        foreach(var city in auxJA){
          shield = (city["shield"] != null);
          List<Orientation> posList = new List<Orientation>();
          foreach(var o in (JArray)city["position"])
          {
            posList.Add((Orientation)Enum.Parse(typeof(Orientation), (string)o, true));
          }
          cityCompList.Add(new CityComp(shield, posList));
        }
      }
      auxJA = (JArray)tile["road"];
      List<List<Orientation>> roadList = null;
      if (auxJA != null){
        roadList = new List<List<Orientation>>();
        foreach (var road in auxJA){
          var r = new List<Orientation>();
          foreach(var o in (JArray)road){
            r.Add((Orientation)Enum.Parse(typeof(Orientation), (string)o, true));
          }
          roadList.Add(r);
        }
      }
      tilesList.Add(new Tile(name, cityCompList, roadList));
      
    }
    return tilesList;
  }
	
	public static void Main()
	{
    
    tilesList = parseTilesJson();
    foreach (var tile in tilesList){
      System.Console.WriteLine(tile.ToString());
    }
	}
}