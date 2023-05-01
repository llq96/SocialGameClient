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
            MessageInfo = messageInfo;

            _tmp_message.text = messageInfo.Message;
            _tmp_messageIndex.text = $"Message ID : {messageInfo.Index}";
            _tmp_senderName.text = messageInfo.Sender.Name;
            // transform.RecursiveForceRebuildLayoutImmediate();

            _tmp_message.transform.RecursiveForceRebuildLayoutImmediate();
            _tmp_messageIndex.transform.RecursiveForceRebuildLayoutImmediate();
            _tmp_senderName.transform.RecursiveForceRebuildLayoutImmediate();
        }

        public void SetNullText()
        {
            _tmp_message.text = "null";
            _tmp_messageIndex.text = "null";
            _tmp_senderName.text = "null";
        }
    }
}