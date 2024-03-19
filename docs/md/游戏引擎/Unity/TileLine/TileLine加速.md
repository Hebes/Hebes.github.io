# TileLine加速

<https://sole-game-creater.com/unity-timeline-change-speed/>
<https://blog.csdn.net/foxlively/article/details/115269583>
<https://blog.csdn.net/foxlively/article/details/115269968>

运行模式，代码修改：
修改全局速度：

```C#
using UnityEngine.Playables;

public class TimelineController : MonoBehaviour, IPointerClickHandler
{
  public PlayableDirector playableDirector;


  public void OnPointerClick(PointerEventData pointerData)
  {
    playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(5.0f);
  }
}
```

运行模式，代码修改：
修改指定轨道速度：

```C#
//var timeline = playableDirector != null ? (playableDirector.playableAsset as TimelineAsset) : null;
using UnityEngine.Timeline;

public class TimelineController : MonoBehaviour, IPointerClickHandler
{
  public TimelineAsset timelineAsset;

  public void OnPointerClick(PointerEventData pointerData)
  {
      IEnumerable<TrackAsset&gt tracks = timelineAsset.GetOutputTracks();
      foreach (var track in tracks)
      {
        Debug.Log(track.name);
        IEnumerable<TimelineClip> clips = track.GetClips();
        foreach (var clip in clips)
        {
          Debug.Log(clip.displayName);
          clip.timeScale = 5.0f;
        }
      }
    }
  }
}
```

校验判断

```C#
if (playableDirector.playableGraph.Equals(default(PlayableGraph)) == false)
```