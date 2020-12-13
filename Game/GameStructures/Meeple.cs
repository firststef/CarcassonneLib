using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using LibCarcassonne.GameStructures;
using LibCarcassonne.GameComponents;
using LibCarcassonne.ArrayAccessMethods;

namespace LibCarcassonne
{
    namespace GameStructures
    {


        public enum MeepleColor
        {
            Red = 0,
            Black = 1,
            Green = 2,
            Yellow = 3,
            Blue = 4
        }


        public class Meeple
        {
            public MeepleColor MeepleColor { get; set; }
            public static int MeepleMaxId { get; set; }
            public int MeepleId { get; set; }
            public Tile MeeplePlacement { get; set; }
            public int PlacementId { get; set; }


            public Meeple(MeepleColor meepleColor)
            {
                this.MeepleId = Meeple.MeepleMaxId++;
                this.MeepleColor = meepleColor;
                this.MeeplePlacement = null;
                this.PlacementId = -1;
            }


            public void PlaceMeeple(Tile meeplePlacement, int placementId)
            {
                this.MeeplePlacement = meeplePlacement;
                this.PlacementId = placementId;
                //TODO: Aici ar trebuii o functie VIZUALA de punere a meeple-ului pe Tile-ul meeplePlacement pe pozitia care trebuie
            }


            /**
             * returns False if meeple is placed (placement Id is not -1), True otherwise
             */
            public bool IsMeepleFree()
            {
                return (-1 == this.PlacementId);
            }


            public override string ToString()
            {
                return $"{{\nMeeple: meeple_{this.MeepleId}\nMeepleColor: {this.MeepleColor.ToString()}\nMeeplePlacementId: {this.PlacementId}\n}}";
            }


        }
    }
}