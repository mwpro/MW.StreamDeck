using OpenMacroBoard.SDK;
using StreamDeckSharp;

var red = KeyBitmap.Create.FromRgb(237, 41, 57);
var white = KeyBitmap.Create.FromRgb(255, 255, 255);
var rowColors = new KeyBitmap[] { red, white, red };

//Open the Stream Deck device
using (var deck = StreamDeck.OpenDevice())
{
    deck.SetBrightness(100);

    //Send the bitmap informaton to the device
    for (int i = 0; i < deck.Keys.Count; i++)
        deck.SetKeyBitmap(i, rowColors[i / 5]);

    Console.ReadKey();
}