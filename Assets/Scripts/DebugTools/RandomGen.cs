using System;

namespace DebugTools
{
    public class RandomGen
    {
        private bool intIsDefault = true;
        private int i;
        private readonly Random r;

        public RandomGen()
        {
            r = new Random();
        }

        public void SetInt(int val)
        {
            intIsDefault = false;
            i = val;
        }

        public int GetInt(int min, int max)
        {
            return intIsDefault ? r.Next(min, max) : i;
        }
    }
}