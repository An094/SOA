using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Platformer
{
    public class PlayerController : MonoBehaviour, IDamagable
    {
        [Header("References")]
        [SerializeField] Rigidbody rb;
        [SerializeField] Animator animator;
        [SerializeField] GroundChecker groundChecker;
        [SerializeField] InputReader input;

        [Header("Stats")]
        [SerializeField] private float _maxHP;

        [Header("Movement Settings")]
        [SerializeField] float moveSpeed = 6f;
        [SerializeField] float rotationSpeed = 15f;
        [SerializeField] float smoothTime = 0.2f;

        [Header("Jump Settings")]
        [SerializeField] float jumpForce = 10f;
        [SerializeField] float jumpDuration = 0.5f;
        [SerializeField] float jumpCooldown = 0f;
        [SerializeField] float gravityMultiplier = 3f;

        [Header("Dash Settings")]
        [SerializeField] float dashForce = 10f;
        [SerializeField] float dashDuration = 0.5f;
        [SerializeField] float dashCooldown = 1f;

        [Header("SharedVariables")]
        [SerializeField] private FloatVariable _currentHealthPercent;

        [Header("GameEvents")]
        [SerializeField] private GameEvent _onPlayerDied;

        Transform mainCam;

        const float ZeroF = 0f;

        float currentSpeed;
        float velocity;
        float jumpVelocity;
        float dashVelocity = 1f;

        Vector3 movement;
        Vector3 dashDirection;

        List<Timer> timers;
        CountdownTimer jumpTimer;
        CountdownTimer jumpCooldownTimer;
        CountdownTimer dashTimer;
        CountdownTimer dashCooldownTimer;

        StateMachine stateMachine;

        //Animator parameters
        static readonly int Speed = Animator.StringToHash("Speed");

        private void Awake()
        {
            mainCam = Camera.main.transform;
       
            rb.freezeRotation = true;

            //Setup Timers
            jumpTimer = new CountdownTimer(jumpDuration);
            jumpCooldownTimer = new CountdownTimer(jumpCooldown);

            jumpTimer.OnTimerStart += () => jumpVelocity = jumpForce;
            jumpTimer.OnTimerStop += () => jumpCooldownTimer.Start();

            dashTimer = new CountdownTimer(dashDuration);
            dashCooldownTimer = new CountdownTimer(dashCooldown);

            dashTimer.OnTimerStart += () => dashVelocity = dashForce;
            dashTimer.OnTimerStop += () =>
            {
                dashVelocity = 1.0f;
                jumpCooldownTimer.Start();
            };
            timers = new List<Timer>(4) { jumpTimer, jumpCooldownTimer, dashTimer, dashCooldownTimer };

            //State Machine
            stateMachine = new StateMachine();

            //Declare states
            var locomotionState = new LocomotionState(this, animator);
            var jumpState = new JumpState(this, animator);
            var dashState = new DashState(this, animator);

            //Define transitions
            At(locomotionState, jumpState, new FuncPredicate(() => jumpTimer.IsRunning));
            At(locomotionState, dashState, new FuncPredicate(() => dashTimer.IsRunning));
            Any(locomotionState, new FuncPredicate(() => groundChecker.IsGrounded && !jumpTimer.IsRunning && !dashTimer.IsRunning));

            //Set initial state
            stateMachine.SetState(locomotionState);
        }

        void At(IState from, IState to, IPredicate predicate) => stateMachine.AddTransition(from, to, predicate);
        void Any(IState to, IPredicate predicate) => stateMachine.AddAnyTransition(to, predicate);

        private void OnEnable()
        {
            input.EnablePlayerActions();
            input.Jump += OnJump;
            input.Dash += OnDash;
        }

        private void OnDisable()
        {
            input.Jump -= OnJump;
            input.Dash -= OnDash;
        }

        private void OnJump(bool performed)
        {
            if (performed && !jumpTimer.IsRunning && !jumpCooldownTimer.IsRunning && groundChecker.IsGrounded)
            {
                jumpTimer.Start();
            }
            else if (!performed && jumpTimer.IsRunning)
            {
                jumpTimer.Stop();
            }
        }

        private void OnDash(bool performed)
        {
            if (performed && !dashTimer.IsRunning && !dashCooldownTimer.IsRunning)
            {
                dashTimer.Start();
            }
            else if (!performed && dashTimer.IsRunning)
            {
                dashTimer.Stop();
            }
        }
        
        private void Update()
        {
            movement = new Vector3(input.Direction.x, 0f, input.Direction.y);

            stateMachine.Update();

            UpdateTimers();
            UpdateAnimator();
        }

        private void UpdateTimers()
        {
            foreach(var timer in timers)
            {
                timer.Tick(Time.deltaTime);
            }
        }

        private void FixedUpdate()
        {
            stateMachine.FixedUpdate();
        }

        public void HandleJump()
        {
            //If not jumping and grounded, keep jump velocity at 0
            if(!jumpTimer.IsRunning && groundChecker.IsGrounded)
            {
                jumpVelocity = ZeroF;
                jumpTimer.Stop();
                return;
            }

            //If jumping or failing calculate velocity
            if (!jumpTimer.IsRunning)
            {
                //Gravity takes over
                jumpVelocity += Physics.gravity.y * gravityMultiplier * Time.fixedDeltaTime;
            }

            //Apply velocity
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpVelocity, rb.linearVelocity.z);
        }

        private void UpdateAnimator()
        {
            animator.SetFloat(Speed, currentSpeed);
        }

        public void HandleMovement()
        {
            //Rotate movement direction to match camera rotation
            var adjustedDirection = Quaternion.AngleAxis(mainCam.eulerAngles.y, Vector3.up) * movement;

            if(adjustedDirection.magnitude > ZeroF)
            {
                HandleRotation(adjustedDirection);

                HandleHorizontalMovement(adjustedDirection);

                SmoothSpeed(adjustedDirection.magnitude);
            }
            else
            {
                SmoothSpeed(ZeroF);

                //Reset horizontal velocity for a snappy stop
                rb.linearVelocity = new Vector3(ZeroF, rb.linearVelocity.y, ZeroF);
            }
        }

        private void HandleHorizontalMovement(Vector3 adjustedDirection)
        {
            //Move the player
            var velocity = adjustedDirection * (dashVelocity * moveSpeed * Time.deltaTime);
            rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);
        }

        private void HandleRotation(Vector3 adjustedDirection)
        {
            var targetRotation = Quaternion.LookRotation(adjustedDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        private void SmoothSpeed(float value)
        {
            currentSpeed = Mathf.SmoothDamp(currentSpeed, value, ref velocity, smoothTime);
        }

        public bool CanTakeDamage() => true;

        public bool TakeDamage(float InDamage)
        {
            if(_currentHealthPercent.Value < 0f || Mathf.Approximately(_maxHP, 0f))
            {
                return false;
            }

            float newHealth = Mathf.Clamp(_currentHealthPercent.Value * _maxHP - InDamage, 0, _maxHP);
            float newHealthPercent = Mathf.Clamp01(newHealth /  _maxHP);
            _currentHealthPercent.Value = newHealthPercent;

            if(newHealthPercent <= 0f)
            {
                _onPlayerDied.Raise();
            }

            return true;
        }
    }
}
