using System.Drawing;
using System.Drawing.Imaging;
using SharpDeck;
using SharpDeck.Events.Received;

namespace MW.StreamDeck;

[StreamDeckAction("com.mwpro.streamdeck.helloworld")]
public class HelloWorld : StreamDeckAction
{
    private int _counter = 0;
    private PeriodicTimer _timer;

    public HelloWorld()
    {
        _timer = new PeriodicTimer(TimeSpan.FromSeconds(5));
        DoWorkAsync();
    }

    protected override void OnInit(ActionEventArgs<AppearancePayload> args)
    {
        SetImageAsync(PrepareBackground(Color.Green, "OK"));
        base.OnInit(args);
    }

    private async Task DoWorkAsync()
    {
        while (await _timer.WaitForNextTickAsync())
        {
            _counter++;
            var isDanger = _counter >= 3;
            
            SetImageAsync(PrepareBackground(isDanger ? Color.Red : Color.Yellow, isDanger ? "ALARM" : "WARN"));
        }
    }

    private string PrepareBackground(Color color, string text)
    {
        var ms = new MemoryStream();
        var bitmap = new Bitmap(72, 72);
        var img = System.Drawing.Graphics.FromImage(bitmap);
        var rect = new Rectangle(0, 0, 72, 72);
        img.DrawRectangle(new Pen(color), rect);
        img.FillRectangle(new SolidBrush(color), rect);
        img.DrawString(text, new Font(FontFamily.GenericSansSerif, 15), Brushes.Black, 0, (72-15)/2);
        bitmap.Save(ms, ImageFormat.Png);
        byte[] byteImage = ms.ToArray();
        var str = Convert.ToBase64String(byteImage);
        return "data:image/png;base64," + str;
    }
    
    // Methods can be overriden to intercept events received from the Stream Deck.
    protected override async Task OnKeyDown(ActionEventArgs<KeyPayload> args)
    {
        _counter = 0;
        await SetImageAsync(PrepareBackground(Color.Green, "OK"));
    }
}