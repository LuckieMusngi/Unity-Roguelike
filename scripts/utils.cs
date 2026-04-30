using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public static class Utils
{
    //gets the angle in degrees from one point to another 
    public static float GetAngle(Vector2 from, Vector2 to)
    {
        Vector2 dif = to - from;
        return Mathf.Atan2(dif.y, dif.x) * Mathf.Rad2Deg;
    }

    public static Vector2 GetVector(float rotz, float distance)
    {
        rotz *= Mathf.Deg2Rad;
        float y = distance * Mathf.Sin(rotz);
        float x = distance * Mathf.Cos(rotz);
        return new Vector2(x, y);
    }

    public static void ClearLog()
    {
        Assembly assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
        System.Type type = assembly.GetType("UnityEditor.LogEntries");
        MethodInfo method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }

    public static void ClearGoNulls(List<GameObject> list)
    {
        for (int i = list.Count - 1; i > -1; i--)
        {
            if (list[i] == null)
            {
                list.RemoveAt(i);
            }
        }
    }

    public static bool InArray(Vector2Int[] array, int current)
    {
        for (int i = 0; i < current; i++)
        {
            if (array[current] == array[i])
            {
                return true;
            }
        }
        return false;
    }

}