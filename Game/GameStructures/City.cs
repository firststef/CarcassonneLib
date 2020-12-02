using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GameStructures;
using GameComponents;
using ArrayAccessMethods;


namespace GameStructures {


  public class City : GameStructure {
    


    public City() : base(StructureType.city)  {
      // ce cancer ca nu poti sa apelezi aicea base
      System.Console.WriteLine($"{this.StructureId} {this.StructureType.ToString()}");
    }


  }


}