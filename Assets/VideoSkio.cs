using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoSkio : MonoBehaviour
{
    VideoPlayer MyVideo;
    bool IsPlaying;

    float TimeToWait;
    // Start is called before the first frame update
    void Awake()
    {
        MyVideo = GetComponent<VideoPlayer>();
        
    }

    private void Start()
    {
        MyVideo.Play();
        IsPlaying = true;
    }

    // Update is called once per frame
    void Update()
    {

        TimeToWait += Time.deltaTime;
        if (!MyVideo.isPlaying && IsPlaying && TimeToWait > 2f)
            SceneManager.LoadScene("Menu");
    }
}
