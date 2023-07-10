using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionManager : MonoBehaviour
{
    private Vector2 outVec;
    private bool xPos, yPos;

    public Vector2 to8dir(Vector2 uVec)
    {
        if (uVec.x >= 0) { xPos = true; }
        else { xPos = false; }
        if (uVec.y >= 0) {  yPos = true; }
        else { yPos = false; }

        uVec.x = Mathf.Abs(uVec.x);
        uVec.y = Mathf.Abs(uVec.y);

        float angle = Mathf.Atan2(uVec.y, uVec.x) * Mathf.Rad2Deg;

        if (angle > 67.5f) { outVec = new Vector2(0, 1); }
        else if (angle < 22.5f) { outVec = new Vector2(1, 0); }
        else { outVec = new Vector2(1, 1).normalized; }
        
        if (!xPos) { outVec.x *= -1; }
        if (!yPos) { outVec.y *= -1; }
        
        return outVec.normalized;
    }

    public int toSprite(Vector2 uVec)
    {
        if (uVec.x == 0f)
        {
            if (uVec.y == 0f) { return 0; }
            if (uVec.y < 0f) { return 3; }
            if (uVec.y > 0f) { return 7; }
        }
        else if (uVec.y == 0f)
        {
            if (uVec.x < 0f) { return 5; }
            if (uVec.x > 0f) { return 1; }
        }
        else if (uVec.x == uVec.y)
        {
            if (uVec.x > 0f) { return 8; }
            if (uVec.x < 0f) { return 4; }
        }
        else if (uVec.x == -uVec.y)
        {
            if (uVec.x > 0f) { return 2; }
            if (uVec.x < 0f) { return 6; }
        }
        return -1;
    }
}
