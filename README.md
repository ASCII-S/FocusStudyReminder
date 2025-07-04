# 专注学习提醒器

一个简单而实用的专注学习提醒工具，帮助你在学习和休息之间保持平衡。

## 界面展示

### 主页展示

![主页展示](index.png)

### 设置页面

![设置页面](settings.png)

## 主要功能

- **专注学习和休息计时**：设置学习和休息时间，帮助你保持高效的学习节奏。
- **随机提醒**：在学习过程中随机时间弹出提醒，帮助你调整姿势、休息眼睛。
- **冥想环节**：在切换学习和休息状态时提供短暂的冥想时间，帮助调整状态。
- **系统托盘支持**：最小化到系统托盘后台运行，不打扰你的工作。
- **自定义音效**：可自定义提醒音效。

## 界面改进

最新版本对UI进行了全面改进：

1. **扁平化设计**：采用现代扁平化UI风格，视觉更加清爽。
2. **集成式设置**：将设置功能从独立窗口集成到主窗口，操作更加便捷。
3. **左侧导航栏**：添加了左侧导航栏，包含"主页"和"设置"标签，可以快速切换。
4. **标签页式设置**：设置页面采用标签页设计，分为"常规"、"通知设置"和"窗口行为"三个部分。
5. **自适应布局**：使用FlowLayoutPanel优化了设置面板的自适应布局，窗口大小调整时控件能正确适应。
6. **优化的视觉层次**：统一对齐方式，确保内容从导航栏右侧边缘开始，保持一致的布局。

## 使用方法

1. 启动应用程序后，可以在主页面设置学习时间和休息时间。
2. 点击"开始"按钮开始计时。
3. 程序会显示当前状态（学习中/休息中）和剩余时间。
4. 在学习过程中会随机弹出提醒，提示您休息或调整姿势。
5. 点击左侧导航栏的"设置"可以进行详细设置。

## 系统要求

- Windows 7/8/10/11
- .NET Framework 4.5或更高版本

## 开发信息

- 开发语言：C#
- 框架：.NET Framework Windows Forms

## 工作原理

1. 启动大循环（默认90分钟）专注学习
2. 在学习期间，启动随机时间（默认3-5分钟）的小计时器
3. 小计时器结束时，提示用户闭眼冥想20秒
4. 冥想结束后继续随机计时，直到大循环学习时间结束
5. 大循环结束后提醒休息20分钟，然后自动开始新的学习周期

这种模式基于以下健康理念：
- 避免长时间不间断盯着屏幕
- 定期休息眼睛可以减轻视疲劳
- 90分钟左右为一个专注力周期，之后需要休息恢复

## 安装方法

1. 下载最新版本的发布包
2. 解压到任意目录
3. 运行 FocusStudyReminder.exe

## 使用说明

1. 点击"开始"按钮开始学习循环
2. 学习过程中会随机提示您闭眼休息
3. 点击"停止"按钮可以随时终止计时
4. 点击"设置"按钮可自定义各种参数
5. 关闭窗口时程序会继续在后台运行，双击托盘图标可重新打开界面

## 使用的技术

- C# / .NET Framework
- Windows Forms

## 开发者

如果您想参与开发，欢迎Fork本项目并提交Pull Request。

### 构建项目

1. 使用Visual Studio打开解决方案文件
2. 还原NuGet包
3. 编译解决方案

## 开源协议

[MIT License](LICENSE)

---

希望这个小工具能帮助你更专注、更健康地学习和工作！ 