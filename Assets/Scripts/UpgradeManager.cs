using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;
    [SerializeField] Profecor profecor;
    [SerializeField] Transform weaponUpgradePanel;
    [SerializeField] GameObject weaponUpgradeUIPrefab;
    [SerializeField] List<WeaponData> weaponUpgradeDatas;


    [SerializeField] Transform skillUpgradePanel;
    [SerializeField] GameObject skillUpgradeUIPrefab;
    public List<SkillData> skillUpgradeDatas;
    List<int> weaponHavingList = new List<int>();
    public List<int> skillHavingList = new List<int>();
    int equippedWeaponIndex = 0;
    [SerializeField] Button weaponUpgradeButton;
    SkillManager skillManager;
    void Awake()
    {
        if(Instance != null) Destroy(this.gameObject);
        Instance = this;
    }

    void Start()
    {
        weaponHavingList.Add(0);
        InitWeaponUpgradePanel();
        InitSkillUpgradePanel();
        skillManager = FindAnyObjectByType<SkillManager>();
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
            $"{weaponData.charge}\n" +
            $"{weaponData.size}";
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
            upgradeItem.Find("EquipButton").Find("EquipText").GetComponent<TextMeshProUGUI>().text = (equippedWeaponIndex == index) ? "장착 중" : "장착";

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

    void InitSkillUpgradePanel()
    {
        for(int i = 0; i < skillUpgradeDatas.Count; i++)
        {
            Transform upgradeItem = Instantiate(skillUpgradeUIPrefab, skillUpgradePanel).transform;
            SkillData skillData = skillUpgradeDatas[i];
            upgradeItem.Find("Icon").GetComponent<Image>().sprite = skillData.icon;
            upgradeItem.Find("Name").GetComponent<TextMeshProUGUI>().text = skillData.name;
            upgradeItem.Find("Description").GetComponent<TextMeshProUGUI>().text = skillData.description;

            int temp = i;   
            upgradeItem.Find("BuyButton").GetComponent<Button>().onClick.AddListener(() => BuySkill(temp));
            RefreshSkillButton(i);
        }
    }
    void RefreshSkillButton(int index)
    {
        Transform upgradeItem = skillUpgradePanel.GetChild(index);
        if(CheckSkillHaving(index) == true)
        {
            upgradeItem.Find("BuyButton").Find("Price").GetComponent<TextMeshProUGUI>().text = "구매 완료";
        }
        else
        {
            upgradeItem.Find("BuyButton").Find("Price").GetComponent<TextMeshProUGUI>().text = skillUpgradeDatas[index].price + "명";
        }
    }
    
    public bool CheckSkillHaving(int index)
    {   
        return skillHavingList.Contains(index);
    }

    void BuySkill(int index)
    {
        if(CheckSkillHaving(index) == true) return;
        if(TryUpgrade(skillUpgradeDatas[index].price))
        {
            skillHavingList.Add(index);
            RefreshSkillButton(index);
            skillManager.SkillPurchased(index);
        }
    }
    public bool TryUpgrade(int price)
    {
        if(profecor.score < price) return false;
        profecor.UseScore(price);
        return true;
    }

    public void EquipWeapon(int index)
    {
        if(equippedWeaponIndex == index) return;
        if(CheckWeaponHaving(index) == true)
        {
            int prevIndex = equippedWeaponIndex;
            equippedWeaponIndex = index;
            profecor.EquipWeapon(weaponUpgradeDatas[index]);
            RefreshWeaponButton(index);
            RefreshWeaponButton(prevIndex);
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
