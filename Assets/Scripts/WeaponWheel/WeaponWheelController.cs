using UnityEngine;
using UnityEngine.UI;


public class WeaponWheelController : MonoBehaviour
{

    public Animator anim;
    private bool WeaponWheelSelected = false;
    public Image selectedItem;
    public Sprite NoImage;
    public static int weaponID;

    [SerializeField] private int playerNumber;

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("OpenCloseWeaponWheel" + playerNumber))
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

    public void SelectWeapon(int playerNumber)
    {
        if (playerNumber == 1)
        {
            switch (weaponID)
            {
                case 0: // nothing is selected
                    selectedItem.sprite = NoImage;
                    ObjectPool.instance.ObjectToPool1(0);
                    break;

                case 1: // Bullet
                    ObjectPool.instance.ObjectToPool1(1);
                    break;

                case 2: // BulletOsci
                    ObjectPool.instance.ObjectToPool1(2);
                    break;

                case 3: // Rocket
                    ObjectPool.instance.ObjectToPool1(3);
                    break;
            }
        }

        if (playerNumber == 2)
        {
            switch (weaponID)
            {
                case 0: // nothing is selected
                    selectedItem.sprite = NoImage;
                    ObjectPool.instance.ObjectToPool2(0);
                    break;

                case 1: // Bullet
                    ObjectPool.instance.ObjectToPool2(1);
                    break;

                case 2: // BulletOsci
                    ObjectPool.instance.ObjectToPool2(2);
                    break;

                case 3: // Rocket
                    ObjectPool.instance.ObjectToPool2(3);
                    break;
            }
        }
        
    }
}
