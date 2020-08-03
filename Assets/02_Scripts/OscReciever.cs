using System;
using System.Collections.Generic;
using UnityEngine;

public class OscReciever : MonoBehaviour {
    [SerializeField] private Camera camera;
    private Vector3 cameraInitialPos;
    private Vector3 cameraGoalPos;

    [SerializeField] private OSC osc;
    [SerializeField] private Human humanPrefab;
    private List<Human> human2Ds = new List<Human>();
    private List<Human> human3Ds = new List<Human>();

    private bool isForward = true;
    void Start() {
        cameraGoalPos = camera.transform.position;
        cameraInitialPos = Vector3.Lerp(camera.transform.position, camera.transform.position, 1.0f);

        osc.SetAddressHandler("/pose_2d", OnReceive2d);
        osc.SetAddressHandler("/pose_3d", OnReceive3d);

        ///////  2 person  start ////
        Vector3 pos = camera.transform.position;
        pos.x = 4.35f;
        pos.y = 8.89f;
        pos.z = -7.67f;
        camera.transform.position = pos;
        ///////  2 person  end ////
    }

    void Update() {
        camera.transform.LookAt(Vector3.up * 4.0f);
        
//        camera.transform.position = Vector3.Lerp(camera.transform.position, cameraGoalPos, 0.012f);
//        camera.transform.LookAt(Vector3.up * 4.0f);
//
//        float maxNeckPosX = -9999;
//        for (int i = 0; i < human2Ds.Count; i++) {
//            maxNeckPosX = Math.Max(human2Ds[i].getNeckPos().x, maxNeckPosX);
//        }
//
//        if (human2Ds.Count > 0) {
//            float absNeckPosX = Math.Abs(maxNeckPosX);
//            if (absNeckPosX > 1.3f) {
//                cameraGoalPos = new Vector3(
//                    maxNeckPosX * 7.0f,
//                    cameraInitialPos.y + absNeckPosX * 4.0f,
//                    cameraInitialPos.z + absNeckPosX * 2.0f
//                );
//            } else {
//                cameraGoalPos = new Vector3(
//                    maxNeckPosX / 2.0f,
//                    cameraInitialPos.y + absNeckPosX / 3.0f,
//                    cameraInitialPos.z + absNeckPosX / 2.0f
//                );
//            }
//        }
        
    }

    void OnReceive2d(OscMessage message) {
        int index = (int) message.values[0];

        if (index >= human2Ds.Count) {
            Human human = Instantiate(humanPrefab);
            human.transform.parent = transform;
            human2Ds.Add(human);
        }

        List<Vector3> partsPositions = new List<Vector3>();
        for (int i = 1; i < message.values.Count; i += 2) {
            if ((float) message.values[i] == 0f && (float) message.values[i + 1] == 0f) {
                partsPositions.Add(Vector3.zero);
            } else {
                float partsX = (float) message.values[i] * -8f + 4f;
                float partsY = (float) message.values[i + 1] * -8f + 8f;
                partsPositions.Add(new Vector3(partsX, partsY, 0));
            }
        }


        float neckPosX = partsPositions[1].x;
        for (int i = 0; i < partsPositions.Count; i++) {
            if (partsPositions[i].x != 0 && partsPositions[i].y != 0) {
                Vector3 partsPosition = partsPositions[i];
                partsPosition.x += neckPosX;
                partsPositions[i] = partsPosition;
            }
        }

        human2Ds[index].updatePose(partsPositions);
    }


    void OnReceive3d(OscMessage message) {
        int index = (int) message.values[0];

        if (index >= human2Ds.Count) {
            Human human = Instantiate(humanPrefab);
            human.transform.parent = transform;
            human3Ds.Add(human);
        }

        List<Vector3> partsPositions = new List<Vector3>();
        for (int i = 1; i < message.values.Count; i += 3) {
            float partsX = (float) message.values[i] * 8f - 4f;
            float partsY = (float) message.values[i + 1] * -8f + 4f;
            float partsZ = (float) message.values[i + 2] * -8f + 4f;
            partsPositions.Add(new Vector3(partsX, partsY, partsZ));
        }

        human3Ds[index].updatePose(partsPositions);
    }
}