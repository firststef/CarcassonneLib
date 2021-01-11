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

            public delegate int Heuristic(int aiReward, int othersReward);
            public Heuristic heuristic { get; set; }


            /**
             * AI extends player, and has custom Prediction depending on difficulty
             */
            public AI(GameRunner gameRunner, MeepleColor meepleColor, int difficulty) : base(meepleColor: meepleColor)
            {
                this.GameRunner = gameRunner;
                this.Rand = new System.Random();
                this.Predict = RandomPrediction;
                this.ChangeDifficulty(difficulty);
                this.heuristic = BasicHeuristFunction;
            }


            /**
             * changes AI difficulty in game
             */
            public void ChangeDifficulty(int difficulty)
            {
                this.Difficulty = difficulty;
                if (this.Difficulty == 1)
                {
                    this.Predict = StrategyOne;
                }
                else if (this.Difficulty == 2)
                {
                    //TODO: implement heuristic + ??
                }
                else if (this.Difficulty == 3)
                {
                    //TODO: implement heuristic + ???
                }
                else
                {
                    this.Predict = RandomPrediction;
                }
            }


            /**
             * basic heuristic method used for chosing tile placement
             * returns predicted values for current input
             * 
             * @aiReward => reward predicted to be gained by AI when current position is chosen
             * @othersReward => reward predicted for other players when current position is chosen
             */
            public int BasicHeuristFunction(int aiReward, int othersReward)
            {
                return  aiReward * aiReward - othersReward;
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
             * @currentTile => current tile to be placed on board
             * @returns => estimated position and rotation best suited for current tile
             * 
             * computes a reward for each possible placing of current tile
             * returns position with maximum reward
             * 
             */
            private Tuple<(int, int), int> StrategyOne(Tile currentTile)
            {
                var initialList = this.GameRunner.GetFreePositionsForTile(currentTile);
                var evaluatedList = new List<Tuple<(int, int), int>>();

                if (initialList.Count == 0)
                {
                    throw new Exception("no possible positions for current tile");
                }

                foreach (var tuple in initialList)
                {
                    foreach (var rotation in tuple.Item2)
                    {
                        evaluatedList.Add(new Tuple<(int, int), int>(tuple.Item1, rotation));
                    }
                }

                var estimatedRewards = new List<int>();
                foreach(var instance in evaluatedList)
                {
                    estimatedRewards.Add(EstimatePosition(instance, currentTile));
                }


                return evaluatedList[estimatedRewards.IndexOf(estimatedRewards.Max())];

            }


            /**
             * @position => coordinates for current tile to be placed + respective rotation
             * @currentTile => current tile to be placed on board
             * 
             * all neighbors in position are estimated
             * for each neighbor, we take its border in our direction
             * for each border, we add gameStructure indexes to our structureIds list
             * 
             * after estimating all structures, we compute aiReward and othersReward based on how many poits can be got for each structure, depending on meeples on it and its points gained per extension
             * 
             * @returns heuristic of the 2 variables
             */
            private int EstimatePosition(Tuple<(int, int), int> position, Tile currentTile)
            {
                var aiReward = 0;
                var othersReward = 0;
                var neighbors = this.GameRunner.GameBoard.GetTileNeighborsAndPosition(position.Item1);
                if (neighbors == null)
                {
                    return 0;
                }
                var structureIds = new List<int>();
                foreach (var neighbor in neighbors)
                {
                    var tileMargin = neighbor.Item1.GetBorderInReversedPosition<int>(neighbor.Item2);
                    // tileMargin int array of structure ids which we are sure to match and be extended by our current tile.
                    for (var i = 1; i < tileMargin.Length - 1; ++i)
                    {
                        if (! structureIds.Contains(tileMargin[i]))
                        {
                            structureIds.Add(tileMargin[i]);
                        }
                    }
                }

                foreach (var structureId in structureIds)
                {
                    var gameStructure = this.GameRunner.GameBoard.GetGameStructureWithId(structureId);

                    if (gameStructure.MeepleList.Count == 0)
                    {
                        if (this.HasMeeples())
                        {
                            aiReward += gameStructure.GetStructurePoints() + gameStructure.PointsGainedPerExtension;
                        }
                        else
                        {
                            othersReward += gameStructure.GetStructurePoints() + gameStructure.PointsGainedPerExtension;
                        }
                    }

                    foreach (var meeple in gameStructure.MeepleList)
                    {
                        if (meeple.MeepleColor == this.MeepleColor)
                        {
                            aiReward += gameStructure.PointsGainedPerExtension;
                        }
                        else
                        {
                            othersReward += gameStructure.PointsGainedPerExtension;
                        }
                    }
                }

                if (currentTile.HasMonastery())
                {
                    aiReward += 9;
                }

                return this.heuristic(aiReward, othersReward);
            }



        }
    }

    
}
