using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cursor Animation Clip", menuName = "Scriptable Objects/Cursor Animation Clip")]
public class CursorAnimationClip : ScriptableObject
{
    [Header("Cursor Setting")]
    public Vector2 cursorOffset;
    public CursorMode cursorMode;

    [Header("Animation Setting")]
    public Texture2D[] textureArray;
    public int frameRate;
    public bool loop;
}
