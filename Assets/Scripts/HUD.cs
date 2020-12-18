using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Sprite[] heart_sprites;
    public Image HeartUI;
    private PlayerController player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        HeartUI.sprite = heart_sprites[(int) player.health];
    }
    private void Update()
    {
        HeartUI.sprite = heart_sprites[(int) player.health];
    }
}
