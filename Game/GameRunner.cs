using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GameComponents;

namespace GameLogic
{
  class GameRunner {
    public GameBoard GameBoard { get; set; }

    public List<Player> PlayerList { get; set; }

    public GameRunner(List<Player> playerList) {
      this.PlayerList = playerList;
      this.GameBoard = new GameBoard(); 
    }

    /**getPossiblePositions = getPossibleTransitions(AI)
    * @Tile currentTile => tile curent pentru care trebuie luata o decizie
    * @return List<Tuple<int, List<int>>>
    *   => o lista care contine tuple care contin:
    *     indexul pozitie din PossiblePositions care e compatibil cu tile-ul currentTile
    *     o lista de rotiri care poate fi ([0-3]+) (nu poate fi nula, are dim maxima = 4)
    *   => sau null, caz in care tile-ul currentTile nu poate fi pus nicaieri
    */
    public List<Tuple<int, List<int>>> GetPossiblePositions(Tile currentTile) {
      //TODO: de verificat daca exista pozitie dintre PossiblePositions care este accesibila pentru tile-ul currentTile
      //TODO: de returnat pozitiile accesibile + o lista de rotiri posibile pentru fiecare pozitie
      var returnList = new List<Tuple<int, List<int>>>();
      var isCompatible = false;
      Tile testTile;
      for (var i = 0; i < this.GameBoard.PossiblePositions.Count; i++) {
        isCompatible = false;
        for (var rotation = 0; rotation < 4; rotation++) {
          testTile = this.GetClonedRotatedTile(currentTile, rotation);
          System.Console.WriteLine(testTile.ToString());
        }
      }
      return null;
    }


    public Tile GetClonedRotatedTile(Tile currentTile, int rotation) {
      Tile cloneTile = (Tile)currentTile.Clone();
      cloneTile.Name += rotation.ToString();
      cloneTile.City[0].Position.Add((Orientation) rotation);
      return cloneTile;
    }

  }
}