using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvSlotController : MonoBehaviour
{

    private GardenManager gardenManager;
    public bool isDragging;
    public int slotNum;
    public int itemID;
    public GameObject image;
    private Vector3 originalPos;
    public GameObject collidedSlot;
    public Sprite blankSlot;

    void Start(){
        gardenManager = transform.root.GetComponent<GardenManager>();
    }
    void Update(){
        if(isDragging){
            image.gameObject.transform.position = Input.mousePosition;
        }
    }
    public void Dragging(){
        originalPos = image.gameObject.transform.position;
        Debug.Log("Dragging");
        isDragging = true;
    }

    public void Clicked(){
        Debug.Log("Clicked once");
        gameObject.transform.parent.GetComponent<Button>().onClick.Invoke();
    }

    public void LetGo(){
        Debug.Log("not dragging");
        isDragging = false;
        if(collidedSlot != null){
            InvSlotController slot = collidedSlot.GetComponent<InvSlotController>();
            if(slot.slotNum != slotNum){
                if(slot.itemID == 0){
                    Debug.Log("Moving item");
                    gardenManager.dataManager.player.inventory[slot.slotNum] = itemID;
                    gardenManager.dataManager.player.inventory[slotNum] = 0;
                } else {
                    Debug.Log("Switching");
                    gardenManager.dataManager.player.inventory[slot.slotNum] = itemID;
                    gardenManager.dataManager.player.inventory[slotNum] = slot.itemID;
                }
                gardenManager.dataManager.SaveAll();
                gardenManager.ReloadInventory();
            }
        }
        ResetPos();
    }

    public void ResetPos(){
        gameObject.transform.position = originalPos;
    }
    void OnCollisionStay(Collision col){
        Debug.Log("Entered: " + col.gameObject.GetComponent<InvSlotController>().slotNum);
        if(!col.gameObject.GetComponent<InvSlotController>().isDragging){
            collidedSlot = col.gameObject;
        }
    }

    void OnCollisionExit(Collision col){
        Debug.Log("Leaving: " + col.gameObject.GetComponent<InvSlotController>().slotNum);
        collidedSlot = null;
        Debug.Log("Exited");
    }
}
