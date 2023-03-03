using Infrastructure.Services;
using Infrastructure.Services.ResetLevel;
using UnityEngine;

namespace Logic
{
    public class LevelResetTrigger : MonoBehaviour
    {
        private IResetLevelService _resetLevelService;

        private AudioSource _audioSource;

        public void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void Construct(IResetLevelService resetLevelService)
        {
            _resetLevelService = resetLevelService;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                ResetLevel();
            }
        }

        private void ResetLevel()
        {
            PickUp();
            _resetLevelService.ResetLevel(Constants.MainLevel);
        }
    
        private void PickUp()
        {
            _audioSource.Play();
        }
    }
}
