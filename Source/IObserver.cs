using Azzazelloqq.Observer.Source.Data;

namespace Azzazelloqq.Observer
{
	public interface IObserver<TData> where TData : struct, INotifyData
	{
		void OnNotified(in TData data);
	}
}