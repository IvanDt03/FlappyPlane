
using System;

namespace FlappyPlane.Model;

public static  class RandomExtentions
{
    public static float NextSingle(this Random rand, float min, float max)
    {
        return min + (rand.NextSingle() * (max - min));
    }
}
