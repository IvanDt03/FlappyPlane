using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Net.Sockets;



namespace FlappyPlane.Model;


public static class TextureExtentions
{
    /// <summary>
    /// Данный метод позволяет привести текстуру меньшего размера
    /// к текстуре большего размера без масштабирования. 
    /// Причем текстура будет выравнена по ценрту.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="device"></param>
    /// <param name="largeTexture"></param>
    /// <returns></returns>
    public static Texture2D ConversionTexture(this Texture2D source, GraphicsDevice device, Texture2D largeTexture)
    {
        var result = new Texture2D(device, largeTexture.Width, largeTexture.Height);

        Color[] dataLargeTexture = new Color[largeTexture.Width * largeTexture.Height];
        Color[] dataSourceTexture = new Color[source.Width * source.Height];
        source.GetData(dataSourceTexture);

        int offsetX = (largeTexture.Width - source.Width) / 2;
        int offsetY = (largeTexture.Height - source.Height) / 2;

        for (int y = 0; y < source.Height; y++)
            for (int x = 0; x < source.Width; x++)
                dataLargeTexture[(y + offsetY) * largeTexture.Width + (x + offsetX)] = 
                    dataSourceTexture[y * source.Width + x];

        result.SetData(dataLargeTexture);
        return result;
    }
}
