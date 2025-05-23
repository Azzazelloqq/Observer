using System;
using Azzazelloqq.Observer.Source.Data;

namespace Azzazelloqq.Observer.Source.Provider
{
public interface IEventChannel : IDisposable
{
	public void Publish<TData>(in TData data) where TData : struct, INotifyData;
	public Subscription<TData>  Subscribe<TData>(IObserver<TData> observer) where TData : struct, INotifyData;
	public Subscription<TData> Subscribe<TData>(Action<TData> handler) where TData : struct, INotifyData;
	public Subscription<TData> SubscribeOnce<TData>(Action<TData> handler) where TData : struct, INotifyData;
	public void Unsubscribe<TData>(IObserver<TData> observer) where TData : struct, INotifyData;
}
}