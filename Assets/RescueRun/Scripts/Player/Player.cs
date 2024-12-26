using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Watermelon;
namespace RescueRun
{
    public class Player : MonoBehaviour
    {
        #region State Machine
        private PlayerStateMachine stateMachine;
        [HideInInspector] public PlayerIdleState playerIdle;
        [HideInInspector] public PlayerMoveState playerMove;
        #endregion

        #region Component Unity
        [HideInInspector] public Animator anim;
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

        [Header("Animal01 Holder")]
        [SerializeField] private Transform animals01Holder;
        [SerializeField] private int IdxAnimal01;

        [Header("Animal02 Holder")]
        [SerializeField] private Transform animals02Holder;
        [SerializeField] private int IdxAnimal02;

        private Vector2 input;

        public Vector3 GetCurrentVelocity() => currentVelocity;
        public Transform GetTransformHolderCat01() => animals01Holder;

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
            stateMachine = new PlayerStateMachine();
            playerIdle = new PlayerIdleState(stateMachine, this, "Idle");
            playerMove = new PlayerMoveState(stateMachine, this, "Move");

            rb = GetComponent<Rigidbody>();

        }


        private void Start()
        {
            stateMachine.Initialize(playerIdle);
            var listAnimals = LevelController.Instance.Animals;
            for (int i = 0; i < listAnimals.Count; i++)
            {
                listAnimals[i].onCollected.AddListener(ShowAnimal);
            }
        }

        private void Update()
        {
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
            if (Controller.InputType == InputType.UIJoystick)
            {
                UpdatePositionOfJoystick();
            }

            stateMachine.currentState.Update();
        }

        private void FixedUpdate()
        {
            stateMachine.currentState.FixedUpdate();
        }

        public void Initialize()
        {
            speed = GameController.Instance.GetSpeed();
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
                Debug.Log(stamina + Time.deltaTime * speedRegenStamina);
                UIController.Instance.GetUIGameplay().SetSlider(stamina);
                UIController.Instance.GetUIGameplay().SetTextSlider(stamina, (int)defaultStamina);
            }
        }

        public void ResetCountdownTimeStamina()
        {
            curTimeRegenStamina = timeRegenStamina;
        }

        public void ShowAnimal(bool isCat01)
        {
            GetAnimalFromHolder(isCat01);
        }


        public void GetAnimalFromHolder(bool isCat01)
        {
            if (isCat01)
            {
                var animal = animals01Holder.GetChild(IdxAnimal01++).gameObject;
                if (animal != null)
                {
                    if (!animal.activeSelf) animal.gameObject.SetActive(true);
                }
            }
            else
            {
                var animal = animals02Holder.GetChild(IdxAnimal02++).gameObject;
                if (animal != null)
                {
                    if (!animal.activeSelf) animal.gameObject.SetActive(true);
                }

            }
        }

        public void MovingWithJoyStick()
        {
            rb.velocity = currentVelocity;
            if (CheckMoving())
            {
                transform.forward = currentVelocity.normalized;
            }
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
            Debug.Log("Called couroutine : " + sta);
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
            if (Input.GetMouseButtonDown(0))
            {
                if (CanUseStamina())
                {
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

        public void UpdatePositionOfJoystick()
        {
            input = Watermelon.Joystick.Instance.input;
            currentVelocity = new Vector3(input.x * speed, 0, input.y * speed);

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("DangerZone"))
            {
                Debug.Log("You Lose");
            }
        }

    }
}

