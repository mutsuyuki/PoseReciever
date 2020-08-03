using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class Human : MonoBehaviour {
    [SerializeField] private GameObject partsPrefab;
    private List<GameObject> parts = new List<GameObject>();
    private List<Vector3> goalPositions = new List<Vector3>();
    private VectorLine bodyLine;

    private int counter = 0;
    
    void Start() {
         
        // 0  MPIIPart.Head
        // 1  MPIIPart.Neck
        // 2  MPIIPart.RShoulder
        // 3  MPIIPart.RElbow
        // 4  MPIIPart.RWrist
        // 5  MPIIPart.LShoulder
        // 6  MPIIPart.LElbow
        // 7  MPIIPart.LWrist
        // 8  MPIIPart.RHip
        // 9  MPIIPart.RKnee
        // 10 MPIIPart.RAnkle
        // 11 MPIIPart.LHip
        // 12 MPIIPart.LKnee
        // 13 MPIIPart.LAnkle

        List<Vector3> linePositions = new List<Vector3>();
        for (int i = 0; i < 14; i++) {
            GameObject partInstance = Instantiate(partsPrefab);
            partInstance.transform.parent = transform;
            if(i == 0)
                partInstance.transform.localScale = partInstance.transform.localScale * 1.5f;
                
            parts.Add(partInstance);

            linePositions.Add(Vector3.zero);
            linePositions.Add(Vector3.zero);
        }
        partsPrefab.SetActive(false);

        bodyLine = new VectorLine("BodyLine", linePositions, 3.0f, LineType.Discrete);
    }

    private void Update() {
        if (counter > 200) {
            gameObject.SetActive(false);
            bodyLine.rectTransform.gameObject.SetActive(false);
            return;
        }
        counter++;
       
        for (int i = 0; i < goalPositions.Count; i++) {
            Vector3 currentPos;
            if (goalPositions[i].x == 0 && goalPositions[i].y == 0 && goalPositions[i].z == 0) {
                currentPos = new Vector3(-9999, 0, 0);
            } else {
                if (parts[i].transform.position.x < -9998) {
                    currentPos = new Vector3(goalPositions[i].x, goalPositions[i].y, goalPositions[i].z);
                } else {
//                    currentPos = Vector3.Lerp(parts[i].transform.position, goalPositions[i], 0.3f);
                    currentPos = new Vector3(goalPositions[i].x, goalPositions[i].y, goalPositions[i].z);
                }
            }
            
            
            if (i == 0) currentPos.z -= 0.1f;
            if (i == 3 || i == 6) currentPos.z -= 0.1f;
            if (i == 4 || i == 7) currentPos.z -= 0.2f;
            if (i == 9 || i == 12) currentPos.z -= 0.05f;
            
            parts[i].transform.position = currentPos;
        }

        if (parts[0].transform.position.x > -9998 && parts[1].transform.position.x > -9998) {
            bodyLine.points3[0] = parts[0].transform.position;
            bodyLine.points3[1] = parts[1].transform.position;
        } else {
            bodyLine.points3[0] = Vector3.zero;
            bodyLine.points3[1] =  Vector3.zero;
        }
        
        if (parts[1].transform.position.x > -9998 && parts[2].transform.position.x > -9998) {
            bodyLine.points3[2] = parts[1].transform.position;
            bodyLine.points3[3] = parts[2].transform.position;
        } else {
            bodyLine.points3[2] = Vector3.zero;
            bodyLine.points3[3] = Vector3.zero;
        }
        
        if (parts[2].transform.position.x > -9998 && parts[3].transform.position.x > -9998) {
            bodyLine.points3[4] = parts[2].transform.position;
            bodyLine.points3[5] = parts[3].transform.position;
        } else {
            bodyLine.points3[4] = Vector3.zero;
            bodyLine.points3[5] = Vector3.zero;
        }
        
        if (parts[3].transform.position.x > -9998 && parts[4].transform.position.x > -9998) {
            bodyLine.points3[6] = parts[3].transform.position;
            bodyLine.points3[7] = parts[4].transform.position;
        } else {
            bodyLine.points3[6] = Vector3.zero;
            bodyLine.points3[7] = Vector3.zero;
        }
        
        if (parts[1].transform.position.x > -9998 && parts[5].transform.position.x > -9998) {
            bodyLine.points3[8] = parts[1].transform.position;
            bodyLine.points3[9] = parts[5].transform.position;
        } else {
            bodyLine.points3[8] = Vector3.zero;
            bodyLine.points3[9] = Vector3.zero;
        }
        
        if (parts[5].transform.position.x > -9998 && parts[6].transform.position.x > -9998) {
            bodyLine.points3[10] = parts[5].transform.position;
            bodyLine.points3[11] = parts[6].transform.position;
        } else {
            bodyLine.points3[10] = Vector3.zero;
            bodyLine.points3[11] = Vector3.zero;
        }
        
        if (parts[6].transform.position.x > -9998 && parts[7].transform.position.x > -9998) {
            bodyLine.points3[12] = parts[6].transform.position;
            bodyLine.points3[13] = parts[7].transform.position;
        } else {
            bodyLine.points3[12] = Vector3.zero;
            bodyLine.points3[13] = Vector3.zero;
        }
        
        if (parts[5].transform.position.x > -9998 && parts[11].transform.position.x > -9998) {
            bodyLine.points3[14] = parts[5].transform.position;
            bodyLine.points3[15] = parts[11].transform.position;
        } else {
            bodyLine.points3[14] = Vector3.zero;
            bodyLine.points3[15] = Vector3.zero;
        }
        
        if (parts[2].transform.position.x > -9998 && parts[8].transform.position.x > -9998) {
            bodyLine.points3[16] = parts[2].transform.position;
            bodyLine.points3[17] = parts[8].transform.position;
        } else {
            bodyLine.points3[16] = Vector3.zero;
            bodyLine.points3[17] = Vector3.zero;
        }
        
        if (parts[8].transform.position.x > -9998 && parts[11].transform.position.x > -9998) {
            bodyLine.points3[18] = parts[8].transform.position;
            bodyLine.points3[19] = parts[11].transform.position;
        } else {
            bodyLine.points3[18] = Vector3.zero;
            bodyLine.points3[19] = Vector3.zero;
        }
        
        if (parts[8].transform.position.x > -9998 && parts[9].transform.position.x > -9998) {
            bodyLine.points3[20] = parts[8].transform.position;
            bodyLine.points3[21] = parts[9].transform.position;
        } else {
            bodyLine.points3[20] = Vector3.zero;
            bodyLine.points3[21] = Vector3.zero;
        }
        
        if (parts[9].transform.position.x > -9998 && parts[10].transform.position.x > -9998) {
            bodyLine.points3[22] = parts[9].transform.position;
            bodyLine.points3[23] = parts[10].transform.position;
        } else {
            bodyLine.points3[22] = Vector3.zero;
            bodyLine.points3[23] = Vector3.zero;
        }
        
        if (parts[11].transform.position.x > -9998 && parts[12].transform.position.x > -9998) {
            bodyLine.points3[24] = parts[11].transform.position;
            bodyLine.points3[25] = parts[12].transform.position;
        } else {
            bodyLine.points3[24] = Vector3.zero;
            bodyLine.points3[25] = Vector3.zero;
        }
        
        if (parts[12].transform.position.x > -9998 && parts[13].transform.position.x > -9998) {
            bodyLine.points3[26] = parts[12].transform.position;
            bodyLine.points3[27] = parts[13].transform.position;
        } else {
            bodyLine.points3[26] = Vector3.zero;
            bodyLine.points3[27] = Vector3.zero;
        }
        

        bodyLine.Draw3D();
    }

    public void updatePose(List<Vector3> __partsPositions) {
        goalPositions = __partsPositions;
        
        gameObject.SetActive(true);
        bodyLine.rectTransform.gameObject.SetActive(true);
        counter = 0;
    }

    public Vector3 getNeckPos() {
        return parts[1].transform.position;

    }
}