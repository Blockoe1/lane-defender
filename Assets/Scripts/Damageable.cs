/*****************************************************************************
// File Name : Damageable.cs
// Author : Brandon Koederitz
// Creation Date : August 26, 2025
// Last Modified : August 26, 2025
//
// Brief Description : Allows an object to be damaged when it comes into contact with a projectile.
*****************************************************************************/
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace LaneDefender
{
    [RequireComponent(typeof(Collider2D))]
    public class Damageable : MonoBehaviour
    {
        [SerializeField] private int health;
        [SerializeField] private float hurtTime;
        [SerializeField, Tooltip("The tag that identifies what objects can deal damage to this.")] 
        private string damageTag;
        [SerializeField] private bool destroyAssailant;
        [Header("Events")]
        [SerializeField] private UnityEvent<int> OnTakeDamageEvent;
        [SerializeField] private UnityEvent OnFinishHurtTimeEvent;
        [SerializeField] private UnityEvent OnDeathEvent;

        private bool isHurt;
        private bool isDead;

        /// <summary>
        /// When this object collides with a projectile, it should lose health.
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag(damageTag))
            {
                TakeDamage(1);

                // Destroys the object that collided with this object if that setting is checked.
                if (destroyAssailant)
                {
                    Destroy(collision.collider.gameObject);
                }
            }
        }

        /// <summary>
        /// Causes this object to take damage.
        /// </summary>
        /// <param name="damage"></param>
        public void TakeDamage(int damage)
        {
            // Enemies that are in hurt time can't be hit again.
            if (isHurt) { return; }
            health -= damage;

            OnTakeDamageEvent?.Invoke(damage);

            // Always enter hurt time, even if we just died.  We'll use that information later to handle clean up after
            // a delay.
            StartCoroutine(HurtRoutine(hurtTime));

            if (health <= 0)
            {
                Die();
            }
        }

        /// <summary>
        /// Controls this object being in hurt time and being immune to damage.
        /// </summary>
        /// <param name="hurtTime"></param>
        /// <returns></returns>
        private IEnumerator HurtRoutine(float hurtTime)
        {
            isHurt = true;
            yield return new WaitForSeconds(hurtTime);
            isHurt = false;

            // Broadcast out that we are finished with hurt time so anything that is synced to hurt time, like
            // animations, can end.
            OnFinishHurtTimeEvent?.Invoke();

            // If we died from the attack that put us in this hurt time, then we should destroy this object after hurt
            // time is done.
            if (isDead)
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Causes this object to die, and handles any behaviour that happens on death.
        /// </summary>
        private void Die()
        {
            OnDeathEvent?.Invoke();
            isDead = true;
        }
    }
}
