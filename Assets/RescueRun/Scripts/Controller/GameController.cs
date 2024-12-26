using System.Collections;
using System.Collections.Generic;
using Lib;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
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
        [SerializeField] private CameraTransition cameraTransition;

        [Header("About Game")]
        [SerializeField] private int coin;
        [SerializeField] private int stamina;
        [SerializeField] private float speed = 2;
        [SerializeField] private int inCome;
        [SerializeField] private int seedRandom;

        [Header("About Zone")]
        [SerializeField] private Transform dangerZone;


        [Header("Game State")]
        [SerializeField] private GameState curState;
        private GameState prevState;

        public GameState GetCurrentState() => curState;
        public void SetCurrentState(GameState gameState) => curState = gameState;

        public Lines GetLineDatabase() => lineDatabase.Line;
        public int GetSeed() => seedRandom;
        public int GetStamina() => stamina;
        public float GetSpeed() => speed;
        public Player GetPlayer() => player;

        private void Awake()
        {
            Init();
        }

        private void Start()
        {
            prevState = curState;
            player.Initialize();
        }

        private void Update()
        {
            var currentState = GameController.Instance.GetCurrentState();

            if (currentState != prevState)
            {
                prevState = currentState;

                switch (currentState)
                {
                    case GameState.Setup:
                        StartCoroutine(cameraTransition.TransitionSetUpGame());
                        levelController.Initialize();
                        break;

                    case GameState.Start:
                        cameraTransition.Show("CameraJoystick");
                        break;

                    case GameState.MainMenu:
                        GetDataFromPlayerPrefs();
                        cameraTransition.Show("CameraKeyboard");
                        break;

                    case GameState.Win:
                        break;
                    case GameState.Lose:
                        break;
                    case GameState.Pause:
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
            //levelController.Initialize();
            //player.Initialize();
            Controller.Initialize(Watermelon.InputType.Keyboard);
        }

        private void GetDataFromPlayerPrefs()
        {
            if (PlayerPrefs.GetFloat("Speed").IsUnityNull())
            {
                speed = PlayerPrefs.GetFloat("Speed");
            }
            if (PlayerPrefs.GetFloat("Stamina").IsUnityNull())
            {
                stamina = PlayerPrefs.GetInt("Stamina");
            }
            if (PlayerPrefs.GetFloat("Coin").IsUnityNull())
            {
                coin = PlayerPrefs.GetInt("Coin");
            }
            if (PlayerPrefs.GetFloat("Income").IsUnityNull())
            {
                inCome = PlayerPrefs.GetInt("Income");
            }
        }





        private void OnValidate()
        {
            if (player == null)
            {
                player = GameObject.Find("Player").GetComponent<Player>();
            }
        }

    }
}

