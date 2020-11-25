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
    //parses all tiles as an array
    JArray jA = JArray.Parse(tilesJson);
    string name;
    JArray auxJA;
    bool shield;
    foreach (var tile in jA){
      //for each tile in array
      name = (string)tile["name"];
      //parses all cities in tile
      auxJA = (JArray)tile["city"];
      List<CityComp> cityCompList = null;
      if (auxJA != null){
        //checks city not to be null
        cityCompList = new List<CityComp>();
        foreach(var city in auxJA){
          //for each city in tile
          shield = (city["shield"] != null);
          List<Orientation> posList = new List<Orientation>();
          //parses city position array
          foreach(var o in (JArray)city["position"])
          {
            posList.Add(ParseEnum<Orientation>((string) o));
          }
          cityCompList.Add(new CityComp(shield, posList));
        }
      }
      //parses all roads in tile
      auxJA = (JArray)tile["road"];
      List<List<Orientation>> roadList = null;
      if (auxJA != null) {
        //checks roads not to be null
        roadList = new List<List<Orientation>>();
        foreach (var road in auxJA){
          //for each road in city
          var r = new List<Orientation>();
          //parses road position array
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

    foreach (var tile in tilesList) {
      System.Console.WriteLine(tile.ToString() + "\n");
    }

    
    GameBoard gameBoard = new GameBoard();
    System.Console.WriteLine(gameBoard.ToString());
	}
}