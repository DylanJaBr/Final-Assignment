using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro; // Add this for TextMeshPro

public class GameEnding : MonoBehaviour
{
    public float fadeDuration = 1f;
    public float displayImageDuration = 1f;
    public float levelTimeLimit = 30f; // Level time limit in seconds
    public GameObject player;
    public CanvasGroup exitBackgroundImageCanvasGroup;
    public AudioSource exitAudio;
    public CanvasGroup caughtBackgroundImageCanvasGroup;
    public AudioSource caughtAudio;
    public TextMeshProUGUI timerText; // UI Text element for the timer

    bool m_IsPlayerAtExit;
    bool m_IsPlayerCaught;
    float m_Timer;
    float m_LevelTimer;
    bool m_HasAudioPlayed;

    void Start()
    {
        m_LevelTimer = levelTimeLimit; // Initialize the timer
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            m_IsPlayerAtExit = true;
        }
    }

    public void CaughtPlayer()
    {
        m_IsPlayerCaught = true;
    }

    void Update()
    {
        if (!m_IsPlayerAtExit && !m_IsPlayerCaught)
        {
            UpdateTimer();
        }

        if (m_IsPlayerAtExit)
        {
            EndLevel(exitBackgroundImageCanvasGroup, false, exitAudio);
        }
        else if (m_IsPlayerCaught || m_LevelTimer <= 0)
        {
            EndLevel(caughtBackgroundImageCanvasGroup, true, caughtAudio);
        }
    }

    void UpdateTimer()
    {
        m_LevelTimer -= Time.deltaTime;
        timerText.text = "Time Left: " + Mathf.Max(0, Mathf.CeilToInt(m_LevelTimer)) + "s";

        if (m_LevelTimer <= 0 && !m_IsPlayerCaught)
        {
            m_IsPlayerCaught = true; // Trigger the caught state if time runs out
        }
    }

    void EndLevel(CanvasGroup imageCanvasGroup, bool doRestart, AudioSource audioSource)
    {
        if (!m_HasAudioPlayed)
        {
            audioSource.Play();
            m_HasAudioPlayed = true;
        }

        m_Timer += Time.deltaTime;
        imageCanvasGroup.alpha = m_Timer / fadeDuration;

        if (m_Timer > fadeDuration + displayImageDuration)
        {
            if (doRestart)
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                Application.Quit();
            }
        }
    }
}

