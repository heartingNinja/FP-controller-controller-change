using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    // basic controller to work with animations. Still need to add jump
    public class PlayerControllerFP : MonoBehaviour
    {
        [SerializeField] Transform playercamera = null;
        [SerializeField] float mouseSensitivity = 3.5f;
        [SerializeField] float walkSpeed = 6f;
        [SerializeField] float gravity = -13f;

        [SerializeField] bool lockCursor = true;
        [SerializeField] [Range(0f, .5f)] float moveSmoothTime = .3f;
        [SerializeField] [Range(0f, .5f)] float mouseSmoothTime = .03f;

        public AnimatorHandler animatorHandler;
        public Animator animator;


        public float move;
        float cameraPitch = 0f;
        float velocityY = 0f;
        CharacterController controller = null;

        Vector2 currentDir = Vector2.zero;
        Vector2 currentDirVelocity = Vector2.zero;

        Vector2 currentMouseDelta = Vector2.zero;
        Vector2 currentMouseDeltaVelocity = Vector2.zero;

      

        private void Start()
        {
            // mouse lock 
            controller = GetComponent<CharacterController>();
            if (lockCursor)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        private void Update()
        {
            UpdateMouseLook();
            UpdateMovement();
            Dash();
            Sprint();
        }
        
        //basic look
        void UpdateMouseLook ()
        {
            Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

            currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

            cameraPitch -= currentMouseDelta.y * mouseSensitivity;

            cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f);

            playercamera.localEulerAngles = Vector3.right * cameraPitch;

            transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
        }

        //basic movement
        void UpdateMovement()
        {
            Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            targetDir.Normalize();

            currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

            if (controller.isGrounded)
                velocityY = 0f;

            velocityY += gravity * Time.deltaTime;

            Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * walkSpeed + Vector3.up * velocityY;         

           controller.Move(velocity * Time.deltaTime);

            // float test for animator
            animator.SetFloat("Vertical", velocity.z);
            animator.SetFloat("Horizontal", velocity.x);

           
        }
        // trigger test for animator on a very quick movement in direction that is pushed on movement
        private void Dash()
        {
            if (Input.GetButtonDown("Fire2")) 
            {
                animator.SetTrigger("Dash");

                Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                targetDir.Normalize();

                currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

                if (controller.isGrounded)
                    velocityY = 0f;

                velocityY += gravity * Time.deltaTime;

                Vector3 velocity = (transform.forward * currentDir.y * 5 + transform.right * currentDir.x * 5) * walkSpeed * 30  + Vector3.up * velocityY;

                controller.Move(velocity * Time.deltaTime);
            }
        }

        // bool test for animatior to make Player mode run in a straight line. Error it also goes back
        private void Sprint() 
        {
            if (Input.GetButton("Fire2"))
            {

                animator.SetBool("Sprint", true);

                Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                targetDir.Normalize();

                currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

                if (controller.isGrounded)
                    velocityY = 0f;

                velocityY += gravity * Time.deltaTime;

                Vector3 velocity = (transform.forward * currentDir.y ) * walkSpeed  + Vector3.up * velocityY;

                controller.Move(velocity * Time.deltaTime);

            }
            else
            {
                animator.SetBool("Sprint", false);
            }
        }
      
       

        }
