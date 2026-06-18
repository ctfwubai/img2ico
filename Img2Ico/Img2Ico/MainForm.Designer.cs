#nullable disable
namespace Img2Ico;

partial class MainForm
{
    private System.ComponentModel.IContainer? components = null;

    // 控件
    private Panel titleBar;
    private Label titleLabel;
    private Panel card1, card2, card3;
    private Label lblCard1, lblCard2, lblCard3;
    private ListBox lstFiles;
    private Button btnAdd, btnRemove, btnClear, btnBrowseOut, btnConvert, btnOpenDir;
    private Label lblCount, lblSizePreview;
    private ComboBox cboPreset;
    private TextBox txtOutputDir, txtSuffix;
    private Label statusLabel, infoLabel;
    private Panel statusBar;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    void InitializeComponent()
    {
        this.Text = "图片转 ICO 图标工具";
        this.ClientSize = new Size(650, 580);
        this.MinimumSize = new Size(580, 480);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Font = new Font("Microsoft YaHei UI", 10);
        this.BackColor = Color.FromArgb(240, 240, 245);
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.AllowDrop = true;

        // ── 顶部标题栏 ──
        titleBar = new Panel
        {
            BackColor = Color.FromArgb(74, 108, 247),
            Height = 48,
            Dock = DockStyle.Top,
        };
        titleLabel = new Label
        {
            Text = "🖼️ 图片转 ICO 图标工具",
            Font = new Font("Microsoft YaHei UI", 14, FontStyle.Bold),
            ForeColor = Color.White,
            AutoSize = true,
            Location = new Point(14, 10),
        };
        titleBar.Controls.Add(titleLabel);

        // ── 卡片1: 选择图片 ──
        card1 = new Panel
        {
            Location = new Point(10, 56),
            Size = new Size(630, 200),
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle,
        };
        lblCard1 = new Label
        {
            Text = "📂 选择图片",
            Font = new Font("Microsoft YaHei UI", 11, FontStyle.Bold),
            ForeColor = Color.FromArgb(55, 65, 81),
            AutoSize = true,
            Location = new Point(10, 8),
        };

        // 按钮栏
        btnAdd = MakeButton("➕ 添加图片", new Point(10, 32), Color.FromArgb(74, 108, 247), Color.White);
        btnAdd.Click += btnAdd_Click;
        btnRemove = MakeButton("❌ 移除选中", new Point(130, 32), Color.FromArgb(243, 244, 246), Color.Black);
        btnRemove.Click += btnRemove_Click;
        btnClear = MakeButton("🗑 清空列表", new Point(250, 32), Color.FromArgb(243, 244, 246), Color.Black);
        btnClear.Click += btnClear_Click;

        // 提示
        var hintLabel = new Label
        {
            Text = "📂 拖放文件到此区域即可添加",
            Font = new Font("Microsoft YaHei UI", 9),
            ForeColor = Color.FromArgb(156, 163, 175),
            AutoSize = true,
            Location = new Point(420, 38),
        };

        // 文件列表
        lstFiles = new ListBox
        {
            Location = new Point(10, 62),
            Size = new Size(610, 100),
            Font = new Font("Consolas", 10),
            BackColor = Color.FromArgb(250, 251, 252),
            BorderStyle = BorderStyle.FixedSingle,
            AllowDrop = true,
            IntegralHeight = false,
        };
        lstFiles.DragEnter += lstFiles_DragEnter!;
        lstFiles.DragDrop += lstFiles_DragDrop!;
        lstFiles.KeyDown += (s, e) => { if (e.KeyCode == Keys.Delete) btnRemove_Click(s, e); };

        lblCount = new Label
        {
            Text = "尚未选择文件",
            Font = new Font("Microsoft YaHei UI", 9),
            ForeColor = Color.FromArgb(156, 163, 175),
            AutoSize = true,
            Location = new Point(10, 172),
        };

        card1.Controls.AddRange([lblCard1, btnAdd, btnRemove, btnClear, hintLabel, lstFiles, lblCount]);

        // ── 卡片2: 设置 ──
        card2 = new Panel
        {
            Location = new Point(10, 262),
            Size = new Size(630, 150),
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle,
        };
        lblCard2 = new Label
        {
            Text = "⚙️ 输出设置",
            Font = new Font("Microsoft YaHei UI", 11, FontStyle.Bold),
            ForeColor = Color.FromArgb(55, 65, 81),
            AutoSize = true,
            Location = new Point(10, 8),
        };

        // 尺寸预设
        int y = 38;
        var lblSize = new Label
        {
            Text = "图标尺寸:",
            AutoSize = true,
            Location = new Point(12, y + 5),
        };
        cboPreset = new ComboBox
        {
            Location = new Point(95, y + 2),
            Width = 180,
            DropDownStyle = ComboBoxStyle.DropDownList,
            Font = new Font("Microsoft YaHei UI", 10),
        };
        cboPreset.Items.AddRange(IcoHelper.SizePresets.Keys.ToArray<object>());
        cboPreset.SelectedIndex = 0;
        cboPreset.SelectedIndexChanged += cboPreset_SelectedIndexChanged;

        lblSizePreview = new Label
        {
            Text = "",
            AutoSize = true,
            Location = new Point(285, y + 5),
            ForeColor = Color.FromArgb(107, 114, 128),
            Font = new Font("Consolas", 9),
        };

        // 输出目录
        y += 32;
        var lblOut = new Label
        {
            Text = "输出目录:",
            AutoSize = true,
            Location = new Point(12, y + 5),
        };
        txtOutputDir = new TextBox
        {
            Location = new Point(95, y + 2),
            Width = 420,
            Font = new Font("Consolas", 10),
            Text = "",
        };
        btnBrowseOut = new Button
        {
            Text = "浏览...",
            Location = new Point(522, y),
            Width = 90,
            Height = 26,
            Font = new Font("Microsoft YaHei UI", 10),
            BackColor = Color.FromArgb(243, 244, 246),
            FlatStyle = FlatStyle.Flat,
        };
        btnBrowseOut.FlatAppearance.BorderSize = 0;
        btnBrowseOut.Click += btnBrowseOut_Click;

        // 后缀
        y += 32;
        var lblSuffix = new Label
        {
            Text = "文件名后缀:",
            AutoSize = true,
            Location = new Point(12, y + 5),
        };
        txtSuffix = new TextBox
        {
            Text = "_icon",
            Location = new Point(110, y + 2),
            Width = 120,
            Font = new Font("Consolas", 10),
        };
        var lblSuffixHint = new Label
        {
            Text = "(如 _icon → logo_icon.ico)",
            AutoSize = true,
            Location = new Point(240, y + 5),
            ForeColor = Color.FromArgb(156, 163, 175),
            Font = new Font("Microsoft YaHei UI", 9),
        };

        card2.Controls.AddRange([lblCard2, lblSize, cboPreset, lblSizePreview,
            lblOut, txtOutputDir, btnBrowseOut, lblSuffix, txtSuffix, lblSuffixHint]);

        // ── 卡片3: 操作 ──
        card3 = new Panel
        {
            Location = new Point(10, 420),
            Size = new Size(630, 72),
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle,
        };
        lblCard3 = new Label
        {
            Text = "🚀 执行转换",
            Font = new Font("Microsoft YaHei UI", 11, FontStyle.Bold),
            ForeColor = Color.FromArgb(55, 65, 81),
            AutoSize = true,
            Location = new Point(10, 8),
        };

        btnConvert = new Button
        {
            Text = "🔄 开始转换",
            Font = new Font("Microsoft YaHei UI", 12, FontStyle.Bold),
            ForeColor = Color.White,
            BackColor = Color.FromArgb(74, 108, 247),
            FlatStyle = FlatStyle.Flat,
            Location = new Point(14, 34),
            Width = 160,
            Height = 30,
            Cursor = Cursors.Hand,
        };
        btnConvert.FlatAppearance.BorderSize = 0;
        btnConvert.Click += btnConvert_Click;

        btnOpenDir = new Button
        {
            Text = "📁 打开输出目录",
            Font = new Font("Microsoft YaHei UI", 10),
            BackColor = Color.FromArgb(243, 244, 246),
            FlatStyle = FlatStyle.Flat,
            Location = new Point(184, 34),
            Width = 130,
            Height = 30,
            Cursor = Cursors.Hand,
        };
        btnOpenDir.FlatAppearance.BorderSize = 0;
        btnOpenDir.Click += btnOpenDir_Click;

        card3.Controls.AddRange([lblCard3, btnConvert, btnOpenDir]);

        // ── 状态栏 ──
        statusBar = new Panel
        {
            Height = 28,
            Dock = DockStyle.Bottom,
            BackColor = Color.FromArgb(229, 231, 235),
        };
        statusLabel = new Label
        {
            Text = "就绪 — 请添加图片文件",
            Font = new Font("Microsoft YaHei UI", 9),
            ForeColor = Color.FromArgb(75, 85, 99),
            AutoSize = true,
            Location = new Point(12, 5),
        };
        infoLabel = new Label
        {
            Text = "v2.0.0 | CTF_无白 | github.com/ctfwubai",
            Font = new Font("Consolas", 8),
            ForeColor = Color.FromArgb(107, 114, 128),
            AutoSize = true,
            Location = new Point(400, 6),
        };
        statusBar.Controls.Add(statusLabel);
        statusBar.Controls.Add(infoLabel);

        // ── 加入窗体 ──
        this.Controls.AddRange([card1, card2, card3, titleBar, statusBar]);

        // 窗口大小调整时 info 标签跟随
        this.Resize += (s, e) =>
        {
            infoLabel.Left = this.ClientSize.Width - infoLabel.Width - 16;
        };
    }

    static Button MakeButton(string text, Point loc, Color back, Color fore)
    {
        var btn = new Button
        {
            Text = text,
            Font = new Font("Microsoft YaHei UI", 10),
            BackColor = back,
            ForeColor = fore,
            FlatStyle = FlatStyle.Flat,
            Location = loc,
            AutoSize = true,
            Cursor = Cursors.Hand,
        };
        btn.FlatAppearance.BorderSize = 0;
        return btn;
    }
}
