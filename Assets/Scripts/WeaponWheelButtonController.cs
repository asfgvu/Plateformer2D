using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponWheelButtonController : MonoBehaviour
{
    public int Id;
    private Animator anim;
    public string ItemName;
    public TextMeshProUGUI itemText;
    public Image selectedItem;
    private bool selected = false;
    public Sprite icon;
    public WeaponWheelController weaponWheelController;
    public int playerNumber;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (selected)
        {
            selectedItem.sprite = icon;
            itemText.text = ItemName;
        }
    }


    public void Selected()
    {
        selected = true;
        WeaponWheelController.weaponID = Id;
        weaponWheelController.SelectWeapon(playerNumber);
    }

    public void Deselected()
    {
        selected = false;
        WeaponWheelController.weaponID = 0;
    }

    public void HoverEnter()
    {
        anim.SetBool("Hover",true);
        itemText.text = ItemName;
    }

    public void HoverExit()
    {
        anim.SetBool("Hover", false);
        itemText.text = "";
    }
}
