using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using OldGameComponents;

namespace OldGameStructures {
  class RoadPart {
    public Tile ParentTile { get; set; }
    public List<Orientation> Road { get; set; }

    public RoadPart(Tile parentTile, List<Orientation> road) {
      //Structure is made of only 1 road and points to parent tile which may have more roads
      this.ParentTile = parentTile;
      this.Road = road;
    }
  }

  class RoadStruct {
    public List<RoadPart> RoadParts { get; set; }
    public List<Meeple> MeepleList { get; set; }

    public RoadStruct() {
      
    }

    
  }
}