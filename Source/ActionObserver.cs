using System;
using Azzazelloqq.Observer.Source.Data;

namespace Azzazelloqq.Observer.Source
{
internal sealed class ActionObserver<TData> : IObserver<TData>
	where TData : struct, INotifyData
{
	private readonly Action<TData> _handler;

	public ActionObserver(Action<TData> handler)
	{
		_handler = handler;
	}

	public void OnNotified(in TData data)
	{
		_handler(data);
	}
}
}