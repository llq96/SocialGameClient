using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VladB.SGC.Messenger
{
    public class MessageSenderView : MonoBehaviour
    {
        [SerializeField] private Button _button_send;
        [SerializeField] private TMP_InputField _inputField;
        private MessageSenderViewModel _viewModel;

        public void Init(MessageSenderViewModel viewModel)
        {
            _viewModel = viewModel;

            _button_send.onClick.AddListener(SendNewMessage);

            _inputField.onSubmit.AddListener(_ => { SendNewMessage(); });
        }

        private void SendNewMessage()
        {
            var messageText = _inputField.text;
            if (string.IsNullOrEmpty(messageText)) return;
            if (string.IsNullOrWhiteSpace(messageText)) return;

            _viewModel.SendNewMessage(messageText);

            ClearInputFieldTask().Forget();
        }


        private async UniTaskVoid ClearInputFieldTask()
        {
            await UniTask.Yield();
            _inputField.DeactivateInputField(true);
            _inputField.text = "";
        }
    }
}