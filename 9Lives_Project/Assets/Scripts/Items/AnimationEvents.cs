using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public CharacterEffect effect;

    public void Drop_PauseAnimation()
    {
        gameObject.GetComponentInParent<Drops>().PauseAnimation();
    }

    public void Argue_Shoot()
    {
        
        Vector3 gunPort = GetComponentInChildren<Transform>().position;
        BulletManager.GetInstance().ShootBullet(gunPort, Vector3.right, 12f, BulletType.pizza);
        transform.parent.GetComponent<Argue>().PauseShoot();
    }

    public void DialogParent_CloseBackground()
    {
        gameObject.GetComponentInParent<Dialog>().background.gameObject.SetActive(false);
        //GetComponentInChildren<Transform>().gameObject.SetActive(false);
    }

    public void Thief_Attck()
    {
        Vector3 gunPort = GetComponentInChildren<Transform>().position;
        Vector3 dir = (GameObject.FindWithTag("Player").transform.position + Vector3.up * 0.4f - gunPort).normalized;
        BulletManager.GetInstance().ShootBullet(gunPort, dir, 12f, BulletType.underwear);
    }

    public void CharacterEffectEnd(CharacterEffectType type)
    {
        effect.ResetPosition(type);
        //effect.ResetPosition(CharacterEffectType.Jump_1);
        //effect.ResetPosition(CharacterEffectType.Jump_2);
    }
}
