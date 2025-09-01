/*****************************************************************************
// File Name : ImpactEffects.cs
// Author : Brandon Koederitz
// Creation Date : August 26, 2025
// Last Modified : August 26, 2025
//
// Brief Description : Spawns visual effects when this object collides with another object.
*****************************************************************************/
using UnityEngine;

namespace LaneDefender
{
    public class ImpactEffects : MonoBehaviour
    {
        [SerializeField] private GameObject effectsToSpawn;

        /// <summary>
        /// Spawns effects when this object collides with another object.
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Creates the effects to spawn at the collision point between this and the other object.
            Vector3 spawnPoint;
            if (collision.contactCount > 0)
            {
                spawnPoint = collision.GetContact(0).point;
            }
            else
            {
                spawnPoint = transform.position;
            }
            Instantiate(effectsToSpawn, spawnPoint, Quaternion.identity);
        }
    }
}
