using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using LibCarcassonne.ArrayAccessMethods;
using LibCarcassonne.GameComponents;

namespace LibCarcassonne {
    namespace GameComponents {


        public class TileComponent {
            public string Name { get; set; }
            public int[,] Matrix { get; set; }
            // lista de tipuri reprezinta o legenda a matricii asociata acestui tile
            // fiecarui id din matrice ii este asociat un anumit tip care spune ceva despre structura din care id-ul face parte
            public List<ComponentType> Types { get; set; } 
    

            public TileComponent(string name, int[,] matrix, List<ComponentType> types)
            {
                this.Name = name;
                this.Matrix = matrix;
                this.Types = types;
            }


            public TileComponent(TileComponent another) 
            {
                this.Name = another.Name;
                this.Matrix = (int[,]) another.Matrix.Clone();
                this.Types = another.CloneTypes();
            }


            public List<ComponentType> CloneTypes() 
            {
                List<ComponentType> returnList = new List<ComponentType>();
                foreach (var i in this.Types) 
                {
                    returnList.Add(i.Clone());
                }
                return returnList;
            }


            public TileComponent Clone() {
                return new TileComponent(this);
            }


            /**
            * returns a 5 x 5 matrix of "road", "field" and/or "city" for neigborhooding purposes
            */
            public T[,] GetCharacteristicMatrix<T>() 
            {
                var returnMatrix = new T[5, 5];

                for (var i = 0; i < 5; ++i) 
                {
                    for (var j = 0; j < 5; ++j)
                    {
                        returnMatrix[i, j] = this.GetCharacteristicInPosition<T>(i, j);
                    }
                }

                return returnMatrix;
            }


            /**
            * takes id of component in position, searches through all types and returns "field", "city" or "road" of component with searched id
            */
            public T GetCharacteristicInPosition<T>(int i, int j) 
            {
                var componentId = this.Matrix[i, j];
                if (componentId < 0) 
                {
                    return default(T);
                }

                foreach (var component in this.Types) 
                {
                    if (component.Id == componentId) 
                    {
                        return component.GetType<T>();
                    }
                }
      
                System.Console.WriteLine(componentId);
                System.Console.WriteLine(this.Name);
                throw new Exception("Characteristic not found");
            }





            /**
            * searches for to search in each component type and replaces id
            * then replaces matrix ids
            */
            public void ChangeComponentIds(int toSearch, int toReplace) 
            {
                foreach (var component in this.Types)  
                {
                    if (component.Id == toSearch) 
                    {
                        component.Id = toReplace;
                    }
                }

                for (var i = 0; i < this.Matrix.GetLength(0); ++i) 
                {
                    for (var j = 0; j < this.Matrix.GetLength(1); ++j) 
                    {
                        if (this.Matrix[i, j] == toSearch) 
                        {
                            this.Matrix[i, j] = toReplace;
                        }
                    }
                }
            }


            public bool ComponentHasShield(int componentId) 
            {
                foreach (var component in this.Types) 
                {
                    if (component.Id == componentId) 
                    {

                        if (component.Type != "city") 
                        {
                            throw new Exception("Asta trebuia sa fie oras");
                        }

                        return component.HasShield;
                    }
                }
                throw new Exception("nu exista componenta cautata in tile-ul asta");
            }


            /**
            * iterates through all components and returns a list of all neigbors for all fields with given fieldId
            */
            public List<int> GetNeighborCities(int fieldId) 
            {
                var returnList = new List<int>();

                foreach (var component in this.Types) 
                {
                    if (component.Id == fieldId) 
                    {

                        if (component.Type != "field") 
                        {
                            throw new Exception("Asta trebuia sa fie campie");
                        }

                        var neigborsIndices = component.Neighbors;
                        //each field may have neighbors
                        if (neigborsIndices == null) 
                        {
                            continue;
                        }
                        //each neighbor is the initial id (and index) of city in Tile Component
                        foreach (var neighborIndex in neigborsIndices) 
                        {
                            var toAdd = this.Types[neighborIndex].Id;
                            //visits index of city and takes its new Id
                            if (! returnList.Contains(toAdd)) 
                            {
                                returnList.Add(toAdd);
                            }
                        }
                    }
                }

                return returnList;
            }


            public override string ToString() 
            {
                return "{\n" + $"\"name\": {this.Name}\n\"matrix\": \n{this.PrintMatrix()}\n\"types\": {this.PrintTypes()}" + "\n}";
            }


            public string PrintMatrix() 
            {
                var x = new CustomArray<int>();
                return x.PrintArray(this.Matrix);
            }


            public string PrintTypes() 
            {
                var returnString = "";
                foreach (var t in this.Types) 
                {
                    if (returnString == "")
                    {
                        returnString += "[\n" + t.ToString();
                    } 
                    else
                    {
                        returnString += ", \n" + t.ToString();
                    }
                }
                return returnString + "\n]";

            }

        }


    }
}