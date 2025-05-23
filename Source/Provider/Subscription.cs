using System;
using Azzazelloqq.Observer.Source.Data;

namespace Azzazelloqq.Observer.Source.Provider
{
public readonly struct Subscription<TData> : IDisposable where TData : struct, INotifyData
{
	private readonly IObserver<TData> _observer;

	internal Subscription(IObserver<TData> observer)
	{
		_observer = observer;
	}

	public void Dispose()
	{
		EventChannel.RemoveObserver(_observer);
	}
}
}