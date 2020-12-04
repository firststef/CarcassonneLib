using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using LibCarcassonne.GameComponents;
using LibCarcassonne.ArrayAccessMethods;

namespace LibCarcassonne
{
    namespace GameComponents
    {

        /**
        * a Component Type contains the current and actualized id of the component (part of tile
        * matrix) it describes, along with it's type ("field", "city", "road", "monastery") and
        * its center (the position relative to tile center to where the meeple should be placed)
        * 
        * Neighbors is a field specific only to "field" and should contain all ORIGINAL ids of 
        * cities on this tile which are near current field. ORIGINAL ids were the position of 
        * component in Types list of TileComponent 
        * 
        * HasShield is True only for "city" with shield, false otherwise
        */
        public class ComponentType
        {
            public int Id { get; set; }
            public string Type { get; set; }
            public List<int> Neighbors { get; set; }
            public List<float> Center { get; set; }
            public bool HasShield { get; set; }


            public ComponentType(int id, string type, List<int> neighbors, List<float> center, bool hasShield)
            {
                this.Id = id;
                this.Type = type;
                this.Neighbors = neighbors;
                this.Center = center;
                this.HasShield = hasShield;
            }


            public ComponentType(ComponentType another)
            {
                this.Id = another.Id;
                this.Type = another.Type;
                if (another.Neighbors == null)
                {
                    this.Neighbors = null;
                }
                else
                {
                    this.Neighbors = new List<int>(another.Neighbors);
                }
                this.Center = new List<float>(another.Center);
                this.HasShield = another.HasShield;
            }


            public ComponentType Clone()
            {
                return new ComponentType(this);
            }


            public override string ToString()
            {
                return $"{{\n\"id\": {this.Id}\n\"center\": {this.PrintList<float>(this.Center)}\n\"type\": {this.Type}\n\"neighbors\": {this.PrintList<int>(this.Neighbors)}\n\"shield\": {this.HasShield}\n}}";
            }


            public string PrintList<T>(List<T> l)
            {
                var x = new CustomArray<T>();
                return x.PrintList(l);
            }

            public T GetType<T>()
            {
                if (typeof(T) == typeof(int))
                {
                    return (T)(object)this.Id;
                }
                return (T)(object)this.Type;
            }



        }


    }
}