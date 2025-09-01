/*****************************************************************************
// File Name : LivesDisplay.cs
// Author : Brandon Koederitz
// Creation Date : August 26, 2025
// Last Modified : August 26, 2025
//
// Brief Description : Shows the lives the player has remaining on the UI.
*****************************************************************************/
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LaneDefender
{
    public class LivesDisplay : MonoBehaviour
    {
        [SerializeField] private List<GameObject> displayImages;

        /// <summary>
        /// Organizes the display images by their X position, so that images are always removed right to left.
        /// </summary>
        private void Awake()
        {
            displayImages = displayImages.OrderBy(item => item.transform.position.x).Reverse().ToList();
        }

        /// <summary>
        /// Removes a life from our life display
        /// </summary>
        public void RemoveLife()
        {
            if (displayImages != null && displayImages.Count > 0)
            {
                GameObject toDestroy = displayImages[0];
                displayImages.RemoveAt(0);
                Destroy(toDestroy);
            }
        }
    }
}