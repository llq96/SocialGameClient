using TMPro;
using UnityEngine;

public class MessageView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _tmp_message;
    [SerializeField] private TextMeshProUGUI _tmp_messageIndex;
    public MessageInfo MessageInfo { get; private set; }

    public void SetMessage(MessageInfo messageInfo)
    {
        MessageInfo = messageInfo;

        _tmp_message.text = messageInfo.Message;
        _tmp_messageIndex.text = $"Message ID : {messageInfo.Index}";
        transform.RecursiveForceRebuildLayoutImmediate();

        // _tmp_message.transform.RecursiveForceRebuildLayoutImmediate();
        // _tmp_messageIndex.transform.RecursiveForceRebuildLayoutImmediate();
    }

    public void SetNullText()
    {
        _tmp_message.text = "Not Initialized";
        _tmp_messageIndex.text = "-1";
    }
}