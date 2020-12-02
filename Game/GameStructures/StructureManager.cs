using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GameStructures;


namespace GameStructures {
  // In GameStructures avem structurile din joc, adica tile-urile preluate din GameComponents, si apoi jucate

  public class StructureManager {


    public StructureManager() {
      System.Console.WriteLine("StructureManager");
      GameStructure.StructureMaxId = 10;
    }


  }


  public enum StructureType {
    city = 0,
    road = 1,
    field = 2,
    monastery = 3
  }


  public class GameStructure {
    public static int StructureMaxId { get; set; }
    public List<Tile> ComponentTiles { get; set; }
    public int StructureId { get; set; }
    public StructureType StructureType { get; set; }
    public List<Meeple> MeepleList { get; set; }


    public GameStructure(StructureType structureType) {
      // StructureMaxId ar trebuii facut 10 initial ca sa nu avem coleziuni pentru primul tile
      // ex: initiem oras-ul 1 cu id-ul 1, schimbam toate 2-urile acelui oras in 1, 
      // si apoi initiem campia 1 cu id-ul 2, schimbam toate 1-urile campiei in 2 
      this.StructureId = GameStructure.StructureMaxId++;
      this.StructureType = structureType;
    }


  }
}