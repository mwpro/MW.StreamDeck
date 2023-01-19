using SharpDeck;
using SharpDeck.Events.Received;

namespace MW.StreamDeck;

public class Program
{
    /// <summary>
    /// Defines the entry point of the application.
    /// </summary>
    /// <param name="args">The arguments.</param>
    public static void Main(string[] args)
    {
#if DEBUG
        System.Diagnostics.Debugger.Launch();
#endif

        SharpDeck.StreamDeckPlugin.Run();
    }
}
[StreamDeckAction("com.mwpro.streamdeck.helloworld")]
public class HelloWorld : StreamDeckAction
{
    // Methods can be overriden to intercept events received from the Stream Deck.
    protected override Task OnKeyDown(ActionEventArgs<KeyPayload> args)
    {
        Console.WriteLine($"Action: {args.Action}");
        Console.WriteLine($"Context: {args.Context}");
        Console.WriteLine($"Device: {args.Device}");
        Console.WriteLine($"Column: {args.Payload.Coordinates.Column}");
        Console.WriteLine($"Row: {args.Payload.Coordinates.Row}");
        return this.SetTitleAsync($"{DateTime.UtcNow.Second}");
    }

    protected override async Task OnKeyUp(ActionEventArgs<KeyPayload> args)
    {
        Console.WriteLine($"Action: {args.Action}");
        Console.WriteLine($"Context: {args.Context}");
        Console.WriteLine($"Device: {args.Device}");
        Console.WriteLine($"Column: {args.Payload.Coordinates.Column}");
        Console.WriteLine($"Row: {args.Payload.Coordinates.Row}");
    }
}