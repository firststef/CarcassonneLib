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
          //for each road in road
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

  public static void Tester() {
    if (Program.tilesList.Count == 0) {
      Program.tilesList = parseTilesJson();
    }
    GameRunner gameRunner = new GameRunner(null, tilesList);
    var r = gameRunner.AreCompatible(tilesList[1], tilesList[2], 1);
    System.Console.WriteLine(r); // should be true
    r = gameRunner.AreCompatible(tilesList[1], tilesList[2], 0);
    System.Console.WriteLine(r); // should be false
    r = gameRunner.AreCompatible(tilesList[1], gameRunner.GetClonedRotatedTile(tilesList[2], 2), 2); //should be true
    System.Console.WriteLine(r);
    r = gameRunner.AreCompatible(tilesList[1], tilesList[19], 3);
    System.Console.WriteLine(r); //should be false
    //TODO de testat mai toate cazurile ca sa vedem daca merge sau nu
  }

  public static void GameDemo() {
    if (Program.tilesList.Count == 0) {
      Program.tilesList = parseTilesJson();
    }
    GameRunner gameRunner = new GameRunner(null, tilesList);
    while (! gameRunner.EndGame) {
      gameRunner.PrepareRound();
    }
  }
	
	public static void Main()
	{
    tilesList = parseTilesJson();

    GameRunner gameRunner = new GameRunner(null, tilesList);
    foreach (var tile in gameRunner.ShuffledTiles) {
      // System.Console.WriteLine(tile.ToString() + "\n"); // printing tiles
    }

    GameBoard gameBoard = new GameBoard();
    gameBoard.PlacedTiles[120, 70] = tilesList[12];
	  gameBoard.PlacedTiles[120, 69] = gameRunner.GetClonedRotatedTile(tilesList[12], 2);
	  gameBoard.PlacedTiles[119, 70] = gameRunner.GetClonedRotatedTile(tilesList[12], 2);
    gameBoard.PlacedTiles[121, 70] = gameRunner.GetClonedRotatedTile(tilesList[2], 2);
    gameBoard.PlacedTiles[121, 71] = tilesList[15];
    // System.Console.WriteLine(gameBoard.ToString()); // old game board to string
    //System.Console.WriteLine(gameBoard.GameBoardToString());

    
    var possibleTransitions = gameRunner.GetPossiblePositions(tilesList[0]);
    // System.Console.WriteLine(gameRunner.PossiblePositionsToString(possibleTransitions)) // shows possible transitions for first tile

    //Program.Tester();
    Program.GameDemo();
	}
}