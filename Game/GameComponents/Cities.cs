using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GameComponents;

namespace GameComponents
{
  public class CityComp
	{
		public bool Shield { get; set; }
		public List<Orientation> Position { get; set; }

    public CityComp(bool shield, List<Orientation> position)
    {
      this.Shield = shield;
      this.Position = position;
    }

    /**
    * city component to string = { shield: bool, position: {N, E, S}}
    */
    public override string ToString()
    {
      return "{\n" + $"\"shield\": {this.Shield}\n\"position\": {string.Join(",", this.Position)}" +"\n}";
    }
	}
}