using System;
using Azzazelloqq.Observer.Source.Data;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif

namespace Azzazelloqq.Observer.Source.Provider
{
internal static class ChannelHolder<TData> where TData : struct, INotifyData
{    
	internal const int InitialCapacity = 4;

	internal static IObserver<TData>[] Buffer = new IObserver<TData>[InitialCapacity];
	internal static int Count;
	internal static readonly object Sync = new();

	#if UNITY_2017_1_OR_NEWER
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	private static void ResetAllChannels()
	{
		Buffer = Array.Empty<IObserver<TData>>();
	}
	#endif
}
}