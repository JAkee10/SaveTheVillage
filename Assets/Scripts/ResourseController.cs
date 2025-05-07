using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class ResourseController : MonoBehaviour
{

    [SerializeField] private Text _wheatCountText;
    [SerializeField] private Text _peasantCountText;
    [SerializeField] private Text _warriorCountText;
    [SerializeField] private Text _notEnoughWheatText;

    [SerializeField] private Button _hirePeasantButton;
    [SerializeField] private Button _hireWarriorButton;

    [SerializeField] private Image _hirePeasantButtonImg;
    [SerializeField] private Image _hireWarriorButtonImg;
    [SerializeField] private Image _wheatLoadBarImg;
    [SerializeField] private Image _warriorMealBarImg;


    public int wheatCountTarget;
    public int peasantCountTarget;
    public int wheatStartValue;
    public int peasantStartValue;
    public int warriorStartValue;
    public int peasantHireCount;
    public int warriorHireCount;
    public int peasantHireCost;
    public int warriorHireCost;
    public int warriorMealMultiply;
    public float warriorMealCycleLength;
    public float wheatCycleLength;
    public float wheatPerSecond;
    public float peasantCooldownLength;
    public float warriorCooldownLength;
    public bool isGameVictory;


    private float notEnoughToHireDisappearanceDuration;
    private float notEnoughToHireVisability;
    private float notEnoughToHireCurrentTime;
    private float peasantCurrentTime;
    private float warriorCurrentTime;
    private float wheatCurrentTime;
    private float warriorMealCurrentTime;
    private float warriorMealBarGreenHue;
    private float wheatBarRedHue;
    private bool cooldownPeasantActivated;
    private bool cooldownWarriorActivated;
    private bool isFirstPeasantIteration;
    private bool isFirstWarriorIteration;
    private bool notEnoughToHire;
    private Color notEnoughToHireColor;



    private void Start()
    {
        SetStartValues();
    }


    private void Update()
    {

        if (Convert.ToInt32(_wheatCountText.text) >= wheatCountTarget && Convert.ToInt32(_peasantCountText.text) >= peasantCountTarget)
        {
            isGameVictory = true;
        }

        if (cooldownPeasantActivated)
        {
            CooldownPeasant();
        }
        if (cooldownWarriorActivated)
        {
            CooldownWarrior();
        }



        if (_peasantCountText.text != "0")
        {
            if (wheatCurrentTime <= wheatCycleLength)
            {
                wheatCurrentTime += Time.deltaTime;
                _wheatLoadBarImg.fillAmount = wheatCurrentTime / wheatCycleLength;
                wheatBarRedHue = 1 - (wheatCurrentTime / wheatCycleLength);
                _wheatLoadBarImg.color = new Color(wheatBarRedHue, 1, 0, 1);
            }
            else
            {
                _wheatLoadBarImg.fillAmount = 0;
                wheatCurrentTime = 0;
                _wheatCountText.text = (Convert.ToInt32(_wheatCountText.text) + (Convert.ToInt32(wheatPerSecond * wheatCycleLength) * Convert.ToInt32(_peasantCountText.text))).ToString();
            }
        }



        if (_warriorCountText.text != "0")
        {
            if (warriorMealCurrentTime <= warriorMealCycleLength)
            {
                warriorMealCurrentTime += Time.deltaTime;
                _warriorMealBarImg.fillAmount = warriorMealCurrentTime / warriorMealCycleLength;
                warriorMealBarGreenHue = 1 - (warriorMealCurrentTime / warriorMealCycleLength);
                _warriorMealBarImg.color = new Color(1, warriorMealBarGreenHue, 0, 1);
            }
            else
            {
                _warriorMealBarImg.fillAmount = 0;
                warriorMealCurrentTime = 0;

                if (Convert.ToInt32(_wheatCountText.text) < Convert.ToInt32(_warriorCountText.text))
                {
                    _warriorCountText.text = (Convert.ToInt32(_warriorCountText.text) - ((Convert.ToInt32(_warriorCountText.text) * warriorMealMultiply) - Convert.ToInt32(_wheatCountText.text))).ToString();
                    _wheatCountText.text = "0";
                }
                else
                {
                    _wheatCountText.text = (Convert.ToInt32(_wheatCountText.text) - (Convert.ToInt32(_warriorCountText.text) * warriorMealMultiply)).ToString();
                }

            }
        }
        else
        {
            _warriorMealBarImg.fillAmount = 0;
        }

        if (notEnoughToHire)
        {
            notEnoughToHireCurrentTime -= Time.deltaTime;
            notEnoughToHireVisability = notEnoughToHireCurrentTime / notEnoughToHireDisappearanceDuration;
            _notEnoughWheatText.color = new Color(249 / 255f, 255 / 255f, 59 / 255f, notEnoughToHireVisability);

            if (notEnoughToHireVisability <= 0)
            {
                notEnoughToHire = false;
            }
        }

    }


    private void HireElseBackend()
    {
        _notEnoughWheatText.color = notEnoughToHireColor;
        notEnoughToHireCurrentTime = notEnoughToHireDisappearanceDuration;
        notEnoughToHireVisability = 255;
        notEnoughToHire = true;
    }


    private void CooldownPeasant()
    {
        if (isFirstPeasantIteration)
        {
            isFirstPeasantIteration = false;
            _hirePeasantButton.enabled = false;
            _hirePeasantButtonImg.fillAmount = 0;
            peasantCurrentTime = 0;
        }

        peasantCurrentTime += Time.deltaTime;
        _hirePeasantButtonImg.fillAmount = peasantCurrentTime / peasantCooldownLength;

        if (peasantCurrentTime >= peasantCooldownLength)
        {
            isFirstPeasantIteration = true;
            cooldownPeasantActivated = false;
            _hirePeasantButton.enabled = true;
            IncreasePeasantCounter();
        }
    }

    private void CooldownWarrior()
    {
        if (isFirstWarriorIteration)
        {
            isFirstWarriorIteration = false;
            _hireWarriorButton.enabled = false;
            _hireWarriorButtonImg.fillAmount = 0;
            warriorCurrentTime = 0;
        }

        warriorCurrentTime += Time.deltaTime;
        _hireWarriorButtonImg.fillAmount = warriorCurrentTime / warriorCooldownLength;

        if (warriorCurrentTime >= warriorCooldownLength)
        {
            isFirstWarriorIteration = true;
            cooldownWarriorActivated = false;
            _hireWarriorButton.enabled = true;
            IncreaseWarriorCounter();
        }
    }


    private void HirePeasantButtonReset()
    {
        _hirePeasantButton.enabled = true;
        _hirePeasantButtonImg.fillAmount = 100;
        peasantCurrentTime = 0;
    }

    private void HireWarriorButtonReset()
    {
        _hireWarriorButton.enabled = true;
        _hireWarriorButtonImg.fillAmount = 100;
        warriorCurrentTime = 0;
    }


    private void IncreasePeasantCounter()
    {
        _peasantCountText.text = (Convert.ToInt32(_peasantCountText.text) + peasantHireCount).ToString();
    }


    private void IncreaseWarriorCounter()
    {
        _warriorCountText.text = (Convert.ToInt32(_warriorCountText.text) + warriorHireCount).ToString();
    }



    public void SetStartValues()
    {
        _wheatCountText.text = wheatStartValue.ToString(); // "10";
        _peasantCountText.text = peasantStartValue.ToString(); // "0";
        _warriorCountText.text = warriorStartValue.ToString(); // "2";

        notEnoughToHireDisappearanceDuration = 2.0f;
        notEnoughToHireVisability = 0;
        notEnoughToHireCurrentTime = 0;
        peasantCurrentTime = 0;
        warriorCurrentTime = 0;
        wheatCurrentTime = 0;
        warriorMealCurrentTime = 0;

        notEnoughToHire = false;
        cooldownPeasantActivated = false;
        cooldownWarriorActivated = false;
        isFirstPeasantIteration = true;
        isFirstWarriorIteration = true;
        isGameVictory = false;

        _notEnoughWheatText.text = "";
        notEnoughToHireColor = new Color(249 / 255f, 1, 59 / 255f, 1);

        HirePeasantButtonReset();
        HireWarriorButtonReset();
    }


    public void HirePeasant()
    {
        if (Convert.ToInt32(_wheatCountText.text) >= peasantHireCost)
        {
            _wheatCountText.text = (Convert.ToInt32(_wheatCountText.text) - peasantHireCost).ToString();
            cooldownPeasantActivated = true;
        }
        else
        {
            _notEnoughWheatText.text = "<b>Not enough wheat to hire <i>peasant</i></b>";
            HireElseBackend();
        }
    }


    public void HireWarrior()
    {
        if (Convert.ToInt32(_wheatCountText.text) >= warriorHireCost)
        {
            _wheatCountText.text = (Convert.ToInt32(_wheatCountText.text) - warriorHireCost).ToString();
            cooldownWarriorActivated = true;
        }
        else
        {
            _notEnoughWheatText.text = "<b>Not enough wheat to hire <i>warrior</i></b>";
            HireElseBackend();
        }
    }
}
