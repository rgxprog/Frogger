using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //-------------------------------------------

    public enum GameState
    {
        MainMenu,
        InGame,
        Pause,
        Win,
        GameOver
    }

    //-------------------------------------------

    static public GameManager instance;

    public GameObject RiverTransports;
    public GameObject StreetCars;
    public GameObject Goals;
    public GameObject smallLog;
    public GameObject medLog;
    public GameObject largeLog;
    public GameObject turtle;
    public List<GameObject> cars;
    public AudioSource GoalSound, WinSound, IntroSound;
    public GameState gameState { get; private set; }
    public TMPro.TextMeshProUGUI livesCount;
    public GameObject levelCanvas;

    private int frogsInGoalCount;
    private int lives;
    private int level;
    private float winTime;

    //-------------------------------------------

    private void Awake()
    {
        instance = this;

        CreateCars();
        CreateLogs();
        CreateTurtles();

        gameState = GameState.MainMenu;
    }

    //-------------------------------------------

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && gameState == GameState.InGame)
        {
            GetComponent<MenuManager>().PauseGame();
        }

        if (gameState == GameState.Win)
        {
            winTime += Time.deltaTime;

            if (winTime > 2f)
            {
                winTime = 0;
                gameState = GameState.InGame;
                ResetGoals();
            }
        }
    }

    //-------------------------------------------

    private IEnumerator PlayIntroSound()
    {
        IntroSound.Play();
        yield return new WaitForSeconds(IntroSound.clip.length);
        gameState = GameState.InGame;
    }

    //-------------------------------------------

    public void StartGame()
    {
        ResetGame();
        //StartCoroutine(PlayIntroSound());
        IntroSound.Play();
        gameState = GameState.InGame;
        levelCanvas.GetComponent<LevelTextLogic>().ShowLevel(level);
    }

    //-------------------------------------------

    public void PauseGame()
    {
        gameState = GameState.Pause;
        Time.timeScale = 0;
    }

    //-------------------------------------------

    public void ResumeGame()
    {
        gameState = GameState.InGame;
        Time.timeScale = 1f;
    }

    //-------------------------------------------

    private void ResetGame()
    {
        lives = 3;
        livesCount.text = lives.ToString();
        level = 1;
        winTime = 0;
        ResetGoals();
    }

    //-------------------------------------------

    private void ResetGoals()
    {
        frogsInGoalCount = 0;
        
        foreach (Transform item in Goals.transform)
        {
            item.Find("FrogWin").gameObject.SetActive(false);
        }
    }

    //-------------------------------------------

    private void CreateCars()
    {
        // First row
        float x = cars[0].GetComponent<SpriteRenderer>().localBounds.size.x;
        float separation = x * 4;
        for (int i = 0; i < 5; i++)
        {
            float startX = -8 + separation * i + i * x;
            InstantiateMovingObject(cars[0], StreetCars.transform, startX, -3.25f, -1, 1.3f);
        }

        // Second row
        x = cars[1].GetComponent<SpriteRenderer>().localBounds.size.x;
        separation = x * 4;
        for (int i = 0; i < 5; i++)
        {
            float startX = -8 + separation * i + i * x;
            InstantiateMovingObject(cars[1], StreetCars.transform, startX, -2.59f, 1, 1.3f);
        }

        // Third row
        x = cars[2].GetComponent<SpriteRenderer>().localBounds.size.x;
        separation = x * 4;
        for (int i = 0; i < 5; i++)
        {
            float startX = -8 + separation * i + i * x;
            InstantiateMovingObject(cars[2], StreetCars.transform, startX, -1.93f, -1, 1.5f);
        }

        // Fourth row
        x = cars[3].GetComponent<SpriteRenderer>().localBounds.size.x;
        separation = x * 4;
        for (int i = 0; i < 2; i++)
        {
            float startX = -8 + separation * i + i * x;
            InstantiateMovingObject(cars[3], StreetCars.transform, startX, -1.27f, 1, 5f);
        }

        // Fourth row
        x = cars[4].GetComponent<SpriteRenderer>().localBounds.size.x;
        separation = x * 3.5f;
        for (int i = 0; i < 3; i++)
        {
            float startX = -8 + separation * i + i * x;
            InstantiateMovingObject(cars[4], StreetCars.transform, startX, -0.61f, -1, 1.1f);
        }
    }

    //-------------------------------------------

    private void CreateLogs()
    {
        // Small logs
        float x = smallLog.GetComponent<SpriteRenderer>().localBounds.size.x;
        for (int i = 0; i < 6; i++)
        {
            float startX = -8 + x + i * 2 * x;
            InstantiateMovingObject(smallLog, RiverTransports.transform, startX, 1.37f, -1, 1.5f);
        }

        // Large logs
        x = largeLog.GetComponent<SpriteRenderer>().localBounds.size.x;
        for (int i = 0; i < 4; i++)
        {
            float startX = -8 + x + i * 2 * x;
            InstantiateMovingObject(largeLog, RiverTransports.transform, startX, 2.03f, -1, 2.5f);
        }

        // Large logs
        x = largeLog.GetComponent<SpriteRenderer>().localBounds.size.x;
        for (int i = 0; i < 4; i++)
        {
            float startX = -8 + x + i * 2 * x;
            InstantiateMovingObject(medLog, RiverTransports.transform, startX, 3.35f, -1, 2f);
        }
    }

    //-------------------------------------------

    private void CreateTurtles()
    {
        // Row near sidewalk
        float x = turtle.GetComponent<SpriteRenderer>().localBounds.size.x;
        float separation = x * 7;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                float startX = -8 + separation * i + x + j * x;
                GameObject t = InstantiateMovingObject(turtle, RiverTransports.transform, startX, 0.71f, 1, 2f);
                
                if (i == 2) t.GetComponent<TurtleLogic>().SetEnableSink();
            }
        }

        // Row middle river
        separation = x * 6;
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                float startX = -8 + separation * i + x + j * x;
                GameObject t = InstantiateMovingObject(turtle, RiverTransports.transform, startX, 2.69f, 1, 1.5f);
                
                if (i == 4 || i == 2) t.GetComponent<TurtleLogic>().SetEnableSink();
            }
        }
    }

    //-------------------------------------------

    private GameObject InstantiateMovingObject(GameObject obj, Transform parent, float startX, float startY, int direction, float speed)
    {
        GameObject newObj = Instantiate(obj, Vector3.zero, Quaternion.identity);
        newObj.GetComponent<HorizontalMove>().Init(new Vector3(startX, startY, 0), direction, speed);
        newObj.transform.parent = parent;

        return newObj;
    }

    //-------------------------------------------

    public bool IsOnTransport(Transform frog)
    {
        foreach (Transform item in RiverTransports.transform)
        {
            float difX = item.GetComponent<SpriteRenderer>().localBounds.size.x / 2;
            float difY = item.GetComponent<SpriteRenderer>().localBounds.size.y / 2;
            if (
                frog.position.y > item.position.y - difY && frog.position.y < item.position.y + difY &&
                frog.position.x > item.position.x - difX && frog.position.x < item.position.x + difX
                )
            {
                if (item.CompareTag("Turtle"))
                {
                    return !item.GetComponent<TurtleLogic>().IsTotalSink();
                }

                return true;
            }
        }

        return false;
    }

    //-------------------------------------------

    public bool IsOnGoal(Transform frog)
    {
        foreach (Transform item in Goals.transform)
        {
            float difX = item.Find("Sprite").GetComponent<SpriteRenderer>().localBounds.size.x / 2;
            float difY = item.Find("Sprite").GetComponent<SpriteRenderer>().localBounds.size.y / 2;
            if (
                frog.position.y > item.position.y - difY && frog.position.y < item.position.y + difY &&
                frog.position.x > item.position.x - difX && frog.position.x < item.position.x + difX &&
                !item.Find("FrogWin").gameObject.activeInHierarchy
                )
            {
                item.Find("FrogWin").gameObject.SetActive(true);
                frogsInGoalCount++;
                GoalSound.Play();
                if (frogsInGoalCount >= Goals.transform.childCount)
                {
                    gameState = GameState.Win;
                    level++;
                    levelCanvas.GetComponent<LevelTextLogic>().ShowLevel(level);
                    WinSound.Play();
                }

                return true;
            }
        }

        return false;
    }

    //-------------------------------------------

    public void LostLive()
    {
        lives--;
        livesCount.text = lives.ToString();

        if (lives <= 0)
        {
            gameState = GameState.GameOver;
            GetComponent<MenuManager>().ShowGameOver();
        }
    }

    //-------------------------------------------
}
