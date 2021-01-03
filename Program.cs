using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Threading;
using LibCarcassonne.GameComponents;
using LibCarcassonne.GameStructures;
using LibCarcassonne.GameLogic;
using LibCarcassonne.ArrayAccessMethods;

public class Program
{


    public static T ParseEnum<T>(string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }


    public static void run()
    {
        System.Console.WriteLine("da");
        var gameBoard = new GameBoard();
        var componentManager = new ComponentManager();
        var structureManager = new StructureManager();
		string tilesJson = System.IO.File.ReadAllText("tiles_map.json");
        var tileComps = componentManager.ParseJson(tilesJson);
        foreach (var tileComp in tileComps)
        {
            // System.Console.WriteLine(tileComp.ToString());
        }

        // // // testing rotation
        // var tile1 = new Tile(gameBoard, tileComps[0], (72, 72));
        // var str = "";
        // str = tile1.TileComponent.PrintMatrix();
        // System.Console.WriteLine(str);
        // tile1.RotateTile(1);
        // str = tile1.TileComponent.PrintMatrix();
        // System.Console.WriteLine(str);
        // var tile2 = new Tile(gameBoard, tileComps[0], (72, 73));
        // tile2.RotateTile(3);
        // str = tile2.TileComponent.PrintMatrix();
        // System.Console.WriteLine(str);

        // // // testing neighbors
        // var tile3 = new Tile(gameBoard, tileComps[0], (73, 73));
        // var tile4 = new Tile(gameBoard, tileComps[0], (74, 73));
        // tile4.RotateTile(1);
        // var x = tile4.CanBeNeighbors(tile3, 3);
        // System.Console.WriteLine(x);
        // x= tile3.CanBeNeighbors(tile3, 1);
        // System.Console.WriteLine(x);

        // // // testing meeple creation
        // Meeple m1 = new Meeple(MeepleColor.Red);
        // Meeple m2 = new Meeple(MeepleColor.Yellow);
        // System.Console.WriteLine(m1.ToString());
        // System.Console.WriteLine(m2.ToString());

        // // // testing structure creation and joining
        // GameStructure g1 = new City();
        // GameStructure g2 = new City();
        // var g3 = new City();
        // System.Console.WriteLine(g3.GetType()); // is City
        // System.Console.WriteLine(g2.GetType()); // is City again
        // var tile5 = new Tile(gameBoard, tileComps[0], (71, 72));
        // var tile6 = new Tile(gameBoard, tileComps[0], (71, 71));

        // g1.AddTile(tile5, 0);
        // g3.AddTile(tile6, 0);
        // g3.MeepleList.Add(new Meeple(MeepleColor.Blue));

        // System.Console.WriteLine(g1.PrintTileMatrices());
        // System.Console.WriteLine(g3.PrintTileMatrices());

        // g3.JoinStructures(g1);

        // System.Console.WriteLine(g3.PrintTileMatrices());
        // System.Console.WriteLine(g3.ToString());
        // g3.GetStructurePoints();


        var gameRunner = new GameRunner(tileComps, 5);
        // // testing game runner
        Program.PlayRounds(gameRunner);


        // // // retesting tile rotation
        // var tiles = gameRunner.UnplayedTiles;
        // foreach (var t in tiles) {
        //   System.Console.WriteLine(t.PrintMatrix());
        //   for (var pos = 0; pos < 4; ++pos ) {
        //     var clonedT = t.Clone();
        //     clonedT.RotateTile(pos);
        //     System.Console.WriteLine(clonedT.PrintMatrix());
        //   }
        //   Thread.Sleep(5000);
        // }

    }


    public static void PlayRounds(GameRunner gameRunner)
    {
        var playerManager = new PlayerManager(5);
        var turn = 0;
        var i = 0;
        int j = 0;
        while (true)
        {
            var gameStructures = gameRunner.GameBoard.GameStructures;
            System.Console.Write("Structures: ");
            foreach (var gs in gameStructures)
            {
                System.Console.Write($"{gs.StructureId} ");
            }
            System.Console.WriteLine($"\n\t\tTura {++i}\n");
            var tile = gameRunner.GetCurrentRoundTile();
            if (tile == null)
            {
                gameRunner.TriggerEndGame();
                break;
            }

			System.Console.WriteLine($"Tile {tile.TileComponent.Name}");
            System.Console.WriteLine(tile.PrintMatrix());
            var freePositions = gameRunner.GetFreePositionsForTile(tile);
            System.Console.WriteLine("Pozitiile libere: ");
            j = 0;
            foreach (var pos in freePositions)
            {
                System.Console.WriteLine($"Pozitia {j++}: {pos.Item1} with rotations {string.Join(" ", pos.Item2)}");
            }
            System.Console.WriteLine("Alege un index de pozitie libera: ");

            //var userInput = Convert.ToInt32(System.Console.ReadLine());
            var userInput = 0;

            System.Console.WriteLine($"S-a introdus: {freePositions[userInput].Item1}");
            System.Console.WriteLine("Alege o rotatie disponibila pentru pozitia aleasa: ");

            //var rotation = Convert.ToInt32(System.Console.ReadLine());
            var rotation = freePositions[0].Item2[0];

            System.Console.WriteLine($"S-a introdus rotatia: {rotation}");
            System.Console.WriteLine("\n");

            var possiblePositionsForMeeple = gameRunner.AddTileInPositionAndRotation(tile, freePositions[userInput].Item1, rotation);

            if (possiblePositionsForMeeple == null)
            {
                System.Console.WriteLine("Nu se poate pune meeple");
                continue;
            }

            System.Console.WriteLine($"Possible positions for meeple: {string.Join(" ", possiblePositionsForMeeple)}");


            System.Console.WriteLine("Alege un index de unde sa pui meeple-ul: ");
            //var meepleInput = Convert.ToInt32(System.Console.ReadLine());
            var meepleInput = possiblePositionsForMeeple[0];
            var meeplePositionToPlace = meepleInput;

            if (playerManager.GetPlayer(turn % 5).HasMeeples())
            {
                var meeple = playerManager.GetPlayer(turn % 5).GetFreeMeeple();

                
                gameRunner.PlaceMeeple(meeple, meeplePositionToPlace);
                System.Console.WriteLine($"Placed meeple: {meeple.MeepleId}");
            }
            else
            {
                System.Console.WriteLine($"Current player has no meeples");
            }

            var meepleToRaise = gameRunner.CommitChanges();
            if (meepleToRaise == null)
            {
                System.Console.WriteLine("Nici o structura nu a fost terminata, nici un ,eeple nu trebuie ridicat");
            }
            else
            {
                foreach (var ii in meepleToRaise)
                {
                    System.Console.WriteLine($"meeple: {ii}");
                    ii.RaiseMeeple();
                    System.Console.WriteLine($"Player now has {playerManager.GetPlayer(turn % 5).PlayerPoints} points");
                }
                System.Console.WriteLine(gameRunner.GameBoard.ToString());
                
                //throw new Exception("DA");
            }

            
            

            System.Console.WriteLine(gameRunner.GameBoard.ToString());
            turn++;
        }
        foreach (var player in playerManager.PlayerList)
        {
            System.Console.WriteLine($"Player {player.MeepleColor.ToString()} has {player.PlayerPoints} points");
        }
    }


    public static void Main()
    {
        Program.run();
    }
}