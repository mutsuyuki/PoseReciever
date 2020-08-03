using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AmbientParticle: MonoBehaviour
{
      [SerializeField] private int partcleAmount = 99;

      private ParticleSystem.Particle[] m_targetParticles;
      private ParticleSystem targetParticleSystem;
      private float elapsed = 0f;

      private int mode = 0;

      private Vector3[] goalPositions;
      private Color32[] goalColors;
      private float[] goalSizes;

      private int modeChangeClock = 0;

      void Start()
      {
            m_targetParticles = new ParticleSystem.Particle[partcleAmount];
            targetParticleSystem = gameObject.GetComponent<ParticleSystem>();
            targetParticleSystem.maxParticles = partcleAmount;

            this.goalPositions = new Vector3[this.partcleAmount];
            this.goalColors = new Color32[this.partcleAmount];
            this.goalSizes = new float[this.partcleAmount];

            for (int i = 0; i < partcleAmount; i++)
            {
                  m_targetParticles[i].color = Color.cyan;
                  m_targetParticles[i].remainingLifetime = 10f;
                  m_targetParticles[i].startLifetime = 5f;
                  m_targetParticles[i].size = 0.2f;
            }
      }

      void Update()
      {
            this.elapsed += Time.deltaTime;
//            if (this.elapsed <= Math.PI)
//            {
//                  return;
//            }


            if (Input.GetKeyDown(KeyCode.A))
            {
                  this.mode = 0;
                  this.modeChangeClock = 1;
            }
            else if (Input.GetKeyDown(KeyCode.B))
            {
                  this.mode = 1;
                  this.modeChangeClock = 1;
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                  this.mode = 2;
                  this.modeChangeClock = 1;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                  this.mode = 3;
                  this.modeChangeClock = 1;
            }

            if (this.modeChangeClock == 1)
            {
                  this.setRandomPosition();
                  this.modeChangeClock++;
            }
            else
            {
                  if (this.modeChangeClock > 15)
                  {
                        if (this.mode == 0)
                        {
                              this.setGridMotion();
                        }
                        else if (this.mode == 1)
                        {
                              this.setWaveMotion();
                        }
                        else if (this.mode == 2)
                        {
                              this.setPerlinMotion();
                        }
                        else if (this.mode == 3)
                        {
                              this.setSelectMode();
                        }
                  }
                  this.modeChangeClock++;
            }


            for (int i = 0; i < m_targetParticles.Length; i++)
            {
                  m_targetParticles[i].position = (this.goalPositions[i] - m_targetParticles[i].position) * 0.2f +
                                                  m_targetParticles[i].position;
                  m_targetParticles[i].size = (this.goalSizes[i] - m_targetParticles[i].size) * 0.2f +
                                              m_targetParticles[i].size;
                  m_targetParticles[i].color = new Color32(
                        (byte)
                        ((this.goalColors[i].r - m_targetParticles[i].color.r) * 0.2f + m_targetParticles[i].color.r),
                        (byte)
                        ((this.goalColors[i].g - m_targetParticles[i].color.g) * 0.2f + m_targetParticles[i].color.g),
                        (byte)
                        ((this.goalColors[i].b - m_targetParticles[i].color.b) * 0.2f + m_targetParticles[i].color.b),
                        (byte)
                        ((this.goalColors[i].a - m_targetParticles[i].color.a) * 0.2f + m_targetParticles[i].color.a)
                  );
            }
            targetParticleSystem.SetParticles(m_targetParticles, m_targetParticles.Length);

            //パーティクルシステム全体をカメラと正対するように
//            this.transform.position = ((Camera.main.transform.position * -1f) - this.transform.position) * 0.1f +
//                                      this.transform.position;
//            this.transform.LookAt(Camera.main.transform.position);
      }

      private void setRandomPosition()
      {
            int sideSize = Mathf.FloorToInt(Mathf.Sqrt(this.partcleAmount));

            for (int x = 0; x < sideSize; x++)
            {
                  for (int y = 0; y < sideSize; y++)
                  {
                        this.goalPositions[sideSize * x + y] = new Vector3(
                              Random.Range(-1f, 1f) * 600f - 300f,
                              Random.Range(-1f, 1f) * 600f - 300f,
                              0f
                        );
                        this.goalSizes[sideSize * x + y] = 2f;
//                        this.goalColors[sideSize * x + y] = Color.white;
                  }
            }
      }

      private void setSelectMode()
      {
            int sideSize = Mathf.FloorToInt(Mathf.Sqrt(this.partcleAmount));

            for (int x = 0; x < sideSize; x++)
            {
                  for (int y = 0; y < sideSize; y++)
                  {
                        if (x % 2 == 0)
                        {

                        }
                        this.goalPositions[sideSize * x + y] = new Vector3(
                              (float) x % 10f * 600f - 300f,
                              (float) y % 10f * 600f - 300f,
                              0f
                        );
                        this.goalSizes[sideSize * x + y] = 3f;
                        this.goalColors[sideSize * x + y] = Color.white;
                  }
            }
      }

      private void setPerlinMotion()
      {
            int sideSize = Mathf.FloorToInt(Mathf.Sqrt(this.partcleAmount));

            for (int x = 0; x < sideSize; x++)
            {
                  for (int y = 0; y < sideSize; y++)
                  {
                        this.goalPositions[sideSize * x + y] = new Vector3(
                              (float) x / (float) sideSize * 600f - 300f,
                              (float) y / (float) sideSize * 600f - 300f,
                              Mathf.PerlinNoise(this.elapsed + (float) x * 0.1f, this.elapsed + (float) y * 0.1f) * 10f -
                              5f
                        );
                        this.goalSizes[sideSize * x + y] =
                              Mathf.PerlinNoise(this.elapsed + (float) x * 0.1f, this.elapsed + (float) y * 0.1f) * 7f;
                        this.goalColors[sideSize * x + y] = Color.magenta;
                  }
            }
      }

      private void setWaveMotion()
      {
//            int sideSize = Mathf.FloorToInt(Mathf.Sqrt(this.partcleAmount));
//
//            for (int x = 0; x < sideSize; x++)
//            {
//                  for (int y = 0; y < sideSize; y++)
//                  {
//                        this.goalPositions[sideSize * x + y] = new Vector3(
//                              (float) x / (float) sideSize * 600f - 300f,
//                              Mathf.Sin(this.elapsed + y + (float) x / sideSize * 10f) * 100f,
//                              (float) y / (float) sideSize * 20f - 10f
//                        );
//                        m_targetParticles[sideSize * x + y].size = 2f;
//                  }
//            }

            for (int i = 0; i < this.goalPositions.Length; i++)
            {
                  this.goalPositions[i] = new Vector3(
                        (float) i / (float) this.partcleAmount * 600f - 300f,
                        Mathf.Sin(this.elapsed + (float) i / this.partcleAmount * 10f) * 100f,
                        i % 30f * 100f
                  );
                  this.goalSizes[i] = 3f;
                  this.goalColors[i] = Color.cyan;
            }
      }

      private void setGridMotion()
      {
            int sideSize = Mathf.FloorToInt(Mathf.Sqrt(this.partcleAmount));

            for (int x = 0; x < sideSize; x++)
            {
                  for (int y = 0; y < sideSize; y++)
                  {
                        this.goalPositions[sideSize * x + y] = new Vector3(
                              Mathf.Sin((float) x / (float) sideSize * Mathf.PI * 2f) * 300f,
                              (float) y / (float) sideSize * 2000f - 1000f,
                              Mathf.Cos((float) x / (float) sideSize * Mathf.PI * 2f) * 300f
                        );
                        this.goalSizes[sideSize * x + y] = Mathf.Sin(this.elapsed * 2f + (float) y / sideSize * 3f) * 4f + 5f;
                        this.goalColors[sideSize * x + y] = Color.cyan;
                  }
            }
      }
}