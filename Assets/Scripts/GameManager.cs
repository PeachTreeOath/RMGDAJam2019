using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{

    public Image completeFill;
    public TextMeshProUGUI completeText;

    private int completeGameTileCount;
    private int currCompletedTiles;
    private float percentToCompletion = .75f;
    private int damageCount = 75;
    private Stack<ColorTile> lastTiles = new Stack<ColorTile>();

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
        CheckVictory();
    }

    public void DecompleteTile(ColorTile tile)
    {
        tile.SwitchToBnW();
        currCompletedTiles--;

        UpdateScore();
    }

    public void SetTileCount(int numTiles)
    {
        completeGameTileCount = (int)(percentToCompletion * numTiles);
    }

    public void UpdateScore()
    {
        float ratio = (float)currCompletedTiles / completeGameTileCount;
        completeFill.fillAmount = ratio;
        int ratioText = (int)(ratio * 100);
        completeText.text = ratioText + "%";
    }

    private void CheckVictory()
    {
        //todo show only 100% in the victory bar, maybe a mark at 75%?
        if (currCompletedTiles >= completeGameTileCount)
        {
            Debug.Log("BEAT GAME");
        }
    }
}
