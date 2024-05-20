﻿using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int health;
    public bool isLocalPlayer;

    public RectTransform healthBar;
    private float originalHealthBarSize;


    [Header("UI")]
    
    public TextMeshProUGUI healthText;


    private void Start()
    {
        originalHealthBarSize = healthBar.sizeDelta.x;
    }


    [PunRPC]
    // Mặc định [PunRPC(Target = PunRPCTarget.All)]

    public void TakeDamage(int _damage)
    {
        health -= _damage;

        // cập nhật thanh máu
        healthBar.sizeDelta = new Vector2(originalHealthBarSize * health / 100f, healthBar.sizeDelta.y);

        healthText.text = health.ToString();

        // Hồi sinh khi ngỏm
        if (health <= 0)
        {
            if (isLocalPlayer)
            {
                RoomManager.instance.SpawnPlayer();
                RoomManager.instance.deaths++;
                RoomManager.instance.SetHashes();
            }

            Destroy(gameObject);
        }
    }  
}
