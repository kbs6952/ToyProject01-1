using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace CameraSetting
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovementManager : MonoBehaviour
    {
        PlayerManager player;

        [Header("�÷��̾� �Ŵ��� ��ũ��Ʈ")]
        [HideInInspector] public PlayerAnimationManager animationManager;

        [Header("�÷��̾� �Է� ���� ����")]
        [SerializeField] private float moveSpeed;       // �÷��̾��� �⺻ �ӵ�
        [SerializeField] private float runSpeed;        // �÷��̾ �޸� ���� �ӵ�
        [SerializeField] private float jumpForce;       // �÷��̾��� ������

        private CharacterController cCon;               // Rigidbody��� Character�� ���� �浹 ��ɰ� �̵��� ���� ������Ʈ

        [Header("ī�޶� ���� ����")]
        [SerializeField] ThirdCamController thirdCam;        // 3��Ī ī�޶� ��Ʈ�ѷ��� �ִ� ���� ������Ʈ�� ���������� �Ѵ�.
        [SerializeField] float smoothRotation = 100f;        // ī�޶��� �ڿ������� ȸ���� ���� ����ġ
        Quaternion targetRotation;                           // Ű���� �Է��� ���� �ʾ��� �� ī�޶� �������� ȸ���ϱ� ���Ͽ� ȸ�� ������ �����ϴ� ����

        [Header("���� ���� ����")]
        [SerializeField] private float gravityModifier = 3f; // �÷��̾ ���� �������� �ӵ��� ������ ����
        [SerializeField] private Vector3 groundCheckPoint; // ���� �Ǻ��ϱ� ���� üũ ����Ʈ
        [SerializeField] private float groundCheckRadius;    // �� üũ�ϴ� ���� ũ�� ������
        [SerializeField] private LayerMask groundLayer;      // üũ�� ���̾ ������ �Ǻ��ϴ� ����
        private bool isGrounded;                             // true�̸� ������ ����, false�̸� ���� ����

        private float activeMoveSpeed;                  // ������ �÷��̾ �̵��� �ӷ��� ������ ����
        private Vector3 movement;                       // �÷��̾ �����̴� ����� �Ÿ��� ���Ե� ���� Vector ��

        [Header("�ִϸ�����")]
        private Animator playerAnimator;                // 3D ĳ������ �ִϸ��̼��� ��������ֱ� ���� �ִϸ�����

        private void Awake()
        {
            player = GetComponent<PlayerManager>();
        }

        // Start is called before the first frame update
        void Start()
        {
            cCon = GetComponent<CharacterController>();
            playerAnimator = GetComponentInChildren<Animator>();
            cCon.enabled = true;
        }

        // Update is called once per frame ��ǻ�Ͱ� ���� ���� frame�� ���� �����ǰ� Update�� ���� ȣ�� �˴ϴ�.
        void Update()
        {
            HandleMovement();
            HandleComboAttack();
            HandleActionInput();            
        }

        private void GroundCheck() // �÷��̾ ������ �ƴ��� �Ǻ��ϴ� �Լ�
        {
            isGrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckPoint), groundCheckRadius, groundLayer); // ���̾ ground�� groundCheckRadius�̰� ��ġ �������� checkPoint�� ���� �浹�� �߻��ϸ� True, false
            playerAnimator.SetBool("IsGround", isGrounded);
        }

        private void OnDrawGizmos() // ���� �Ⱥ��̴� ��üũ �Լ��� ����ȭ �ϱ� ���� ����
        {
            Gizmos.color = new Color(0, 1, 0, 0.5f);
            Gizmos.DrawWireSphere(transform.TransformPoint(groundCheckPoint), groundCheckRadius);
        }
        
        private void HandleMovement()
        {
            if (player.isPerformingAction) return;

            // 1. Input Ŭ������ �̿��Ͽ� Ű���� �Է��� ����

            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");


            //playerAnimator.SetFloat("Horizontal", horizontal, 0.2f, Time.deltaTime);
            //playerAnimator.SetFloat("Vertical", horizontal, 0.2f, Time.deltaTime);

            // 2. Ű���� Input�� �Է� ���� Ȯ���ϱ� ���� ���� ����
            Vector3 moveInput = new Vector3(horizontal, 0, vertical).normalized;           // Ű���� �Է°��� �����ϴ� ���� 
            float moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical)); // Ű����� �����¿� Ű �Ѱ��� �Է��� �ϸ� 0���� ū ���� moveAmount�� �����Ѵ�.
                                                                                           // ���� ��ġ + �ӵ� * ���ӵ� = �̵� �Ÿ�
                                                                                           // �̵��Ÿ���ŭ (�� �����Ӹ��� �����Դϴ� * Time.deltaTime)=> �����Ӽ��� ������� ���� �ð��� ���� �Ÿ��� �����Դϴ�.

            // 3. �÷��̾� ĳ���� �̵��� ������ ������ ���� ����
            // �÷��̾ �̵��� ������ �����ϴ� ���� moveDirection.  
            Vector3 moveDirection = thirdCam.transform.forward * moveInput.z + thirdCam.transform.right * moveInput.x;
            moveDirection.y = 0;

            // 4. �÷��̾��� �̵� �ӵ��� �ٸ��� ���ִ� �ڵ� (�޸��� ���) - 
            if (Input.GetKey(KeyCode.LeftShift))  // Key Down : ���� �� �ѹ�, Key : Key��ư�� ���� ������ ���        
            {
                activeMoveSpeed = runSpeed;
                moveAmount++;
                playerAnimator.SetBool("IsRun", true);
            }
            else
            {
                activeMoveSpeed = moveSpeed;
                playerAnimator.SetBool("IsRun", false);
            }


            // 5. ������ �ϱ� ���� ���� - 

            float yValue = movement.y;                               // �������� �ִ� y�� ũ�⸦ ����
            movement = moveDirection * activeMoveSpeed;               // ��ǥ�� �̵��� x,0,z ���� ���� ����  -> y�������� �ִ� ���� 0���� �ʱ�ȭ
            movement.y = yValue;                                      // �߷¿� ���� ��� �޵��� �Ҿ���� ������ �ٽ� �ҷ��´�.

            // ���� ���� �Ǵ� �������� �߻� -> ���� ���°� ������ �������� �ƴ��� �Ǵ��� �ʿ䰡 �ִ�. 

            GroundCheck();

            if (cCon.isGrounded)
            {
                movement.y = 0;                                        // ������ ������ �� y�� ��� -�� ���� ����˴ϴ�.               
            }


            // ����Ű�� �Է��Ͽ� ���� ����
            if (Input.GetButtonDown("Jump") && isGrounded)
            {

                playerAnimator.CrossFade("Jump", 0.2f);               // �� ��° �Ű����� : ���� State���� �����ϰ� ���� �ִϸ��̼��� �ڵ����� Blend���ִ� �ð�
                movement.y = jumpForce;
            }

            movement.y += Physics.gravity.y * gravityModifier * Time.deltaTime;

            // 6. CharacterController ����Ͽ� ĳ���͸� �����δ�.
            if (moveAmount > 0) // moveDir 0�� �� moveMent�� 0�� �ȴ�.
            {
                targetRotation = Quaternion.LookRotation(moveDirection);
                player.playerAudioManager.PlayFootStep();
            }

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, smoothRotation);
            cCon.Move(movement * Time.deltaTime);
            playerAnimator.SetFloat("moveAmount", moveAmount, 0.2f, Time.deltaTime);           // dampTime : 1��° ����(���� ��), 2��° ����(��ȭ ��Ű�� ���� ��)  
        }

        private void HandleActionInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                HandleAttackAction();
            }
        }

        private void HandleAttackAction()
        {
            player.playerAnimationManager.PlayerTargetActionAnimation("ATK0", true);
            player.canCombo = true;                                                    // canCombo True�� �븸 �޺� ������ �� �� �ְ� ���� ���� ����
        }

        private void HandleComboAttack()
        {
            if (!player.canCombo) return; // ���� ���� ó��

            // �޺� ������ ����� �Է� Ű ����

            if (Input.GetMouseButtonDown(0))
            {
                player.animator.SetTrigger("doAttack");
            }
        }
    }


}