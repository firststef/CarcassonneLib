using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using LibCarcassonne.GameComponents;
using LibCarcassonne.GameStructures;
using LibCarcassonne.GameLogic;
using LibCarcassonne.ArrayAccessMethods;

namespace LibCarcassonne
{
    namespace GameStructures
    {
        public class AI : Player
        {
            public GameRunner GameRunner { get; set; }
            public int Difficulty { get; set; }
            public Random Rand { get; set; }

            public delegate Tuple<(int, int), int> Prediction(Tile currentTile);
            public Prediction Predict { get; set; }


            /**
             * AI extends player, and has custom Prediction depending on difficulty
             */
            public AI(GameRunner gameRunner, MeepleColor meepleColor, int difficulty) : base(meepleColor: meepleColor)
            {
                this.Difficulty = difficulty;
                this.GameRunner = gameRunner;
                this.Rand = new System.Random();
                if (this.Difficulty == 0)
                {
                    this.Predict = RandomPrediction;
                }
                else
                {
                    //TODO: de implementat 1, 2 si 3
                }
            }


            /**
             * returns random prediction
             * a tuple of coordinates for tile to be placed, and a rotation to be made for this set of coordinates
             */
            private Tuple<(int, int), int> RandomPrediction(Tile currentTile)
            {
                var list = this.GameRunner.GetFreePositionsForTile(currentTile);

                if (list.Count == 0)
                {
                    throw new Exception("no possible positions for current tile");
                }

                var index = this.Rand.Next(0, list.Count - 1);
                var index2 = this.Rand.Next(0, list[index].Item2.Count - 1);
                return new Tuple<(int, int), int>(list[index].Item1, list[index].Item2[index2]);
            }


            /**
             * returns ai chosen meeple position, or -1 if AI chooses not to place meeple
             * TODO: to implement
             *  ! AI should consider the points he may receive for each possible placement
             *  ! He may also consider further future rewards for their chosen location
             *  ! If no location is thought to bring enough benefits, -1 is provided
             */
            public int ChooseMeeplePlacement(List<int> possiblePositionsForMeeple)
            {
                System.Console.WriteLine("OHAAA");
                System.Console.WriteLine(string.Join("  ", possiblePositionsForMeeple));
                return this.Rand.Next(0, possiblePositionsForMeeple.Count - 1);
                return 0;
                return -1;
            }



        }
    }

    
}
