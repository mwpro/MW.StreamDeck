using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Imaging;
using SharpDeck;
using SharpDeck.Events.Received;

namespace MW.StreamDeck;

[StreamDeckAction("com.mwpro.streamdeck.helloworld")]
[SuppressMessage("Interoperability", "CA1416")]
public class HelloWorld : StreamDeckAction
{
    private int _counter = 0;
    private PeriodicTimer _timer;
    private PeriodicTimer _blinker = null;

    public HelloWorld()
    {
        _timer = new PeriodicTimer(TimeSpan.FromSeconds(10));
        EmulateAlertsWithTimer();
    }

    protected override void OnInit(ActionEventArgs<AppearancePayload> args)
    {
        Reset();
        base.OnInit(args);
    }

    protected override async Task OnKeyDown(ActionEventArgs<KeyPayload> args)
    {
        await Reset();
    }

    private async Task EmulateAlertsWithTimer()
    {
        while (await _timer.WaitForNextTickAsync())
        {
            _counter++;
            var isDanger = _counter >= 3;

            SetAlert(isDanger);
        }
    }

    private void SetAlert(bool isDanger)
    {
        _blinker = new PeriodicTimer(TimeSpan.FromSeconds(1));
        DoBlink(isDanger ? Color.Red : Color.Yellow, 
            isDanger ? Color.White : Color.Black,
            isDanger ? "ALARM" : "WARN");
    }

    private async Task DoBlink(Color backgroundColor, Color textColor, string text)
    {
        var previousStateWasColor = false;
        while (await _blinker.WaitForNextTickAsync())
        {
            SetImageAsync(PrepareBackground(previousStateWasColor ? Color.Black : backgroundColor, 
                previousStateWasColor ? Color.White : textColor, 
                text));
            previousStateWasColor = !previousStateWasColor;
        }
    }

    private async Task Reset()
    {
        _counter = 0;
        _blinker?.Dispose();
        await SetImageAsync(PrepareBackground(Color.Green, Color.White, "OK"));
    }

    private string PrepareBackground(Color backgroundColor, Color textColor, string text)
    {
        var ms = new MemoryStream();
        var bitmap = new Bitmap(72, 72);
        var img = Graphics.FromImage(bitmap);
        var rect = new Rectangle(0, 0, 72, 72);
        img.DrawRectangle(new Pen(backgroundColor), rect);
        img.FillRectangle(new SolidBrush(backgroundColor), rect);
        img.DrawString(text, new Font(FontFamily.GenericSansSerif, 15), new SolidBrush(textColor), 0, (72-15)/2);
        bitmap.Save(ms, ImageFormat.Png);
        var byteImage = ms.ToArray();
        var str = Convert.ToBase64String(byteImage);
        return $"data:image/png;base64,{str}";
    }
}