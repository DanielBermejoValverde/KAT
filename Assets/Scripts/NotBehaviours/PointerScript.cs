using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerScript
{
    public static Vector3 CursorToWorld(Vector3 cursor)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(cursor);
        worldPosition.z = 0.0f;
        return worldPosition;
    }
}
