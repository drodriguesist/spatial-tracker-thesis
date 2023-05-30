using UnityEngine;

namespace SuicideBomber.Audio.Roar
{
    public class RoarAudioController : MonoBehaviour
    {
        #region PROPERTIES
        [SerializeField] AudioClip[] roarAC;
        AudioSource roarAS;
        GameObject GameManager;
        Transform target;
        float previousDistance;
        #endregion
        // Start is called before the first frame update
        void Start()
        {
            GameManager = GameObject.FindGameObjectWithTag("GameController");
            target = GameManager.GetComponent<GameManagerController>().player.transform;
		    previousDistance = float.MaxValue;
            roarAS = GetComponent<AudioSource>();
            roarAS.volume = 0f;
        }

        public AudioClip GetRandomClip()
        {
            return roarAS.clip = roarAC[0];
        }

        public void SetVolume(float value)
        {
            roarAS.volume = value;
        }

        public void PlayAudio()
        {
            if (!roarAS.isPlaying)
            {
                roarAS.clip = GetRandomClip();
                roarAS.Play();
            }
        }

        public void StopAudio()
        {
            roarAS.Stop();
        }

        public void CheckDistanceToPlayer()
        {
            float newdistanceToTarget = Vector3.Distance(target.position, transform.parent.position);
            if (newdistanceToTarget < previousDistance)
            {
                roarAS.volume += 0.0035f;
            }
        }
    }
}
