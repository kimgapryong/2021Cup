using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PopUi : BaseUi, IBeginDragHandler, IDragHandler
{
    private Vector2 offset;

    public void OnBeginDrag(PointerEventData eventData)
    {

        offset = (Vector2)transform.position - eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {

        transform.position = eventData.position + offset;
    }
}
