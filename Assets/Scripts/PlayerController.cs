using UnityEngine;
using System.Collections.Generic;

namespace DejaLoop
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float jumpForce = 5f;
        
        [Header("UI Reference")]
        [SerializeField] private RecordingUI recordingUI;
        
        private Rigidbody2D rb;
        private bool isGrounded;
        private bool isRecording = false;
        private bool isPlaying = false;
        private List<Vector2> recordedPositions = new List<Vector2>();
        private List<Vector2> recordedVelocities = new List<Vector2>();
        private int currentPlaybackIndex = 0;
        
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }
        
        private void Start()
        {
            if (recordingUI != null)
            {
                recordingUI.UpdateUI(false);
            }
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!isRecording && !isPlaying)
                {
                    // 녹화 시작
                    StartRecording();
                }
                else if (isRecording)
                {
                    // 녹화 종료 및 재생 시작
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
                // 일반적인 플레이어 컨트롤
                float horizontalInput = Input.GetAxisRaw("Horizontal");
                rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
                
                if (Input.GetKeyDown(KeyCode.W) && isGrounded)
                {
                    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                }
            }
        }
        
        private void StartRecording()
        {
            isRecording = true;
            recordedPositions.Clear();
            recordedVelocities.Clear();
            if (recordingUI != null)
            {
                recordingUI.UpdateUI(true);
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
                recordingUI.UpdateUI(false);
            }
            Debug.Log($"Recording stopped, {recordedPositions.Count} frames recorded");
        }
        
        private void RecordPlayerState()
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
            
            if (Input.GetKeyDown(KeyCode.W) && isGrounded)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
            
            // Record position and velocity
            recordedPositions.Add(transform.position);
            recordedVelocities.Add(rb.linearVelocity);
        }
        
        private void PlaybackRecordedState()
        {
            if (currentPlaybackIndex < recordedPositions.Count)
            {
                transform.position = recordedPositions[currentPlaybackIndex];
                rb.linearVelocity = recordedVelocities[currentPlaybackIndex];
                currentPlaybackIndex++;
            }
            else
            {
                // 재생 완료
                isPlaying = false;
                currentPlaybackIndex = 0;
                Debug.Log("Playback completed");
            }
        }
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = true;
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