/*****************************************************************************
// File Name : EnemyMovement.cs
// Author : Brandon Koederitz
// Creation Date : August 26, 2025
// Last Modified : August 26, 2025
//
// Brief Description : Moves enemies towards the goal at the left end of the scren.
*****************************************************************************/
using System.Collections;
using UnityEngine;

namespace LaneDefender.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;

        private bool isPaused;

        #region Component References
        [SerializeReference, HideInInspector] private Rigidbody2D rb;
        /// <summary>
        /// Automatically get components on reset
        /// </summary>
        [ContextMenu("Reset Component References")]
        private void Reset()
        {
            rb = GetComponent<Rigidbody2D>();
        }
        #endregion

        #region Properties
        public bool IsPaused
        {
            get
            {
                return isPaused;
            }
            set
            {
                isPaused = value;
            }
        }
        #endregion

        /// <summary>
        /// Continually moves the enemies during FixedUpdate
        /// </summary>
        private void FixedUpdate()
        {
            if (!isPaused)
            {
                rb.MovePosition(rb.position + (Vector2.left * moveSpeed * Time.fixedDeltaTime));
            }
        }
    }
}
