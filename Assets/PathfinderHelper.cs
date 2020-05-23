using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PathfinderHelper
{
    public static int ManhattanDistance(int startX, int startY, int endX, int endY)
    {
        return Mathf.Abs(endX - startX) + Mathf.Abs(endY - endX);
    }
}
