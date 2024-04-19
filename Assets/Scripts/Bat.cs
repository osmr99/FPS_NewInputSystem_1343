using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Gun
{
    int damage;
    [SerializeField] int force;
    [SerializeField] AudioSource sound;
    public override bool AttemptFire()
    {
        if (!base.AttemptFire())
            return false;

        damage = Random.Range(26, 34);

        var b = Instantiate(bulletPrefab, gunBarrelEnd.transform.position, gunBarrelEnd.rotation);
        b.GetComponent<Projectile>().Initialize(damage, 1, 0.02f, force, null); // version without special effect

        sound?.Play();

        anim.SetTrigger("shoot");
        elapsed = 0;
        //ammo -= 1; Max ammo must be 1 in order to swing the bat infinite times, the ammo won't ever decrease
        // so this can work correctly.

        return true;
    }

    public override void Equip(FPSController p)
    {
        base.Equip(p);
        transform.localPosition = new Vector3(-0.02f, 0.22f, 0);
    }

}
