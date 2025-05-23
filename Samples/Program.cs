using System;
using Azzazelloqq.Observer.Source.Provider;

namespace Observer.Samples
{
public class Program
{
	static void Main(string[] args)
	{
		// Resolve the channel from DI
		var channel = new EventChannel();

		// --- Basic Subscription ---
		using var sub = channel.Subscribe<MyEvent>(OnMyEvent);
		Console.WriteLine("Publishing MyEvent(Value=100)");
		channel.Publish(new MyEvent { Value = 100 });

		// --- One-Time Subscription ---
		using var once = channel.SubscribeOnce<MyEvent>(e =>
		{
			Console.WriteLine($"[Once] Received: {e.Value}");
		});
		Console.WriteLine("Publishing MyEvent(Value=200) twice");
		channel.Publish(new MyEvent { Value = 200 });
		channel.Publish(new MyEvent { Value = 300 }); // won't trigger once handler

		// --- Inspect and Clear ---
		Console.WriteLine($"Subscribers before clear: {channel.Count<MyEvent>()}");
		channel.Clear<MyEvent>();
		Console.WriteLine($"Subscribers after clear:  {channel.Count<MyEvent>()}");

		// --- Dispose the channel ---
		channel.Dispose();
		Console.WriteLine("Channel disposed.");
	}

	private static void OnMyEvent(MyEvent e)
	{
		Console.WriteLine($"[Handler] Received: {e.Value}");
	}
}
}