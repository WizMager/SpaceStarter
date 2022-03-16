﻿using System;
using UnityEngine;
using Utils;

namespace View
{
    public class BonusView : MonoBehaviour
    {
        [SerializeField] private BonusType _bonusType;
        public event Action<BonusType> OnBonusPickUp;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.GetComponent<PlayerView>()) return;
            
            OnBonusPickUp?.Invoke(_bonusType);
            Destroy(gameObject);
        }
    }
}