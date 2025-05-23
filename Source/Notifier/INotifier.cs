using Azzazelloqq.Observer.Source.Data;

namespace Azzazelloqq.Observer.Source.Notifier
{
public interface INotifier<TData> where TData : struct, INotifyData
{
}
}