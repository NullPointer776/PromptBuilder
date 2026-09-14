# PromptBuilder v2.0 - 重构版本

一个专业的 AI 提示词构建和敏感信息清理工具。

> **重构完成！** 该版本经过全面架构重构，现在具有更强的可用性、可维护性和功能性。

## 📚 文档

- **[快速开始指南](QUICK_START.md)** - 新用户必读，包含详细的使用示例
- **[重构总结](REFACTORING_SUMMARY.md)** - 了解架构改进和新增功能
- **[验收清单](VERIFICATION_CHECKLIST.md)** - 测试覆盖和功能清单

## 🎯 主要功能

### 核心功能
✅ **结构化提示词构建** - Task、Context、Output Format、Constraints 四部分
✅ **自动敏感信息检测** - 邮箱、IP、电话、API密钥等 8+ 种类型
✅ **智能替换和清理** - 自动生成合理的替代值
✅ **项目保存/打开** - `.prompt` 格式保存完整项目
✅ **完整撤销/重做** - Ctrl+Z/Y 支持
✅ **导出功能** - 导出为纯文本

### UI/UX 改进
✅ 现代化菜单系统（File、Edit、Help）
✅ 美观的对话框和按钮样式
✅ 响应式界面（Segoe UI 字体、淡蓝色主题）
✅ 快捷键支持
✅ 完善的错误处理

## 🏗️ 架构

该应用采用分层架构设计：

```
UI Layer (MainForm.cs)
    ↓
Service Layer
    ├── PromptService (提示词构建)
    ├── SensitiveDetectionService (敏感信息检测)
    ├── ReplacementService (替换逻辑)
    ├── FileService (文件I/O)
    └── CommandHistory (撤销/重做)
    ↓
Model Layer
    └── PromptData (数据模型)
```

## 🚀 快速开始

### 第一步：构建提示词
```
1. 填写 Task、Context、Output Format、Constraints
2. 点击 "Assemble Draft" 生成格式化提示词
3. 勾选 "Editable" 可在需要时编辑
```

### 第二步：检查敏感信息
```
1. 点击 "Next: Check for Sensitive Info"
2. 点击 "Detect Sensitive Info" 自动扫描
3. 检查列表中的敏感项
```

### 第三步：清理敏感信息
```
方法 A: 点击 "Auto Replace" 自动替换
方法 B: 选择项目并点击 "Manual Review" 逐个审查
方法 C: 在文本中选中敏感信息并点击 "Mark Selection"
```

### 第四步：保存和导出
```
保存项目: File → Save
打开项目: File → Open
导出文本: File → Export Text
复制文本: 点击 "Copy Final"
```

## 🎨 使用示例

```markdown
## Instruction
**Summarize** the following document in no more than 300 words, focusing on key decisions.

## Context
The audience is a non-technical product team. The document is a 2-page meeting notes.

## Output Format
Bullet list with 5 bullets, each 1-2 sentences. Include a 2-sentence summary at the top.

## Constraints
Use plain English, avoid jargon. Do not reference internal project names. Maximum 300 words.
```

## ⌨️ 快捷键

| 快捷键 | 功能 |
|--------|------|
| Ctrl+Z | 撤销 |
| Ctrl+Y | 重做 |
| Ctrl+C | 复制 |
| Ctrl+V | 粘贴 |

## 🔒 敏感信息检测

自动检测以下类型：

| 类型 | 检测示例 |
|------|---------|
| 邮箱 | john@company.com |
| IP | 192.168.1.1 |
| 电话 | +1 (415) 555-1234 |
| API密钥 | sk-abc123xyz... |
| 银行账户 | 1234-5678-9012 |
| 密码 | password=secret123 |
| 人名 | John, Alice, Bob |
| 地址 | 123 Main Street |

## 💾 项目格式

`.prompt` 文件采用 JSON 格式，包含：
- 原始输入（Task、Context 等）
- 生成的提示词（Draft、Sanitized）
- 检测到的敏感项和替换规则
- 时间戳信息

## 🔍 开发者信息

### 文件结构
```
PromptBuilder/
├── Models/
│   └── PromptData.cs           # 数据模型
├── Services/
│   ├── PromptService.cs        # 提示词构建
│   ├── SensitiveDetectionService.cs  # 敏感检测
│   ├── ReplacementService.cs   # 替换逻辑
│   └── FileService.cs          # 文件操作
├── Utils/
│   └── CommandHistory.cs       # 撤销/重做
├── UI/
│   └── Dialogs/                # 对话框
├── MainForm.cs                 # 主窗口
└── Program.cs                  # 入口点
```

### 技术栈
- **语言**: C# 10+
- **框架**: .NET 8 Windows Forms
- **序列化**: System.Text.Json
- **模式**: MVC分层、Command模式、Service定位器

## 📝 许可证

此项目仅供学习使用。

---

**版本**: v2.0 | **最后更新**: 2024-09-15

详见 [QUICK_START.md](QUICK_START.md) 了解更多使用方法。