using System.Collections;
using System.Collections.Generic;
using Lib;
using TMPro;
using Unity.Services.Analytics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Watermelon;
namespace RescueRun
{
    public class Animal : MonoBehaviour
    {
        [Header("Reference")]
        [HideInInspector] public AnimalStateMachine stateMachine;
        [HideInInspector] public Animal_IdleState animalIdle;
        [HideInInspector] public Animal_MoveState animalMove;

        [Header("About Slider")]
        [SerializeField] private AnimalSlider animalSlider;
        [SerializeField] private float radius;

        [Header("About Scan")]
        [SerializeField] private float timeScan;
        [SerializeField] private float curTimeScan;
        [SerializeField] private float scanSpeed;
        [SerializeField] private bool isOverlapped;

        [Header("About Moving")]
        [SerializeField] private float speedMoving;
        [SerializeField] private float speedMoveToPlayer;
        [SerializeField] private float timeMoveToPlayer;
        [SerializeField] private float curTimeMoving;

        [SerializeField] private Vector3 currentVelocity;
        [SerializeField] private Vector3 defaultPos;

        [Header("About Component")]
        public Animator anim;

        public NavMeshAgent navMeshAgent;

        public bool isCollected;
        public bool isHolder;
        public UnityEvent<bool> onCollected = new UnityEvent<bool>();

        public bool isCat01;

        public bool IsOverlapped
        {
            get => isOverlapped;
            set
            {
                isOverlapped = value;
            }
        }

        private void Awake()
        {
            animalSlider = GetComponentInChildren<AnimalSlider>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            stateMachine = new AnimalStateMachine();
            animalIdle = new Animal_IdleState(stateMachine, this, "Idle");
            animalMove = new Animal_MoveState(stateMachine, this, "Run");
        }

        private void Start()
        {
            defaultPos = transform.position;
            stateMachine.Initialize(animalIdle);
            onCollected?.AddListener((bool a) =>
            {
                isCollected = true;
            });
        }

        private void Update()
        {
            if (GameController.Instance.GetCurrentState() == GameState.Start)
            {
                stateMachine.currentState.Update();
                Collection();
            }

        }

        private void FixedUpdate()
        {
            if (GameController.Instance.GetCurrentState() == GameState.Start)
                stateMachine.currentState.FixedUpdate();
        }

        public IEnumerator MoveToPlayer()
        {
            Transform holderCat01 = GameController.Instance.GetPlayer().GetTransformHolderCat01();
            Vector3 vPos = transform.position - holderCat01.position;
            Vector3 startPosition = transform.position;

            while (curTimeMoving <= 1)
            {
                curTimeMoving += Time.deltaTime * speedMoveToPlayer;
                float t = curTimeMoving / timeMoveToPlayer;

                transform.position = Vector3.Lerp(startPosition, vPos, t);
                transform.rotation = Quaternion.LookRotation(vPos);
                yield return null;
            }
            transform.position = vPos;
            Hide();


        }

        public Vector3 RandomPos()
        {
            return defaultPos + Random.onUnitSphere * radius;
        }

        public void Moving(Vector3 randomPos)
        {
            NavMeshPath path = new NavMeshPath();
            bool pathExists = navMeshAgent.CalculatePath(randomPos, path);

            if (pathExists && path.status == NavMeshPathStatus.PathComplete)
            {
                Debug.Log("Can move : " + randomPos);
                navMeshAgent.SetDestination(new Vector3(randomPos.x, 0, randomPos.z));
            }
        }




        private void Collection()
        {
            if (isHolder) return;
            if (isCollected)
            {
                animalSlider.HideSlider();
                if (isCat01)
                {
                    if (curTimeMoving <= 0.3f)
                    {
                        Transform holderCat01 = GameController.Instance.GetPlayer().GetTransformHolderCat01();
                        Vector3 vPos = holderCat01.position;
                        Vector3 startPosition = transform.position;

                        curTimeMoving += Time.deltaTime * speedMoveToPlayer;
                        float t = Mathf.Clamp01(curTimeMoving / timeMoveToPlayer);

                        transform.position = Vector3.Lerp(startPosition, vPos, t);
                        transform.rotation = Quaternion.LookRotation(transform.position - vPos);
                    }
                    else
                    {
                        Hide();
                    }
                }
                else Hide();

            }

            else if (isOverlapped)
            {
                Scanning();
            }
            else ResetScan();
        }

        public void Scanning()
        {
            curTimeScan += Time.deltaTime * scanSpeed;
            float value = curTimeScan / timeScan;
            animalSlider.SetSlider(value);

            bool isSliderFull = animalSlider.CheckIsSliderFull();
            if (isSliderFull) onCollected?.Invoke(isCat01);

            animalSlider.ShowSlider();
        }

        public void ResetScan()
        {
            animalSlider.SetSlider(0);
            curTimeScan = 0;

            animalSlider.HideSlider();
        }

        public void Show()
        {
            this.gameObject.SetActive(true);
        }

        public void Hide()
        {
            ObjectPool.Instance.ReturnToPool(transform.tag, this.gameObject);
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, radius);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("DangerZone"))
            {
                Hide();
            }
        }
    }
}

