using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LibCarcassonne
{
    namespace GameComponents
    {
        // In GameComponents avem componentele, Tile-urile parsate din JSON

        public class ComponentManager
        {


            public ComponentManager()
            {
                System.Console.WriteLine("ComponentManager");
            }


            /**
            * metoda custom de parsare json creata pentru modelul folosit
            */
            public List<TileComponent> ParseJson(string path)
            {
                string tilesJson = System.IO.File.ReadAllText(path);

                JArray tileArray = JArray.Parse(tilesJson);
                var tileComponenets = new List<TileComponent>();

                int i = 0, j = 0;
                foreach (var tile in tileArray)
                {

                    var name = (string)tile["name"];
                    var matrixJson = (JArray)tile["matrix"];
                    var matrix = new int[5, 5];

                    i = 0;
                    foreach (var row in matrixJson)
                    {
                        j = 0;
                        foreach (var element in (JArray)row)
                        {
                            matrix[i, j] = Int32.Parse((string)element);
                            j++;
                        }
                        i++;
                    }

                    var typesJson = (JArray)tile["types"];
                    var types = new List<ComponentType>();
                    i = 0;
                    foreach (var t in typesJson)
                    {

                        var center = new List<float>();
                        var centerJson = (JArray)t["center"];
                        foreach (var element in centerJson)
                        {
                            center.Add(float.Parse((string)element));
                        }

                        var type = (string)t["type"];
                        var shieldJson = t["shield"];
                        bool shield;
                        if (shieldJson == null)
                        {
                            shield = false;
                        }
                        else
                        {
                            shield = bool.Parse((string)shieldJson);
                        }


                        var neighbors = new List<int>();
                        var neighborsJson = (JArray)t["neighbour"];
                        if (neighborsJson == null)
                        {
                            neighbors = null;
                        }
                        else
                        {
                            foreach (var element in (JArray)t["neighbour"])
                            {
                                neighbors.Add(Int32.Parse((string)element));
                            }
                        }

                        types.Add(new ComponentType(i, type, neighbors, center, shield));
                        i++;
                    }

                    tileComponenets.Add(new TileComponent(name, matrix, types));
                }

                return tileComponenets;
            }


        }


    }
}