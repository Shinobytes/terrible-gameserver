using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Shinobytes.Terrible
{
    public struct Vector2Int
    {
        public Vector2Int(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public Vector2Int(Vector2 vector)
        {
            this.X = (int) vector.X;
            this.Y = (int) vector.Y;
        }

        public int X { get; set; }
        public int Y { get; set; }
    }
}
