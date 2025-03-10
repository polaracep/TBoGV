using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace TBoGV;

public class ButtonImageHeadline : Button
{
    Texture2D Sprite;
    ImageOrientation ImageOrientation;
    float ImageScale;
    protected string Headline;
    public ButtonImageHeadline(string headline, string text, SpriteFont font, Action onClick, Texture2D sprite) : base(text, font, onClick)
    {
        Sprite = sprite;
        ImageOrientation = ImageOrientation.TOP;
        Headline = headline;
        int sizeX = (int)Math.Max(Math.Max(font.MeasureString(headline).X, font.MeasureString(text).X), Sprite.Width) + 2*BorderOffset;
        int sizeY = (int)(font.MeasureString(headline).Y + font.MeasureString(text).Y + Sprite.Height);
        ImageScale = 1;
        Size = new Vector2 (sizeX, sizeY);

    }
    public override void Draw(SpriteBatch spriteBatch)
    {
        Rectangle rect = GetRect();
        spriteBatch.Draw(SpriteBackground, rect, color[ColorIndex]);

        Vector2 headlineSize = Font.MeasureString(Headline);
        Vector2 textSize = Font.MeasureString(Text);
        Vector2 imageSize = new Vector2(Sprite.Width * ImageScale, Sprite.Height * ImageScale);

        // Celková výška prvků
        float totalHeight = headlineSize.Y + imageSize.Y + textSize.Y + 2 * BorderOffset;

        // Výchozí Y pozice pro centrování vertikálně
        float startY = rect.Y + (rect.Height - totalHeight) / 2;

        // Pozice nadpisu
        Vector2 headlinePos = new Vector2(rect.X + (rect.Width - headlineSize.X) / 2, startY);
        startY += headlineSize.Y + BorderOffset;

        // Pozice obrázku
        Vector2 imagePos = new Vector2(rect.X + (rect.Width - imageSize.X) / 2, startY);
        startY += imageSize.Y + BorderOffset;

        // Pozice textu
        Vector2 textPos = new Vector2(rect.X + (rect.Width - textSize.X) / 2, startY);

        // Vykreslení prvků
        spriteBatch.DrawString(Font, Headline, headlinePos, Color.White);
        spriteBatch.Draw(Sprite, imagePos, null, Color.White, 0, Vector2.Zero, ImageScale, SpriteEffects.None, 0);
        spriteBatch.DrawString(Font, Text, textPos, TextColor);
    }

    public override void SetSize(Vector2 size)
    {
        Vector2 textSize = Font.MeasureString(Text) + Font.MeasureString(Headline);
        float maxScaleX = 1f, maxScaleY = 1f;
        Vector2 newSize = new Vector2(Math.Max(Size.X,size.X), Math.Max(Size.Y, size.Y));

        maxScaleY = (newSize.Y - textSize.Y - BorderOffset) / Sprite.Height;
        maxScaleX = newSize.X / Sprite.Width;


        ImageScale = Math.Min(maxScaleX, maxScaleY);

        Size = newSize;
    }
}