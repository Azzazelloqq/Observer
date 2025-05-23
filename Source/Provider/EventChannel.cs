using System;
using System.Collections.Generic;
using System.Linq;
using Azzazelloqq.Observer.Source.Data;

namespace Azzazelloqq.Observer.Source.Provider
{
public sealed class EventChannel : IEventChannel
{
	private readonly Action<Exception> _logException;
	private readonly List<Action> _clearActions = new();
	private bool _disposed;

	public EventChannel(Action<Exception> logException = null)
	{
		_logException = logException;
	}

	public void Dispose()
	{
		if (_disposed)
		{
			return;
		}

		foreach (var clear in _clearActions)
		{
			clear();
		}

		_clearActions.Clear();
		_disposed = true;
		_disposed = true;
	}
	
	public int Count<TData>() where TData : struct, INotifyData
	{
		return ChannelHolder<TData>.Count;
	}

	public void Clear<TData>() where TData : struct, INotifyData
	{
		lock (ChannelHolder<TData>.Sync)
		{
			ChannelHolder<TData>.Buffer = new IObserver<TData>[ChannelHolder<TData>.InitialCapacity];
			ChannelHolder<TData>.Count = 0;
		}
	}

	public Subscription<TData> Subscribe<TData>(IObserver<TData> observer) where TData : struct, INotifyData
	{
	
		
		lock (ChannelHolder<TData>.Sync)
		{
			ThrowIfDisposed();
			var has = false;
			foreach (var action in _clearActions)
			{
				if (action.Method.GetGenericMethodDefinition() !=
					typeof(EventChannel).GetMethod(nameof(Clear))?.GetGenericMethodDefinition())
				{
					continue;
				}

				has = true;
				break;
			}

			if (!has)
			{
				_clearActions.Add(Clear<TData>);
			}

			var buf = ChannelHolder<TData>.Buffer;
			for (var i = 0; i < ChannelHolder<TData>.Count; i++)
			{
				if (ReferenceEquals(buf[i], observer))
				{
					return new Subscription<TData>(observer);
				}
			}

			if (ChannelHolder<TData>.Count == buf.Length)
			{
				Array.Resize(ref ChannelHolder<TData>.Buffer, buf.Length * 2);
				buf = ChannelHolder<TData>.Buffer;
			}

			buf[ChannelHolder<TData>.Count++] = observer;
		}

		return new Subscription<TData>(observer);
	}

	public Subscription<TData> Subscribe<TData>(Action<TData> handler) where TData : struct, INotifyData
	{
		var wrapper = new ActionObserver<TData>(handler);
		return Subscribe(wrapper);
	}

	public Subscription<TData> SubscribeOnce<TData>(Action<TData> handler) where TData : struct, INotifyData
	{
		var oneTime = new OneTimeObserver<TData>(this, handler);
		return oneTime.Subscription;
	}

	public void Unsubscribe<TData>(IObserver<TData> observer) where TData : struct, INotifyData
	{
		RemoveObserver(observer);
	}

	public void Publish<TData>(in TData data) where TData : struct, INotifyData
	{
		var buf = ChannelHolder<TData>.Buffer;
		var cnt = ChannelHolder<TData>.Count;

		for (var i = cnt - 1; i >= 0; i--)
		{
			try
			{
				buf[i].OnNotified(data);
			}
			catch (Exception ex)
			{
				_logException?.Invoke(ex);
			}
		}
	}

	internal static bool RemoveObserver<TData>(IObserver<TData> observer)
		where TData : struct, INotifyData
	{
		lock (ChannelHolder<TData>.Sync)
		{
			var buf = ChannelHolder<TData>.Buffer;
			var cnt = ChannelHolder<TData>.Count;
			for (var i = 0; i < cnt; i++)
			{
				if (!ReferenceEquals(buf[i], observer))
				{
					continue;
				}

				buf[i] = buf[cnt - 1];
				buf[cnt - 1] = null!;
				ChannelHolder<TData>.Count--;
				if (ChannelHolder<TData>.Count <= buf.Length / 4 && buf.Length > ChannelHolder<TData>.InitialCapacity)
				{
					Array.Resize(ref ChannelHolder<TData>.Buffer, buf.Length / 2);
				}

				return true;
			}
		}

		return false;
	}

	private void ThrowIfDisposed()
	{
		if (_disposed)
		{
			throw new ObjectDisposedException(nameof(EventChannel));
		}
	}
}
}