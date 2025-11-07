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

    
    [SerializeField] Transform schoolUpgradePanel;
    [SerializeField] GameObject schoolUpgradeUIPrefab;
    public List<SchoolData> schoolUpgradeDatas;


    List<int> weaponHavingList = new List<int>();
    public List<int> skillHavingList = new List<int>();
    List<int> schoolHavingList = new List<int>();
    [SerializeField] SpriteRenderer platformSpriteRenderer;
    [SerializeField] SpriteRenderer fieldSpriteRenderer;
    
    int equippedWeaponIndex = -1;
    int equippedSchoolIndex = -1;




    SkillManager skillManager;
    void Awake()
    {
        if(Instance != null) Destroy(this.gameObject);
        Instance = this;
    }

    void Start()
    {
        weaponHavingList.Add(0);
        schoolHavingList.Add(0);
        InitWeaponUpgradePanel();
        InitSkillUpgradePanel();
        skillManager = FindAnyObjectByType<SkillManager>();
        InitSchoolUpgradePanel();
        EquipWeapon(0);
        EquipSchool(0);
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

    void InitSchoolUpgradePanel()
    {
        for(int i = 0; i < schoolUpgradeDatas.Count; i++)
        {
            Transform upgradeItem = Instantiate(schoolUpgradeUIPrefab, schoolUpgradePanel).transform;
            SchoolData schoolData = schoolUpgradeDatas[i];
            upgradeItem.Find("Icon").GetComponent<Image>().sprite = schoolData.icon;
            upgradeItem.Find("Name").GetComponent<TextMeshProUGUI>().text = schoolData.name;
            upgradeItem.Find("Description").GetComponent<TextMeshProUGUI>().text = schoolData.description;
            int temp = i;
            upgradeItem.Find("EquipButton").GetComponent<Button>().onClick.AddListener(() => EquipSchool(temp));
            upgradeItem.Find("BuyButton").GetComponent<Button>().onClick.AddListener(() => BuySchool(temp));
            RefreshSchoolButton(i);
        }
    }

    void RefreshSchoolButton(int index)
    {
        Transform upgradeItem = schoolUpgradePanel.GetChild(index);
        if(CheckSchoolHaving(index) == true)
        {
            upgradeItem.Find("BuyButton").gameObject.SetActive(false);
            upgradeItem.Find("EquipButton").gameObject.SetActive(true);
            upgradeItem.Find("EquipButton").Find("Price").GetComponent<TextMeshProUGUI>().text = (equippedSchoolIndex == index) ? "장착 중" : "장착";

        }
        else
        {
            upgradeItem.Find("BuyButton").gameObject.SetActive(true);
            upgradeItem.Find("EquipButton").gameObject.SetActive(false);

            upgradeItem.Find("BuyButton").Find("Price").GetComponent<TextMeshProUGUI>().text = schoolUpgradeDatas[index].price + "명";
        }
    }
    bool CheckSchoolHaving(int index)
    {
        return schoolHavingList.Contains(index);
    }
    void BuySchool(int index)
    {
        if(CheckSchoolHaving(index) == true) return;
        if(TryUpgrade(schoolUpgradeDatas[index].price))
        {
            schoolHavingList.Add(index);
            RefreshSchoolButton(index);
        }
    }
    void EquipSchool(int index)
    {
        if(equippedSchoolIndex == index) return;
        if(CheckSchoolHaving(index) == true)
        {
            int prevIndex = equippedSchoolIndex;
            equippedSchoolIndex = index;
            StudentSpawner.Instance.SetSchool(schoolUpgradeDatas[index]);
            platformSpriteRenderer.sprite = schoolUpgradeDatas[index].platformSprite;
            fieldSpriteRenderer.sprite = schoolUpgradeDatas[index].fieldSprite;
            RefreshSchoolButton(index);

            if(prevIndex != -1)
            {
                RefreshSchoolButton(prevIndex);
            }
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
            if(prevIndex != -1)
            {
                RefreshWeaponButton(prevIndex);
            }
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
