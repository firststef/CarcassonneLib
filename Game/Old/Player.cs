using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OldGameLogic
{
  class Player {
    public string PlayerName { get; set; }

    public Player(string name) {
      this.PlayerName = name;
      System.Console.WriteLine(name);
    }
  }

 
}
