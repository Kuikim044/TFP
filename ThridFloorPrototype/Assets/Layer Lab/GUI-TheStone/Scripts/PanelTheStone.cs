﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LayerLabT.heStone
{
    public class PanelTheStone : MonoBehaviour
    {
        [SerializeField] private GameObject[] otherPanels;

        public void OnEnable()
        {
            for (int i = 0; i < otherPanels.Length; i++) otherPanels[i].SetActive(true);
        }

        public void OnDisable()
        {
            for (int i = 0; i < otherPanels.Length; i++) otherPanels[i].SetActive(false);
        }
    }
}