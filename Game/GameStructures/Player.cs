using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using LibCarcassonne.GameComponents;
using LibCarcassonne.GameStructures;
using LibCarcassonne.GameLogic;
using LibCarcassonne.ArrayAccessMethods;


namespace LibCarcassonne.GameStructures
{
    public class Player
    {
        public MeepleColor MeepleColor { get; set; }
        public List<Meeple> MeepleList { get; set;  }
        public int PlayerPoints { get; set; }


        public Player(MeepleColor meepleColor)
        {
            this.PlayerPoints = 0;
            this.MeepleColor = meepleColor;
            this.MeepleList = new List<Meeple>();
            for (var i = 0; i < 6; ++i)
            {
                MeepleList.Add(new Meeple(this.MeepleColor, this));
            }
        }


        /**
         * returns first free meeple of current player, or null, if no meeple is available
         */
        public Meeple GetFreeMeeple()
        {
            foreach(var meeple in this.MeepleList)
            {
                if (meeple.IsMeepleFree())
                {
                    return meeple;
                }
            }
            return null;
        }


        /**
         * returns True if current player may place meeple, False otherwise
         */
        public bool HasMeeples()
        {
            return (this.GetFreeMeeple() != null);
        }
    }
}
