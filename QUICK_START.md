# PromptBuilder 快速开始指南

## 💡 主要改进

这个版本的 PromptBuilder 经过全面重构，现在具有：

### ✨ 核心功能
1. **结构化提示词构建** - 通过 Task、Context、Output、Constraints 四个部分构建专业的提示词
2. **自动敏感信息检测** - 检测邮箱、IP、电话、API密钥等
3. **智能替换和清理** - 自动生成合理的替代值
4. **项目保存/加载** - 保存你的工作为 `.prompt` 文件
5. **完整撤销/重做** - Ctrl+Z 和 Ctrl+Y
6. **导出功能** - 将清理后的提示词导出为文本

---

## 🚀 快速上手

### 第一步：创建新提示词

1. 打开应用后，在 **Prompt Structuring** 标签页填写：
   - **Task**: 描述你要 AI 做什么（例："总结以下文档")
   - **Context**: 背景信息（例："受众是产品团队")
   - **Output Format**: 输出格式（例："5条要点列表")
   - **Constraints**: 约束条件（例："不超过300词")

2. 点击 **"Assemble Draft"** 按钮生成格式化的提示词

3. 你可以在文本框中**编辑**生成的提示词（勾选"Editable"）

---

### 第二步：检查敏感信息

1. 点击 **"Next: Check for Sensitive Info"** 移至下一标签页

2. 点击 **"Detect Sensitive Info"** 按钮自动扫描敏感信息

3. 检测到的项目会显示在右侧列表，并在文本中高亮显示

---

### 第三步：清理敏感信息

#### 方法 A：自动替换（推荐）
```
点击 "Auto Replace" → 所有敏感信息被自动替换
```

替换规则：
- 名字 → Alice, Bob, Charlie, Dana, ...
- 邮箱 → test1@example.com, test2@example.com, ...
- IP 地址 → 192.168.1.1, 192.168.1.2, ...
- 电话 → 021 123 4567
- 密码 → ••••••••
- API 密钥 → sk-test-XXXXXXXXXXXXXXXXXXXX

#### 方法 B：手动审查
```
1. 在右侧列表选择一个敏感项
2. 点击 "Manual Review"
3. 修改建议的替换值
4. 点击 "Accept & Replace"
```

#### 方法 C：手动标记
```
1. 在左侧文本框选中你认为敏感的文字
2. 点击 "Mark Selection"
3. 选择类别
4. 该项会被添加到检测列表
```

---

### 第四步：保存和导出

#### 保存项目
```
菜单 File → Save... → 选择位置 → 输入文件名 → 保存为 .prompt 文件
```

这样你可以**稍后继续编辑**这个项目：
```
菜单 File → Open... → 选择 .prompt 文件 → 继续工作
```

#### 导出最终文本
```
菜单 File → Export Text... → 选择位置 → 输入文件名 → 保存为 .txt 文件
```

或者直接：
```
点击 "Copy Final" → 复制到剪贴板 → 粘贴到任何应用
```

---

## 🎯 常用快捷键

| 快捷键 | 功能 |
|--------|------|
| Ctrl+Z | 撤销上一步 |
| Ctrl+Y | 重做下一步 |
| Ctrl+C | 复制选中文本 |
| Ctrl+V | 粘贴 |

---

## 💡 使用技巧

### 1. 使用示例
每个输入字段旁都有 **"Insert Example"** 按钮，快速加载提示词示例：

```
任务示例: "**Summarize** the following {{document_type}} in {{max_words}} words, focusing on {{key_topic}}."
```

### 2. 编辑技巧
- 生成后勾选 **"Editable"** 可以直接编辑提示词
- 使用 **Find/Replace** 按钮快速查找和替换文本

### 3. 查看写作建议
点击 **"Writing tip (toggle)"** 查看关于如何写好提示词的建议

### 4. 查看敏感项详情
在敏感项列表中**双击**某个项目，文本框会跳转到该项目的位置

---

## 🔒 敏感信息清单

应用会自动检测以下类型的敏感信息：

| 类型 | 示例 | 替换为 |
|------|------|--------|
| 邮箱 | john@company.com | test1@example.com |
| IP地址 | 192.168.1.1 | 192.168.1.1 |
| 电话 | +1 (415) 555-1234 | 021 123 4567 |
| API密钥 | sk-abc123xyz... | sk-test-XXX... |
| 银行账户 | 1234-5678-9012 | 12-3456-7890123-00 |
| 密码 | password=secret123 | •••••••• |
| 人名 | John, Alice, Bob | 随机选择 |
| 地址 | 123 Main St, ... | 123 Queen Street, ... |

---

## 📝 项目文件格式

`.prompt` 文件是 JSON 格式，包含：
- Task, Context, OutputFormat, Constraints（原始输入）
- DraftPrompt, SanitizedPrompt（生成的提示词）
- DetectedItems, ReplacementMap（敏感项和替换规则）
- CreatedAt, ModifiedAt（时间戳）

```json
{
  "id": "...",
  "createdAt": "2024-09-15T...",
  "task": "...",
  "context": "...",
  "outputFormat": "...",
  "constraints": "...",
  "draftPrompt": "...",
  "sanitizedPrompt": "...",
  "detectedItems": [...],
  "replacementMap": {...}
}
```

---

## ⚠️ 注意事项

1. **检测不是100%准确** - 敏感信息检测使用正则表达式，可能有误报或漏报，请始终手动审查

2. **备份重要文件** - 操作前备份你的原始文本

3. **自定义检查** - 如果检测器遗漏了某些敏感信息，使用"Mark Selection"手动标记

4. **替换值定制** - 在"Manual Review"中可以自定义任何替换值

---

## 🐛 故障排除

### 问题：检测不到某些敏感信息
**解决**：
- 使用"Mark Selection"手动标记
- 检查敏感信息的格式是否与检测规则匹配

### 问题：替换不够准确
**解决**：
- 使用"Manual Review"逐个检查
- 修改建议的替换值
- 提供反馈以改进应用

### 问题：保存文件失败
**解决**：
- 确保文件路径有效
- 检查磁盘空间
- 确保你有写权限

---

## 📧 反馈和改进建议

如果你有建议或发现问题，欢迎反馈！

**改进方向**：
- 更准确的敏感信息检测
- 支持更多敏感项类型
- 自定义检测规则
- 黑暗主题
- 国际化支持

---

**祝你使用愉快！** 🎉

版本: v2.0 | 最后更新: 2024-09-15
