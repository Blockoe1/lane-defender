/*****************************************************************************
// File Name : TankShooting.cs
// Author : Brandon Koederitz
// Creation Date : August 26, 2025
// Last Modified : August 26, 2025
//
// Brief Description : Allows the player to shoot bullets when they press the spacebar.
*****************************************************************************/
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace LaneDefender
{
    [RequireComponent (typeof (PlayerInput))]
    public class TankShooting : MonoBehaviour
    {
        #region CONSTS
        private const string SHOOT_ACTION_NAME = "Shoot";
        #endregion

        [SerializeField] private Transform barrelLaunchPoint;
        [SerializeField] private Rigidbody2D projectilePrefab;
        [SerializeField] private float projectileSpeed;
        [SerializeField] private float shootCooldown;
        [SerializeField] private UnityEvent OnShoot;

        // Input
        private InputAction shootAction;

        private bool isShooting;
        private bool isCooldown;

        /// <summary>
        /// Setup input on awake.
        /// </summary>
        private void Awake()
        {
            if (TryGetComponent(out PlayerInput input))
            {
                shootAction = input.currentActionMap.FindAction(SHOOT_ACTION_NAME);

                shootAction.started += ShootAction_Started;
                shootAction.canceled += ShootAction_Canceled;
            }
        }

        /// <summary>
        /// Unsubscribe events
        /// </summary>
        private void OnDestroy()
        {
            shootAction.started -= ShootAction_Started;
            shootAction.canceled -= ShootAction_Canceled;
        }

        /// <summary>
        /// Toggle isShooting based on if the shoot button is being held down or not.
        /// </summary>
        /// <remarks>
        /// We track if the palyer is holding down shoot so that we can immediately shoot agian once we come off
        /// cooldown if the button is held.
        /// </remarks>
        /// <param name="obj"></param>
        private void ShootAction_Started(InputAction.CallbackContext obj)
        {
            isShooting = true;
            Shoot();
        }
        private void ShootAction_Canceled(InputAction.CallbackContext obj)
        {
            isShooting = false;
        }

        /// <summary>
        /// Causes this object to shoot it's given projectile prefab.
        /// </summary>
        private void Shoot()
        {
            // Prevent any shooting if we're on cooldown.
            if (isCooldown) { return;  }

            // Shoot Here
            // Spawn the projectile prefab at the location of our barrel with our rotation.
            Rigidbody2D projectileRB = Instantiate(projectilePrefab, barrelLaunchPoint.position, transform.rotation);
            Vector2 launchVector = new Vector2(Mathf.Cos(transform.eulerAngles.z), Mathf.Sin(transform.eulerAngles.z));
            projectileRB.linearVelocity = launchVector.normalized * projectileSpeed;

            // Call OnShoot so we can perform other code when a shot is fired.
            OnShoot?.Invoke();

            // Puts the player on cooldown for their given cooldown time.
            StartCoroutine(CooldownRoutine(shootCooldown));
        }

        /// <summary>
        /// Prevents the player from shooting for a certain amount of time
        /// </summary>
        private IEnumerator CooldownRoutine(float cooldownDuration)
        {
            isCooldown = true;
            yield return new WaitForSeconds(cooldownDuration);
            isCooldown = false;

            // Once the player is finished with their cooldown, immediately shoot again if we're still holding down
            // the shoot button
            if (isShooting)
            {
                Shoot();
            }
        }
    }
}
