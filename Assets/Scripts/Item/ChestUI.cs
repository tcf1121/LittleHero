using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


// 상자깡 UI 관리 코드
public class ChestUI : MonoBehaviour
{
    [SerializeField] private Button _chestBtn;
    [SerializeField] private TMP_Text _chestNum;
    [SerializeField] private GameObject _itemObj;
    [SerializeField] private Image _itemImage;
    private Coroutine _showItemCor;

    void Start()
    {
        _showItemCor = null;
        _chestBtn.onClick.AddListener(OpenChest);
    }

    void OnEnable()
    {
        RefreshNum();
    }

    // 개수 새로고침
    private void RefreshNum()
    {
        _chestNum.text = "X" + GameManager.Instance.Chest.GetChestNum();
    }

    // 상자 열기 (클릭 할 때 실행)
    private void OpenChest()
    {
        if (GameManager.Instance.Chest.CheckChestNum())
        {
            GameManager.Instance.Chest.UseChestNum();
            EquipmentData getItem = GameManager.Instance.Chest.OpenChest();

            _itemImage.sprite = getItem.EquipSprite;
            RefreshNum();
            _showItemCor = StartCoroutine(ShowItemCorutine());
        }
    }

    private IEnumerator ShowItemCorutine()
    {
        _itemObj.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        _itemObj.SetActive(false);

        StopCoroutine(_showItemCor);
        _showItemCor = null;
    }
}
