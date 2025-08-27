/*****************************************************************************
// File Name : KillZone.cs
// Author : Brandon Koederitz
// Creation Date : August 26, 2025
// Last Modified : August 26, 2025
//
// Brief Description : Destroys bullets when they go too far off screen.
*****************************************************************************/
using UnityEngine;

namespace LaneDefender
{
    public class KillZone : MonoBehaviour
    {
        [SerializeField] private float maxX;

        /// <summary>
        /// Check for being beyond the kill zone, and destroy if exceeded.
        /// </summary>
        private void FixedUpdate()
        {
            if (transform.position.x > maxX)
            {
                Destroy(gameObject);
            }
        }
    }
}
