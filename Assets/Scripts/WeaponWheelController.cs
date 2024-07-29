using UnityEngine;
using UnityEngine.UI;


public class WeaponWheelController : MonoBehaviour
{
    public static WeaponWheelController instance;

    public Animator anim;
    private bool WeaponWheelSelected = false;
    public Image selectedItem;
    public Sprite NoImage;
    public static int weaponID;

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("OpenCloseWeaponWheel"))
        {
            WeaponWheelSelected = !WeaponWheelSelected;
        }

        if (WeaponWheelSelected)
        {
            anim.SetBool("OpenWeaponWheel", true);
        }
        else
        {
            anim.SetBool("OpenWeaponWheel", false);
        }
    }

    public void SelectWeapon()
    {
        switch (weaponID)
        {
            case 0: // nothing is selected
                selectedItem.sprite = NoImage;
                ObjectPool.instance.ObjectToPool(0);
                break;

            case 1: // Bullet
                ObjectPool.instance.ObjectToPool(1);
                break;

            case 2: // BulletOsci
                ObjectPool.instance.ObjectToPool(2);
                break;

            case 3: // Rocket
                ObjectPool.instance.ObjectToPool(3);
                break;
        }
    }
}
