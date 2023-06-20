# Unity问题-AudioSource无声音

1. 是否有AudioLisener
2. 代码运行中是否Play

   ```C#
   public AudioClip audioClip;//获取音乐
    private AudioSource audioSource;
    private void Start(){
        audioSource = this.GetComponent<AudioSource>();//获取Audio Source
        audioSource.Play();//播放
    }
   ```
