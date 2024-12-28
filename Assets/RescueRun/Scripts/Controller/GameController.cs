using System.Collections;
using System.Collections.Generic;
using Lib;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Watermelon;
namespace RescueRun
{
    public class GameController : Lib.Singleton<GameController>
    {
        [Header("Reference")]
        [SerializeField] private LevelController levelController;
        [SerializeField] private UIController uiController;
        [SerializeField] private LineDatabase lineDatabase;
        [SerializeField] private BuffDatabase buffDatabase;
        [SerializeField] private Player player;
        [SerializeField] private PlayerMainMenu playerMainMenu;

        [SerializeField] private CameraTransition cameraTransition;
        [SerializeField] private CameraTransition cameraTransition1;

        [Header("About Game")]
        [SerializeField] private int increaseCoinClicked;
        [SerializeField] private int coin;
        [SerializeField] private int stamina;
        [SerializeField] private float speed = 2;
        [SerializeField] private int inCome;
        [SerializeField] private int seedRandom;

        //[Header("About Zone")]
        //[SerializeField] private Transform dangerZone;


        [Header("Game State")]
        [SerializeField] private GameState curState;
        private GameState prevState;
        public void AdditionCoin(int coin)
        {
            this.coin += coin;
            PlayerPrefs.SetInt("Coin", this.coin);
        }
        public void MinusCoin(int coin)
        {
            this.coin -= coin;
            PlayerPrefs.SetInt("Coin", this.coin);
        }

        public int GetIncreaseCoinClicked() => increaseCoinClicked;
        public CameraTransition GetCameraTransition() => cameraTransition1;
        public GameState GetCurrentState() => curState;

        public void SetSpeed(float value)
        {
            this.speed = value;
            PlayerPrefs.SetFloat("Speed", value);
        }
        public void SetStamina(int value)
        {
            this.stamina = value;
            PlayerPrefs.SetInt("Stamina", stamina);
        }
        public void SetIncome(int value)
        {
            this.inCome = value;
            PlayerPrefs.SetInt("Income", value);

        }
        public void SetCurrentState(GameState gameState) => curState = gameState;

        public BuffDatabase GetListBuff() => buffDatabase;
        public Lines GetLineDatabase() => lineDatabase.Line;
        public int GetIncome() => inCome;
        public int GetCoin() => coin;
        public int GetSeed() => seedRandom;
        public int GetStamina() => stamina;
        public float GetSpeed() => speed;
        public Player GetPlayer() => player;
        public PlayerMainMenu GetPlayerMainMenu() => playerMainMenu;

        private void Awake()
        {
            Init();
        }

        private void Start()
        {
            prevState = curState;
            playerMainMenu.Initialize();
        }

        private void Update()
        {
            if (curState == GameState.MainMenu)
            {
                UIController.instance.GetUIMainMenu().SetCoinText(coin.ToString());
            }
            if (Controller.InputType == InputType.Keyboard)
            {
                uiController.GetUIGameplay().GetJoystick().gameObject.SetActive(false);
            }
            else
            {
                uiController.GetUIGameplay().GetJoystick().gameObject.SetActive(true);
            }
            var currentState = GameController.Instance.GetCurrentState();

            if (currentState != prevState)
            {
                prevState = currentState;

                switch (currentState)
                {
                    case GameState.Setup:
                        Controller.SetControl(InputType.UIJoystick);
                        cameraTransition.gameObject.SetActive(false);
                        StartCoroutine(cameraTransition1.TransitionSetUpGame());
                        player.GetScanAnimal().GetAnimalTarget();
                        player.AssignAnimals();
                        break;

                    case GameState.Start:
                        cameraTransition.ShowOnly("CameraJoystick");
                        break;

                    case GameState.MainMenu:
                        Time.timeScale = 1;
                        Controller.SetControl(Watermelon.InputType.Keyboard);
                        cameraTransition.gameObject.SetActive(true);
                        cameraTransition1.gameObject.SetActive(false);
                        playerMainMenu.gameObject.SetActive(true);
                        GetDataFromPlayerPrefs();
                        cameraTransition.ShowOnly("CameraKeyboard");
                        break;

                    case GameState.Win:

                        uiController.GetUIGameplay().GetUIWin().Show();
                        break;
                    case GameState.Lose:
                        uiController.GetUIGameplay().GetUILose().Show();
                        break;
                    case GameState.Pause:
                        uiController.GetUIGameplay().GetUIPause().Show();
                        Time.timeScale = 0;
                        break;
                    default:
                        break;
                }
            }
        }


        private void Init()
        {
            GetDataFromPlayerPrefs();
            InitializeReference();

        }



        private void InitializeReference()
        {

            uiController.Initialize();
            cameraTransition.Initialize();

            Controller.Initialize(Watermelon.InputType.Keyboard);

        }

        private void GetDataFromPlayerPrefs()
        {
            if (PlayerPrefs.HasKey("Speed"))
            {
                speed = PlayerPrefs.GetFloat("Speed");
            }
            if (PlayerPrefs.HasKey("Stamina"))
            {
                stamina = PlayerPrefs.GetInt("Stamina");
            }
            if (PlayerPrefs.HasKey("Coin"))
            {
                coin = PlayerPrefs.GetInt("Coin");
                Debug.Log("Coinn :" + coin);
            }
            if (PlayerPrefs.HasKey("Income"))
            {
                inCome = PlayerPrefs.GetInt("Income");
            }
        }


        private void OnEnable()
        {
            Debug.Log("Registering sceneLoaded event.");
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            Debug.Log("Unregistering sceneLoaded event.");
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }


        private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
        {
            Debug.Log($"Scene Loaded : {scene.name}, Mode: {mode}");

            if (cameraTransition1 == null)
            {
                if (GameObject.Find("Cameras1") != null)
                {
                    cameraTransition1 = GameObject.Find("Cameras1").GetComponent<CameraTransition>();

                }
            }
            if (levelController == null)
            {
                if (GameObject.Find("LevelController") != null)
                {
                    playerMainMenu.gameObject.SetActive(false);
                    levelController = GameObject.Find("LevelController").GetComponent<LevelController>();
                    player = GameObject.Find("Player").GetComponent<Player>();

                    levelController.Initialize();
                    player.Initialize();
                    uiController.GetUIGameplay().ShowCountdownPanel();
                    curState = GameState.Setup;
                }


            }
        }


        private void OnValidate()
        {
            if (levelController == null)
            {
                if (GameObject.Find("LevelController") != null)
                {
                    levelController = GameObject.Find("LevelController").GetComponent<LevelController>();
                }
            }
            if (player == null)
            {
                if (GameObject.Find("Player") != null)
                {
                    player = GameObject.Find("Player").GetComponent<Player>();
                }
            }
            if (cameraTransition1 == null)
            {
                if (GameObject.Find("Cameras1") != null)
                {
                    cameraTransition1 = GameObject.Find("Cameras1").GetComponent<CameraTransition>();

                }
            }

        }

    }
}

