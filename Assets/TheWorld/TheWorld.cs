using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using DG.Tweening;
using System.Collections;
using UnityEditor.Rendering.PostProcessing;
using UnityEngine.Rendering.PostProcessing;

namespace UnityStandardAssets.ImageEffects
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]

    public class TheWorld : PostEffectsBase { 

        public Vector2 center;
        [Range(0, 2)] public float radius;
        public Color impactColor;
        [Range(0, 2)] public float impactRadius;
        [Range(0, 2)] public float impactRadius1;
        public bool isGray;
        public float wave_intensity;
        public float wave_shape;
        public float twist_intensity;
        public float twist_speed;
        public float blur_intensity;
        public float blur_radius;
        public Shader colorShader = null;
        public Shader blurShader = null;
        private Material colorMaterial = null;
        private Material blurMaterial = null;
        public Texture2D tex;
        // you can set it to null to see how it works with the scene
        public Texture2D testImage = null;

        static public bool ifPause = false; // 游戏是否暂停

        // 暂停时的spbtm状态
        private GameObject spbtm;
        private Vector3 spbtm_speed;
        private Vector3 spbtm_rspeed;
        private Vector3 spbtm_pos;
        private Quaternion spbtm_rot;


        RenderTexture mySource;
        public override bool CheckResources()
        {
            CheckSupport(true);

            colorMaterial = CheckShaderAndCreateMaterial(colorShader, colorMaterial);
            blurMaterial = CheckShaderAndCreateMaterial(blurShader, blurMaterial);
            colorMaterial.SetTexture("_NoiseTex", tex);
            if (!isSupported)
                ReportAutoDisable();
            return isSupported;
        }

        IEnumerator TimeStop() {
            //long ugly brute animate
            DOTween.To(() => impactRadius, x => impactRadius = x, 2, 1f).SetEase(Ease.InCubic);
            yield return new WaitForSeconds(0.05f);
            DOTween.To(() => radius, x => radius = x, 2, 0.5f).SetEase(Ease.InCubic);
            yield return new WaitForSeconds(0.1f);
            DOTween.To(() => impactRadius1, x => impactRadius1 = x, 2, 0.5f).SetEase(Ease.InCubic);
            Tween t = DOTween.To(() => blur_intensity, x => blur_intensity = x, 1.5f, 0.2f).SetEase(Ease.InCubic);
            t.onComplete = delegate { DOTween.To(() => blur_intensity, x => blur_intensity = x, 0f, 0.2f).SetEase(Ease.InCubic); };
            yield return new WaitForSeconds(0.4f);
            Tween t1 = DOTween.To(() => blur_intensity, x => blur_intensity = x, 1.3f, 0.2f).SetEase(Ease.InCubic);
            t1.onComplete = delegate { DOTween.To(() => blur_intensity, x => blur_intensity = x, 0f, 0.2f).SetEase(Ease.InCubic); };
            yield return new WaitForSeconds(0.4f);
            isGray = true;
            Tween t2 = DOTween.To(() => blur_intensity, x => blur_intensity = x, 1.2f, 0.2f).SetEase(Ease.InCubic);
            t2.onComplete = delegate { DOTween.To(() => blur_intensity, x => blur_intensity = x, 0f, 0.2f).SetEase(Ease.InCubic); };
            yield return new WaitForSeconds(0.4f);
            Tween t3 = DOTween.To(() => blur_intensity, x => blur_intensity = x, 1.4f, 0.2f).SetEase(Ease.InCubic);
            t3.onComplete = delegate { DOTween.To(() => blur_intensity, x => blur_intensity = x, 0f, 0.2f).SetEase(Ease.InCubic); };
            DOTween.To(() => radius, x => radius = x, 0, 0.5f).SetEase(Ease.InCubic);
            DOTween.To(() => impactRadius, x => impactRadius = x, 0, 0.5f).SetEase(Ease.InCubic);
            DOTween.To(() => impactRadius, x => impactRadius = x, 0, 0.6f).SetEase(Ease.InCubic);
            Time.timeScale = 0;
        }


        IEnumerator RecordFrame()
        {
            yield return new WaitForEndOfFrame();
            //var texture = ScreenCapture.CaptureScreenshotAsTexture();
            testImage = ScreenCapture.CaptureScreenshotAsTexture();
        }

        public IEnumerator ScreenCaptureGame(Rect rect/*, string fileName*/)
        {
            yield return new WaitForEndOfFrame();//等到帧结束，不然会报错  
            Texture2D tex = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.ARGB32, false);//新建一个Texture2D对象  
            tex.ReadPixels(rect, 0, 0);//读取像素，屏幕左下角为0点  
            tex.Apply();//保存像素信息  
            testImage = tex;

            //byte[] bytes = tex.EncodeToPNG();//将纹理数据，转化成一个png图片  
            //System.IO.File.WriteAllBytes(fileName, bytes);//写入数据  
            //Debug.Log(string.Format("截取了一张图片: {0}", fileName));
        }

        void Awake (){
            if (testImage) {
                mySource = new RenderTexture(testImage.width / 2, testImage.height / 2, 0);
                RenderTexture.active = mySource;
                Graphics.Blit(testImage, mySource);
            }        
            //StartCoroutine(TimeStop());
           
        }

        private void LateUpdate()
        {
            
            if (Input.GetKeyDown(KeyCode.P))
            {
                ifPause = !ifPause;
                if (ifPause == true)
                {
                    testImage = ScreenCapture.CaptureScreenshotAsTexture(0);
                    mySource = new RenderTexture(testImage.width / 2, testImage.height / 2, 0);
                    RenderTexture.active = mySource;
                    Graphics.Blit(testImage, mySource);
                    StartCoroutine(TimeStop());
                    spbtm = GameObject.Find("spbtm");
                    spbtm_pos = spbtm.transform.position;
                    spbtm_rot = spbtm.transform.rotation;
                    spbtm_speed = spbtm.GetComponent<Rigidbody>().velocity;
                    spbtm_rspeed = spbtm.GetComponent<Rigidbody>().angularVelocity;
                    Debug.Log("velocity: " + spbtm_speed);
                    Debug.Log("angularvelocity: " + spbtm_rspeed);
                }
                else
                {
                    testImage = null;
                    isGray = false;
                    Time.timeScale = 1;
                    spbtm = GameObject.Find("spbtm");
                    spbtm.transform.position = spbtm_pos;
                    spbtm.transform.rotation = spbtm_rot;
                    Debug.Log("now, velocity: " + spbtm_speed);
                    spbtm.GetComponent<Rigidbody>().velocity = spbtm_speed * 4;
                    spbtm.GetComponent<Rigidbody>().angularVelocity = spbtm_rspeed;
                }
            }
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (CheckResources() == false)
            {
                Graphics.Blit(source, destination);
                return;
            }

            blurMaterial.SetFloat("_SampleStrength", blur_intensity);
            blurMaterial.SetFloat("_SampleDist", blur_radius);
            blurMaterial.SetFloat("_CenterX", center.x);
            blurMaterial.SetFloat("_CenterY", center.y);
            
           

            colorMaterial.SetFloat("_Radius", radius);
            colorMaterial.SetColor("_ImpactColor", impactColor);
            colorMaterial.SetFloat("_ImpactRadius", impactRadius);
            colorMaterial.SetFloat("_ImpactRadius1", impactRadius1);
            colorMaterial.SetFloat("_CenterX", center.x);       
            colorMaterial.SetFloat("_CenterY", center.y);
            colorMaterial.SetFloat("_TwistIntensity", twist_intensity);
            colorMaterial.SetFloat("_TwistSpeed", twist_speed);
            colorMaterial.SetFloat("_WaveIntensity", wave_intensity);
            colorMaterial.SetFloat("_WaveShape", wave_shape);
            float gv = isGray ? 1 : 0;
            colorMaterial.SetFloat("_Gray", gv);
            RenderTexture rt = RenderTexture.GetTemporary(source.width, source.height);
            if (testImage)
            {
                Graphics.Blit(mySource, rt, colorMaterial);
            }
            else {
                Graphics.Blit(source, rt, colorMaterial);
            }
            Graphics.Blit(rt, destination, blurMaterial);
            RenderTexture.ReleaseTemporary(rt);
        }
    }
}