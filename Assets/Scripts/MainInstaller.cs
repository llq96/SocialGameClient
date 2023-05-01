using UnityEngine;
using VladB.SGC.Messenger;
using Zenject;

namespace VladB.SGC
{
    public class MainInstaller : MonoInstaller
    {
        [SerializeField] private MessagesScrollController _messagesScrollController;
        [SerializeField] private MockMessagesModel _mockMessagesModel;
        [SerializeField] private MessageSenderView _messageSenderView;

        public override void InstallBindings()
        {
            Container.Bind<WebRequester>().FromInstance(new WebRequester()).AsSingle().NonLazy();
            Container.Bind<MessagesScrollController>().FromInstance(_messagesScrollController).AsSingle().NonLazy();
            Container.Bind<MessagesKeeper>().FromInstance(new MessagesKeeper()).AsSingle().NonLazy();
            Container.Bind<IMessagesModel>().FromInstance(_mockMessagesModel).AsSingle().NonLazy();

            Container.Bind<MessageSenderViewModel>().FromInstance(new MessageSenderViewModel()).AsSingle().NonLazy();
            Container.Bind<MessageSenderView>().FromInstance(_messageSenderView).AsSingle().NonLazy();
        }

        public override void Start()
        {
            Init();
        }

        public void Init()
        {
            Container.Resolve<WebRequester>().Init();

            Container.Resolve<IMessagesModel>().Init();
            Container.Resolve<MessagesKeeper>().Init(Container.Resolve<IMessagesModel>());
            _messagesScrollController.Init(Container.Resolve<MessagesKeeper>());

            Container.Resolve<MessageSenderViewModel>().Init(Container.Resolve<MessagesKeeper>());
            Container.Resolve<MessageSenderView>().Init(Container.Resolve<MessageSenderViewModel>());

            Debug.Log("All Inited");
        }
    }
}