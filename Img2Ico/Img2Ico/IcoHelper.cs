namespace Img2Ico;

/// <summary>
/// ICO 文件格式写入器 — 生成多尺寸 PNG 编码的 ICO 文件 (Vista+ 标准)
/// </summary>
public static class IcoHelper
{
    // ICO 文件头: 保留(2) + 类型(2) + 图像数(2) = 6 字节
    // 每个目录项: 16 字节
    const int HEADER_SIZE = 6;
    const int DIR_ENTRY_SIZE = 16;

    public static readonly HashSet<string> SupportedExtensions = new(
        StringComparer.OrdinalIgnoreCase)
    {
        ".png", ".jpg", ".jpeg", ".bmp", ".gif", ".tiff", ".tif", ".webp", ".ico"
    };

    public static readonly Dictionary<string, int[]> SizePresets = new()
    {
        ["标准 (16–256)"] = [16, 20, 24, 32, 40, 48, 64, 96, 128, 256],
        ["精简 (16–64)"]  = [16, 24, 32, 48, 64],
        ["完整 (16–256)"] = [16, 20, 24, 28, 32, 36, 40, 44, 48, 56, 64, 72, 80, 96, 128, 256],
        ["仅 256×256"]    = [256],
        ["仅 128×128"]    = [128],
        ["仅 64×64"]      = [64],
        ["仅 32×32"]      = [32],
        ["仅 16×16"]      = [16],
    };

    /// <summary>
    /// 转换图片为 ICO，返回成功生成的文件路径
    /// </summary>
    public static string ConvertToIco(
        string sourcePath,
        string outputDir,
        int[] sizes,
        string suffix = "_icon")
    {
        using var src = Image.FromFile(sourcePath);
        int srcW = src.Width, srcH = src.Height;

        // 过滤可用尺寸
        var usable = sizes.Where(s => s <= Math.Max(srcW, srcH)).ToArray();
        if (usable.Length == 0)
            usable = [sizes.Min()];

        // 确保 RGBA 格式
        using var safeBmp = new Bitmap(srcW, srcH, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        using (var g = Graphics.FromImage(safeBmp))
        {
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.DrawImage(src, 0, 0, srcW, srcH);
        }

        // 生成各尺寸帧 (从大到小, 保持原始比例, 不补白边)
        var frames = new List<(byte[] pngData, int w, int h)>();
        foreach (int s in usable.OrderByDescending(x => x))
        {
            double ratio = Math.Min((double)s / srcW, (double)s / srcH);
            int newW = Math.Max(1, (int)(srcW * ratio));
            int newH = Math.Max(1, (int)(srcH * ratio));

            // 直接用缩放后的实际尺寸, 不强制正方形
            using var resized = new Bitmap(newW, newH, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (var g = Graphics.FromImage(resized))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(safeBmp, 0, 0, newW, newH);
            }

            using var ms = new MemoryStream();
            resized.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            frames.Add((ms.ToArray(), newW, newH));
        }

        // 写入 ICO 文件
        string baseName = Path.GetFileNameWithoutExtension(sourcePath);
        string outPath = Path.Combine(outputDir, $"{baseName}{suffix}.ico");

        using var fs = new FileStream(outPath, FileMode.Create, FileAccess.Write);
        using var bw = new BinaryWriter(fs);

        // 1) ICO 头
        bw.Write((short)0);   // 保留
        bw.Write((short)1);   // 类型: ICO
        bw.Write((short)frames.Count); // 图像数量

        // 2) 计算偏移
        int dataOffset = HEADER_SIZE + frames.Count * DIR_ENTRY_SIZE;

        // 3) 写目录项
        for (int i = 0; i < frames.Count; i++)
        {
            var (pngData, w, h) = frames[i];
            // ICO 目录: 256 记作 0
            byte bw_val = (byte)(w > 255 ? 0 : w);
            byte bh_val = (byte)(h > 255 ? 0 : h);

            bw.Write(bw_val);          // 宽度
            bw.Write(bh_val);          // 高度
            bw.Write((byte)0);         // 颜色数 (0 = 真彩色)
            bw.Write((byte)0);         // 保留
            bw.Write((short)1);        // 平面数
            bw.Write((short)32);       // 位深度
            bw.Write(pngData.Length);  // 数据大小
            bw.Write(dataOffset);      // 数据偏移
            dataOffset += pngData.Length;
        }

        // 4) 写 PNG 图像数据
        for (int i = 0; i < frames.Count; i++)
        {
            bw.Write(frames[i].pngData);
        }

        return outPath;
    }
}
