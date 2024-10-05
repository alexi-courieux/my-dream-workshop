using System;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private static readonly int Walk = Animator.StringToHash("IsWalking");
        
    [SerializeField] private Player player;
    [SerializeField] private Animator animator;

    private void Start()
    {
        player.OnMove += Player_OnPlayerMove;
        player.OnStopMove += Player_OnStopMove;
    }

    private void Player_OnPlayerMove(object sender, EventArgs e)
    {
        animator.SetBool(Walk, true);
    }
        
    private void Player_OnStopMove(object sender, EventArgs e)
    {
        animator.SetBool(Walk, false);
    }
}