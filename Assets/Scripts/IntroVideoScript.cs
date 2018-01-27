using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroVideoScript : MonoBehaviour {

    public UnityEngine.Video.VideoClip introVid;
    UnityEngine.Video.VideoPlayer videoPlayer;
    public string nextLevelToLoad = "MainMenu";

	// Use this for initialization
	void Start () {
        videoPlayer = gameObject.AddComponent<UnityEngine.Video.VideoPlayer>();
        videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.CameraNearPlane;
        videoPlayer.clip = introVid;
	}

    void Update()
    {
        float currentFrame = videoPlayer.frame;
        float frameCount = videoPlayer.frameCount;
        if (currentFrame == frameCount)
        {
            SceneManager.LoadScene(nextLevelToLoad);
        }
    }

}
