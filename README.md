# 🖼️ 图片转 ICO 图标工具 v2.0.0

> PNG / JPG / JPEG / BMP / GIF / TIFF / WebP → Windows ICO 多尺寸图标  
> 作者: **CTF_无白** &nbsp;|&nbsp; github.com/ctfwubai

---

## 📥 下载

> 👉 **[GitHub Releases](../../releases)** 下载最新版本

| 版本 | 文件名 | 大小 | 需要额外安装？ | 适用场景 |
|------|--------|------|:---:|------|
| **标准版** | `Img2Ico.exe` | **176 KB** | ⚠️ 需 .NET 8 | 日常使用，机器有 .NET |
| **独立版** | `Img2Ico_standalone.exe` | **25 MB** | ✅ 无需任何依赖 | 复制到任何 Windows 直接跑 |

---

## ⚙️ 系统要求

### 标准版 (176 KB)

| 依赖 | 说明 |
|------|------|
| **.NET 8 Desktop Runtime** | [点击下载](https://dotnet.microsoft.com/zh-cn/download/dotnet/8.0) → 选「.NET Desktop Runtime 8.0.x」→ Windows x64 |
| Windows 10 1809+ / Windows 11 | x64 架构 |

> 💡 如果双击报错「要运行此应用程序，必须安装 .NET Desktop Runtime」，点击对话框中的 **"是"** 按钮即可自动跳转下载页。安装约 50MB，之后本机所有 .NET 8 程序都能跑。

**如何检查是否已安装？** — 打开 CMD 执行：
```cmd
dotnet --list-runtimes | findstr "Desktop 8"
```
有输出即已安装。Windows Update 通常会自动推送。

### 独立版 (25 MB)

| 依赖 | 说明 |
|------|------|
| **无** | 不依赖任何运行环境，复制即用 |
| Windows 10 1809+ / Windows 11 | x64 架构 |

---

## 🚀 使用

```
1. 双击 Img2Ico.exe 启动
2. 把图片文件拖到窗口里（或点 ➕ 添加图片）
3. 选择图标尺寸预设（默认 16–256 px）
4. 选输出目录
5. 点 🔄 开始转换
```

---

## ✨ 功能特性

- 📂 **拖放添加** — 把图片文件直接拖到列表即可添加
- 🔄 **批量转换** — 多张图片一次批量生成 ICO
- 📐 **多尺寸输出** — 自动生成多档尺寸，适配所有 Windows 显示场景
- 🎨 **等比缩放居中** — 非正方形图片自动补透明边缘，保持比例
- ⚙️ **8 种尺寸预设** — 从完整 16 档到单尺寸自由选择
- 🏷️ **自定义后缀** — 输出文件名可加后缀，如 `app_icon.ico`
- 🖱️ **右键快捷删除** — 选中文件按 Delete 键即可移除

## 📸 支持的输入格式

| 格式 | 扩展名 | 透明通道 | 备注 |
|------|--------|:---:|------|
| **PNG** | `.png` | ✅ 支持 | 推荐格式，保留透明背景 |
| **JPEG** | `.jpg` `.jpeg` | ❌ 不支持 | 最通用的照片格式 |
| **BMP** | `.bmp` | ✅ 支持 | Windows 原生位图 |
| **GIF** | `.gif` | ✅ 支持 | 动图仅取首帧 |
| **TIFF** | `.tiff` `.tif` | ✅ 支持 | 高质量印刷格式 |
| **WebP** | `.webp` | ✅ 支持 | 新一代网页图片格式 |
| **ICO** | `.ico` | ✅ 支持 | 已有图标可重新指定尺寸 |

> 📌 所有格式**统一输出**为 `.ico` (Windows 图标)，内部使用 PNG 编码（Vista+ 标准），最大支持 256×256 px。

### 输出尺寸预设

| 预设名称 | 包含尺寸 |
|----------|----------|
| **标准 (16–256)** | 16, 20, 24, 32, 40, 48, 64, 96, 128, 256 |
| **精简 (16–64)** | 16, 24, 32, 48, 64 |
| **完整 (16–256)** | 16, 20, 24, 28, 32, 36, 40, 44, 48, 56, 64, 72, 80, 96, 128, 256 |
| **仅 256×256** | 256 |
| **仅 128×128** | 128 |
| **仅 64×64** | 64 |
| **仅 32×32** | 32 |
| **仅 16×16** | 16 |

> 💡 如果图片尺寸小于目标尺寸，自动跳过不可用的尺寸（如 64px 的图不会生成 128/256 档）。

---

## 🔨 编译

```bash
# 需要 .NET 8+ SDK
git clone https://github.com/ctfwubai/img2ico.git
cd img2ico/Img2Ico/Img2Ico

# 标准版 (框架依赖, 176KB)
dotnet publish -c Release -o publish

# 独立版 (自包含, 25MB)
dotnet publish -c Release -r win-x64 -o publish_standalone \
  -p:TargetFramework=net10.0-windows \
  -p:SelfContained=true -p:PublishTrimmed=true \
  -p:TrimMode=partial -p:_SuppressWinFormsTrimError=true \
  -p:EnableCompressionInSingleFile=true
```

## Star History

<a href="https://www.star-history.com/?repos=ctfwubai%2Fimg2ico&type=date&legend=top-left">
 <picture>
   <source media="(prefers-color-scheme: dark)" srcset="https://api.star-history.com/chart?repos=ctfwubai/img2ico&type=date&theme=dark&legend=top-left" />
   <source media="(prefers-color-scheme: light)" srcset="https://api.star-history.com/chart?repos=ctfwubai/img2ico&type=date&legend=top-left" />
   <img alt="Star History Chart" src="https://api.star-history.com/chart?repos=ctfwubai/img2ico&type=date&legend=top-left" />
 </picture>
</a>

