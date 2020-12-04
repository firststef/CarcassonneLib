using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using LibCarcassonne.GameStructures;
using LibCarcassonne.GameComponents;
using LibCarcassonne.ArrayAccessMethods;

namespace LibCarcassonne
{
    namespace GameStructures
    {


        public class City : GameStructure
        {
            public int ShieldCount { get; set; }


            public City() : base(StructureType.city)
            {
                // ce cancer ca nu poti sa apelezi aicea base
                this.ShieldCount = 0;
                System.Console.WriteLine($"{this.StructureId} {this.StructureType.ToString()}");
            }


            /**
            * joins structure and adds shield too
            */
            public override void JoinStructures(GameStructure another)
            {
                this.ShieldCount += ((City)another).ShieldCount;
                base.JoinStructures(another);
            }


            /**
            * triggers base add tile and increments ShieldCount if needed 
            */
            public override void AddTile(Tile tile, int tileComponentId)
            {
                if (tile.ComponentHasShield(tileComponentId))
                {
                    this.ShieldCount++;
                }
                base.AddTile(tile, tileComponentId);
            }


            /**
            * Structure points are calculated by adding 2 foreach tile and 2 for each ShieldCount
            * returns computed sum
            */
            public override int GetStructurePoints()
            {
                int sum = 0;
                foreach (var tile in this.ComponentTiles)
                {
                    sum += 2;
                }
                return sum + 2 * this.ShieldCount;
            }
        }


        public class Road : GameStructure
        {


            public Road() : base(StructureType.road)
            {
                System.Console.WriteLine($"{this.StructureId} {this.StructureType.ToString()}");
            }


            /**
            * returns points equal to road length count
            */
            public override int GetStructurePoints()
            {
                return this.ComponentTiles.Count;
            }
        }


        public class Field : GameStructure
        {


            public Field() : base(StructureType.field)
            {
                System.Console.WriteLine($"{this.StructureId} {this.StructureType.ToString()}");
            }


            /**
            * returns 2 * each distinct city on current field
            */
            public override int GetStructurePoints()
            {
                var neighborCities = new List<int>();
                foreach (var tile in this.ComponentTiles)
                {
                    var neighbors = tile.GetNeighborCities(this.StructureId);
                    neighborCities.AddRange(neighbors.Except(neighborCities));
                }

                return neighborCities.Count * 2;
            }
        }


        public class Monastery : GameStructure
        {


            public Monastery() : base(StructureType.monastery)
            {
                System.Console.WriteLine($"{this.StructureId} {this.StructureType.ToString()}");
            }


            public override void JoinStructures(GameStructure another)
            {
                throw new Exception("manastirea nu face join");
            }


            /**
            * returns points by counting neighbor tiles + 1
            */
            public override int GetStructurePoints()
            {
                if (this.MeepleList.Count == 0)
                {
                    return 0;
                }
                return 1 + this.ComponentTiles[0].CountMonasteryNeighbors();
            }
        }


    }
}