using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public struct MinMaxFloat { public float min; public float max; }
[System.Serializable]
public struct MinMaxInt { public int min; public int max; }
public enum CardinalDirection { North, East, South, West }
public enum Direction { Up, Down, Left, Right, Forward, Backward }


public class Vector3Variable : ScriptableObject
{
    public Vector3 value;
}

[Serializable]
public class Vector3Reference
{
    public bool useConstant = true;
    public Vector3 constantValue;
    public Vector3Variable variable;

    public Vector3 value
    {
        get { return useConstant ? constantValue : variable.value; }
    }
}

public static class VariableTypes
{
    public static Vector3 GetDirection(Transform t, Direction direction)
    {
        switch(direction)
        {
            case Direction.Right:
                return t.right;
            case Direction.Left:
                return -t.right;
            case Direction.Up:
                return t.up;
            case Direction.Down:
                return -t.up;
            case Direction.Forward:
                return t.forward;
            case Direction.Backward:
                return -t.forward;
            default:
                return Vector3.zero;
        }
    }
}