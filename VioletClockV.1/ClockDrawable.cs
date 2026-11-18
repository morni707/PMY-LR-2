namespace VioletClockV._1;

public class ClockDrawable : IDrawable
{
    // Матрицы цифр от 0 до 9.
    private static readonly string[][] Digits =
    {
        new[] //0
        {
            "1111111", "1000001", "1000001", "1000001",
            "1000001", "1000001", "1111111"
        },
        new[] //1
        {
            "00100", "01100", "00100", "00100",
            "00100", "00100", "01110"
        },
        new[] // 2
        {
            "11111", "00001", "00001", "11111",
            "10000", "10000", "11111"
        },
        new[] //3
        {
            "11111", "00001", "00001", "01111",
            "00001", "00001", "11111"
        },
        new[] //4
        {
            "10001", "10001", "10001", "11111",
            "00001", "00001", "00001"
        },
        new[] //5
        {
            "11111", "10000", "10000", "11111",
            "00001", "00001", "11111"
        },
        new[] //6
        {
            "11111", "10000", "10000", "11111",
            "10001", "10001", "11111"
        },
        new[] //7
        {
            "11111", "00001", "00010", "00100",
            "01000", "01000", "01000"
        },
        new[] //8
        {
            "11111", "10001", "10001", "11111",
            "10001", "10001", "11111"
        },
        new[] //9
        {
            "11111", "10001", "10001", "11111",
            "00001", "00001", "11111"
        }
    };

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.FillColor = Color.FromArgb("#0B0714");
        canvas.FillRectangle(dirtyRect);

        string time = DateTime.Now.ToString("HH:mm:ss");

        float gapRatio = 0.18f;
        float characterSpacingRatio = 0.8f;

        int maxRows = Digits.Max(digit => digit.Length);
        float heightUnits = maxRows + (maxRows - 1) * gapRatio;

        float widthUnits = 0;

        // Рассчитываем общую ширину часов.
        for (int i = 0; i < time.Length; i++)
        {
            if (time[i] == ':')
            {
                widthUnits += 1;
            }
            else
            {
                int digitIndex = time[i] - '0';
                int cols = Digits[digitIndex][0].Length;
                // Рассичтывается ширина цифры с учётом промежутков между пикселями 
                widthUnits += cols + (cols - 1) * gapRatio;
            }

            if (i < time.Length - 1)
                widthUnits += characterSpacingRatio;
        }

        // Подбираем размер пикселя под экран.
        float availableWidth = dirtyRect.Width * 0.9f;
        float availableHeight = dirtyRect.Height * 0.9f;

        float pixel = Math.Min(
            availableWidth / widthUnits,
            availableHeight / heightUnits
        );

        if (pixel <= 0)
            return;

        float gap = pixel * gapRatio;
        float step = pixel + gap;
        float characterSpacing = pixel * characterSpacingRatio;

        float totalWidth = widthUnits * pixel;
        float totalHeight = heightUnits * pixel;

        float startX = dirtyRect.X + (dirtyRect.Width - totalWidth) / 2f;
        float startY = dirtyRect.Y + (dirtyRect.Height - totalHeight) / 2f;

        float x = startX;

        // Отрисовываем цифры и двоеточия.
        foreach (char c in time)
        {
            if (c == ':')
            {
                DrawColon(canvas, x, startY, pixel, totalHeight);
                x += pixel + characterSpacing;
                continue;
            }

            int digitIndex = c - '0';
            string[] digit = Digits[digitIndex];

            int rows = digit.Length;
            // Кол-во столбцов конкретной цифры
            int cols = digit[0].Length;

            float digitHeight = rows * pixel + (rows - 1) * gap;
            float digitStartY = startY + (totalHeight - digitHeight) / 2f;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < digit[row].Length; col++)
                {
                    if (digit[row][col] != '1')
                        continue;

                    float px = x + col * step;
                    float py = digitStartY + row * step;

                    canvas.FillColor = Color.FromArgb("#B66DFF");

                    canvas.FillRoundedRectangle(
                        px, py, pixel, pixel, pixel * 0.18f
                    );
                }
            }
            // Программа определяет на какаом расстоянии начинать рисовать след цифры 
            x += cols * step - gap + characterSpacing;
        }
    }

    // Рисуем две точки двоеточия.
    private static void DrawColon(
        ICanvas canvas,
        float x,
        float y,
        float pixel,
        float totalHeight)
    {
        canvas.FillColor = Color.FromArgb("#E4B5FF");

        float centerX = x + pixel / 2f;
        float centerY1 = y + totalHeight * 0.32f;
        float centerY2 = y + totalHeight * 0.68f;

        float radius = pixel * 0.38f;

        canvas.FillCircle(centerX, centerY1, radius);
        canvas.FillCircle(centerX, centerY2, radius);
    }
}
