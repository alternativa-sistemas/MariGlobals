# MariGlobals

Global things for all Mari's projects

# Usage

**MariGlobals** is an agreggate of global things of all other Mari's projects.

## AsyncEvent

You can simply creates an async event with all things already handled (lock, multiples handlers, etc).
<br>
Unfortunatelly if you want 2 or more parameters in your event you'll need to create your own EventArgs or something like.
<br>
**Obs:** You can use an AsyncEvent without any args too.

### Creating an AsyncEvent with args.

```csharp
using MariGlobals.Class.Event;
using System;
using System.Threading.Tasks;

namespace Example
{
    public class Example
    {
        public Example()
            => _myEvent = new AsyncEvent<ExampleEventArgs>();

        public event AsyncEventHandler<ExampleEventArgs> MyEvent
        {
            add => _myEvent.Register(value);
            remove => _myEvent.Unregister(value);
        }

        private readonly AsyncEvent<ExampleEventArgs> _myEvent;

        internal Task InvokeEventAsync(ExampleEventArgs args)
            => _myEvent.InvokeAsync(args);
    }

    public class ExampleEventArgs : EventArgs
    {
    }
}
```

### Creating an AsyncEvent without args

```csharp
using MariGlobals.Class.Event;
using System;
using System.Threading.Tasks;

namespace Example
{
    public class Example
    {
        public Example()
            => _myEvent = new AsyncEvent();

        public event AsyncEventHandler MyEvent
        {
            add => _myEvent.Register(value);
            remove => _myEvent.Unregister(value);
        }

        private readonly AsyncEvent _myEvent;

        internal Task InvokeEventAsync()
            => _myEvent.InvokeAsync();
    }
}
```

# License

**MariGlobals** is provided under [The MIT License.](https://github.com/MariBotOfficial/MariGlobals/blob/master/LICENSE)
