using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Watermelon;
namespace RescueRun
{
    public class PlayerMainMenu : MonoBehaviour
    {
        #region State Machine
        private PlayerMainMenuStateMachine stateMachine;
        [HideInInspector] public PlayerMainMenuIdleState playerIdle;
        [HideInInspector] public PlayerMainMenuMoveState playerMove;
        #endregion

        #region Component Unity
        [SerializeField] public Animator anim;
        [HideInInspector] public Rigidbody rb;
        #endregion

        [Header("Properties Player")]
        [SerializeField] private float speed;
        [SerializeField] private float limitSpeedKeyboard;
        [SerializeField] private Vector3 currentVelocity;
        [SerializeField] private float deceleration;
        [SerializeField] private float aceleration;

        [Space(5)]
        [SerializeField] private float defaultStamina;
        [SerializeField] private int stamina;
        [SerializeField] private float consumeStamina;
        [SerializeField] private float timeRegenStamina;
        [SerializeField] private float speedRegenStamina;
        [SerializeField] private float curTimeRegenStamina;
        [SerializeField] private bool canUseStamina;


        private Vector2 input;
        public Vector3 GetCurrentVelocity() => currentVelocity;

        public float GetMeterFromPlayer() => transform.position.z;

        public void SetSpeed(float speed)
        {
            this.speed = speed;
        }

        public void SetStamina(int stamina)
        {
            this.stamina = stamina;
            defaultStamina = stamina;
        }

        private void Awake()
        {
            stateMachine = new PlayerMainMenuStateMachine();
            playerIdle = new PlayerMainMenuIdleState(stateMachine, this, "Idle");
            playerMove = new PlayerMainMenuMoveState(stateMachine, this, "Move");

            rb = GetComponent<Rigidbody>();

        }


        private void Start()
        {
            stateMachine.Initialize(playerIdle);
        }



        private void Update()
        {
            defaultStamina = GameController.Instance.GetStamina();
            if (stamina < defaultStamina)
            {
                RegenerationStamina();
            }
            else if (stamina >= defaultStamina)
            {
                canUseStamina = true;
                ResetCountdownTimeStamina();
                UIController.Instance.GetUIGameplay().ToDefaultColorSlider();
            }
            if (Controller.InputType == InputType.Keyboard)
            {
                UpdatePositionOfKeyBoard();
            }
            stateMachine.currentState.Update();
        }

        private void FixedUpdate()
        {
            stateMachine.currentState.FixedUpdate();
        }

        public void Initialize()
        {
            stamina = GameController.Instance.GetStamina();

            defaultStamina = stamina;
            curTimeRegenStamina = timeRegenStamina;
            canUseStamina = true;

            UIController.Instance.GetUIGameplay().SetMaxSlider(stamina);
            UIController.Instance.GetUIGameplay().SetSlider(stamina);
            UIController.Instance.GetUIGameplay().SetTextSlider(stamina, (int)defaultStamina);
            UIController.Instance.GetUIGameplay().ToDefaultColorSlider();
        }

        public void RegenerationStamina()
        {
            curTimeRegenStamina -= Time.deltaTime;
            if (curTimeRegenStamina <= 0)
            {
                stamina = (int)Mathf.Clamp(stamina + Time.deltaTime + speedRegenStamina, 0.0f, defaultStamina);
                UIController.Instance.GetUIGameplay().SetSlider(stamina);
                UIController.Instance.GetUIGameplay().SetTextSlider(stamina, (int)defaultStamina);
            }
        }

        public void ResetCountdownTimeStamina()
        {
            curTimeRegenStamina = timeRegenStamina;
        }


        public bool CheckMoving()
        {
            return currentVelocity.sqrMagnitude > 0.01f;
        }

        public void UsingStamina()
        {
            StartCoroutine(CouroutineChangeValueSlider(stamina, consumeStamina));
        }

        public IEnumerator CouroutineChangeValueSlider(int sta, float consumeStamina)
        {
            int targetSta = (int)Mathf.Clamp(sta - consumeStamina, 0, defaultStamina);
            if (sta <= 0)
            {
                canUseStamina = false;
            }
            while (stamina != targetSta)
            {
                stamina = (int)Mathf.MoveTowards(stamina, targetSta, Time.deltaTime * speedRegenStamina);
                UIController.Instance.GetUIGameplay().SetSlider(stamina);
                UIController.Instance.GetUIGameplay().SetTextSlider(stamina, (int)defaultStamina);
                yield return null;
            }
        }

        public bool CanUseStamina()
        {
            return Mathf.Sign(stamina - consumeStamina) > 0 && canUseStamina;
        }

        public void UpdatePositionOfKeyBoard()
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                if (CanUseStamina())
                {
                    Debug.Log("Using Keyboard");

                    int coinIncrease = GameController.Instance.GetIncreaseCoinClicked();
                    GameController.Instance.AdditionCoin(coinIncrease);
                    UsingStamina();
                    ResetCountdownTimeStamina();
                }
                else
                {
                    UIController.Instance.GetUIGameplay().ToUselessColorSlider();
                    canUseStamina = false;
                }
                input = new Vector2(0, aceleration);
            }
            else
            {
                float yVelocity = input.y;
                yVelocity -= Time.deltaTime * deceleration;
                input.y = Mathf.Max(yVelocity, 0);
            }
            currentVelocity = new Vector3(input.x * speed, 0, Mathf.Clamp(input.y * speed, 0, CanUseStamina() ? limitSpeedKeyboard : limitSpeedKeyboard / 3));
        }
    }
}

