# Azzazelloqq.Observer 🚀

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?style=flat-square)](LICENSE)
[![GitHub release (latest by SemVer)](https://img.shields.io/github/release/Azzazelloqq/Observer.svg?style=flat-square\&cacheSeconds=86400)](https://github.com/Azzazelloqq/Observer/releases)

Azzazelloqq.Observer is a high-performance, low-level publish/subscribe library for .NET and Unity projects. It uses struct-based subscriptions and static generic channels to achieve zero allocations on event publish and minimal allocations on subscribe.

> **Performance Note:**
>
> * **Publish:** no GC allocations or locks.
> * **Subscribe/Unsubscribe:** dynamic buffer with swap-and-shrink strategy (O(1) removal, O(log N) resizing).

---

## ✨ Key Features

* **Zero-Allocation Publish**
  Publish events over a simple array of `IObserver<T>` without lists, LINQ, or locks.
* **Minimal-Allocation Subscribe**
  Dynamic buffer that grows and shrinks, with O(1) removal via swap-and-shrink.
* **Struct-Based Subscriptions**
  `Subscription<T>` is a `struct`, so `Dispose()` allocates nothing on the heap.
* **Action and One-Time APIs**
  Convenient `Subscribe<T>(Action<T>)` and `SubscribeOnce<T>(Action<T>)` methods.
* **Automatic Clear on Dispose**
  `EventChannel` implements `IDisposable` and clears all channels on disposal.
* **Dependency Injection Friendly**
  Register `IEventChannel` as a singleton in your DI container.
* **Optional Unity Integration**
  Separate Unity package for automatic static reset when domain reload is disabled.

---

## 📦 Project Structure

```plaintext
Observer/
├── src/
│   ├── Azzazelloqq.Observer.Runtime/
│   │   ├── INotifyData.cs             # Marker interface for event data
│   │   ├── IObserver<TData>.cs        # Observer interface
│   │   ├── Subscription<TData>.cs     # Struct subscription token
│   │   ├── ChannelHolder<TData>.cs    # Static generic buffer holder
│   │   ├── EventChannel.cs            # Core API and implementation
│   │   └── IEventChannel.cs           # Channel interface with IDisposable
│   └── Azzazelloqq.Observer.Unity/    # (Optional) Unity support package
│       └── UnityObserverReset.cs      # Automatic static reset for Unity
└── samples/
    └── ExampleUsage.cs                # Example usage code
```

---

## 🚀 Quick Start

### Installation

```bash
Installation can be done in two ways:

- **Manual (all platforms)**: Clone or download this repository and add the `src/Azzazelloqq.Observer.Runtime` folder into your project.
- **Unity (UPM)**: Add to your `Packages/manifest.json` either:
  ```json
  "com.azzazelloqq.observer": "https://github.com/Azzazelloqq/Observer.git#1.0.0"
  ```
  or simply:
  ```json
  "com.azzazelloqq.observer": "https://github.com/Azzazelloqq/Observer.git"
```

### Basic Usage

```csharp
var channel = new EventChannel(logException: Console.WriteLine);

// Subscribe to events:
using var sub = channel.Subscribe<MyEvent>(e =>
{
    Console.WriteLine($"Received: {e.Value}");
});

// Publish an event:
channel.Publish(new MyEvent { Value = 42 });
```

### One-Time Subscription

```csharp
using var once = channel.SubscribeOnce<MyEvent>(e =>
{
    Console.WriteLine("This runs only once");
});
```

### Dependency Injection

```csharp
services.AddSingleton<IEventChannel, EventChannel>();

public class MyService
{
    private readonly IEventChannel _events;
    private Subscription<MyEvent> _subscription;

    public MyService(IEventChannel events)
    {
        _events = events;
    }

    public void Initialize()
    {
        _subscription = _events.Subscribe<MyEvent>(HandleEvent);
    }

    private void HandleEvent(MyEvent e)
    {
        // Handle event
    }
}
```

---

## 🔧 Unity Support (Optional)

Add the **Azzazelloqq.Observer.Unity** package to enable automatic static reset in Unity Editor and PlayMode when domain reload is disabled.

---

## 📄 License

This project is licensed under the MIT License. See [LICENSE](LICENSE) for details.

---

## 📚 Contributing

Contributions, issues, and feature requests are welcome!
Please open an issue or submit a pull request at [GitHub Issues](https://github.com/Azzazelloqq/Observer/issues).

---

*Enjoy building!*
