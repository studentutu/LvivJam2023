using UnityEngine;

namespace Invector.vCharacterController
{
    public class vThirdPersonAnimator : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] public vThirdPersonMotor.vMovementSpeed freeSpeed, strafeSpeed;
        public bool useContinuousSprint;
        public bool isStrafing;

        public bool isGrounded;
        public float groundDistance;
        public Vector2 input;

        private bool isSprinting;
        private float horizontalSpeed;
        private float verticalSpeed;
        private float inputMagnitude;
        public bool isJumping;

        #region Variables

        public const float walkSpeed = 0.5f;
        public const float runningSpeed = 1f;
        public const float sprintSpeed = 1.5f;

        #endregion

        public void UpdateAnimator()
        {
            if (animator == null || !animator.enabled) return;

            // animator.SetBool(vAnimatorParameters.IsStrafing, isStrafing); ;
            animator.SetBool(vAnimatorParameters.IsSprinting, isSprinting);
            animator.SetBool(vAnimatorParameters.IsGrounded, isGrounded);
            animator.SetFloat(vAnimatorParameters.GroundDistance, groundDistance);


            animator.SetFloat(vAnimatorParameters.InputHorizontal, horizontalSpeed, strafeSpeed.animationSmooth,
                Time.deltaTime);
            animator.SetFloat(vAnimatorParameters.InputVertical, verticalSpeed, strafeSpeed.animationSmooth,
                Time.deltaTime);

            animator.SetFloat(vAnimatorParameters.InputMagnitude, inputMagnitude,
                isStrafing ? strafeSpeed.animationSmooth : freeSpeed.animationSmooth, Time.deltaTime);
        }

        public void SetAnimatorMoveSpeed(Vector2 move)
        {
            horizontalSpeed = move.x;
            verticalSpeed = move.y;

            var newInput = move;

            inputMagnitude = Mathf.Clamp(isSprinting ? newInput.magnitude + 0.5f : newInput.magnitude, 0,
                isSprinting ? sprintSpeed : runningSpeed);
        }

        public void Sprint(bool value)
        {
            var sprintConditions = (input.sqrMagnitude > 0.1f && isGrounded &&
                                    !(isStrafing && !strafeSpeed.walkByDefault && (horizontalSpeed >= 0.5 ||
                                        horizontalSpeed <= -0.5 || verticalSpeed <= 0.1f)));

            if (value && sprintConditions)
            {
                if (input.sqrMagnitude > 0.1f)
                {
                    if (isGrounded && useContinuousSprint)
                    {
                        isSprinting = !isSprinting;
                    }
                    else if (!isSprinting)
                    {
                        isSprinting = true;
                    }
                }
                else if (!useContinuousSprint && isSprinting)
                {
                    isSprinting = false;
                }
            }
            else if (isSprinting)
            {
                isSprinting = false;
            }
        }

        public void Jump()
        {
            // trigger jump behaviour
            isJumping = true;

            // trigger jump animations
            if (input.sqrMagnitude < 0.1f)
                animator.CrossFadeInFixedTime("Jump", 0.1f);
            else
                animator.CrossFadeInFixedTime("JumpMove", .2f);
        }
    }

    public static partial class vAnimatorParameters
    {
        public static int InputHorizontal = Animator.StringToHash("InputHorizontal");
        public static int InputVertical = Animator.StringToHash("InputVertical");
        public static int InputMagnitude = Animator.StringToHash("InputMagnitude");
        public static int IsGrounded = Animator.StringToHash("IsGrounded");
        public static int IsSprinting = Animator.StringToHash("IsSprinting");
        public static int GroundDistance = Animator.StringToHash("GroundDistance");
    }
}