using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] Profecor profecor;
    [SerializeField] Transform weaponUpgradePanel;
    [SerializeField] GameObject weaponUpgradeUIPrefab;
    [SerializeField] List<WeaponData> weaponUpgradeDatas;
    List<int> weaponHavingList = new List<int>();
    int weaponUpgradeIndex = 0;
    [SerializeField] Button weaponUpgradeButton;
    
    void Start()
    {
        weaponHavingList.Add(0);
        InitWeaponUpgradePanel();
    }

    void InitWeaponUpgradePanel()
    {
        for(int i = 0; i < weaponUpgradeDatas.Count; i++)
        {
            Transform upgradeItem = Instantiate(weaponUpgradeUIPrefab, weaponUpgradePanel).transform;
            WeaponData weaponData = weaponUpgradeDatas[i];
            upgradeItem.Find("Icon").GetComponent<Image>().sprite = weaponData.icon;
            upgradeItem.Find("Status_Right").GetComponent<TextMeshProUGUI>().text = 
            $"{weaponData.velocity}\n" +
            $"{weaponData.limit}\n" +
            $"{weaponData.size}\n" +
            $"{weaponData.charge}";
            RefreshWeaponButton(i);
            int temp = i;
            upgradeItem.Find("EquipButton").GetComponent<Button>().onClick.AddListener(() => EquipWeapon(temp));
            upgradeItem.Find("BuyButton").GetComponent<Button>().onClick.AddListener(() => BuyWeapon(temp));
        }
    }

    void RefreshWeaponButton(int index)
    {
        Transform upgradeItem = weaponUpgradePanel.GetChild(index);
        if(CheckWeaponHaving(index) == true)
        {
            upgradeItem.Find("BuyButton").gameObject.SetActive(false);
            upgradeItem.Find("EquipButton").gameObject.SetActive(true);
        }
        else
        {
            upgradeItem.Find("BuyButton").gameObject.SetActive(true);
            upgradeItem.Find("EquipButton").gameObject.SetActive(false);

            upgradeItem.Find("BuyButton").Find("Price").GetComponent<TextMeshProUGUI>().text = weaponUpgradeDatas[index].price + "명";
        }
    }
    bool CheckWeaponHaving(int index)
    {
        return weaponHavingList.Contains(index);
    }
    public bool TryUpgrade(int price)
    {
        if(profecor.score < price) return false;
        profecor.UseScore(price);
        return true;
    }

    public void EquipWeapon(int index)
    {
        if(CheckWeaponHaving(index) == true)
        {
            profecor.EquipWeapon(weaponUpgradeDatas[index]);
        }
    }

    public void BuyWeapon(int index)
    {
        if(TryUpgrade(weaponUpgradeDatas[index].price))
        {
            weaponHavingList.Add(index);
            RefreshWeaponButton(index);
        }
    }
}
