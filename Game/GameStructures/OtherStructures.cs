using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GameStructures;
using GameComponents;
using ArrayAccessMethods;


namespace GameStructures {


  public class City : GameStructure {
    public int ShieldCount { get; set; }


    public City() : base(StructureType.city)  {
      // ce cancer ca nu poti sa apelezi aicea base
      this.ShieldCount = 0;
      System.Console.WriteLine($"{this.StructureId} {this.StructureType.ToString()}");
    }


    /**
    * joins structure and adds shield too
    */
    public override void JoinStructures(GameStructure another) {
      this.ShieldCount += ((City) another).ShieldCount;
      base.JoinStructures(another);
    }


    /**
    * triggers base add tile and increments ShieldCount if needed 
    */
    public override void AddTile(Tile tile, int tileComponentId) {
      base.AddTile(tile, tileComponentId);
      if (tile.ComponentHasShield(tileComponentId)) {
        this.ShieldCount++;
      }
    }


    /**
    * Structure points are calculated by adding 2 foreach tile and 2 for each ShieldCount
    * returns computed sum
    */
    public override int GetStructurePoints() {
      int sum = 0;
      foreach (var tile in this.ComponentTiles) {
        sum += 2;
      }
      return sum + 2 * this.ShieldCount;
    }
  }


  public class Road : GameStructure {


    public Road() : base(StructureType.road) {
      System.Console.WriteLine($"{this.StructureId} {this.StructureType.ToString()}");
    }


    public override int GetStructurePoints() {
      return 0;
    }
  }


  public class Field : GameStructure {


    public Field() : base(StructureType.field) {
      System.Console.WriteLine($"{this.StructureId} {this.StructureType.ToString()}");
    }
  }


  public class Monastery : GameStructure {


    public Monastery() : base(StructureType.monastery) {
      System.Console.WriteLine($"{this.StructureId} {this.StructureType.ToString()}");
    }


    public override void JoinStructures(GameStructure another) {
      throw new Exception("manastirea nu face join");
    }


    public override int GetStructurePoints() {
      if (this.MeepleList.Count == 0) {
        return 0;
      }
      return this.ComponentTiles[0].CountMonasteryNeighbors();
    }
  }


}