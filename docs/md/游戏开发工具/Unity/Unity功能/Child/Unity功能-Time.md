# Unity功能-Time

## UnityEngine.Time

Time.time:（只读）表示游戏开始到现在的时间,若游戏暂停则停止计算

Time.timeSinceLevelLoad:（只读）表示从当前Scene开始到目前为止的时间,游戏暂停而停止

Time.deltaTime:（只读）表示从上一帧到当前帧的时间,以秒为单位

Time.fixedTime:（只读）表示以秒计游戏开始的时间，固定时间以定期间隔更新（相当于fixedDeltaTime）直到达到time属性。

TIme.fixedDeltaTime:表示以秒计间隔，在物理和其他固定帧率进行更新，在Edit->ProjectSettings->Time的Fixed Timestep可以自行设置。

Time.maximumDeltaTime：一帧能获得的最长时间。物理和其他固定帧速率更新(类似MonoBehaviour FixedUpdate)。

Time.SmoothDeltaTime：（只读）表示一个平稳的deltaTime，根据前 
N帧的时间加权平均的值。

Time.timeScale：时间缩放，默认值为1。若设置<1，表示时间减慢；若设置>1,表示时间加快；若设置=0，则游戏暂停。可以用来加速、减速和暂停游戏，非常有用。

Time.frameCount：（只读）总帧数

Time.realtimeSinceStartup：（只读）表示自游戏开始后的总时间，即使暂停也会不断的增加。

Time.captureFramerate：表示设置每秒的帧率，然后不考虑真实时间。

Time.unscaledDeltaTime：（只读）不考虑timescale时候与deltaTime相同，若timescale被设置，则无效。

Time.unscaledTime：（只读）不考虑timescale时候与time相同，若timescale被设置，则无效。
