using UnityEngine;
using UnityEngine.UI;


// 장비 선택 버튼
public class EquipBtn : MonoBehaviour
{
    [SerializeField] private GameObject _lockedPanel;
    [SerializeField] private GameObject _equippedText;
    [SerializeField] private Button _btn;
    [SerializeField] private Image _equipmentSprite;
    private EquipmentData _equipment;

    // 아이콘 설정하기
    public void SetIcon(EquipmentData equipment)
    {
        _equipment = equipment;
        _equipmentSprite.sprite = equipment.EquipSprite;
        _btn.onClick.AddListener(Equipped);
        RefreshIcon(equipment);
    }

    // 장착, 획득 여부 새로 고침
    public void RefreshIcon(EquipmentData equipment)
    {
        _lockedPanel.SetActive(!equipment.Get);
        _equippedText.SetActive(equipment.Equipped);
    }

    // 장착하기
    private void Equipped()
    {
        GameManager.Instance.Installing.Invoke(_equipment);
    }
}
