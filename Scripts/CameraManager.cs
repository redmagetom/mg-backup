using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    public GameObject gardenOverlay;
    public MainManager mainManager;
    public Camera mainCam;
    public Camera gardenCam;
    public GameObject gardenHolder;
    public GardenManager gardenManager;
    private Vector3 originalPos;
    private Quaternion originalRot;
 
    [Header("Camera Controls")]
    public bool gardenActive;
    public Camera cam;
    float MouseZoomSpeed = 15.0f;
    float TouchZoomSpeed = 0.1f;
    float ZoomMinBound = 1.6f;
    float ZoomMaxBound = 50f;
    public float speed = 400f;
    private bool dragging;
    private Vector3 initPos;
    private float rotX;
    private float rotY;
    private float dir;
    private Vector3 savedPos;
    private bool posSaved;
    private bool canMove;
    private Vector3 measureFrom;
    public Vector3 savedCamRot;
    public Vector3 savedCamPos;
    public GameObject readout;
    public string readText;

    void Start(){
        originalPos = gardenCam.transform.position;
        originalRot = gardenCam.transform.rotation;
        cam = gardenCam;
        dir = -1;
        // cam.transform.LookAt(gardenManager.currentIsland.transform);
    }

    void Update(){
        // readout.GetComponent<Text>().text = readText;
        if(!gardenManager.windowOpen){
            if(gardenActive){
                if(gardenManager.objectToAdjust == null){
                    savedCamRot = cam.transform.rotation.eulerAngles;
                    savedCamPos = cam.transform.position;
                    measureFrom = gardenManager.currentIsland.transform.position;
                } else {
                    measureFrom = gardenManager.objectToAdjust.transform.position;
                }   
            }
    
            if(Input.GetMouseButton(0) && gardenActive && !gardenManager.planting && !gardenManager.placingPot){
                if(!dragging){
                    initPos = Input.mousePosition;
                    dragging = true;
                    canMove = true;
                }
        
            if(Input.touchSupported){
                if(Input.touchCount == 1){
                    Vector3 pos = Input.mousePosition;
                    float dragDistance = Vector3.Distance(initPos, pos);
                    float hCenter = Screen.width/2;
                    if(pos.x < initPos.x){
                        dragDistance *= -1;
                    }
                    float camKeepY = cam.transform.position.y;
                    if(dragDistance > 0){
                        cam.transform.RotateAround(measureFrom, cam.transform.up, dragDistance * 0.005f);
                        cam.transform.position = new Vector3(cam.transform.position.x, camKeepY, cam.transform.position.z);
                    } else {
                        cam.transform.RotateAround(measureFrom, cam.transform.up, dragDistance * 0.005f);
                        cam.transform.position = new Vector3(cam.transform.position.x, camKeepY, cam.transform.position.z);
                    }
                    
                }
            } else {
                Vector3 pos = Input.mousePosition;
                float dragDistance = Vector3.Distance(initPos, pos);
                float hCenter = Screen.width/2;
                if(pos.x < initPos.x){
                    dragDistance *= -1;
                }
                int layerMask = 1 << LayerMask.NameToLayer("Soil");
                int layerMask2 = 1 << LayerMask.NameToLayer("Island");
                float xDrag = pos.x - initPos.x;
                float yDrag = pos.y - initPos.y;
              
                // if(Mathf.Abs(xDrag) > Mathf.Abs(yDrag)){
                //     Vector3 camMove = cam.transform.right;
                //     camMove.y = 0;
                //     cam.transform.Translate(camMove * (yDrag * 0.01f), Space.World);
                    // RaycastHit hit;
                    // Ray ray = cam.ScreenPointToRay(new Vector2(Screen.width/2, Screen.height/2));
                    // int layerMask = 1 << LayerMask.NameToLayer("Soil");
                    // if(Physics.Raycast(ray, out hit, 1000f, ~layerMask)){
                    //     // Debug.Log("hit");
                    //     cam.transform.RotateAround(hit.point, Vector3.up, dragDistance * 0.005f);
                    // }
                // } else {



        
                    Vector3 totalVecCheck = new Vector3(cam.transform.position.x - (xDrag * 0.05f) , cam.transform.position.y, cam.transform.position.z - (yDrag * 0.05f));

                    RaycastHit checkHit;
                    if(Physics.Raycast(totalVecCheck, Vector3.down, out checkHit, 1000f, ~layerMask | ~layerMask2)){
        
                        float hitD1 = 0;
                        float hitD2 = 0;
    
                        RaycastHit hit;
                        Vector3 shootVec = new Vector3(cam.transform.position.x, cam.transform.position.y, cam.transform.position.z);
                
                        if(Physics.Raycast(shootVec, Vector3.down, out hit, 1000f, ~layerMask | ~layerMask2)){ 
                            // Debug.Log("hitting " + hit.transform.gameObject.name);
                            hitD1 = hit.distance;

                            Vector3 camMove1 = -cam.transform.up;
                            Vector3 camMove2 = -cam.transform.right;

                            camMove1.y = 0;
                            camMove2.y = 0;

                            cam.transform.Translate(camMove1 * (yDrag * 0.02f), Space.World);
                            cam.transform.Translate(camMove2 * (xDrag * 0.01f), Space.World);

                            RaycastHit hit2;
                            Vector3 shootVec2 = new Vector3(cam.transform.position.x, cam.transform.position.y, cam.transform.position.z);
                            if(Physics.Raycast(shootVec2, Vector3.down, out hit2, 1000f, ~layerMask | ~layerMask2)){
                                hitD2 = hit2.distance;
                                float diff = (hitD1 - hitD2);
                                if(Mathf.Abs(diff) > 0 && Mathf.Abs(diff) < 1){
                                    cam.transform.position = (new Vector3(cam.transform.position.x, cam.transform.position.y + diff, cam.transform.position.z));
                                }
                            }
                        }

                  
                    } 
                // }

                
            }
            
            }

            if(Input.GetMouseButtonUp(0) && gardenActive){
                savedPos = cam.transform.position;
                dragging = false;
                posSaved = false;
            }

            if(Input.GetKey(KeyCode.LeftShift)){
                // Debug.Log("Zooming");
                ZoomIn();
            } 

            if(Input.GetKey(KeyCode.LeftControl)){
                // Debug.Log("ZoomingOUt");
                ZoomOut();
            }

            if(Input.touchSupported){
                if(Input.touchCount == 2){
                    Touch tZero = Input.GetTouch(0);
                    Touch tOne = Input.GetTouch(1);
    
                    Vector2 tZeroPrevious = tZero.position - tZero.deltaPosition;
                    Vector2 tOnePrevious = tOne.position - tOne.deltaPosition;

                    float oldTouchDistance = Vector2.Distance (tZeroPrevious, tOnePrevious);
                    float currentTouchDistance = Vector2.Distance (tZero.position, tOne.position);

                    float difference = currentTouchDistance - oldTouchDistance;
                    // readText = ("1: " + oldTouchDistance.ToString() + " --- 2: " + currentTouchDistance.ToString());
                    Vector3 direction = cam.transform.position - measureFrom;
                
                    if(difference > 3){
                        ZoomIn();
                    } else if(difference < -3){
                        ZoomOut();
                    }              
                }
            }   
        }
      
    }

    public void ZoomIn(){
        // Debug.Log("zooming");
        // float offset = 60;
        // if(gardenManager.objectToAdjust != null){
        //     offset = 20;
        // } else {
        //     offset = 60;
        // }
        Quaternion camRot = cam.transform.rotation;
        
        RaycastHit hit;
        Vector3 shootVec = new Vector3(cam.transform.position.x, cam.transform.position.y, cam.transform.position.z);
        int layerMask = 1 << LayerMask.NameToLayer("Soil");
        if(Physics.Raycast(shootVec, Vector3.down, out hit)){
            // Debug.Log(hit.distance);
            if(hit.distance > 3){
                cam.transform.Translate(cam.transform.forward * 3);
            }
        }
        
        // if(Vector3.Distance(measureFrom, cam.transform.position) > offset){
        //     cam.transform.position = Vector3.MoveTowards(cam.transform.forward, measureFrom, speed);
        //     // cam.transform.LookAt(measureFrom);
        //     cam.transform.rotation = camRot;
        // } else {
        //     cam.transform.position = Vector3.Slerp(cam.transform.forward, measureFrom, -speed);
        //     // cam.transform.LookAt(measureFrom);
        //     cam.transform.rotation = camRot;
        // }
    }

    public void ZoomOut(){
        float offset = 300;
        Quaternion camRot = cam.transform.rotation;
        cam.transform.Translate(-cam.transform.forward * 3);
        // if(Vector3.Distance(measureFrom, cam.transform.position) < offset){
        //     cam.transform.position = Vector3.MoveTowards(cam.transform.position, measureFrom, -speed);
        //     cam.transform.LookAt(measureFrom);
        //     cam.transform.rotation = camRot;
        // } else {
        //     cam.transform.position = Vector3.Slerp(cam.transform.position, measureFrom, -speed);
        //     cam.transform.LookAt(measureFrom);
        //     cam.transform.rotation = camRot;
        // }
    }

    public void ResetObjectCam(){
        LeanTween.move(cam.gameObject, savedCamPos, 1f);
        LeanTween.rotate(cam.gameObject, savedCamRot, 1f);
        // cam.transform.localPosition = savedCamPos;
        // cam.transform.localRotation = Quaternion.Euler(savedCamRot);
    }
    public void SwitchCamera(){
        StartCoroutine(I_SwitchCamera());
    }

    IEnumerator I_SwitchCamera(){
        if(mainCam.gameObject.activeSelf){
            gardenOverlay.SetActive(true);
            Animation anims = gardenOverlay.GetComponent<Animation>();
            yield return anims.Play("overlayFadeOut");
            mainManager.mainPagePanel.SetActive(false);
            gardenHolder.SetActive(true);
            gardenCam.gameObject.SetActive(true);
            mainCam.gameObject.SetActive(false);
            gardenManager.LoadAllExisting();
            StartCoroutine(StartFade(mainManager.GetComponent<AudioSource>(), 2f, 0));
            StartCoroutine(StartFade(gardenManager.wind, 2f, .03f));
            cam = gardenCam;
            gardenActive = true;
        } else {
            gardenActive = false;
            mainManager.soundPlayer.GetComponent<AudioSource>().PlayOneShot(mainManager.confirmChime1);
            gardenOverlay.SetActive(true);
            Animation anims = gardenOverlay.GetComponent<Animation>();
            anims.PlayQueued("overlayFinalFade");
            yield return StartCoroutine(WaitForAnimation(anims));
            mainCam.gameObject.SetActive(true);
            gardenCam.gameObject.SetActive(false);
            StartCoroutine(StartFade(mainManager.GetComponent<AudioSource>(), 2f, .08f));
            StartCoroutine(StartFade(gardenManager.wind, 2f, 0));
            // Animation anims = gardenOverlay.GetComponent<Animation>();
            // anims.PlayQueued("overlayFadeIn");
            // StartCoroutine(WaitForAnimation(anims));
            
            // mainManager.mainPagePanel.SetActive(true);
            // gardenManager.zoomOutButton.SetActive(false);
            gardenHolder.SetActive(false);
            // gardenCam.gameObject.SetActive(false);
            // mainCam.gameObject.SetActive(true);
            mainManager.LoadMainScreen();
        }
    }

    // IEnumerator FadeMusic(float finalVal){
    //     while(mainManager.GetComponent<AudioSource>().volume != finalVal){
    //         if(mainManager.GetComponent<AudioSource>().volume > finalVal){
    //             mainManager.GetComponent<AudioSource>().volume = (mainManager.GetComponent<AudioSource>().volume - 0.001f);
    //         } else {
    //             mainManager.GetComponent<AudioSource>().volume = (mainManager.GetComponent<AudioSource>().volume + 0.001f);
    //         }
    //     }
    //     yield return null;
    // }


    public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }

    public void WholeGardenView(){
        gardenManager.zoomOutButton.SetActive(false);
        gardenManager.backToMainButton.SetActive(true);
        LeanTween.move(gardenCam.gameObject, originalPos, 1f);
        LeanTween.rotate(gardenCam.gameObject, originalRot.eulerAngles, 1f);
        gardenManager.selectedPot = null;
        Destroy(gardenManager.plantToPlant);
        Destroy(gardenManager.potToPlace);
        gardenManager.planting = false;
    }

    private IEnumerator WaitForAnimation ( Animation animation ){
        do { yield return null; } while ( animation.isPlaying );
    }



}
