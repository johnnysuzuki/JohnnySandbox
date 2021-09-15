﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidvaniaTools
{
    public class Weapon : Abilities
    {
        [SerializeField]
        protected List<WeaponTypes> weaponTypes;
        [SerializeField]
        protected Transform gunBarrel;
        [SerializeField]
        protected Transform gunRotation;

        public List<GameObject> currentPool = new List<GameObject>();
        public GameObject currentProjectile;

        private GameObject projectileParentFolder;


        protected virtual void Update()
        {
            if (WeaponFired())
            {
                FireWeapon();
            }
        }

        protected virtual bool WeaponFired()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                return true;
            }
            else
                return false;

        }

        protected override void Initialization()
        {
            base.Initialization();
            foreach(WeaponTypes weapon in weaponTypes)
            {
                GameObject newPool = new GameObject();
                projectileParentFolder = newPool;
                objectPooler.CreatePool(weapon,currentPool,projectileParentFolder);
            }
        }

        protected virtual void FireWeapon()
        {
            currentProjectile = objectPooler.GetObject(currentPool);
            if(currentProjectile != null)
            {
                Invoke("PlaceProjectile", .1f);
            }
        }

        protected virtual void PlaceProjectile()
        {
            currentProjectile.transform.position = gunBarrel.position;
            currentProjectile.transform.rotation = gunRotation.rotation;
            currentProjectile.SetActive(true);
            if (!character.isFacingLeft)
            {
                currentProjectile.GetComponent<Projectile>().left = false;
            }
            else
            {
                currentProjectile.GetComponent<Projectile>().left = true;

            }
            currentProjectile.GetComponent<Projectile>().fired = true;
        }
    }
}