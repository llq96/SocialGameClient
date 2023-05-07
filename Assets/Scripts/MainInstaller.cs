using Newtonsoft.Json;
using UnityEngine;
using VladB.SGC.Messenger;
using Zenject;

namespace VladB.SGC
{
    public class MainInstaller : MonoInstaller
    {
        [SerializeField] private MessagesScrollController _messagesScrollController;
        [SerializeField] private MessageSenderView _messageSenderView;

        public override void InstallBindings()
        {
            Container.Bind<WebRequester>().FromInstance(new WebRequester()).AsSingle().NonLazy();
            Container.Bind<MessagesScrollController>().FromInstance(_messagesScrollController).AsSingle().NonLazy();
            Container.Bind<MessagesKeeper>().FromInstance(new MessagesKeeper()).AsSingle().NonLazy();


            var messagesModel =
                new MessagesModel(Container.Resolve<WebRequester>(), Container.Resolve<MessagesKeeper>());
            Container.Bind<IMessagesModel>().FromInstance(messagesModel).AsSingle().NonLazy();

            Container.Bind<MessageSenderViewModel>().FromInstance(new MessageSenderViewModel()).AsSingle().NonLazy();
            Container.Bind<MessageSenderView>().FromInstance(_messageSenderView).AsSingle().NonLazy();
        }

        public override void Start()
        {
            Init();
        }

        public async void Init()
        {
            await Container.Resolve<IMessagesModel>().Init();

            Container.Resolve<MessagesKeeper>().Init(Container.Resolve<IMessagesModel>());
            _messagesScrollController.Init(Container.Resolve<MessagesKeeper>());

            Container.Resolve<MessageSenderViewModel>().Init(Container.Resolve<MessagesKeeper>());
            Container.Resolve<MessageSenderView>().Init(Container.Resolve<MessageSenderViewModel>());

            Debug.Log("All Inited");

            // Container.Resolve<WebRequester>().MessagesGetRequest().Forget();
        }
    }
}