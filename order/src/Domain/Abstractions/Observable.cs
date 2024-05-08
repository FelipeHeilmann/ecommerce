namespace Domain.Abstractions;

public class Observable
{
    public IList<Observer> Observers { get; }

    public Observable()
    {
        Observers = new List<Observer>();
    }

    public void Register(string eventName, Func<IDomainEvent, Task> callback)
    {
        Observers.Add(new Observer(eventName, callback));
    }

    public async void Notify(IDomainEvent domainEvent) {
        foreach(var observer in Observers)
        {
            if(observer.Event == domainEvent.EventName)
            {
                await observer.Callback(domainEvent);
            }
        }
    }

}

public class Observer
{
    public string Event { get; set; }
    public Func<IDomainEvent, Task> Callback { get; set; }

    public Observer(string eventName, Func<IDomainEvent, Task> callback)
    {
        Event = eventName;
        Callback = callback;
    }
}
