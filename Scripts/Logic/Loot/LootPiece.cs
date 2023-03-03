using Data;
using UnityEngine;

namespace Loot
{
    public class LootPiece : MonoBehaviour
    {
        [SerializeField] private GameObject _visual; 
        private Data.Loot _loot;
        private WorldData _worldData;
        private bool _picked;

        private AudioSource _audioSource;

        private int _destroyDelay = 5;

        public void Construct(WorldData worldData)
        {
            _worldData = worldData;
            _audioSource = gameObject.GetComponent<AudioSource>();
        }

        public void Initialize(Data.Loot loot)
        {
            _loot = loot;
        }
        
        private void OnTriggerEnter(Collider other) => PickUp();
        private void PickUp()
        {
            if (_picked)
                return;

            _picked = true;
            
            _worldData.LootData.Collect(_loot);

            HideModel();
            
            _audioSource.Play();
            
            Destroy(gameObject, _destroyDelay);
        }

        private void HideModel()
        {
            _visual.SetActive(false);
        }
    }
}