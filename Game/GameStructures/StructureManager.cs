using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using LibCarcassonne.GameStructures;
using LibCarcassonne.GameLogic;


namespace LibCarcassonne
{
    namespace GameStructures
    {
        // In GameStructures avem structurile din joc, adica tile-urile preluate din GameComponents, si apoi jucate

        public class StructureManager
        {


            public StructureManager()
            {
                System.Console.WriteLine("StructureManager");
                GameStructure.StructureMaxId = 10;
            }


        }


        public enum StructureType
        {
            city = 0,
            road = 1,
            field = 2,
            monastery = 3
        }


        public class GameStructure
        {
            public static int StructureMaxId { get; set; }
            public List<Tile> ComponentTiles { get; set; }
            public int StructureId { get; set; }
            public StructureType StructureType { get; set; }
            public List<Meeple> MeepleList { get; set; }
            public GameBoard GameBoard { get; set; }
            public bool IsClosed { get; set; }


            public GameStructure(StructureType structureType, GameBoard gameBoard)
            {
                // StructureMaxId ar trebuii facut 10 initial ca sa nu avem coleziuni de id pentru primul tile
                this.StructureId = GameStructure.StructureMaxId++;
                this.StructureType = structureType;
                this.ComponentTiles = new List<Tile>();
                this.MeepleList = new List<Meeple>();
                this.GameBoard = gameBoard;
            }


            /**
            * joins 2 same type Structures
            * replaces all another's structures id with current structure id
            * joins tile and meeple list
            * disposes of 2nd structure
            * 
            * may be overriden in derived classes
            * eg: Monastery(does not exist), City (has to add shields too)
            */
            public virtual void JoinStructures(GameStructure another)
            {
                if (!this.CanJoin(another))
                {
                    return;
                }
                //TODO: de vazut ca teoretic trebuie o lista de structuri in joc, cand se creaza o noua structura se adauga in lista respectiva, cand se face join se scoate o structura din lista respectiva
                if (this.StructureType != another.StructureType)
                {
                    throw new Exception("Structures are not same type");
                }
                if (this.StructureId == another.StructureId)
                {
                    throw new Exception("Nu ar trebuii sa faci join cu tine insuti");
                }
                another.ReplaceStructureId(this.StructureId);
                this.ComponentTiles.AddRange(another.ComponentTiles);
                this.MeepleList.AddRange(another.MeepleList);
                another.Dispose();
            }


            /***
             * checks if 2 structures may join each other. 
             * returns True if structeres have the same type and diffrent ids, False otherwise
             */
            public virtual bool CanJoin(GameStructure another)
            {
                return (this.StructureType == another.StructureType) && (this.StructureId != another.StructureId); 
            }


            /**
            * replaces structure id for all meeples and component tiles
            */
            public void ReplaceStructureId(int anotherStructureId)
            {
                foreach (var meeple in MeepleList)
                {
                    meeple.PlacementId = anotherStructureId;
                }
                foreach (var tile in ComponentTiles)
                {
                    tile.ReplaceStructureIds(this.StructureId, anotherStructureId);
                }
            }


            public void Dispose()
            {
                //TODO: de verificat ca aceste nulificari nu afecteaza lista la care s-au adaugat componentele. Probabil ca nu
                //TODO: de scos aceasta structura din lista cu toate structurile sa poata fi colectata de garbage collector. nullificarile nu is asa importante (probabil)
                this.ComponentTiles = null;
                this.MeepleList = null;
                GameBoard.RemoveStructure(this);
            }


            /**
            * adds tile to structure and triggers id of component in tile to be changed accordingly
            * 
            * may be overriden in derived classes, eg: City may update shield count
            */
            public virtual void AddTile(Tile tile, int tileComponentId)
            {
                this.ComponentTiles.Add(tile);
                tile.ReplaceStructureIds(tileComponentId, this.StructureId);
            }


            /**
            * returns true only if no other meeple is in structure, False otherwise
            */
            public bool CanPlaceMeeple(Tile tile)
            {
                if (!this.ComponentTiles.Contains(tile))
                {
                    return false;
                }

                return (this.MeepleList.Count == 0);
            }


            /**
            * places meeple in current structure, then triggers meeple own placement
            */
            public void PlaceMeeple(Tile tile, Meeple meeple)
            {
                if (!this.ComponentTiles.Contains(tile))
                {
                    throw new Exception("Tile not in Component tiles");
                }
                if (this.MeepleList.Count != 0)
                {
                    throw new Exception("Meeple is not alone in structure");
                }

                this.MeepleList.Add(meeple);
                meeple.PlaceMeeple(tile, this.StructureId);
            }


            public virtual int GetStructurePoints()
            {
                throw new NotImplementedException("trebuie implementata in clasele de baza");
            }


            public override string ToString()
            {
                return $"{{\nid: {this.StructureId}\ntype: {this.StructureType}\ntiles: {this.ComponentTiles.Count}\nmeeples: {this.MeepleList.Count}\n}}";
            }


            public string PrintTileMatrices()
            {
                var returnString = "begin\n";

                foreach (var tile in this.ComponentTiles)
                {
                    returnString += tile.TileComponent.PrintMatrix() + "\n";
                }

                return returnString + "end\n";
            }


            public bool CheckIfClosable()
            {
                if (this.IsClosed)
                {
                    return true;
                }

                foreach (var tile in this.ComponentTiles)
                {
                    if (tile.IsOpenBorderForStructure(this.StructureId))
                    {
                        return false;
                    }
                }

                this.IsClosed = true;
                return true;
            }


        }
    }
}