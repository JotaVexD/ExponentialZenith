using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotChests : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler,IPointerExitHandler,IBeginDragHandler,IEndDragHandler,IDragHandler,IDropHandler
{
    [SerializeField] Image image;

    public event Action<SlotChests> OnPointerEnterEvent;
    public event Action<SlotChests> OnPointerClickEvent;
    public event Action<SlotChests> OnPointerExitEvent;
    public event Action<SlotChests> OnRightClickEvent;
    public event Action<SlotChests> OnBeginDragEvent;
    public event Action<SlotChests> OnEndDragEvent;
    public event Action<SlotChests> OnDragEvent;
    public event Action<SlotChests> OnDropEvent;

    private Color normalColor = Color.white;
    private Color disabledColor = new Color(0,0,0,0);

    private Item _item;
    public Item Item{
        get{return _item; }
        set{
            _item = value;
            if(_item == null)
            {
                image.color = disabledColor;
                image.sprite = null;

            }else{
                image.sprite = _item.Icon;
                image.color = normalColor;
            }
        }
    }

    protected virtual void OnValidate() {
        if(image == null){
            image = GetComponent<Image>();
        }

    }

    public virtual bool CanReceiveItem(Item item){
        return true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData != null && eventData.button == PointerEventData.InputButton.Right && image.sprite != null){
            if(OnRightClickEvent != null){
                OnRightClickEvent(this);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(OnPointerEnterEvent != null){
            OnPointerEnterEvent(this);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(OnPointerExitEvent != null){
            OnPointerExitEvent(this);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(OnBeginDragEvent != null){
            OnBeginDragEvent(this);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
         if(OnEndDragEvent != null){
            OnEndDragEvent(this);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
         if(OnDragEvent != null){
            OnDragEvent(this);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
         if(OnDropEvent != null){
            OnDropEvent(this);
        }
    }
}
