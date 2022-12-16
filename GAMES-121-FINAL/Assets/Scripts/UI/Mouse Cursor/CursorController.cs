using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditorInternal.ReorderableList;

public class CursorController : MonoBehaviour
{
    public static CursorController instance;

    [HideInInspector] public UnityEvent<CursorAnimationClip> PlayCursorAnimation;

    #region Default Cursor
    private Texture2D m_defaultCursor;
    private Vector2 m_defaultCursorOffset;
    private CursorMode m_defaultCursorMode;
    #endregion

    #region Animator Variables
    private CursorAnimationClip m_activeCursorAnimation;
    private int m_currentFrame;
    private int m_totalFrame;
    private float m_frameInterval;
    private float m_frameTimer;
    #endregion

    private void Awake()
    {
        instance = this;

        PlayCursorAnimation.RemoveAllListeners();
        PlayCursorAnimation.AddListener(ExecuteCursorAnimation);
    }
    
    private void SetDefaultCursor()
    {
        Cursor.SetCursor(m_defaultCursor, m_defaultCursorOffset, m_defaultCursorMode);
    }

    public void SetDefaultCursor(Texture2D _default, Vector2 _offset, CursorMode _mode)
    {
        Cursor.SetCursor(_default, _offset, _mode);
        m_defaultCursor = _default;
        m_defaultCursorOffset = _offset;
        m_defaultCursorMode = _mode;
    }

    private void ExecuteCursorAnimation(CursorAnimationClip _cursorAnimation)
    {
        StopAllCoroutines();
        this.m_activeCursorAnimation = _cursorAnimation;
        m_currentFrame = 0;
        m_frameTimer = 0f;
        m_frameInterval = (float) 1 / _cursorAnimation.frameRate;
        m_totalFrame = _cursorAnimation.textureArray.Length;
        StartCoroutine(CursorAnimationPlayer());
    }

    IEnumerator CursorAnimationPlayer()
    {
        while (true)
        {
            m_frameTimer -= Time.deltaTime;
            if (m_frameTimer <= 0f)
            {
                Cursor.SetCursor(m_activeCursorAnimation.textureArray[m_currentFrame], m_activeCursorAnimation.cursorOffset, m_activeCursorAnimation.cursorMode);
                m_currentFrame = (m_currentFrame + 1) % m_totalFrame;
                m_frameTimer += m_frameInterval;
            }
            if (m_currentFrame + 1 >= m_totalFrame && !m_activeCursorAnimation.loop) break;
            yield return null;
        }
        SetDefaultCursor();
        yield return null;
    }
}
