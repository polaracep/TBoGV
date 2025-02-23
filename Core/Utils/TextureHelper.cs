using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// gipiti hihi
public static class Texture2DHelper
{
    /// <summary>
    /// Flips the pixel data of the given Texture2D horizontally in place.
    /// </summary>
    /// <param name="texture">The Texture2D to flip.</param>
    public static void FlipHorizontally(this Texture2D texture)
    {
        // 1. Get the texture’s pixel data
        int width = texture.Width;
        int height = texture.Height;
        Color[] originalData = new Color[width * height];
        texture.GetData(originalData);

        // 2. Create a temporary array for the flipped data
        Color[] flippedData = new Color[width * height];

        // 3. Flip each row (reversing columns)
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int srcIndex = y * width + x;
                int dstIndex = y * width + (width - 1 - x);
                flippedData[dstIndex] = originalData[srcIndex];
            }
        }

        // 4. Copy the flipped data back into the texture
        texture.SetData(flippedData);
    }
}
