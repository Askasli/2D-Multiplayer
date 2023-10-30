using UnityEngine;
using Zenject;

namespace Common.Infrastructure
{
    public class LocationInstaller : MonoInstaller
    {
        [SerializeField] private Transform startPoint;
        [SerializeField] private GameObject playerPrefab;

        public override void InstallBindings()
        {
         //   BindPlayer();
        }

        private void BindPlayer()
        {
      
        }

       


    }
}