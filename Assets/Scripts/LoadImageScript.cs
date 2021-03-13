using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadImageScript : MonoBehaviour
{
    // テクスチャをマテリアル化するので生成しておく
    [SerializeField] Material material;

    // 表示時間の配列のインデックス
    int index = 0;
    int indexMax = 4;

    // 画像ファイルの配列
    string[] imageFilePathes 
        = { "shoshinge01_01.png", "shoshinge01_02.png", 
        "shoshinge01_03.png", "shoshinge01_04.png" };

    // 表示時間の配列
    float[] displayTimes = {15.0F, 12.0F, 10.0F, 12.0F};

    // 表示の開始時刻
    float displayStartTime = 0.0F;

    // 画像表示速度の倍率
    float displayFactor = 1.5F;

    // Start is called before the first frame update
    void Start()
    {
        // マテリアルのシェーダを変更する。
        string shader = "UI/Lit/Transparent";
        material.shader = Shader.Find(shader);

        // マテリアルを設定する。
        SetMaterial(index);

        // 表示の開始時刻を初期化する。
        displayStartTime = Time.time;

    }

    // Update is called once per frame
    void Update()
    {
        float time = Time.time - displayStartTime;

        // 表示時間を越えたら、次の画像に変更する。
        if (time > displayTimes[index])
        {
            displayStartTime += displayTimes[index];
            time = 0.0F;
            index++;
            if (index >= indexMax) {
                index = 0;
            }
            // マテリアルを設定する。
            SetMaterial(index);
        }


        float sinVal = Mathf.Sin(time / displayTimes[index] * Mathf.PI * displayFactor) / 2 + 0.2F;
        float cosVal = -Mathf.Cos(time / displayTimes[index] * Mathf.PI * displayFactor) / 2 + 0.5F;

        // 透明度を設定
        Color color = new Color(0, 0, 0, sinVal);
        material.color = color;

        // transformを取得
        Transform transform = this.transform;
        
        // 座標を設定
        Vector3 pos = transform.position;
        pos.x = 0;
        pos.y = sinVal + 0.9F;
        pos.z = cosVal * 1.5F;
        transform.position = pos;  

        // スケールを設定
        Vector3 localScale = transform.localScale;
        localScale.x = 0.01F * cosVal;
        localScale.y = 0.07F * cosVal;
        localScale.z = 0.1F * cosVal;
        transform.localScale = localScale;

    }

    /// <summary>
    /// テクスチャを読み込む。
    /// </summary>
    /// <param name="path">画像ファイルのパス</param>
    /// <returns>テクスチャ</returns>
    Texture2D PngToTexture2D(string path)
    {
        BinaryReader bin = new BinaryReader(new FileStream(path, FileMode.Open, FileAccess.Read));
        byte[] rb = bin.ReadBytes((int)bin.BaseStream.Length);
        bin.Close();
        int pos = 16, width = 0, height = 0;
        for (int i = 0; i < 4; i++) width = width * 256 + rb[pos++];
        for (int i = 0; i < 4; i++) height = height * 256 + rb[pos++];
        Texture2D texture = new Texture2D(width, height);
        texture.LoadImage(rb);
        return texture;
    }

    /// <summary>
    /// 画像を読み込みマテリアルに設定する。
    /// </summary>
    /// <param name="index">画像のインデックス</param>
    void SetMaterial(int index)
    {
        // テクスチャを読み込む。
        Texture2D texture = PngToTexture2D(Application.dataPath + "/Resources/Images/" + imageFilePathes[index]);

        // テクスチャをマテリアルにセットする。
        material.SetTexture("_MainTex", texture);

        gameObject.GetComponent<Renderer>().material = material;
    }
}
