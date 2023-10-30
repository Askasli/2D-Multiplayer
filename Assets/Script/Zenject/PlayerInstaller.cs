using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


namespace Common.Infrastructure
{
    public class PlayerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<PlayerSpawner>().FromComponentInHierarchy().AsSingle();
            Container.Bind<PlayerManager>().FromComponentInHierarchy().AsSingle();
        }

    }
}