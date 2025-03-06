using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace TBoGV;
public class Slider : UIElement, IDraw
{
    private float minValue;
    private float maxValue;
    private float _value;
    public float Value
    {
        get => _value;
        set => _value = MathHelper.Clamp(value, minValue, maxValue);
    }

    private int sliderWidth;
    private int sliderHeight;
    private int thumbWidth;
    private int thumbHeight;

    private bool isDragging = false;
    private MouseState prevMouseState;

    private static Texture2D SpriteBackground = TextureManager.GetTexture("whiteSquare");
    public string Label { get; set; } = "";
    public SpriteFont LabelFont { get; set; }
    public Action<float> OnValueChanged;



    /// <summary>
    /// Constructs a Slider with a given value range and dimensions.
    /// </summary>
    /// <param name="minValue">The minimum slider value.</param>
    /// <param name="maxValue">The maximum slider value.</param>
    /// <param name="initialValue">The starting value.</param>
    /// <param name="width">The width of the slider track.</param>
    /// <param name="height">The height of the slider track.</param>
    public Slider(float minValue, float maxValue, float initialValue, int width, int height, string label, SpriteFont font, Action<float> onValueChanged)
    {
        Label = label;
        LabelFont = font;
        this.minValue = minValue;
        this.maxValue = maxValue;
        Value = initialValue;
        sliderWidth = width;
        sliderHeight = height;
        // Set the thumb size (for example, twice the track height).
        thumbWidth = height * 2;
        thumbHeight = height * 2;
        OnValueChanged = onValueChanged;
    }

    /// <summary>
    /// Returns the rectangle representing the slider's track.
    /// </summary>
    public Rectangle GetTrackRect()
    {
        return new Rectangle((int)Position.X, (int)Position.Y, sliderWidth, sliderHeight);
    }

    public Rectangle GetLabelRect()
    {
        if (string.IsNullOrEmpty(Label) || LabelFont == null)
            return Rectangle.Empty;

        int margin = 10;
        Vector2 labelSize = LabelFont.MeasureString(Label);
        Vector2 labelPos = new Vector2(Position.X + sliderWidth + margin, Position.Y + sliderHeight / 2 - labelSize.Y / 2);
        return new Rectangle((int)labelPos.X, (int)labelPos.Y, (int)labelSize.X, (int)labelSize.Y);
    }

    /// <summary>
    /// Returns the rectangle representing the slider's thumb.
    /// </summary>
    public Rectangle GetThumbRect()
    {
        // Normalize current value into a 0-to-1 range.
        float t = (Value - minValue) / (maxValue - minValue);
        int trackStart = (int)Position.X;
        int trackEnd = trackStart + sliderWidth - thumbWidth;
        int thumbX = trackStart + (int)(t * (trackEnd - trackStart));
        // Center the thumb vertically relative to the track.
        int thumbY = (int)Position.Y + sliderHeight / 2 - thumbHeight / 2;
        return new Rectangle(thumbX, thumbY, thumbWidth, thumbHeight);
    }

    /// <summary>
    /// Updates the slider state based on the current mouse input.
    /// </summary>
    public override void Update(MouseState mouseState)
    {
        if (mouseState.LeftButton == ButtonState.Pressed)
        {
            if (!isDragging && GetThumbRect().Contains(mouseState.Position))
            {
                isDragging = true;
            }
            if (isDragging)
            {
                int trackStart = (int)Position.X;
                int trackEnd = trackStart + sliderWidth - thumbWidth;
                // Calculate new thumb position based on the mouse, centering the thumb.
                float clampedX = MathHelper.Clamp(mouseState.X - thumbWidth / 2, trackStart, trackEnd);
                float t = (clampedX - trackStart) / (trackEnd - trackStart);
                float newValue = minValue + t * (maxValue - minValue);
                if (newValue != Value)
                {
                    Value = newValue;
                    OnValueChanged(Value);
                }
            }
        }
        else
        {
            isDragging = false;
        }

        prevMouseState = mouseState;
    }

    /// <summary>
    /// Draws the slider track and thumb.
    /// </summary>
    public void Draw(SpriteBatch spriteBatch)
    {
        // Draw the track.
        spriteBatch.Draw(SpriteBackground, GetTrackRect(), Color.Gray);
        // Draw the thumb.
        spriteBatch.Draw(SpriteBackground, GetThumbRect(), Color.White);
        // Draw the label to the right of the slider, if a label and font are provided.
        if (!string.IsNullOrEmpty(Label) && LabelFont != null)
        {
            int margin = 10; // space between the slider and the label
            Vector2 labelSize = LabelFont.MeasureString(Label);
            Vector2 labelPos = new Vector2(Position.X + sliderWidth + margin, Position.Y + sliderHeight / 2 - labelSize.Y / 2);
            spriteBatch.DrawString(LabelFont, Label, labelPos, Color.White);
        }
    }

    public override Rectangle GetRect()
    {
        return Rectangle.Union(Rectangle.Union(GetThumbRect(), GetTrackRect()), GetLabelRect());
    }

}