# HANDOFF｜交接（暂停/恢复用，先读我）

> 旧版字段（governance-state / Evidence / Human Gate / Promotion / Dispatch ID）已废弃，不填。

- Captured at（YYYY-MM-DD HH:MM）：2026-10-07 10:30
- PROJECT_PHASE：DEVELOP（小活/缺陷修复批次，无独立 Plan 文档）
- PLAN_VERSION：（空）
- PLAN_READINESS_SCORE：（空）
- PLAN_GATE：（空）
- DEV_BASELINE：（空）
- CHANGE_REQUEST：NONE
- Stage ID：STAGE-大交接2（0.7.0 功能已开发完、未发版）
- 剩 P0（没完的才列，多一条都不行）：无
- 当前 Task（正干到哪）：大交接 2 五步（neat-freak 收尾 → 本 HANDOFF → skill 审查清单 → commit+push → 恢复提示词），已完成
- 执行链/Session：（空）
- 未闭环评审意见（code-reviewer/qa 留的还没改的）：无
- 产品验收（追踪矩阵落点 / 未测 AC / 是否待用户签收）：`docs/qa/`；本批三项功能真机证据全绿（probe12 全绿、通知屏内实拍、菜单栏含 Builder 实拍、148 tests 0 错、build 0 错），AC 覆盖见 CHANGELOG 未发布 4 条与 README；发布类型＝迭代更新，不强制用户签收
- 用户签收（触发依据 / 证据包 / 签收人 / 结论）：不需要（迭代更新，非首次发布）
- docs 落盘清单（本轮新增/改了哪几个 docs 文件）：`docs/handoff/HANDOFF.md`（本次重写，旧版备份为 `HANDOFF.md.旧版-2026-08-30`）、`docs/skill-reveiw/SKILL-REVIEW.md`（新建）、`AGENTS.md`（下一步待办更新）
- 下一步（Next Single Action）：见下方「下一步任务」第 1 条（0.7.0 发版，待用户拍板）
- 人要拍什么板（列出来问，不问不许开工）：① 是否发 0.7.0；② `docs/skill-reveiw/SKILL-REVIEW.md` 候选 skill 逐条审查；③ 大残留是否大清理（tests/deprecated、release/ 旧包、各 *.旧版-* 备份——本批只列未删）
- permission_request：2026-10-07 大交接 2 用户明确授权 commit + push（分支以远端实际为准＝master）✓
- 收尾记一笔（neat-freak）：文档已对齐（AGENTS 未发布/待办、CHANGELOG 4 条与代码一致，build 复验 0 错）；temp 仅剩运行中 app 持有 fd 的 `app-stderr.log`；会话残留已清（.DS_Store、系统临时区、截图探针）；未决＝上面「人要拍什么板」

## 当前工作进展（本阶段实绩）

**功能（全部未发布，见 CHANGELOG「未发布」4 条）**

1. 启动不弹卡片：初始化完成后立即隐藏主窗；启动 60 秒内「窗口未显示 + 用户从未打开过」才抑制 Reopen 激活（覆盖 LaunchServices Reopen 晚到 30s+ 实测）；用户打开过一次后照常还原窗口
2. 通知弹窗屏外与透明修复：`RepositionAll` 统一按逻辑点定位（不再二次乘 `RenderScaling`）；淡入淡出改直接属性插值、结束时显式落地目标透明度 + 1.5s 兜底
3. 菜单栏 Builder 推荐状态项：独立 `NSStatusItem`（余额项左侧）、固定 UTC+8 工作日/周末两张表输出 `trae/open/work·ds` 等短文本、Tooltip 原因与下次切换、变化时复用 `MacToastService` 弹 8s 轻通知；规则 SSOT＝共享 `BuilderRecommendationCalculator` + 全量边界测试
4. 菜单栏 OC 月额度恢复天数后缀（如 `OC1 99/89/45% 20D`，随刷新按 `ResetsAt` 重算）

**验证**：148 tests 0 错；probe12 全绿（定位 1644,46 / 134 屏内、全屏帧右上两张通知实拍、fade 日志正常）；`dotnet build` Mac csproj 0 错（4 个既有 warning 不阻塞）；测试后已还原 config `[20,10]`、alert-events 104 行

**对外同步（均已上线复验）**

- README 中英：hero 换 2026-10-07 实拍（邮箱已像素化）、菜单栏实拍条、Builder 标「开发中，未发布」、仓名统一 `p010-deepseek-balance-mac`、标签表补齐 11＝About 实测；`validate_readme.py` → DOCUMENTATION_READY(0)；commit `6404a65` 已推 master
- 站点封面：GitHub hero 同源合成 1600×900@16:9 → 站点仓 `0189147` → Pages run `37559720663` success → 线上 sha256 与本地一致
- 中央 skill `github-homepage-showcase-local-zh` 升 2.1.0（新增「封面与 README hero 同源一致」条款），`validate_skill.py` VALID，`SKILL-REGISTRY.md` 已登记（含 2.0.0 补记）
- 反向对账（v1.2.0 新规首跑）：`audit-remotes` PASS——远端 56 仓、`unregistered=0`、9 条 ghost 全有 renamed/deleted note、NO-HOME 空

**已发布**：0.6.0（tag 与 CI 双架构产物在）

## 下一步任务

1. 【待用户拍板】0.7.0 发版：CHANGELOG 未发布 4 条转版号 → 三处版本一致性（csproj `<Version>` + `Info.plist` CFBundleShortVersionString/CFBundleVersion + README 下载表）→ tag `v0.7.0` push 触发 CI → 真机装包验证
2. WorkBuddy 实际额度接入（AGENTS 待办①，当前仍是占位）
3. 界面截图持续更新：2026-10-07 hero/菜单栏实拍已上 README，胶囊细节图可在 0.7.0 发版时顺带重截
4. 视 Windows 新仓库落地情况，决定是否把 `src/DeepSeekBalanceWidget/`（WPF）从本仓库移除
5. `docs/skill-reveiw/SKILL-REVIEW.md` 候选 skill 逐条审查拍板

## 注意事项及相关规矩（下一 agent 必读）

**仓库边界**

- 只维护 macOS 端（Avalonia）；WPF 专属文件（窗口、DPAPI、`publish.ps1`）只读不维护；共享 `Models/`、`Services/` 物理在 `src/DeepSeekBalanceWidget/` 下、由 Mac csproj `<Compile Include>` 链接，改动只保证本仓编译通过，不跨仓 cherry-pick

**构建与测试**

- 只跑 `dotnet build src/DeepSeekBalanceWidget.Mac/DeepSeekBalanceWidget.Mac.csproj`；**禁** `dotnet build DeepSeekBalanceWidget.sln`（macOS 缺 WindowsDesktop SDK 必失败）
- 测试工程在 macOS 不可运行（引用 WPF csproj）；共享代码验证以 Mac csproj build 为准
- 4 个既有 warning（AVLN3001 等）不阻塞、默认不修

**发版（易漏）**

- 三处版本必须一致：csproj `<Version>` / `Info.plist` 两键 / README 下载表；打 tag 前核对 CHANGELOG 顶部
- push `v*` tag 自动触发 `.github/workflows/release-macos.yml` 构建 arm64+x64 并 `--clobber` 覆盖资产；本地 `release/*.zip` 不是发布物
- 排查闪退先读崩溃报告 `app_version`/`procPath`（Launchpad 与 `~/Applications/DeepSeekBalanceWidget.backup-*` 可能指旧包，误判当前代码）

**红线与规矩**

- commit/push 无用户明确授权一律不做（本次大交接 2 已授权，仅限本批交接文件与阶段成果）；**本仓分支是 master，不是 main**
- 汇报 ≤10 行＝三行心跳＋≤3 要点；只报目标/用户安排的工作/大影响三类，小事自决不报
- 临时文件一律进仓内 `temp/`（已 gitignore、不入库、push 前清空）；派 opencode 通道角色写本仓外目录会被 `external_directory` 拒
- 旧版备份命名 `*.旧版-*`（gitignore 命中、备份不覆盖）；大残留删除等用户明确说大阶段结束再详细清
- 治理规则改动后：`node scripts/model/check-ledger.mjs docs/model` 须 `LEDGER-OK` + `bash scripts/sync-old-projects.sh` 对齐 + HANDOFF 记一行
- Read 工具读图可能串图/旧缓存：用全新唯一文件名＋图片尺寸吻合＋像素统计交叉验证，不裸信目检
- macOS 自带 grep 不支持 `-P`，用 `awk`/`grep -E`

## 恢复读盘（全体系唯一顺序，别乱）

1. AGENTS；2. 角色卡；3. 根 `USER_MODEL_OVERRIDE.md`；4. 本 HANDOFF；5. 根 `经验一句话.md`；6. 任务目标放最后。
只有出现冲突才扩大读取范围。

## 治理规则更新通知 · 2026-10-17
- **发生了什么**：本项目的治理文件（`AGENTS.md`／角色卡／README／编排者提示词／`scripts/*`）于 2026-10-17 被母版编排者批量铺包更新到最新版。
- **为什么**：母版当天做了一轮体系修订（触发口令改名为「你是主开发者」、复核粒度改为「交付单元」、子代理模型与执行层/判定层分界、
  账本新增若干机器门等）。铺包时本项目的旧文件已按惯例备份为同目录的 `*.旧版-2026-10-17`，**未删除任何文件、账本内容零改动**。
- **对你意味着什么**：不需要你做任何操作；下次开工时你读到的 `AGENTS.md` 即为新规则。
  其中影响执行方式的三条：①口令是「你是主开发者」（旧口令「你是唯一开发者」仍可用）；
  ②复核链**不再对每个小任务跑**，改为对「交付单元」（阶段收口）跑一次；
  ③若你的客户端支持派子代理时传 model/强度，执行层子代理**一律用 `opencode-go/deepseek-v4.1-flash`**（真机取证走 `codex/gpt-6-luna`）。
- **需要你确认的**：若本次铺包覆盖了你项目里**自己改过的同名治理文件**，请对照同目录的 `*.旧版-2026-10-17` 备份，把你的专属条款合并回去。
- **回退方式**（如需）：把对应的 `*.旧版-2026-10-17` 改回原名即可。

## 本批实绩 · 2026-10-20｜GPT 额度恢复提醒开关（未发布，已装本机）

- **用户诉求**：关掉声音提醒后仍觉得 GPT「5 小时额度恢复」会吵；要求先诊断是否真响，再给一个能单独关掉这路提醒的开关并测试。
- **诊断结论（复核一致）**：恢复提醒在 v0.5.0 / v0.6.0 / HEAD 三版都走 `ToastAlertStyle.Notice`（默认参数）→ `ToastWindow._sound = soundEnabled && style == Alarm` → **恒为 false**，`AlertSoundEnabled=false` 时不可能发声；过去 24h `afplay` 进程日志零记录、无残留 wav / afplay。夜里被吵到的真实来源＝**恢复通知弹窗本身**（绿色「已恢复」，事件日志 `alert-events.log` 夜间多次 `RECOVERY | GPT | 5 小时额度已恢复 | 剩余 100%`）＋低量 `Alarm` 弹窗（橙色常驻、`alertMode=Limited` ≥10 秒）。
- **改动（4 个源码文件 + CHANGELOG）**：
  - `Models/AppConfig.cs`：新增 `GptQuotaRecoveryNoticeEnabled`（默认 true，与既存 `GptQuotaRecoveredPercent` 阈值配套）
  - `MainWindow.axaml.cs` `RaiseCodexQuotaAlerts`：`AlertEventLogger.Write` 仍在开关判断之前（事件日志不受影响），之后 `if (!toastsEnabled || !recoveryNoticeEnabled) continue;` 只拦「已恢复」弹窗；低量 Alarm 分支未改
  - `SettingsWindow.axaml` / `.axaml.cs`：设置 → 预警 → ChatGPT 额度预警新增「额度恢复时弹通知」勾选框，载入 / 快照 / 保存三处绑定齐全
  - `CHANGELOG.md`：「未发布」首条
- **验证证据（真机，本机）**：
  - `bash scripts/publish-macos.sh arm64` → exit 0
  - 直接跑新包（先 quit 已安装实例）→ stderr `[Alert] evaluate accounts=1 alerts=0 toastsEnabled=True recoveryNotice=True`
  - 注入 `"gptQuotaRecoveryNoticeEnabled": false` 后重跑 → `recoveryNotice=False` ⇒ 开关从 config 到评估/弹窗分支全链路生效
  - `bash scripts/install-macos.sh arm64` → 旧包备份 `~/Applications/DeepSeekBalanceWidget.backup-20261020074732.app`，新包已装并启动，PID 70478 稳定、无新增 `.ips`
  - 探测前已备份配置（`temp/config.before-probe.json`，当时无该键）。**收工时实测本机 `config.json` 仍为 `"gptQuotaRecoveryNoticeEnabled": false`**（2026-10-20 07:57 由运行中的新包落盘）——即用户机器当前处于「恢复弹窗静音」状态；要恢复默认需在设置页勾回。
- **未完成 / 环境限制**：设置页勾选框的**可视与手动保存**未验（本机无辅助访问 -1728、无录屏权限、无 cliclick）；替代证据＝XAML 控件 + 三处绑定 + config 注入对照 + 装好的 `DeepSeekBalanceWidget.dll` 内可查到 `GptQuotaRecoveryNoticeEnabled`（3 处）与 `GptRecoveryNoticeCheck`（1 处），确认新构建确已装机。**请用户点一次：设置 → 预警 →（当前应为未勾选）勾上「额度恢复时弹通知」→ 保存 → 确认 `config.json` 变 `true`，再按需取消。**
- **自审口径**：主开发者模式，自审 + 真机自测，无独立 reviewer/QA 角色；账本记一行（`chain_status=OPEN`，待用户界面确认）；QA 留痕见 `docs/qa/2026-10-20-GPT额度恢复提醒开关.md`；经验一句已追加（「会响」先查 `ToastAlertStyle`，Notice 恒无声）。
- **提交边界**：本任务涉及 7 个文件（4 源码 + `CHANGELOG.md` + `docs/qa/2026-10-20-…md` + `经验一句话.md` 追加一行）；工作区另有既存未提交改动（`README.md` 被 2026-10-17 铺包覆盖、`.gitignore`、`GOVERNANCE-STATE.json`、HANDOFF 铺包通知段）与大量 `._*` / ORCA 脚本未跟踪文件，**均不属本任务**，未提交、未清理。

## 本批实绩 · 2026-10-20｜峰谷口径校正与可视化（未发布，已装本机）

- **用户诉求**：不清楚软件怎么判 DeepSeek 高峰/非高峰（「什么时间段是高峰，周末算不算非高峰？」），要求 ① 按官方最新口径核对设置是否正确；② 在软件里把该信息显示出来供自查。
- **官方口径（2026-10-20 实查 https://api-docs.deepseek.com/quick_start/pricing）**：「Peak hours are 01:00 - 04:00 and 06:00 - 10:00 UTC, Monday through Friday, excluding Chinese public holidays. All other hours are off-peak, including weekends and Chinese public holidays in full.」折算北京时间（UTC+8）＝ **09:00–12:00、14:00–18:00，周一至周五；周末与中国法定节假日全天谷时**；谷时按半价计费。
- **核对结论**：`AppConfig` 默认区间的小时数与 `WeekdaysOnly:true` **本来就与官方一致**；两处缺口：① 中国法定节假日日历未内建（界面已明示「暂未内建」，节假日期间仍按工作日判定）；② **真实 bug**——旧版本把默认两段落盘成 `weekdaysOnly:false`，配置值会压过代码默认值，升级用户周末被算成高峰（本机 `config.json` 实测正是如此）。
- **改动**：
  - `Models/AppConfig.cs` `Normalize()`：小时数恰为 9–12 / 14–18 且 `weekdaysOnly=false` 的旧配置自动迁移为 `true`；空区间回退官方默认；其余自定义小时数原样保留（不擅自改用户的非标准配置）
  - `Services/PeakHourCalculator.cs`：新增 `NextBoundary`（最长找 8 天，供跨周末展示）、`DescribeNextBoundary`（「09:00」/「明天 09:00」/「10月26日 09:00」）、`Describe`（「周一至周五 09:00–12:00、14:00–18:00」）；保留 `MinutesUntilNextBoundary` 的 1–1440 语义不动（`MainWindow` 定时器与测试在用）
  - `MainWindow.axaml.cs` `RefreshPeakStatus`：弹窗标签与迷你胶囊 ToolTip 补区间/下次切换/周末与节假日说明；新增 `_menuBarPeakTooltip` 并入菜单栏 DeepSeek ToolTip；状态变化时 stderr 打一行 `[Peak] peak|offpeak at … bj=… schedule=… next=…` 供取证
  - `SettingsWindow.axaml` / `.axaml.cs`：勾选框下方新增 `PeakScheduleHint` 三行动态说明
  - `CHANGELOG.md`：「未发布」补两条（修复 + 新增）
- **验证证据**：
  - 独立校验程序 `temp/peak-check`（链接 `AppConfig.cs` + `PeakHourCalculator.cs` + `PeakRange`，net8.0 Exe）**16 项 ALL PASS**：默认区间、`Describe` 文案、周一是/周六周日否/12:00 边界、旧配置迁移、自定义小时数不被改写、空区间回退、下次切换跨周末（周五 19:00 → `10月26日 09:00`）。
  - 首次跑曾 FAIL 一条：`DescribeNextBoundary` 复用 24h 上限的 `MinutesUntilNextBoundary`，跨周末找不到边界而错报「明天 19:00」；改为独立 `NextBoundary` 后复跑全过。
  - `dotnet build src/DeepSeekBalanceWidget.Mac/DeepSeekBalanceWidget.Mac.csproj` → 0 error（4 个既存 warning）；`bash scripts/publish-macos.sh arm64` → exit 0；`bash scripts/install-macos.sh arm64` → 旧包备份 `~/Applications/DeepSeekBalanceWidget.backup-20261020080333.app`，新包装机并启动。
  - 真机 stderr：`[Peak] offpeak at 2026-10-20 08:03 bj=2026-10-20 08:03 schedule=周一至周五 09:00–12:00、14:00–18:00 next=09:00`。
  - 迁移生效实测：跑过一次后 `config.json` 的 `peakHourRanges` 两条 `weekdaysOnly` 由 `false` 变为 `true`（此前周六会被判高峰）。
- **未完成 / 环境限制**：设置页三行说明、弹窗 ToolTip、菜单栏 ToolTip 的**可视效果未截屏取证**——本机 `screencapture -x` 报 `could not create image from display`（无录屏权限）。**请用户肉眼确认三处**：① 设置 → 界面 → 行为 勾选框下方三行；② 悬停余额右侧「● 预计非高峰时段」或迷你胶囊「非高峰」标签看 ToolTip；③ 悬停菜单栏余额项看「DeepSeek 计费：谷时价｜…｜下次切换 …」。
- **自审口径**：主开发者模式，自审 + 真机自测，无独立 reviewer/QA 角色；账本记一行（`chain_status=OPEN`，待用户界面确认）；QA 留痕见 `docs/qa/2026-10-20-峰谷时段校正.md`。
- **提交边界**：本批 8 个文件（5 源码 + `CHANGELOG.md` + `docs/qa/…` + `经验一句话.md` 追加）。

## 本批实绩 · 2026-10-20｜节假日日历内建（A1，接上一段「峰谷口径校正」）

- **用户裁决**：`a1` —— 内建 2025–2027 节假日日期表（纯静态数组），判定变成「工作日 且 不在节假日表」；表外年份自动回退「只按周一至周五」并在设置页提示需更新；假日场景补进 `temp/peak-check` 再装机复验。
- **数据来源与口径**：2025 年按国办发明电〔2024〕7 号、2026 年按国办发明电〔2025〕7 号录入**全部放假日期**（2025：元旦 1/1、春节 1/28–2/4、清明 4/4–4/6、劳动 5/1–5/5、端午 5/31–6/2、国庆中秋 10/1–10/8；2026：元旦 1/1–1/3、春节 2/15–2/23、清明 4/4–4/6、劳动 5/1–5/5、端午 6/19–6/21、中秋 9/25–9/27、国庆 10/1–10/7）。**2027 年安排国务院尚未公布（惯例每年 11 月发布）**，故 `LastCoveredYear=2026`，2027 回退工作日判定并在设置页提示。
- **只做放假表、不做补班表**：DeepSeek 原文是「Monday through Friday, excluding Chinese public holidays」，调休上班的周六周日仍属周末 → 仍是谷时，所以补班表对本判定无影响，不必录入。
- **改动**：
  - 新建 `Services/ChinaHolidayCalendar.cs`：`FirstCoveredYear=2025` / `LastCoveredYear=2026`、`IsCovered(year)` / `IsHoliday(beijingTime)` / `DescribeCoverage()` / `DescribePending(year)`；静态 `HashSet<DateTime>` 由 `Range()` 逐日铺开
  - `Services/PeakHourCalculator.cs`：`IsPeakAt` 改为 `r.Contains(...) && !(r.WeekdaysOnly && ChinaHolidayCalendar.IsHoliday(bj))`——**节假日只压制 `WeekdaysOnly` 区间**，用户自定义的「每天」区间不被日历改写；新增 `IsHoliday(localNow)`；`NextBoundary` 搜索窗口 8 天 → **15 天**（春节/国庆长假跨度更长）
  - `Mac.csproj`：手工补 `<Compile Include=".../ChinaHolidayCalendar.cs" />`（该工程 `Services/*.cs` 逐文件列出，不自动收新文件）
  - `MainWindow.axaml.cs` `RefreshPeakStatus`：ToolTip 第 4 行由「暂未内建」改为覆盖文案 / 待更新提示二态；`[Peak]` 日志补 `holiday=… calendar=…`
  - `SettingsWindow.axaml(.cs)`：`PeakScheduleHint` 第三行改为「已内建 2025–2026（国务院公布版）」或「注意：…请更新节假日日历」；静态默认文案补「法定节假日」
  - `CHANGELOG.md`：「未发布」新增「内建中国法定节假日日历」条
- **验证证据（本机实测）**：
  - `temp/peak-check` 扩到 **33 项 ALL PASS**（复跑确认，exit 0），新增：国庆 10-01/10-07 判谷、节后 10-08 判峰、2025 与 2026 春节收发假边界、调休补班周六 2026-02-14 10:00 判谷、自定义「每天」区间不被改写、国庆前 9/30 收市 → **10月8日 09:00**、春节前 → **2月24日 09:00**、2027 未覆盖回退工作日（10-01 10:00 判峰）、两种提示文案
  - `dotnet build src/DeepSeekBalanceWidget.Mac/DeepSeekBalanceWidget.Mac.csproj` → **0 error**（4 个既存 warning）
  - `bash scripts/publish-macos.sh arm64` → exit 0；`bash scripts/install-macos.sh arm64` → 旧包备份 `~/Applications/DeepSeekBalanceWidget.backup-20261020080710.app`，新包装机
  - 真机 stderr（直接跑二进制采 log，采完已 kill 调试实例并用 `open` 正常重启）：`[Peak] offpeak at 2026-10-20 08:07 bj=2026-10-20 08:07 holiday=False calendar=2025–2026 年法定节假日 schedule=周一至周五 09:00–12:00、14:00–18:00 next=09:00`
  - `config.json` 两条 `weekdaysOnly` 均为 `true`（上一批迁移保持生效）
- **未完成 / 环境限制**：与上一段相同——界面文字可视效果仍无截屏取证（`screencapture` 报 `could not create image from display`），**请用户肉眼确认设置页第三行是否已从「暂未内建」变成「已内建 2025–2026 年法定节假日」**。
- **维护口（下次必看）**：2027 年放假安排公布后，① 在 `ChinaHolidayCalendar.BuildHolidays()` 补 `Range(2027, …)`；② 把 `LastCoveredYear` 抬到 2027；③ 在 `temp/peak-check` 补两条 2027 断言复跑。**不抬高 `LastCoveredYear` 会继续回退工作日判定**（安全但会在节假日误判高峰）。
- **自审口径**：主开发者模式，自审 + 真机自测，无独立 reviewer/QA 角色；账本记一行（`chain_status=OPEN`，待用户界面确认）；QA 留痕见 `docs/qa/2026-10-20-峰谷时段校正.md`（含本段 A1 追加）。
