
    using UnityEngine;

    public interface IWeaponShootHandler
    {
        void BowShoot(Animator bodyAnimator, GameObject bullet, Transform spawnPoint, GameObject player);
        void UltimateBowShoot(Animator handAnimator, GameObject bullet, Transform spawnPoint, GameObject player);
    }
