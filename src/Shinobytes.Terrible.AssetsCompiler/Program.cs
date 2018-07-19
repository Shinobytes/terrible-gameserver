using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Shinobytes.Terrible.AssetsCompiler
{
    class Program
    {
        static void Main(string[] args)
        {
            var folder = @"E:\git\terrible\assets\sprites\characters\";

            SpricerSpritesheetToJsonConverter.ConvertFolder(folder);

            Console.WriteLine("Hello World!");
        }
    }

    public class SpricerSpritesheetToJsonConverter
    {
        public static void ConvertFolder(string folder)
        {
            var files = System.IO.Directory.GetFiles(folder, "*.txt");
            foreach (var file in files)
            {
                var fn = System.IO.Path.GetFileNameWithoutExtension(file) + ".json";
                var result = SpricerSpritesheetToJsonConverter.Convert(file);
                System.IO.File.WriteAllText(folder + fn, result);
            }
        }

        public static string Convert(string file)
        {
            var spritesheet = SpricerSpritesheetParser.Parse(file);

            return Convert(spritesheet);
        }

        public static string Convert(SpricerSpritesheet spritesheet)
        {            
            return JsonConvert.SerializeObject(spritesheet.Groups);
        }
    }


    public class SpricerSpritesheetParser
    {

        public static SpricerSpritesheet Parse(string file)
        {
            return ParseText(System.IO.File.ReadAllLines(file));
        }

        private static SpricerSpritesheet ParseText(string[] lines)
        {
            var spritesheet = new SpricerSpritesheet();
            SpricerSpritesheet.Group group = null;
            foreach (var line in lines.Select(x => x.Trim()))
            {
                if (line.StartsWith("#") || line.StartsWith("ColorKey"))
                {
                    continue;
                }

                if (line.StartsWith("["))
                {
                    var data = line.Substring(1).Split(']')[0].Split(':');
                    group = new SpricerSpritesheet.Group(data[1]);
                    spritesheet.Groups.Add(group);
                }
                else if (group != null)
                {
                    var data = line.Split(':');
                    var index = int.Parse(data[0]);
                    var position = data[1].Split(',');
                    var x = int.Parse(position[0]);
                    var y = int.Parse(position[1]);
                    var size = data[2].Split(',');
                    var w = int.Parse(size[0]);
                    var h = int.Parse(size[1]);
                    group.Sprites.Add(new SpricerSpritesheet.Sprite(index, x, y, w, h));
                }
            }
            return spritesheet;
        }
    }

    public class SpricerSpritesheet
    {
        public List<Group> Groups { get; } = new List<Group>();

        public class Sprite
        {
            public Sprite(int index, int x, int y, int width, int height)
            {
                Index = index;
                X = x;
                Y = y;
                Width = width;
                Height = height;
            }
            public int Index { get; }
            public int X { get; }
            public int Y { get; }
            public int Width { get; }
            public int Height { get; }
        }

        public class Group
        {
            public Group(string name)
            {
                Name = name;
            }

            public string Name { get; }

            public List<Sprite> Sprites { get; } = new List<Sprite>();
        }
    }
}
