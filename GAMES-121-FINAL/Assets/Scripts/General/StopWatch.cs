using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StopWatch : MonoBehaviour
{
    //[SerializeField] RecordManager m_recordManager;
    TextMeshProUGUI stopWatchUI;
    float stopWatch;
    double stopWatchRounded;
    GameController m_gameManager;
    //bool m_gameStarted = false;
    bool m_paused = false;

    // Start is called before the first frame update
    void Start()
    {
        //m_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        stopWatchUI = GetComponent<TextMeshProUGUI>();
        stopWatch = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_paused) return;
        stopWatch += Time.deltaTime;
        stopWatchRounded = System.Math.Round(stopWatch, 2);
        stopWatchUI.text = stopWatchRounded.ToString();
    }

    public void ResetStopWatch()
    {
        stopWatch = 0;
    }

    public void PauseStopWatch()
    {
        m_paused = true;
    }

    public void ContinueStopWatch()
    {
        m_paused = false;
    }

/*    public void RecordTime()
    {
        m_recordManager.UpdateRecord(stopWatch);
        this.gameObject.SetActive(false);
    }
*/}