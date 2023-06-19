# Unity组件-AudioSource

+ [Unity 用户手册 2020.3 (LTS)](https://docs.unity.cn/cn/2020.3/Manual/UnityManual.html)
+ [音频](https://docs.unity.cn/cn/2020.3/Manual/Audio.html)
+ 音频参考
+ 音频源

## 音频源

**音频源 (Audio Source)** 在场景中播放[音频剪辑](https://docs.unity.cn/cn/2020.3/Manual/class-AudioClip.html)。剪辑的音源可通过[音频监听器](https://docs.unity.cn/cn/2020.3/Manual/class-AudioListener.html)或者[混音器](https://docs.unity.cn/cn/2020.3/Manual/class-AudioMixer.html)播放。音频源可播放任何类型的[音频剪辑](https://docs.unity.cn/cn/2020.3/Manual/class-AudioClip.html)，可设置以 2D、3D 或混合 (*SpatialBlend*) 模式播放。音频可在扬声器（立体声到 7.1）之间扩散 (*Spread*)，并在 3D 和 2D 之间变换 (*SpatialBlend*)。可通衰减曲线控制传播距离。此外，如果[监听器](https://docs.unity.cn/cn/2020.3/Manual/class-AudioListener.html)位于一个或多个[混响区](https://docs.unity.cn/cn/2020.3/Manual/class-AudioReverbZone.html)内，则会将混响应用于音频源。可对每个音频源应用单独的滤波器，以获得更丰富的音频体验。有关更多详细信息，请参阅[音频效果](https://docs.unity.cn/cn/2020.3/Manual/class-AudioEffect.html)。

![1](https://docs.unity.cn/cn/2020.3/uploads/Main/AudioSourceInspector.png)

## 属性

| ***属性：*** | ***功能：*** |
| --- | --- |
| **Audio Clip** | 参考将要播放的声音剪辑文件。 |
| **Output** | 默认情况下，剪辑将直接输出到场景中的[音频监听器 (Audio Listener)](https://docs.unity.cn/cn/2020.3/Manual/class-AudioListener.html)。使用此属性可以更改为将剪辑输出到[混音器 (Audio Mixer)](https://docs.unity.cn/cn/2020.3/Manual/class-AudioMixer.html)。 |
| **Mute** | 如果启用此选项，则为静音。 |
| **Bypass Effects** | 可快速“绕过”应用于音频源的滤波器效果。启用/停用效果的快捷方式。 |
| **Bypass Listener Effects** | 这是快速启用/停用所有监听器的快捷方式。 |
| **Bypass Reverb Zones** | 这是快速打开/关闭所有混响区的快捷方式。 |
| **Play On Awake** | 如果启用此选项，声音将在场景启动时开始播放。如果禁用此选项，需要通过脚本使用 **Play()** 命令启用播放。 |
| **Loop** | 启用此选项可在\_\_音频剪辑\_\_结束后循环播放。 |
| **Priority** | 从场景中存在的所有音频源中确定此音频源的优先级。（Priority值为0 表示优先级最高。值为256， 表示优先级最低。默认值为 128）。对于音轨值应为 0，避免被意外擦除。 |
| **Volume** | 声音的大小与离\_\_音频监听器\_\_的距离成正比，以米为世界单位。 |
| **Pitch** | 由于\_\_音频剪辑\_\_的减速/加速导致的音高变化量。值 1 表示正常播放速度。 |
| **Stereo Pan** | 设置 2D 声音的立体声位置。 |
| **Spatial Blend** | 设置 3D 引擎对音频源的影响程度。 |
| **Reverb Zone Mix** | 设置路由到混响区的输出信号量。该量是线性的，范围在 0 到 1 之间，但允许在 1到1.1 范围内进行 10dB 放大，这对于实现近场和远距离声音的效果很有用。 |
| ****3D Sound Settings**** | 与 Spatial Blend 参数成正比应用的设置。 |
| **Doppler Level** | 确定将对此音频源应用多普勒效果的程度（如果设置为 0，则不应用任何效果）。 |
| **Spread** | 在发声空间中将扩散角度设置为 3D 立体声或多声道。 |
| **Min Distance** | 在 MinDistance 内，声音将保持可能的最大响度。在 MinDistance 之外，声音将开始减弱。增加声音的 MinDistance 属性可使声音在 3D 世界中“更响亮”，而降低此属性则可以让声音在 3D 世界中“更安静”。 |
| **Max Distance** | 声音停止衰减的距离。超过此距离之后，声音将保持与监听器之间距离 MaxDistance 单位时的音量，不再衰减。 |
| **Rolloff Mode** | 声音衰减的速度。此值越高，监听器必须越接近才能听到声音。（这取决于图）。 |
| **\- Logarithmic Rolloff** | 靠近音频源时，声音很大，但离开对象时，声音降低得非常快。 |
| **\- Linear Rolloff** | 与音频源的距离越远，听到的声音越小。 |
| **\- Custom Rolloff** | 音频源的音频效果是根据曲线图的设置变化的。 |

## 滚降类型

有三种曲线模式：对数 (Logarithmic Rolloff)、线性 (Linear Rolloff) 和自定义 (Custom Rolloff)。可能通过编辑音量距离曲线来自定义音效曲线，如下所述。如果音量距离设置为对数或者线性情况下，尝试去编辑，类型自动变为自定义曲线。

![音频源支持的曲线模式](https://docs.unity.cn/cn/2020.3/uploads/Main/TypesOfRollOff.png)

音频源支持的曲线模式

## 距离函数

可按音频源与音频监听器之间距离的函数形式修改音频的若干属性。

**Volume**：随着距离变化的幅度（0.0 到 1.0）。

**Spatial Blend**：2D（原始声道映射）到 3D（所有声道下混为单声道并根据距离和方向衰减）。

**Spread**：随着距离变化的角度（0.0 到 360.0 度）。

**Low-Pass**（仅当低通滤波器 (LowPassFilter) 附加到音频源时）：随着距离变化的截止频率（22000.0 到 10.0）。

**Reverb Zone**：路由到混响区的信号量。请注意，音量属性以及距离和方向衰减首先应用于信号，因此会同时影响直接信号和回响信号。

![音量 (Volume)、空间混合 (Spatial Blend)、扩散 (Spread)、低通 (Low-Pass) 音频滤波器和混响区混合 (Reverb Zone Mix) 的距离函数。当前与音频监听器的距离在图中用红色竖线标记。](https://docs.unity.cn/cn/2020.3/uploads/Main/AudioDistanceFunctions.png)

音量 (Volume)、空间混合 (Spatial Blend)、扩散 (Spread)、低通 (Low-Pass) 音频滤波器和混响区混合 (Reverb Zone Mix) 的距离函数。当前与音频监听器的距离在图中用红色竖线标记。

要修改距离函数，可直接编辑曲线。有关更多信息，请参阅[编辑曲线](https://docs.unity.cn/cn/2020.3/Manual/EditingCurves.html)指南。

## 创建音频源

如果不为音频源指定\_\_音频剪辑\_\_，音频源不起作用。剪辑音源是将要播放的音源文件。音频源就像一个控制器，用于启动和停止该剪辑音源的播放，以及修改其他音频属性。

要创建新的音频源，请执行以下操作：

1. 将音频文件导入到 Unity 项目中。这些文件成了剪辑音源。
2. 从菜单栏中选择 **GameObject > Create Empty**。 1.在选中新的游戏对象之后，选择 **Component > Audio > Audio Source**。 1.在 Inspector 中，找到 Audio Source 组件上的 **Audio Clip** 属性并分配一个剪辑，方法是从 Project 窗口中拖动一个剪辑，或者单击 Inspector 属性右侧的小圆圈图标，然后从列表中选择一个剪辑。

**注意：**如果只想要为 Assets 文件夹中的一个\_\_音频剪辑\_\_创建\_\_音频源\_\_，则可以将该剪辑音源拖动到 Scene 视图中，随后将自动为其创建一个包含\_\_音频源\_\_组件的游戏对象。如果将剪辑音源拖动到现有游戏对象上，则会附加剪辑以及新的\_\_音频源\_\_（如果还没有音频源）。如果对象已经有了\_\_音频源\_\_，则新拖放的剪辑音源将替换音频源当前使用的剪辑音源。

## API 资源

+ [AudioSource](https://docs.unity.cn/cn/2020.3/ScriptReference/AudioSource.html)
+ [AudioClip](https://docs.unity.cn/cn/2020.3/ScriptReference/AudioClip.html)
+ [AudioListener](https://docs.unity.cn/cn/2020.3/ScriptReference/AudioListener.html)
+ [AudioMixer](https://docs.unity.cn/cn/2020.3/ScriptReference/Audio.AudioMixer.html)

## 来源

**[Unity 用户手册 2020.3 (LTS)](<https://docs.unity.cn/cn/2020.3/Manual/class-AudioSource.html>)**
