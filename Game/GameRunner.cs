using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GameLogic
{
  class GameRunner {
    public GameBoard GameBoard { get; set; }

    public List<Player> PlayerList { get; set; }

    public GameRunner(List<Player> playerList) {
      this.PlayerList = playerList;
      this.GameBoard = new GameBoard(); 
    }
  }
}