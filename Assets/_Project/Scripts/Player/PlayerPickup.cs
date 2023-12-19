using UnityEngine;
using static DragonspiritGames.PlatformerController.Pickup;

namespace DragonspiritGames.PlatformerController
{
    public class PlayerPickup : MonoBehaviour
    {
        [SerializeField] AudioClip _ambrosiaSmallSoundEffect;
        [SerializeField] AudioClip _ambrosiaLargeSoundEffect;
        
        AudioSource _audioSource;
        Pickup _pickup;
        

        void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _pickup = GetComponent<Pickup>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var pickup = collision.GetComponent<Pickup>();
            if (pickup != null)
            {
                if (pickup.pickupType == PickupType.SmallAmbrosia)
                {
                    Debug.Log("Small Ambrosia picked up");
                    _audioSource.PlayOneShot(_ambrosiaSmallSoundEffect);
                }
                if (pickup.pickupType == PickupType.BigAmbrosia)
                {
                    Debug.Log("Big Ambrosia picked up");
                    _audioSource.PlayOneShot(_ambrosiaLargeSoundEffect);
                }
                Destroy(collision.gameObject);
            }
        }
    }
}
