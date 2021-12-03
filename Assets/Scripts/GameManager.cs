using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject scoreText, endGame, brick, powerUp;

    [SerializeField]
    Vector2 startPos;

    [SerializeField]
    List<Sprite> brickSprite, powerSprites;

    public static GameManager instance;

    bool hasGameFinsished;
    int score, comboMul;

    readonly int[,,] levels = new int[5, 5, 10]
   {
        {
            {1,1,1,1,1,1,1,0,0,0},
            {1,1,1,1,1,1,1,0,0,0},
            {1,1,1,1,1,1,1,0,0,0},
            {1,1,1,1,1,1,1,0,0,0},
            {1,1,1,1,1,1,1,0,0,0}
        },
        {
            {1,1,1,1,1,1,1,0,0,0},
            {2,2,2,2,2,2,2,0,0,0},
            {3,3,3,3,3,3,3,0,0,0},
            {4,4,4,4,4,4,4,0,0,0},
            {5,5,5,5,5,5,5,0,0,0}
        },
        {
            {1,1,1,1,1,1,1,0,0,0},
            {1,0,0,0,0,0,1,0,0,0},
            {1,0,0,0,0,0,1,0,0,0},
            {1,0,0,0,0,0,1,0,0,0},
            {1,1,1,1,1,1,1,0,0,0}
        },
        {
            {2,1,2,1,2,1,2,0,0,0},
            {1,2,1,2,1,2,1,0,0,0},
            {2,1,2,1,2,1,2,0,0,0},
            {1,2,1,2,1,2,2,0,0,0},
            {2,1,2,1,2,1,2,0,0,0}
        },
        {
            {4,2,0,2,4,0,0,0,0,0},
            {4,2,0,2,4,0,0,0,0,0},
            {4,2,0,2,4,0,0,0,0,0},
            {4,2,0,2,4,0,0,0,0,0},
            {4,2,0,2,4,0,0,0,0,0}
        },
   };

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Time.timeScale = 1f;
        score = 0;
        comboMul = 1;
        hasGameFinsished = false;
        scoreText.GetComponent<Text>().text = "Score : " + score;
        endGame.SetActive(false);
        SpawnBricks();
    }

    void SpawnBricks()
    {
        int level = PlayerPrefs.GetInt("Level");
        float x = brick.GetComponent<BoxCollider2D>().size.x;
        float y = brick.GetComponent<BoxCollider2D>().size.y;
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                if(levels[level-1,i,j] != 0)
                {
                    GameObject temp =  Instantiate(brick);
                    temp.transform.position = startPos + new Vector2(j * x, -i * y);
                    temp.GetComponent<SpriteRenderer>().sprite = brickSprite[levels[level - 1,i,j] - 1];
                }
            }
        }
    }

    private void Update()
    {
        if (hasGameFinsished) return;
        GameFinished();
    }

    public void UpdateScore(Vector3 pos)
    {
        score += 100 * comboMul;
        comboMul++;
        scoreText.GetComponent<Text>().text = "Score : " + score;

        if(Random.value > 0.75f)
        {
            GameObject temp = Instantiate(powerUp,pos, Quaternion.identity);
            int type = Random.Range(1,6);
            temp.GetComponent<Power>().type = type;
            temp.GetComponent<SpriteRenderer>().sprite = powerSprites[type - 1];
        }
    }

    public void GameFinished()
    {
        if (GameObject.FindGameObjectsWithTag("Brick").Length != 0) return;
        int currentLevel = PlayerPrefs.GetInt("Level");
        currentLevel %= 5;
        currentLevel++;
        PlayerPrefs.SetInt("Level", currentLevel);
        hasGameFinsished = true;
        EndGame();
    }

    public void ResetCombo()
    {
        comboMul = 1;
    }

    public void EndGame()
    {
        if (GameObject.FindGameObjectsWithTag("Ball").Length != 1 && !hasGameFinsished) return;
        Time.timeScale = 0f;
        endGame.SetActive(true);
    }

    public void GameQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void GamePlay()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void GoToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
