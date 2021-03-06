﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{

    public Vector3 WantedDirectionMove { get; protected set; } = new Vector3(0, 0, 1);
    public Vector3 WantedDirectionLook = new Vector3(0, 0, 1);
    public float WantedSpeed { get; protected set; } = 0;
    public bool WantsToJump { get; protected set; } = false;
    public bool WantsToDash { get; protected set; } = false;
    public bool WantsToSlice { get; protected set; } = false;

    public bool DrawDebugDirections = false;

    protected void DrawDebug()
    {
        if (DrawDebugDirections)
        {
            Debug.DrawLine(transform.position, transform.position + WantedDirectionMove * 2.0f, Color.blue);
            Debug.DrawLine(transform.position, transform.position + WantedDirectionLook * 2.0f, Color.green);
        }
    }
}
