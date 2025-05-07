using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class RaidController : MonoBehaviour
{

    [SerializeField] private ResourseController _resourseController;
    [SerializeField] private GameOverController _gameOverController;

    // [SerializeField] private GameObject _victoryScene;
    [SerializeField] private GameObject _gameOverScene;

    [SerializeField] private Image _raidTimerImg;

    [SerializeField] private Text _raidSizeText;
    [SerializeField] private Text _warriorCountText;


    public int raidCounter;
    public int minRaidCount;
    public int maxRaidCount;
    public float minRaidCountGrade;
    public float maxRaidCountGrade;
    public float timerLength;
    public bool isPlayerDefended;
    public bool isGameVictory;


    private float timerBackLength;
    private float currentTimerTime;
    private bool isReturn;
    private bool isFirstIteration;
    private Random random;



    public void PlayAgainButton()
    {
        SetStartValues();
        _resourseController.SetStartValues();
        _gameOverController.SetStartValues();
        isPlayerDefended = true;
        isGameVictory = false;
        _gameOverController.isGameOver = false;
        _gameOverController.isGameVictory = false;
    }


    private void Start()
    {
        SetStartValues();
    }


    private void Update()
    {

        if (_resourseController.isGameVictory)
        {
            this.isGameVictory = true;
        }
        else
        {
            this.isGameVictory = false;
        }

        if (isPlayerDefended && !isGameVictory)
        {
            _gameOverScene.SetActive(false);
            RaidCycle();
        }
        else if (isGameVictory)
        {
            _gameOverScene.SetActive(true);
            _gameOverController.isGameVictory = true;
        }
        else
        {
            _gameOverScene.SetActive(true);
            _gameOverController.isGameOver = true;
        }
    }

    private void SetStartValues()
    {
        raidCounter = 0;
        maxRaidCount = 5;
        timerBackLength = 2;
        currentTimerTime = 0;
        isReturn = false;
        isFirstIteration = true;
        isPlayerDefended = true;
        isGameVictory = false;
        random = new Random();

        _raidTimerImg.fillAmount = 0;
        _raidSizeText.text = (random.Next(minRaidCount, maxRaidCount)).ToString();
    }

    private void RaidCycle()
    {
        if (currentTimerTime < timerLength && !isReturn)
        {
            currentTimerTime += Time.deltaTime;

            if (currentTimerTime >= timerLength)
            {
                isReturn = true;
            }

            _raidTimerImg.fillAmount = currentTimerTime / timerLength;
        }
        else if (currentTimerTime > 0 && isReturn)
        {
            currentTimerTime -= Time.deltaTime;

            if (isFirstIteration)
            {
                raidCounter++;

                isPlayerDefended = RaidAttack();
                if (isPlayerDefended)
                {
                    _warriorCountText.text = (Convert.ToInt32(_warriorCountText.text) - Convert.ToInt32(_raidSizeText.text)).ToString();
                }
                else
                {
                    _raidSizeText.text = "<b>GAME OVER</b>";
                }

                currentTimerTime = timerBackLength;
                isFirstIteration = false;
            }
            if (currentTimerTime <= 0)
            {
                TimerReset();
            }

            _raidTimerImg.fillAmount = currentTimerTime / timerBackLength;
        }
    }
    private void TimerReset()
    {
        isReturn = false;
        isFirstIteration = true;

        minRaidCount = Convert.ToInt32(minRaidCount * minRaidCountGrade);
        maxRaidCount = Convert.ToInt32(maxRaidCount * maxRaidCountGrade);
        _raidSizeText.text = (random.Next(minRaidCount, maxRaidCount)).ToString();
    }

    private bool RaidAttack()
    {
        if (_raidSizeText != null && _warriorCountText != null)
        {
            if (Convert.ToInt32(_raidSizeText.text) <= Convert.ToInt32(_warriorCountText.text))
            {
                return true;
            }
        }
        else
        {
            if (_raidSizeText == null)
            {
                Debug.Log("Raid Size Text is null");
            }

            if (_warriorCountText == null)
            {
                Debug.Log("Warrior Count Text is null");
            }
        }
        return false;
    }
}
