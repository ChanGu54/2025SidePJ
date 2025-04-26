using UnityEngine;
using System.Collections.Generic;

namespace DejaLoop
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float jumpForce = 10f;
        [SerializeField] private float maxJumpTime = 0.3f;
        [SerializeField] private float jumpMultiplier = 1.5f;
        
        [Header("UI Reference")]
        [SerializeField] private RecordingUI recordingUI;
        
        [Header("Animation")]
        [SerializeField] private Animator animator;
        
        private Rigidbody2D rb;
        private bool isGrounded;
        private bool isJumping = false;
        private float jumpTimeCounter = 0f;
        private bool isRecording = false;
        private bool isPlaying = false;
        private List<Vector2> recordedPositions = new List<Vector2>();
        private List<Vector2> recordedVelocities = new List<Vector2>();
        private int currentPlaybackIndex = 0;
        private bool isFacingRight = true;
        
        // Animation Parameters
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
        private static readonly int VerticalSpeed = Animator.StringToHash("VerticalSpeed");
        private static readonly int IsJumping = Animator.StringToHash("IsJumping");
        
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 3f; // 중력 설정
            rb.freezeRotation = true; // 회전 방지
        }
        
        private void Start()
        {
            if (recordingUI != null)
            {
                recordingUI.UpdateUI(RecordingState.Ready);
            }
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!isRecording && !isPlaying)
                {
                    StartRecording();
                }
                else if (isRecording)
                {
                    StopRecording();
                }
            }
            
            if (isRecording)
            {
                RecordPlayerState();
            }
            else if (isPlaying)
            {
                PlaybackRecordedState();
            }
            else
            {
                HandleMovement();
            }
            
            UpdateAnimation();
        }
        
        private void HandleMovement()
        {
            // 좌우 이동
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
            
            // 방향 전환
            if (horizontalInput > 0 && !isFacingRight)
            {
                Flip();
            }
            else if (horizontalInput < 0 && isFacingRight)
            {
                Flip();
            }
            
            // 점프 처리
            if (Input.GetKeyDown(KeyCode.W) && isGrounded)
            {
                isJumping = true;
                jumpTimeCounter = maxJumpTime;
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
            
            // 점프 높이 조절
            if (Input.GetKey(KeyCode.W) && isJumping)
            {
                if (jumpTimeCounter > 0)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * jumpMultiplier);
                    jumpTimeCounter -= Time.deltaTime;
                }
                else
                {
                    isJumping = false;
                }
            }
            
            if (Input.GetKeyUp(KeyCode.W))
            {
                isJumping = false;
            }
        }
        
        private void UpdateAnimation()
        {
            if (animator == null) return;
            
            // 속도 기반 애니메이션
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float speed = Mathf.Abs(horizontalInput);
            animator.SetFloat(Speed, speed);
            
            // 점프 관련 애니메이션
            animator.SetBool(IsGrounded, isGrounded);
            animator.SetFloat(VerticalSpeed, rb.linearVelocity.y);
            animator.SetBool(IsJumping, isJumping);
        }
        
        private void Flip()
        {
            isFacingRight = !isFacingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
        
        private void RecordPlayerState()
        {
            // 녹화 중에는 실제 입력을 받아서 움직임을 기록
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
            
            // 방향 전환
            if (horizontalInput > 0 && !isFacingRight)
            {
                Flip();
            }
            else if (horizontalInput < 0 && isFacingRight)
            {
                Flip();
            }
            
            // 점프 처리
            if (Input.GetKeyDown(KeyCode.W) && isGrounded)
            {
                isJumping = true;
                jumpTimeCounter = maxJumpTime;
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
            
            // 점프 높이 조절
            if (Input.GetKey(KeyCode.W) && isJumping)
            {
                if (jumpTimeCounter > 0)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * jumpMultiplier);
                    jumpTimeCounter -= Time.deltaTime;
                }
                else
                {
                    isJumping = false;
                }
            }
            
            if (Input.GetKeyUp(KeyCode.W))
            {
                isJumping = false;
            }
            
            // 현재 상태 기록
            recordedPositions.Add(transform.position);
            recordedVelocities.Add(rb.linearVelocity);
            
            // 애니메이션 상태도 함께 기록
            if (animator != null)
            {
                float speed = Mathf.Abs(horizontalInput);
                animator.SetFloat(Speed, speed);
                animator.SetBool(IsGrounded, isGrounded);
                animator.SetFloat(VerticalSpeed, rb.linearVelocity.y);
                animator.SetBool(IsJumping, isJumping);
            }
        }
        
        private void PlaybackRecordedState()
        {
            if (currentPlaybackIndex < recordedPositions.Count)
            {
                transform.position = recordedPositions[currentPlaybackIndex];
                Vector2 recordedVelocity = recordedVelocities[currentPlaybackIndex];
                rb.linearVelocity = recordedVelocity;
                
                // 재생 중에도 애니메이션 업데이트
                if (animator != null)
                {
                    // 수평 이동 애니메이션
                    float horizontalSpeed = Mathf.Abs(recordedVelocity.x);
                    animator.SetFloat(Speed, horizontalSpeed);
                    
                    // 점프 애니메이션
                    bool isGroundedInPlayback = Mathf.Abs(recordedVelocity.y) < 0.1f;
                    bool isJumpingInPlayback = recordedVelocity.y > 0.1f;
                    
                    animator.SetBool(IsGrounded, isGroundedInPlayback);
                    animator.SetFloat(VerticalSpeed, recordedVelocity.y);
                    animator.SetBool(IsJumping, isJumpingInPlayback);
                    
                    // 방향 전환
                    if (recordedVelocity.x > 0 && !isFacingRight)
                    {
                        Flip();
                    }
                    else if (recordedVelocity.x < 0 && isFacingRight)
                    {
                        Flip();
                    }
                }
                
                currentPlaybackIndex++;
            }
            else
            {
                isPlaying = false;
                currentPlaybackIndex = 0;
                if (recordingUI != null)
                {
                    recordingUI.UpdateUI(RecordingState.Ready);
                }
                Debug.Log("Playback completed");
            }
        }
        
        private void StartRecording()
        {
            isRecording = true;
            recordedPositions.Clear();
            recordedVelocities.Clear();
            if (recordingUI != null)
            {
                recordingUI.UpdateUI(RecordingState.Recording);
            }
            Debug.Log("Recording started");
        }
        
        private void StopRecording()
        {
            isRecording = false;
            isPlaying = true;
            currentPlaybackIndex = 0;
            if (recordingUI != null)
            {
                recordingUI.UpdateUI(RecordingState.Playing);
            }
            Debug.Log($"Recording stopped, {recordedPositions.Count} frames recorded");
        }
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = true;
                isJumping = false;
            }
        }
        
        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = false;
            }
        }
    }
} 