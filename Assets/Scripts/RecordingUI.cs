using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DejaLoop
{
    public class RecordingUI : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private Text recordingText;
        [SerializeField] private Image recordingIndicator;
        
        [Header("Colors")]
        [SerializeField] private Color recordingColor = Color.red;
        [SerializeField] private Color notRecordingColor = Color.gray;
        
        private void Start()
        {
            // 초기 상태 설정
            UpdateUI(false);
        }
        
        public void UpdateUI(bool isRecording)
        {
            if (recordingText != null)
            {
                recordingText.text = isRecording ? "RECORDING..." : "Press SPACE to Record";
            }
            
            if (recordingIndicator != null)
            {
                recordingIndicator.color = isRecording ? recordingColor : notRecordingColor;
            }
        }
    }
} 