/*****************************************************************************
// File Name : LaneManager.cs
// Author : Brandon Koederitz
// Creation Date : August 26, 2025
// Last Modified : August 26, 2025
//
// Brief Description : Controls the lanes that are present in each scene.
*****************************************************************************/
using System.Linq;
using UnityEngine;

namespace LaneDefender
{
    public class LaneManager : MonoBehaviour
    {
        [SerializeField] private float[] laneHeights;

        private static float[] lanes;

        #region Properties
        public static float[] Lanes
        {
            get
            {
                return lanes;
            }
        }
        #endregion

        /// <summary>
        /// Setup singleton reference for the lane array.
        /// </summary>
        private void Awake()
        {
            if (lanes != null && lanes != laneHeights)
            {
                Debug.Log("Multiple LaneManagers found.");
                return;
            }
            else
            {
                // Ensures the lanes are ordered by their y position, with the lowest lane being first.
                lanes = laneHeights.OrderBy(item => item).ToArray();
            }
        }

        /// <summary>
        /// Reste the lanes reference when this object is destroyed.
        /// </summary>
        private void OnDestroy()
        {
            if (lanes == laneHeights)
            {
                lanes = null;
            }
        }
    }
}