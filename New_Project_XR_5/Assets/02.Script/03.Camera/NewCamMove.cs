//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//namespace Manager
//{
//    public class NewCamMove : MonoBehaviour
//    {
//        protected NewCamMove() {}
//        public Camera C_Camera;
//        public Canvas ApplyAllTimeCanvas;

//        [Range(0,10)]
//        public List<float> TriggerAreaMoveSpeed = new List<float>();
        
//        public int MovedModApplyLevel;
//        [Range(0, 5)]
//        public float MaxLookUpDistance;

//        [Range(0, 5)]
//        public float MaxLookDownDistance;

//        [Range(0, 5)]
//        public float LookSpeed;

//        Rigidbody2D ObjectRigidBody;
//        Vector2 Range;
//        Vector2 LerpPos;
//        Vector3 LerpPosMargin;
//        public float PlayerLookVec;
//        public float PortalLerpSpeed;
//        [Header("")]
//        public bool EnableL;
//        public bool EnableR;
//        public bool EnableB;
//        public bool EnableT;
//        [Header("")]
//        public bool WaitLerp;
//        [Header("")]
//        public bool LookUp;
//        public bool LookDown;
//        public bool OnDelay;
//        public bool GamePlayMod;
//        public bool DoZoom;
//        public bool LoadAllSceneWait;
        
//        public Gauge DelayView;

//        public bool WaitFaidFilter;
//        public GameObject FilterObj;
//        public Image FaidFilter;
//        public Color TargetColor;

//        public float TraceVal;
//        public Vector2 MoveMentSpeed;



//        void Start()
//        {
//            EnableL = true;
//            EnableR = true;
//            EnableB = true;
//            EnableT = true;
//            PlayerLookVec =0;
//            //FilterObj.transform.localPosition=new Vector2(0,0);
//        }
//        public void startDelay()
//        {
//            OnDelay=true;
//        }
//        public void RelaseDelay()
//        {
//            OnDelay=false;
//            DelayView.Val = DelayView.Max;
//        }
//        void Update()
//        {
//            if(GamePlayMod)
//            {
//                MoveToCameraActionPlayer();
//                if(WaitLerp)
//                {
//                    transform.position = Vector3.Lerp(transform.position,LerpPos,PortalLerpSpeed*Time.deltaTime);
//                    if(Vector2.Distance(transform.position, LerpPos) < 1)
//                    {
//                        WaitLerp=false;
//                    }
//                }
//                else
//                {
                    
//                Vector2 PlayerPos = Player.Inst.curPlayer.transform.position;

//                Vector2 TempTargetVector = Vector2.Lerp(transform.position, PlayerPos,Time.deltaTime);
//                //Vector2 TempTargetVector =Vector2.SmoothDamp(transform.position,PlayerPos,ref MoveMentSpeed,TriggerAreaMoveSpeed[MovedModApplyLevel]);
//                TempTargetVector = (Vector2)transform.position - TempTargetVector;

//                Vector2 RealAddTargetVector = Vector2.zero;
//                MoveMentSpeed = Vector2.zero;

                

//                    if(MovedModApplyLevel ==4)
//                    {
                    
//                    }

//                    if (EnableB && TempTargetVector.y >0)
//                    {
//                        RealAddTargetVector += new Vector2(0,TempTargetVector.y);
//                    }
//                    else if ( EnableT && TempTargetVector.y <0)
//                    {
//                        RealAddTargetVector += new Vector2(0,TempTargetVector.y);
//                    }


//                    if (EnableR && TempTargetVector.x <0)
//                    {
//                        RealAddTargetVector += new Vector2(TempTargetVector.x,0);
//                    }
//                    else if (EnableL && TempTargetVector.x >0)
//                    {
//                        RealAddTargetVector += new Vector2(TempTargetVector.x,0);
//                    }

//                    RealAddTargetVector *=  (TriggerAreaMoveSpeed[MovedModApplyLevel] + TraceVal);

//                TempTargetVector = Vector2.Lerp(transform.position,(Vector2)transform.position+ RealAddTargetVector ,
//                Time.deltaTime * TriggerAreaMoveSpeed[MovedModApplyLevel]);

//                transform.Translate(-RealAddTargetVector );
//                if(!Manager.Area.Inst.WaitNullLayer)
//                            Area.Inst.Target.ScrollLayer(-RealAddTargetVector);

//                }
                
                

//            }


//        }

//        void MoveToCameraActionPlayer()
//        {
//        if(OnDelay)
//        {
//            DelayView.Val -=Time.deltaTime;
//        }

//        if(DelayView.Val <0)
//        {
//            if(LookUp)
//            {
//                if(PlayerLookVec< MaxLookDownDistance)
//                {
//                    PlayerLookVec += Time.deltaTime * LookSpeed;
                    
//                }
//                if(Manager.Player.Inst.curPlayer.GetComponent<CharacterBased>().mDoAction != ACTION.LOOK_UP)
//                        LookUp=false;
                    
//            }
//        else if(LookDown)
//        {
//            if(PlayerLookVec> -MaxLookDownDistance)
//            {
//                PlayerLookVec -= Time.deltaTime * LookSpeed;
                
//            }
//            if(Manager.Player.Inst.curPlayer.GetComponent<CharacterBased>().mDoAction != ACTION.LOOK_DOWN)
//                        LookDown=false;
                        
//        }
//        }

//        if(!LookDown && !LookUp)
//        {
//            if(PlayerLookVec> 0.05)
//            {
//                PlayerLookVec -= Time.deltaTime * LookSpeed;
//            }
//            else if(PlayerLookVec< -0.05)
//            {
//                PlayerLookVec += Time.deltaTime * LookSpeed;
//            }
//            else
//            {
//                PlayerLookVec=0;
//            }

//        }
//        if(C_Camera)
//            C_Camera.transform.localPosition =  new Vector3(0,PlayerLookVec,C_Camera.transform.localPosition.z) ;
//        }

//        public void AddCamera(Camera newCamera)
//        {
//            if(C_Camera != null)
//            {
//                C_Camera.transform.SetParent(null);
//                Camera Temp = C_Camera;
//            }
//            newCamera.transform.SetParent(gameObject.transform);
//            C_Camera = newCamera;
//            newCamera.transform.localPosition = new Vector3(0,0,newCamera.transform.localPosition.z);
//            MovedModApplyLevel=3;
//            ApplyAllTimeCanvas.worldCamera = C_Camera;
//        }



//        public void SetLerpMove(Vector2 Pos)
//        {
//            LerpPos = Pos;
//            WaitLerp=true;
//        }
//        public void DoFaidOut(float SetTime,Color targetColor,string Target,int NextPosIdx)
//        {
//            StartCoroutine(SetFaidOutMoveScene(SetTime,targetColor,Target,NextPosIdx));
//        }
//        public void DoFaidIn(float SetTime,Color targetColor)
//        {
//            StartCoroutine(SetFaidInMoveScene(SetTime,targetColor));
//        }


//        IEnumerator SetFaidOutMoveScene(float SetTime,Color targetColor,string Target,int NextPosIdx)
//        {
//            if (DebugSettingManagers.Inst.IsSettingMod)
//            {
//                DebugSettingManagers.Inst.ToggleOnEditDebugMod();
//            }
//            GameObject TempCam=null;
//            Manager.NewCamMove.Inst.WaitFaidFilter=true;
//            if( Manager.NewCamMove.Inst.C_Camera != null)
//            {
//                TempCam = Manager.NewCamMove.Inst.C_Camera.gameObject;
//                Destroy(TempCam);
//            }
//            WaitFaidFilter=true;
//            TargetColor.a=0;
//            FilterObj.GetComponent<Canvas>().enabled=true;
            
//            float FadeAlpha =0 ;
//            float AlphaSpeed = 1 / SetTime;
//            FaidFilter.color = TargetColor;
//            Manager.Scene.Inst.PreLoadNextScene(Target,NextPosIdx);
//            while(FadeAlpha<1)
//            {
//                TargetColor.a = FadeAlpha;
//                FaidFilter.color = TargetColor;
//                yield return null;
//                FadeAlpha +=Time.deltaTime * AlphaSpeed;
//            }
//            WaitFaidFilter=false;
//            LoadAllSceneWait=false;
            

//        }
//        IEnumerator SetFaidInMoveScene(float SetTime,Color targetColor)
//        {
            
//            WaitFaidFilter=true;
//            TargetColor.a=1;
//            float FadeAlpha =1;
//            float AlphaSpeed = 1 / SetTime;
//            while(FadeAlpha>0)
//            {
//                TargetColor.a = FadeAlpha;
//                FaidFilter.color = TargetColor;
//                yield return null;
//                FadeAlpha -= AlphaSpeed * Time.deltaTime;
//            }
//            TargetColor.a=0;
//            FaidFilter.color = TargetColor;
//            FilterObj.GetComponent<Canvas>().enabled=false;
//            WaitFaidFilter=false;
//            Manager.Player.Inst.curPlayerScript.mEnabledGameObject = true;
        
//        }

//        public void DoForcedFaidInOnly(float SetTime)
//        {
//            StartCoroutine(ForcedFaidIn(SetTime,Color.black));
//        }
//        IEnumerator ForcedFaidIn(float SetTime,Color targetColor)
//        {
//            WaitFaidFilter=true;
//            TargetColor.a=1;
//            float FadeAlpha =1;
//            float AlphaSpeed = 1 / SetTime;
//            while(FadeAlpha>0)
//            {
//                TargetColor.a = FadeAlpha;
//                FaidFilter.color = TargetColor;
//                yield return null;
//                FadeAlpha -= AlphaSpeed * Time.deltaTime;
//            }
//            TargetColor.a=0;
//            FaidFilter.color = TargetColor;
//            FilterObj.GetComponent<Canvas>().enabled=false;
//            WaitFaidFilter=false;
//        }

//        public void DoForcedFaidOutOnly(float SetTime)
//        {
//            StartCoroutine(ForcedFaidOutOnly(SetTime,Color.black));
            
//        }
//        IEnumerator ForcedFaidOutOnly(float SetTime,Color targetColor)
//        {
//            TargetColor.a=0;
            
//            FilterObj.GetComponent<Canvas>().enabled=true;
//            float FadeAlpha =0 ;
//            float AlphaSpeed = 1 / SetTime;
//            FaidFilter.color = TargetColor;
//            while(FadeAlpha<1)
//            {
//                TargetColor.a = FadeAlpha;
//                FaidFilter.color = TargetColor;
//                yield return null;
//                FadeAlpha +=Time.deltaTime * AlphaSpeed;
//            }
//            WaitFaidFilter=false;
            
//        }

//        public List<UIMoveMent> MoveMent;

//        public void ZoomOn()
//        {
//            foreach(UIMoveMent A in MoveMent)
//            {
//                if(A!=null && A.gameObject.activeInHierarchy !=false)
//                    A.MoveOn();
//            }
//        }
        
//        public void ZoomOff()
//        {
//            foreach(UIMoveMent A in MoveMent)
//            {
//                if(A!=null && A.gameObject.activeInHierarchy !=false)
//                    A.MoveOff();
//            }
//        }
//        public void ZoomOffForce()
//        {
//            foreach(UIMoveMent A in MoveMent)
//            {
//                if(A!=null && A.gameObject.activeInHierarchy !=false)
//                    A.ForcedMoveOff();
//            }
//        }

//        IEnumerator CalculateZoom(float CamOriginSize)
//        {
//            while(DoZoom)
//            {
//                    if(C_Camera!=null)
//                    {
//                        if(C_Camera.transform.GetChild(1).GetComponent<Camera>().orthographicSize != CamOriginSize)
//                            ZoomOn();
//                    }
//                    else
//                    {
//                        ZoomOff();
//                    }
                
//                    yield return null;
//            }
//            ZoomOffForce();
//            yield return null;
//        }

//        public void SetIsDoZoom(bool val,float CamOriginSize=10){
//            DoZoom=val;
//            if(DoZoom){
//                StartCoroutine(CalculateZoom(CamOriginSize));
//            }

//        }




//    }
//}