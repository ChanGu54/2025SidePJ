using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DejaLoop
{
    public class RecordingUI : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI recordingText;
        [SerializeField] private Image recordingIndicator;
        
        [Header("Colors")]
        [SerializeField] private Color readyColor = Color.gray;
        [SerializeField] private Color recordingColor = Color.red;
        [SerializeField] private Color playingColor = Color.blue;
        
        private void Start()
        {
            // 초기 상태 설정
            UpdateUI(RecordingState.Ready);
        }
        
        public void UpdateUI(RecordingState state)
        {
            if (recordingText != null)
            {
                recordingText.text = state switch
                {
                    RecordingState.Ready => "Press SPACE to Record",
                    RecordingState.Recording => "RECORDING...",
                    RecordingState.Playing => "PLAYING...",
                    _ => "Unknown State"
                };
            }
            
            if (recordingIndicator != null)
            {
                recordingIndicator.color = state switch
                {
                    RecordingState.Ready => readyColor,
                    RecordingState.Recording => recordingColor,
                    RecordingState.Playing => playingColor,
                    _ => readyColor
                };
            }
        }
    }

    public enum RecordingState
    {
        Ready,
        Recording,
        Playing
    }
} 