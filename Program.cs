using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using GameComponents;
using GameLogic;


					
public class Program
{
  static List<Tile> tilesList = new List<Tile>();

  public static T ParseEnum<T>(string value)
  {
    return (T) Enum.Parse(typeof(T), value, true);
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
            posList.Add(ParseEnum<Orientation>((string) o));
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
            r.Add(ParseEnum<Orientation>((string) o));
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
      System.Console.WriteLine(tile.ToString() + "\n");
    }
    
	}
}