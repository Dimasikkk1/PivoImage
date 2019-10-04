using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace PivoLib
{
    public static class Pivo
    {
        /// <summary>
        /// Метод открытия Pivo картинки
        /// </summary>
        /// <param name="filePath">Путь к файлу (*.pivo)</param>
        /// <returns></returns>
        public static Bitmap Open(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException();

            using (FileStream fileStream = File.OpenRead(filePath))
            {
                if (fileStream.Length > int.MaxValue)
                    throw new Exception("Файл слишком большой для типа int.");

                byte[] buffer = new byte[4];
                fileStream.Read(buffer, 0, buffer.Length);

                int width = BitConverter.ToInt32(buffer, 0);
                int height = ((int)fileStream.Length - 4) / 3 / width;

                Bitmap bitmap = new Bitmap(width, height);

                buffer = new byte[3];

                for (int y = 0; y < height; y++)
                    for (int x = 0; x < width; x++)
                    {
                        fileStream.Read(buffer, 0, buffer.Length);
                        bitmap.SetPixel(x, y, Color.FromArgb(buffer[0], buffer[1], buffer[2]));
                    }

                return bitmap;
            }
        }

        /// <summary>
        /// Асинхронный метод открытия pivo картинки
        /// </summary>
        /// <param name="filePath">Путь к файлу (*.pivo)</param>
        /// <returns></returns>
        public static Task<Bitmap> OpenAsync(string filePath) => Task.Run(() => Open(filePath));

        /// <summary>
        /// Метод сохранения Pivo картинки
        /// </summary>
        /// <param name="filePath">Путь сохранения Pivo картинки (*.pivo)</param>
        /// <param name="bitmap">Сохраняемая картинка</param>
        public static void Save(string filePath, Bitmap bitmap)
        {
            using (FileStream fileStream = File.OpenWrite(filePath))
            {
                byte[] buffer = BitConverter.GetBytes(bitmap.Width);
                fileStream.Write(buffer, 0, buffer.Length);

                buffer = new byte[3];

                for (int y = 0; y < bitmap.Height; y++)
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        Color pixel = bitmap.GetPixel(x, y);
                        buffer[0] = pixel.R;
                        buffer[1] = pixel.G;
                        buffer[2] = pixel.B;
                        fileStream.Write(buffer, 0, buffer.Length);
                    }
            }
        }

        /// <summary>
        /// Асинхронный метод сохранения Pivo картинки
        /// </summary>
        /// <param name="filePath">Путь сохранения Pivo картинки (*.pivo)</param>
        /// <param name="bitmap">Сохраняемая картинка</param>
        public static Task SaveAsync(string filePath, Bitmap bitmap) => Task.Run(() => Save(filePath, bitmap));
    }
}
