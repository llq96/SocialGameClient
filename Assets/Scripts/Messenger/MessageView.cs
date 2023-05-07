using TMPro;
using UnityEngine;

namespace VladB.SGC.Messenger
{
    public class MessageView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _tmp_message;
        [SerializeField] private TextMeshProUGUI _tmp_messageIndex;
        [SerializeField] private TextMeshProUGUI _tmp_senderName;

        public MessageInfo MessageInfo { get; private set; }

        public void SetMessage(MessageInfo messageInfo)
        {
            if (messageInfo != null)
            {
                messageInfo.OnMessageInfoUpdated -= UpdateUI;
            }

            MessageInfo = messageInfo;

            MessageInfo.OnMessageInfoUpdated += UpdateUI;

            UpdateUI();
            MessageInfo.TryUpdateMessageInfo();
        }

        private void UpdateUI()
        {
            var height = transform.GetComponent<RectTransform>().rect.height;

            _tmp_message.text = MessageInfo.Message;
            _tmp_messageIndex.text = $"Message ID : {MessageInfo.Index}";
            _tmp_senderName.text = MessageInfo.Sender != null ? MessageInfo.Sender.Name : "Unknown";
            // transform.RecursiveForceRebuildLayoutImmediate();

            _tmp_message.transform.RecursiveForceRebuildLayoutImmediate();
            _tmp_messageIndex.transform.RecursiveForceRebuildLayoutImmediate();
            _tmp_senderName.transform.RecursiveForceRebuildLayoutImmediate();

            var newHeight = transform.GetComponent<RectTransform>().rect.height;
            var deltaHeight = newHeight - height;
        }


        public void SetNullText()
        {
            _tmp_message.text = "null";
            _tmp_messageIndex.text = "null";
            _tmp_senderName.text = "null";
        }
    }
}