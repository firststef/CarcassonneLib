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
      this.ComponentTiles = new List<Tile>();
      this.MeepleList = new List<Meeple>();
    }


    /**
    * joins 2 same type Structures
    * replaces all another's structures id with current structure id
    * joins tile and meeple list
    * disposes of 2nd structure
    */
    public void joinStructures(GameStructure another) {
      if (this.StructureType != another.StructureType) {
        throw new Exception("Structures are not same type");
      }
      another.ReplaceStructureId(this.StructureId);
      this.ComponentTiles.AddRange(another.ComponentTiles);
      this.MeepleList.AddRange(another.MeepleList);
      another.Dispose();
    }


    /**
    * replaces structure id for all meeples and component tiles
    */
    public void ReplaceStructureId(int anotherStructureId) {
      foreach (var meeple in MeepleList) {
        meeple.PlacementId = anotherStructureId;
      }
      foreach (var tile in ComponentTiles) {
        tile.ReplaceStructureIds(this.StructureId, anotherStructureId);
      }
    }


    public void Dispose() {
      //TODO: de verificat ca aceste nulificari nu afecteaza lista la care s-au adaugat componentele
      this.ComponentTiles = null;
      this.MeepleList = null;
    }


    /**
    * adds tile to structure and points to structure id to be changed and is changed accordingly
    */
    public void AddTile(Tile tile, int tileComponentId) {
      this.ComponentTiles.Add(tile);
      tile.ReplaceStructureIds(tileComponentId, this.StructureId);
    }


    public override string ToString() {
      return $"{{\nid: {this.StructureId}\ntype: {this.StructureType}\ntiles: {this.ComponentTiles.Count}\nmeeples: {this.MeepleList.Count}\n}}";
    }


    public string PrintTileMatrices() {
      var returnString = "begin\n";
      
      foreach (var tile in this.ComponentTiles) {
        returnString += tile.TileComponent.PrintMatrix() + "\n";
      }

      return returnString + "end\n";
    }


  }
}