using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Audio.Footsteps
{
    public class FootstepsAudioController : MonoBehaviour
    {
        [SerializeField] AudioClip[] footstepsACL;
        [SerializeField] AudioClip[] footstepsACR;
        AudioSource footstepsAS;
        GameObject GameManager;
        Transform target;
        float previousDistance;
        int audioClipIndex;
        int previousArrayIndex;
        int[] previousArrayL;
        int[] previousArrayR;

        public bool IsEnemy { get; set; }
        
        bool test;

        // Start is called before the first frame update
        void Start()
        {
            var scriptHolderObj = transform.parent.transform.parent;
            footstepsAS = GetComponent<AudioSource>();
            if(scriptHolderObj.tag == "Enemy")
            {
                GameManager = GameObject.FindGameObjectWithTag("GameController");
                target = GameManager.GetComponent<GameManagerController>().player.transform;
		        previousDistance = float.MaxValue;
                footstepsAS.volume = 0f;
                IsEnemy = true;
            }
            test = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public AudioClip GetRandomClip(int [] previousArray, AudioClip[] footstepsAC)
        {
            //Initialize
            if(previousArray == null)
            {
                // Sets the length to half of the number of AudioClips
                // This will round downwards
                // So it works with odd numbers like for example 3
                previousArray = new int[footstepsAC.Length / 2];
            }
            if (previousArray.Length == 0)
            {
                // If the the array length is 0 it returns null
                return null;
            }
            else
            {
                // Pseudo random remembering previous clips to avoid repetition
                do
                {
                    audioClipIndex = Random.Range(0, footstepsAC.Length);
                } while (PreviousArrayContainsAudioClipIndex(previousArray));

                // Adds the selected array index to the array
                previousArray[previousArrayIndex] = audioClipIndex;

                // Wrap the index
                previousArrayIndex++;
                if (previousArrayIndex >= previousArray.Length)
                {
                    previousArrayIndex = 0;
                }
            }

            // Returns the randomly selected clip
            return footstepsAC[audioClipIndex];
        }

        public void PlayPlayersFootstepsAudio()
        {
            if (!footstepsAS.isPlaying)
            {
                AudioClip clip;
                if (test)
                {
                    clip = GetRandomClip(previousArrayL, footstepsACL);
                    test = false;
                }
                else
                {
                    clip = GetRandomClip(previousArrayR, footstepsACR);
                    test = true;
                }
                if(!IsEnemy) footstepsAS.volume = Random.Range(0.25f, 1f);
                footstepsAS.PlayOneShot(clip);
            }
        }

        public void PlayFootstepsAudio()
        {
            if (!footstepsAS.isPlaying)
            {
                AudioClip clip = GetRandomClip(previousArrayR, footstepsACR);
                footstepsAS.volume = Random.Range(0.25f, 1f);
                footstepsAS.PlayOneShot(clip);
            }
        }

        public void StopPlayingAudio()
        {
            footstepsAS.Stop();
        }

        /// <summary>
        /// Returns true if the randomIndex is in the previousArray
        /// Otherwise return false
        /// </summary>
        /// <returns>
        /// Boolean (True or False)
        /// </returns>
        private bool PreviousArrayContainsAudioClipIndex(int[] previousArray)
        {
            for (int i = 0; i < previousArray.Length; i++)
            {
                if (previousArray[i] == audioClipIndex)
                {
                    return true;
                }
            }
            return false;
        }

        public void CheckDistanceToPlayer()
        {
            float newdistanceToTarget = Vector3.Distance(target.position, transform.parent.position);
            if (newdistanceToTarget < previousDistance)
            {
                footstepsAS.volume += 0.0035f;
            }
        }
    }
}
