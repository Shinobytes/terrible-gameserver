using System;

namespace Shinobytes.Terrible.Requests
{
    public class Appearance
    {
        public Appearance(int gender, int head, int hairColor, int body)
        {
            Gender = gender;
            Head = head;
            HairColor = hairColor;
            Body = body;
        }

        public static Appearance Random()
        {
            Random ran = new Random();
            return new Appearance(
                ran.NextDouble() >= 0.5 ? 1 : 0,
                0,
                0,
                0);
        }

        public int Gender { get; set; }
        public int Head { get; set; }
        public int HairColor { get; set; }
        public int Body { get; set; }
    }
}