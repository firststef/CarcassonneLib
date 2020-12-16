using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using LibCarcassonne.GameComponents;
using LibCarcassonne.GameStructures;
using LibCarcassonne.GameLogic;
using LibCarcassonne.ArrayAccessMethods;


namespace LibCarcassonne.GameStructures
{
    public class PlayerManager
    {
        public List<Player> PlayerList { get; set; }


        public PlayerManager(int numberOfPlayers)
        {
            this.PlayerList = new List<Player>();
            for (var i = 0; i < numberOfPlayers; ++i)
            {
                this.PlayerList.Add(new Player(EnumParse<MeepleColor>.IntToEnum(i)));
            }
        }


        /**
         * returns player with given color, or null if it doesn't exist
         */
        public Player GetPlayer(MeepleColor color)
        {
            var playerIndex = (int)color;
            if (playerIndex > this.PlayerList.Count)
            {
                return null;
            }
            return this.PlayerList[playerIndex];
        }

        
        /**
         * returns player with given index, or null if it doesn't exist
         */
        public Player GetPlayer(int playerIndex)
        {
            if (playerIndex > this.PlayerList.Count)
            {
                return null;
            }
            return this.PlayerList[playerIndex];
        }
    }
}
