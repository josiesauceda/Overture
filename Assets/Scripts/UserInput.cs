using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class UserInput : MonoBehaviour
{
    public cat player;
    public List<Transform> targetOrder = new List<Transform>();
    private int currentTargetIndex = 0;
    private float nextThrowTime = 0f;
    private float defaultThrowCooldown = 1f;
    private float hairballThrowCooldown = 0.4f;
    public bool enableThrowing = true;

    void Update()
    {
        Vector3 direction = new Vector3(0, 0, 0);

        if (Keyboard.current.wKey.isPressed)
            direction.z += 1;
        if (Keyboard.current.sKey.isPressed)
            direction.z -= 1;
        if (Keyboard.current.aKey.isPressed)
            direction.x -= 1;
        if (Keyboard.current.dKey.isPressed)
            direction.x += 1;

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
            player.Jump();

        if (Keyboard.current.eKey.wasPressedThisFrame && enableThrowing)
            ThrowRegularBall();

        if (Keyboard.current.qKey.wasPressedThisFrame && enableThrowing)
            ThrowStars();

        if (Keyboard.current.fKey.wasPressedThisFrame)
            TryFreeze();

        player.Move(direction);
    }

    void ThrowRegularBall()
    {
        targetOrder.RemoveAll(t => t == null);
        if (targetOrder.Count == 0) return;
        if (currentTargetIndex >= targetOrder.Count) currentTargetIndex = 0;

        float cooldown = (PlayerAbilities.instance != null &&
                         PlayerAbilities.instance.hasHairball)
                         ? hairballThrowCooldown
                         : defaultThrowCooldown;

        if (Time.time < nextThrowTime) return;
        nextThrowTime = Time.time + cooldown;

        Transform target = targetOrder[currentTargetIndex];
        Debug.Log("Throwing regular ball at: " + target.name);
        player.ThrowRegularBall(target);
    }

    void ThrowStars()
    {
        if (PlayerAbilities.instance == null || !PlayerAbilities.instance.canThrowStars)
        {
            Debug.Log("Star attack not unlocked!");
            return;
        }

        targetOrder.RemoveAll(t => t == null);
        if (targetOrder.Count == 0) return;
        if (currentTargetIndex >= targetOrder.Count) currentTargetIndex = 0;

        float cooldown = (PlayerAbilities.instance.hasHairball)
            ? hairballThrowCooldown
            : defaultThrowCooldown;

        if (Time.time < nextThrowTime) return;
        nextThrowTime = Time.time + cooldown;

        Transform target = targetOrder[currentTargetIndex];
        Debug.Log("Throwing stars at: " + target.name);
        player.ThrowStars(target);
    }

    void TryFreeze()
    {
        if (PlayerAbilities.instance == null) return;
        if (!PlayerAbilities.instance.canFreeze) return;

        targetOrder.RemoveAll(t => t == null);
        if (targetOrder.Count == 0) return;
        if (currentTargetIndex >= targetOrder.Count) currentTargetIndex = 0;

        Transform target = targetOrder[currentTargetIndex];

        pig p = target.GetComponent<pig>() ?? target.GetComponentInParent<pig>();
        if (p != null) { p.Freeze(PlayerAbilities.instance.freezeDuration); return; }

        Penguin pen = target.GetComponent<Penguin>() ?? target.GetComponentInParent<Penguin>();
        if (pen != null) { pen.Freeze(PlayerAbilities.instance.freezeDuration); return; }

        Corgi c = target.GetComponent<Corgi>() ?? target.GetComponentInParent<Corgi>();
        if (c != null) c.Freeze(PlayerAbilities.instance.freezeDuration);
    }
}