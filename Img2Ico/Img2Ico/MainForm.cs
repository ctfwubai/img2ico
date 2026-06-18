using System.ComponentModel;

namespace Img2Ico;

public partial class MainForm : Form
{
    private readonly List<string> _sourceFiles = [];
    private string _outputDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

    public MainForm()
    {
        InitializeComponent();
        UpdateCount();
        UpdateSizePreview();
        txtOutputDir.Text = _outputDir;
    }

    // ═══ 拖放 ═══
    private void lstFiles_DragEnter(object? sender, DragEventArgs e)
    {
        if (e.Data?.GetDataPresent(DataFormats.FileDrop) == true)
            e.Effect = DragDropEffects.Copy;
    }

    private void lstFiles_DragDrop(object? sender, DragEventArgs e)
    {
        if (e.Data?.GetData(DataFormats.FileDrop) is not string[] files) return;
        foreach (string f in files)
        {
            if (!File.Exists(f)) continue;
            string ext = Path.GetExtension(f);
            if (!IcoHelper.SupportedExtensions.Contains(ext)) continue;
            if (_sourceFiles.Contains(f)) continue;
            _sourceFiles.Add(f);
            lstFiles.Items.Add(Path.GetFileName(f));
        }
        UpdateCount();
    }

    // ═══ 按钮 ═══
    private void btnAdd_Click(object? sender, EventArgs e)
    {
        using var dlg = new OpenFileDialog
        {
            Title = "选择图片文件",
            Filter = $"图片文件|{string.Join(";", IcoHelper.SupportedExtensions.Select(e => $"*{e}"))}|所有文件|*.*",
            Multiselect = true,
        };
        if (dlg.ShowDialog() != DialogResult.OK) return;
        foreach (string f in dlg.FileNames)
        {
            if (_sourceFiles.Contains(f)) continue;
            _sourceFiles.Add(f);
            lstFiles.Items.Add(Path.GetFileName(f));
        }
        UpdateCount();
    }

    private void btnClear_Click(object? sender, EventArgs e)
    {
        _sourceFiles.Clear();
        lstFiles.Items.Clear();
        UpdateCount();
    }

    private void btnRemove_Click(object? sender, EventArgs e)
    {
        var indices = lstFiles.SelectedIndices.Cast<int>().OrderByDescending(i => i).ToArray();
        foreach (int i in indices)
        {
            lstFiles.Items.RemoveAt(i);
            _sourceFiles.RemoveAt(i);
        }
        UpdateCount();
    }

    private void btnBrowseOut_Click(object? sender, EventArgs e)
    {
        using var dlg = new FolderBrowserDialog
        {
            Description = "选择输出目录",
            InitialDirectory = _outputDir,
            ShowNewFolderButton = true,
        };
        if (dlg.ShowDialog() == DialogResult.OK)
        {
            _outputDir = dlg.SelectedPath;
            txtOutputDir.Text = _outputDir;
        }
    }

    private async void btnConvert_Click(object? sender, EventArgs e)
    {
        if (_sourceFiles.Count == 0)
        {
            MessageBox.Show("请先添加要转换的图片文件。", "提示",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        string outDir = txtOutputDir.Text.Trim();
        if (string.IsNullOrEmpty(outDir))
        {
            MessageBox.Show("请指定输出目录。", "提示",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try { Directory.CreateDirectory(outDir); }
        catch (Exception ex)
        {
            MessageBox.Show($"无法创建输出目录:\n{ex.Message}", "错误",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        btnConvert.Enabled = false;
        btnConvert.Text = "转换中...";
        statusLabel.Text = "正在转换...";
        statusLabel.ForeColor = Color.DimGray;

        // 在后台线程执行转换
        var results = await Task.Run(() =>
        {
            var saved = new List<string>();
            int[] sizes = IcoHelper.SizePresets[cboPreset.SelectedItem?.ToString() ?? "标准 (16–256)"];
            string suffix = txtSuffix.Text.Trim();
            if (string.IsNullOrWhiteSpace(suffix)) suffix = "_icon";

            foreach (string src in _sourceFiles)
            {
                try
                {
                    string outPath = IcoHelper.ConvertToIco(src, outDir, sizes, suffix);
                    saved.Add(outPath);
                }
                catch (Exception ex)
                {
                    // 记录但继续处理其他文件
                    System.Diagnostics.Debug.WriteLine($"转换失败: {src} — {ex.Message}");
                }
            }
            return saved;
        });

        btnConvert.Enabled = true;
        btnConvert.Text = "开始转换";

        if (results.Count > 0)
        {
            statusLabel.Text = $"转换完成! 成功 {results.Count}/{_sourceFiles.Count} 个文件";
            statusLabel.ForeColor = Color.ForestGreen;

            var ask = MessageBox.Show(
                $"成功转换 {results.Count}/{_sourceFiles.Count} 个文件。\n\n输出目录:\n{outDir}\n\n是否打开输出目录?",
                "转换完成", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (ask == DialogResult.Yes)
                OpenOutputDir();
        }
        else
        {
            statusLabel.Text = "转换失败，请确认图片格式是否受支持。";
            statusLabel.ForeColor = Color.DarkRed;
            MessageBox.Show("没有成功转换任何文件。", "失败",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnOpenDir_Click(object? sender, EventArgs e) => OpenOutputDir();

    private void OpenOutputDir()
    {
        string dir = txtOutputDir.Text.Trim();
        if (Directory.Exists(dir))
            System.Diagnostics.Process.Start("explorer.exe", dir);
        else
            MessageBox.Show($"目录不存在:\n{dir}", "提示");
    }

    private void cboPreset_SelectedIndexChanged(object? sender, EventArgs e) => UpdateSizePreview();

    // ═══ 辅助 ═══
    private void UpdateCount()
    {
        int n = _sourceFiles.Count;
        lblCount.Text = n == 0
            ? "尚未选择文件"
            : n == 1
                ? $"已选择 1 个文件: {Path.GetFileName(_sourceFiles[0])}"
                : $"已选择 {n} 个文件";
    }

    private void UpdateSizePreview()
    {
        string key = cboPreset.SelectedItem?.ToString() ?? "标准 (16–256)";
        if (IcoHelper.SizePresets.TryGetValue(key, out var sizes))
            lblSizePreview.Text = $"→ {string.Join(", ", sizes)} px";
    }
}
