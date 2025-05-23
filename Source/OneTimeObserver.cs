using System;
using Azzazelloqq.Observer.Source.Data;
using Azzazelloqq.Observer.Source.Provider;

namespace Azzazelloqq.Observer.Source
{
public sealed class OneTimeObserver<TData> : IObserver<TData> where TData : struct, INotifyData
{
	public Subscription<TData> Subscription { get; }
	
	private readonly Action<TData> _handler;

	public OneTimeObserver(EventChannel channel, Action<TData> handler)
	{
		_handler = handler;
		Subscription = channel.Subscribe(this);
	}

	public void OnNotified(in TData data)
	{
		_handler(data);
		Subscription.Dispose();
	}
}
}