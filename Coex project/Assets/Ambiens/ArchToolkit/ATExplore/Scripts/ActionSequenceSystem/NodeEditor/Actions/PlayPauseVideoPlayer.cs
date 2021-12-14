using System.Collections;
using System.Collections.Generic;
using ambiens.archtoolkit.atexplore.XNode;
using UnityEngine;
using UnityEngine.Video;

namespace ambiens.archtoolkit.atexplore.actionSystem
{
    [CreateNodeMenuAttribute("Actions/Play-Pause Video Player")]
    public class PlayPauseVideoPlayer : AAction
    {

        public enum VideoPlayerState
        {
            play,
            pause,
            togglePlay,
            stop
        }
        
        public VideoPlayerState startState = VideoPlayerState.play;
        
        protected override void _RuntimeInit()
        {
            if (!this.InitSceneReferences())
            {
                return;
            }

            
        }
        
        protected override bool _StartAction()
        {
            foreach (var go in this.SceneReferences)
            {
                var vp = go.GetComponent<VideoPlayer>();
                if (startState == VideoPlayerState.play)
                    vp.Play();
                else if (startState == VideoPlayerState.stop)
                    vp.Stop();
                else if (startState == VideoPlayerState.pause)
                    vp.Pause();
                else if (startState == VideoPlayerState.togglePlay)
                {
                    if(vp.isPlaying) vp.Pause();
                    else vp.Play();
                }
                    


            }
            
            return true;
        }
        
    }
}