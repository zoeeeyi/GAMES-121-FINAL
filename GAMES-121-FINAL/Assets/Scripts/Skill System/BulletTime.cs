using System.Collections;
using UnityEngine;

public class BulletTime : SkillParent
{
    [SerializeField] bool m_needMouseHold = true;
    [Range(0f, 1f)]
    [SerializeField] float m_targetTimeScale;
    //Interpolation
    [SerializeField] float m_speedOfChange = 2f;
    [SerializeField] float m_exponentialModifier = 10f;
    float m_timeWeStarted;
    IEnumerator ie_recoverTimeScale;


    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
        if (m_needMouseHold)
        {
            if (Input.GetButtonUp("Fire") && ie_recoverTimeScale != null) EndTimeSlow();
        }
    }

    protected override void ExecuteSkill()
    {
        Time.timeScale = m_targetTimeScale;
        //m_characterMovement.SetBulletTimeScaleMult();
        m_timeWeStarted = Time.time;
        StartTimeSlow();
    }

    void StartTimeSlow()
    {
        //Stop previous running coroutine
        if (ie_recoverTimeScale != null)
        {
            EndTimeSlow();
        }

        //Start a new coroutine
        ie_recoverTimeScale = RecoverTimeScale();
        StartCoroutine(ie_recoverTimeScale);
    }

    void EndTimeSlow()
    {
        Time.timeScale = 1;
        StopCoroutine(ie_recoverTimeScale);
        ie_recoverTimeScale = null;
    }

    IEnumerator RecoverTimeScale()
    {
        while (!Mathf.Approximately(Time.timeScale, 1) && Time.timeScale < 1)
        {
            //Time will slowly recover and become faster
            float _elapsedTime = Time.time - m_timeWeStarted;
            float _stepAmount = Mathf.Pow(_elapsedTime * m_speedOfChange, m_exponentialModifier);
            Time.timeScale = Mathf.MoveTowards(Time.timeScale, 1f, _stepAmount);
            yield return null;
        }

        EndTimeSlow();
    }
}
