using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Threading;
using GameComponents;
using GameStructures;
using GameLogic;
					
          
public class Program {


  public static T ParseEnum<T>(string value) {
    return (T) Enum.Parse(typeof(T), value, true);
  }


  public static void run() {
    System.Console.WriteLine("da");
    var gameBoard = new GameBoard();
    var componentManager = new ComponentManager();
    var structureManager = new StructureManager();
    var tileComps = componentManager.ParseJson();
    foreach (var tileComp in tileComps) {
      // System.Console.WriteLine(tileComp.ToString());
    }
    
    // // testing rotation
    var tile1 = new Tile(gameBoard, tileComps[0], (72, 72));
    var str = "";
    str = tile1.TileComponent.PrintMatrix();
    System.Console.WriteLine(str);
    tile1.RotateTile(1);
    str = tile1.TileComponent.PrintMatrix();
    System.Console.WriteLine(str);
    var tile2 = new Tile(gameBoard, tileComps[0], (72, 73));
    tile2.RotateTile(3);
    str = tile2.TileComponent.PrintMatrix();
    System.Console.WriteLine(str);

    // // testing neighbors
    var tile3 = new Tile(gameBoard, tileComps[0], (73, 73));
    var tile4 = new Tile(gameBoard, tileComps[0], (74, 73));
    tile4.RotateTile(1);
    var x = tile4.CanBeNeighbors(tile3, 3);
    System.Console.WriteLine(x);
    x= tile3.CanBeNeighbors(tile3, 1);
    System.Console.WriteLine(x);

    // // testing meeple creation
    Meeple m1 = new Meeple(MeepleColor.Red);
    Meeple m2 = new Meeple(MeepleColor.Yellow);
    System.Console.WriteLine(m1.ToString());
    System.Console.WriteLine(m2.ToString());

    // // testing structure creation and joining
    GameStructure g1 = new City();
    GameStructure g2 = new City();
    var g3 = new City();
    System.Console.WriteLine(g3.GetType()); // is City
    System.Console.WriteLine(g2.GetType()); // is City again
    var tile5 = new Tile(gameBoard, tileComps[0], (71, 72));
    var tile6 = new Tile(gameBoard, tileComps[0], (71, 71));

    g1.AddTile(tile5, 0);
    g3.AddTile(tile6, 0);
    g3.MeepleList.Add(new Meeple(MeepleColor.Blue));

    System.Console.WriteLine(g1.PrintTileMatrices());
    System.Console.WriteLine(g3.PrintTileMatrices());

    g3.JoinStructures(g1);

    System.Console.WriteLine(g3.PrintTileMatrices());
    System.Console.WriteLine(g3.ToString());
    g3.GetStructurePoints();


    // // testing game runner
    var gameRunner = new GameRunner(tileComps);
    var tile7 = new Tile(gameBoard, tileComps[0], (-1, -1));
    var zz = gameRunner.GetFreePositionsForTile(tile7);
    gameRunner.AddTileInPosition(tile7, (72, 72));
    var tile8 = new Tile(gameBoard, tileComps[0], (-1, -1));
    System.Console.WriteLine(tile8.TileComponent.PrintMatrix());
    tile8.RotateTile(2);
    zz = gameRunner.GetFreePositionsForTile(tile8);
    foreach (var t in zz) {
      System.Console.WriteLine(t);
    }
    foreach (var t in gameRunner.GameBoard.FreePositions) {
      System.Console.WriteLine($"t:  {t}");
    }
    System.Console.WriteLine(tile8.TileComponent.PrintMatrix());
    tile8.RotateTile(1);
    System.Console.WriteLine(tile8.TileComponent.PrintMatrix());
    zz = gameRunner.GetFreePositionsForTile(tile8);
    foreach (var t in zz) {
      System.Console.WriteLine(t);
    }



    
  }
  
	
	public static void Main() {
    Program.run();
	}
}