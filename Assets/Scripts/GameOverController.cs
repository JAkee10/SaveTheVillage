using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// TOO: Не работает смена спрайта

public class GameOverController : MonoBehaviour
{

    [SerializeField] private Image _image;
    [SerializeField] private Sprite _sprite;


    [SerializeField] private RaidController _raidController;

    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Image _mainImage;

    [SerializeField] private Sprite _victoryMainSprite;
    [SerializeField] private Sprite _loseMainSprite;

    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _gameOverText;

    [SerializeField] private Button _playAgainButton;



    public float animationSpeed;
    public bool isGameOver;
    public bool isGameVictory;

    // private Image _mainImage;
    private float currentTime;
    private float visabilityValue;
    private bool isFirstIteration;
    private bool isSwitched;



    private void Start()
    {
        SetStartValues();
        isGameOver = false;
        isGameVictory = false;
        isSwitched = false;
        // _mainImage = GetComponent<Image>();
    }


    private void Update()
    {
        if (isGameVictory)
        {
            _mainImage = SwitchGameOverImageSprite(_mainImage, _victoryMainSprite);
            Debug.Log("isGameVictory ============================== true");
            GameVictory();
        }
        else if (isGameOver)
        {
            _mainImage = SwitchGameOverImageSprite(_mainImage, _loseMainSprite);
            Debug.Log("isGameOver = true");
            GameOver();
        }
    }




    public void SetStartValues()
    {
        isFirstIteration = true;
        currentTime = 0f;
        visabilityValue = 0f;

        _backgroundImage.color = new Color(87 / 255f, 26 / 255f, 26 / 255f, 0);                           // Не видно!
        _mainImage.color = new Color(1, 1, 1, 0);                                                         // Не видно!
        _gameOverText.color = new Color(1, 1, 1, 0);                                                      // Не видно!
        _playAgainButton.GetComponent<Image>().color = new Color(131 / 255f, 237 / 255f, 160 / 255f, 0);  // Не видно!ёё
        _playAgainButton.GetComponent<Text>().text = "Play again";
        _gameOverText.text = "";
        _scoreText.text = "";
    }


    public void GameOver()
    {
        if (isFirstIteration)
        {
            Debug.Log("GameOver!");
            // _mainImage = SwitchGameOverImageSprite(_mainImage, _loseMainSprite);
            // _mainImage.color = new Color(1, 1, 1, 0);                                                         // Не видно!
            _gameOverText.text = "<b>GAME OVER</b>";

            if (isFirstIteration)
                isFirstIteration = false;
        }

        GameOverAnimation();
    }


    public void GameVictory()
    {
        if (isFirstIteration)
        {
            Debug.Log("Victory!");
            // _mainImage = SwitchGameOverImageSprite(_mainImage, _victoryMainSprite);
            // _mainImage.color = new Color(1, 1, 1, 0);                                                         // Не видно!
            _backgroundImage.color = new Color(26 / 255f, 79 / 255f, 101 / 255f, 0);
            _gameOverText.text = "<b>VICTORY</b>";
            _scoreText.text = "Your score: " + _raidController.raidCounter.ToString();

            isFirstIteration = false;
        }

        GameOverAnimation();
    }





    private void GameOverAnimation()
    {
        if (currentTime < animationSpeed)
        {
            currentTime += Time.deltaTime;
        }

        visabilityValue = currentTime / animationSpeed;

        _backgroundImage.color = new Color(87 / 255f, 26 / 255f, 26 / 255f, visabilityValue);
        _mainImage.color = new Color(1, 1, 1, visabilityValue);
        _gameOverText.color = new Color(1, 1, 1, visabilityValue);
        _playAgainButton.GetComponent<Image>().color = new Color(131 / 255f, 237 / 255f, 160 / 255f, visabilityValue);
    }


    private Image SwitchGameOverImageSprite(Image image, Sprite sprite)
    {
        image.sprite = sprite;
        return image;
    }

}