using UnityEngine;

namespace ErfanDeveloper
{
    public enum SoundType
    {
        CreateSmallObject,
        CreateBigObject,
        WrongPlacement,
        RemoveObject
    }
    public class SoundFeedBack : MonoBehaviour
    {
        [Header("Audio Setting")]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip createSmallObjectClip;
        [SerializeField] private AudioClip createBigObjectClip;
        [SerializeField] private AudioClip cantCreateClip;
        [SerializeField] private AudioClip removeClick;
        
        public void PlaySound(SoundType type)
        {
            switch (type)
            {
                 case SoundType.CreateSmallObject:
                     _audioSource.PlayOneShot(createSmallObjectClip);
                     break;
                 case SoundType.CreateBigObject :
                     _audioSource.PlayOneShot(createBigObjectClip);
                     break;
                 case SoundType.WrongPlacement:
                     _audioSource.PlayOneShot(cantCreateClip);
                     break;
                 case SoundType.RemoveObject:
                     _audioSource.PlayOneShot(removeClick);
                     break;
            }
        }
    }
}