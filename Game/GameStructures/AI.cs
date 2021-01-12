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
                    this.Predict = StrategyTwo;
                }
                else if (this.Difficulty == 3)
                {
                    this.Predict = StrategyThree;
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
                var rewards = new List<int>();
                foreach (var possiblePosition in possiblePositionsForMeeple)
                {
                    var gameStructure = this.GameRunner.GameBoard.GetGameStructureWithId(this.GameRunner.GetLastPlacedTile().TileComponent.Types[possiblePosition].Id);
                    rewards.Add(gameStructure.GetStructurePoints());
                }
                var maxIndex = rewards.IndexOf(rewards.Max());
                if (rewards.Max() < (8 - this.GetPlayerUsableMeeples()) * 2 && this.Rand.Next(0, 5) > 2)
                {
                    return -1;
                }
                return maxIndex;
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
             * returns position using wheel of fortune
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
                foreach (var instance in evaluatedList)
                {
                    estimatedRewards.Add(EstimatePosition(instance, currentTile));
                }

                int rewardSum = 0;
                for (var i = 0; i < estimatedRewards.Count; ++i)
                {
                    estimatedRewards[i] -= estimatedRewards.Min();
                    rewardSum += estimatedRewards[i];
                }


                var random = this.Rand.Next(0, rewardSum);
                for (var i = 0; i < evaluatedList.Count; ++i)
                {
                    random -= estimatedRewards[i];
                    if (random <= 0)
                    {
                        return evaluatedList[i];
                    }
                }

                return evaluatedList[estimatedRewards.IndexOf(estimatedRewards.Max())];

            }


            /**
             * @currentTile => current tile to be placed on board
             * @returns => estimated position and rotation best suited for current tile
             * 
             * computes a reward for each possible placing of current tile
             * returns position with maximum reward
             */
            private Tuple<(int, int), int> StrategyTwo(Tile currentTile)
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
                foreach (var instance in evaluatedList)
                {
                    estimatedRewards.Add(EstimatePosition(instance, currentTile));
                }

                return evaluatedList[estimatedRewards.IndexOf(estimatedRewards.Max())];
            }


            private Tuple<(int, int), int> StrategyThree(Tile currentTile)
            {
                var gameRunnerThatWilllBeChangedDuringDepthSearch = this.GameRunner.Clone();
                var initialGameRunner = this.GameRunner;
                this.GameRunner = gameRunnerThatWilllBeChangedDuringDepthSearch;
                System.Console.WriteLine(string.Join(" ", GameRunner.GameBoard.FreePositions));
                var initialList = this.GameRunner.GetFreePositionsForTile(currentTile);
                var possibleStatesList = new List<Tuple<(int, int), int>>();
                foreach (var tuple in initialList)
                {
                    possibleStatesList.Add(new Tuple<(int, int), int>(tuple.Item1, tuple.Item2[0]));
                    //foreach (var rotation in tuple.Item2)
                    //{
                    //    possibleStatesList.Add(new Tuple<(int, int), int>(tuple.Item1, rotation));
                    //}
                }

                var estimatedRewards = new List<int>();
                foreach (var possibleStateToJumpInto in possibleStatesList)
                {
                    estimatedRewards.Add(SearchInDepth(possibleStateToJumpInto, depth: 0, maxDepth: 2));
                }

                this.GameRunner = initialGameRunner;
                System.Console.WriteLine(possibleStatesList[estimatedRewards.IndexOf(estimatedRewards.Max())]);
                System.Console.WriteLine(string.Join(" ", GameRunner.GameBoard.FreePositions));
                return possibleStatesList[estimatedRewards.IndexOf(estimatedRewards.Max())];
            }


            private int SearchInDepth(Tuple<(int, int), int> possibleStateToJumpInto, int depth, int maxDepth = 2)
            {
                // System.Console.WriteLine(depth);
                this.GameRunner.SimulatePlay(possibleStateToJumpInto);
                var freePositions = this.GameRunner.GameBoard.FreePositions;
                var possibleStatesList = new List<Tuple<(int, int), int>>();
                foreach (var freePosition in freePositions)
                {
                    possibleStatesList.Add(new Tuple<(int, int), int>(freePosition, 0));
                }

                var estimatedRewards = new List<int>();

                if (depth >= maxDepth)
                {
                    
                    foreach (var instance in possibleStatesList)
                    {
                        estimatedRewards.Add(EstimatePosition(instance, null));
                    }
                } 
                else
                {
                    var thisGameRunner = this.GameRunner.Clone();
                    foreach (var instance in possibleStatesList)
                    {
                        this.GameRunner = thisGameRunner;
                        estimatedRewards.Add(SearchInDepth(instance, depth: depth + 1, maxDepth: maxDepth));
                    }
                }
                // System.Console.WriteLine(possibleStatesList.Count);
                return (depth % 2 == 0) ? estimatedRewards.Max() : estimatedRewards.Min();
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
            private int EstimatePosition(Tuple<(int, int), int> position, Tile currentTile = null)
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
                        if (tileMargin[i] > 10 && !structureIds.Contains(tileMargin[i]))
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

                
                if (currentTile != null && currentTile.HasMonastery())
                {
                    aiReward += 9;
                }

                return this.heuristic(aiReward, othersReward);
            }






        }
    }

    
}
