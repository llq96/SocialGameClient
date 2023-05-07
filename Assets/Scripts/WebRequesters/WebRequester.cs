using System.Net;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using VladB.SGC.Messenger;

namespace VladB.SGC
{
    public class WebRequester
    {
        // private const string BasePath = "https://localhost:7247";
        // private const string BasePath = "https://77.232.137.143:80";
        private const string BasePath = "http://1490293-ck59733.tw1.ru";

        private string MessagesPath => $"{BasePath}/messages";
        private string InformationPath => $"{MessagesPath}/information";
        private string OneMessagePath => $"{MessagesPath}/{{0}}";

        public async UniTask<MessageInfo> DownloadMessage(int index)
        {
            using (var uwr = UnityWebRequest.Get(string.Format(OneMessagePath, index)))
            {
                var request = uwr.SendWebRequest();
                await request;

                if (uwr.result == UnityWebRequest.Result.Success)
                {
                    var result = uwr.downloadHandler.text;
                    // Debug.Log(result);
                    var messageInfo = JsonConvert.DeserializeObject<MessageInfo>(result);
                    return messageInfo;
                }

                return null;
            }
        }

        public async UniTask<MessagesInformation> GetMessagesInformation()
        {
            using (var uwr = UnityWebRequest.Get(InformationPath))
            {
                var request = uwr.SendWebRequest();
                await request;

                if (uwr.result == UnityWebRequest.Result.Success)
                {
                    var result = uwr.downloadHandler.text;
                    // Debug.Log(result);
                    var messagesInformation = JsonConvert.DeserializeObject<MessagesInformation>(result);
                    return messagesInformation;
                }

                return null;
            }
        }

        public async UniTaskVoid SendNewMessage(string messageText)
        {
            WWWForm form = new WWWForm();
            form.AddField("Sender", Dns.GetHostName());
            form.AddField("Message", messageText);

            using (var uwr = UnityWebRequest.Post(MessagesPath, form))
            {
                var request = uwr.SendWebRequest();
                await request;

                // if (uwr.result == UnityWebRequest.Result.Success)
                // {
                //     var result = uwr.downloadHandler.text;
                //     Debug.Log(result);
                // }
            }
        }

        public class MessagesInformation
        {
            public int LastMessageIndex { get; }

            public MessagesInformation(int lastMessageIndex)
            {
                LastMessageIndex = lastMessageIndex;
            }
        }
    }
}