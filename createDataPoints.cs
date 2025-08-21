//using System;
//using UnityEngine;


//public class createDataPoints : MonoBehaviour
//{
//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {
//        for (int i = 0; i < 10; i++) //todo
//        {
//            createSphere();
//        }
        
//    }

//    // Update is called once per frame
//    void Update()
//    {
        
//    }

//    void createSphere()
//    {
//        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

//        // zufällige Position
//        float x = UnityEngine.Random.Range(-10f, 10f);
//        float y = UnityEngine.Random.Range(-10f, 10f);
//        float z = UnityEngine.Random.Range(-10f, 10f);
        

//        sphere.transform.position = new Vector3(x, y, z);
//        sphere.transform.localScale = Vector3.one * 0.5f;
//    }
//}
