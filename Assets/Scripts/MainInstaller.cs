using UnityEngine;
using Zenject;

public class MainInstaller : MonoInstaller
{
    [SerializeField] private MessengerViewController _messengerViewController;
    [SerializeField] private MockMessagesModel _mockMessagesModel;

    public override void InstallBindings()
    {
        Container.Bind<WebRequester>().FromInstance(new WebRequester()).AsSingle().NonLazy();
        Container.Bind<MessengerViewController>().FromInstance(_messengerViewController).AsSingle().NonLazy();
        Container.Bind<MessagesKeeper>().FromInstance(new MessagesKeeper()).AsSingle().NonLazy();
        Container.Bind<IMessagesModel>().FromInstance(_mockMessagesModel).AsSingle().NonLazy();
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
        _messengerViewController.Init(Container.Resolve<MessagesKeeper>());

        Debug.Log("All Inited");
    }
}