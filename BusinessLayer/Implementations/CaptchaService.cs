using BusinessLayer.Interfaces;
using SkiaSharp;

namespace BusinessLayer.Implementations
{
    public class CaptchaService: ICaptchaService
    {
        public (string Code, byte[] ImageBytes) GenerateCaptcha()
        {
            string code = GenerateRandomCode(5);

            using var bitmap = new SKBitmap(200, 60);
            using var canvas = new SKCanvas(bitmap);
            canvas.Clear(SKColors.White);

            var paint = new SKPaint
            {
                Color = SKColors.Black,
                TextSize = 32,
                IsAntialias = true
            };

            canvas.DrawText(code, 20, 40, paint);

            using var image = SKImage.FromBitmap(bitmap);
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);

            return (code, data.ToArray());
        }


        private string GenerateRandomCode(int length)
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        }
    }
