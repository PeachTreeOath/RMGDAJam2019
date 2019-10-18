using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{

    public Image completeFill;
    public TextMeshProUGUI completeText;
    public SpriteRenderer door1;
    public SpriteRenderer door2;
    public GameObject exitLeft;
    public GameObject exitRight;
    public bool wonGame;
    public float wonGameTime;

    private int completeGameTileCount;
    private int totalTiles;
    private int currCompletedTiles;
    private float percentToCompletion = .6f;
    private int damageCount = 75;
    private Stack<ColorTile> lastTiles = new Stack<ColorTile>();
    private bool isVictory;
    private float exitDistance = 7;
    private float cameraSpeed = 1;

    private void Update()
    {
        if (wonGame)
        {
            float timeElapsed = Time.time - wonGameTime;

            if (timeElapsed > 3)
            {
                SceneManager.LoadScene("Victory");
            }
        }

        if (isVictory)
        {
            float distance = PlayerController.instance.transform.position.x + 1.5f;

            if (distance > exitDistance)
            {
                exitLeft.SetActive(true);
                exitRight.SetActive(false);
            }
            else if (distance < -exitDistance)
            {
                exitLeft.SetActive(false);
                exitRight.SetActive(true);
            }
            else
            {
                exitLeft.SetActive(false);
                exitRight.SetActive(false);
            }
        }
    }

    public void HitTaken()
    {
        for (int i = 0; i < damageCount; i++)
        {
            if (lastTiles.Count != 0)
            {
                DecompleteTile(lastTiles.Pop());
            }
        }
    }

    public void CompleteTile(ColorTile tile)
    {
        if (tile.isColor)
            return;

        lastTiles.Push(tile);
        tile.SwitchToColor();
        currCompletedTiles++;
        UpdateScore();
        if (!isVictory)
            CheckVictoryAvailable();
    }

    public void DecompleteTile(ColorTile tile)
    {
        tile.SwitchToBnW();
        currCompletedTiles--;

        UpdateScore();
    }

    public void SetTileCount(int numTiles)
    {
        totalTiles = numTiles;
        completeGameTileCount = (int)(percentToCompletion * numTiles);
    }

    public void UpdateScore()
    {
        float ratio = (float)currCompletedTiles / totalTiles;
        completeFill.fillAmount = ratio;
        int ratioText = (int)(ratio * 100);
        completeText.text = ratioText + "%";
    }

    private void CheckVictoryAvailable()
    {
        if (currCompletedTiles >= completeGameTileCount)
        {
            isVictory = true;
            door1.enabled = false;
            door2.enabled = false;
        }
    }

    public void CheckVictory()
    {
        //todo show only 100% in the victory bar, maybe a mark at 75%?
        if (isVictory)
        {
            wonGame = true;
            wonGameTime = Time.time;
            ScreenFader.instance.StartOutro();
        }
    }
}
