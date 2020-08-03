using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

public class AbsorbableParticle : MonoBehaviour
{
      [SerializeField] private int partcleAmount = 99;
      [SerializeField] private bool isGravityZ = false;

      private int sideSize;
      private float particleSize = 0.8f;
      private float floatingThresholdRate = 1.000f;
      private float floatingThreshold;

      private ParticleSystem _particleSystem;
      private ParticleSystem.Particle[] particles;
      private ParticleMoveModel[] _particleMoveModels;
      private float elapsed = 0f;

      private string modeName = "";
      private bool isModeChange = false;
      private float[] previousSizes;

      private Color32 normalColor = Color.cyan;

      private delegate void SetPattern();

      private SetPattern _currentPattern = delegate { };
      private float timeProcessRate = 1f;
      private float noiseStrength = 0f;
      private float particleSizeRate = 1f;

      void Start()
      {
            this.particles = new ParticleSystem.Particle[this.partcleAmount];
            this._particleSystem = this.gameObject.GetComponent<ParticleSystem>();
            this._particleSystem.maxParticles = this.partcleAmount;
            this.floatingThreshold = this.particleSize * this.floatingThresholdRate;
            this.previousSizes = new float[this.partcleAmount];

            this._particleMoveModels = new ParticleMoveModel[this.partcleAmount];
            for (int i = 0; i < this._particleMoveModels.Length; i++)
            {
                  this._particleMoveModels[i] = new ParticleMoveModel(
                        new Vector3(
                              Random.Range(-200f, 200f),
                              Random.Range(-200f, 200f),
                              Random.Range(-100f, 100f)
                        ),
                        this.normalColor,
                        4f
                  );
                  this._particleMoveModels[i].isGravityZ = this.isGravityZ;
                  this._particleMoveModels[i].sizeThreshold = this.floatingThreshold;
                  this._particleMoveModels[i].setGoalColor(Color.cyan);
                  this._particleMoveModels[i].setFloatingRate(this.timeProcessRate);
            }

            this.sideSize = Mathf.FloorToInt(Mathf.Sqrt(this.partcleAmount));

            this._currentPattern = this.setSolidGrid;
            LeanTween.delayedCall(this.gameObject, 2.5f, () => { this._currentPattern = this.setFlowGridMotion; });
      }

      void Update()
      {
            this.elapsed += Time.deltaTime * this.timeProcessRate;
            this._currentPattern();

            for (int i = 0; i < this._particleMoveModels.Length; i++)
            {
                  this._particleMoveModels[i].update();
                  this.particles[i] = this._particleMoveModels[i].getParticle();
            }
            this._particleSystem.SetParticles(particles, particles.Length);
      }

      public void setParticleColor(Color32 __color)
      {
            this.normalColor = __color;
      }

      public void setWaveSpeed(float __waveSpeed)
      {
            this.timeProcessRate = __waveSpeed;
      }

      public void setParticleSizeRate(float __particleSizeRate)
      {
            this.particleSizeRate= __particleSizeRate;
      }

      private void modeChange(string __mode)
      {
            this.isModeChange = this.modeName != __mode;
            this.modeName = __mode;
            if (isModeChange)
            {
                  if (!this.isGravityZ)
                  {
                        this.elapsed = Mathf.PI / 1.6f;
                  }
                  else
                  {
                        this.elapsed = Mathf.PI;
                  }
            }
      }

      private void setSolidGrid()
      {
            this.modeChange("solidgrid");
            if (!isModeChange)
                  return;

            for (int x = 0; x < sideSize; x++)
            {
                  for (int y = 0; y < sideSize; y++)
                  {
                        this._particleMoveModels[sideSize * x + y].setGoalPosition(new Vector3(
                              (float) x / (float) sideSize * 80f - 40f,
                              (float) y / (float) sideSize * 80f - 40f,
                              0f
                        ));
                        this._particleMoveModels[sideSize * x + y].setGoalColor(this.normalColor);
                        this._particleMoveModels[sideSize * x + y].setGoalSize(this.floatingThreshold * 0.99f);
                  }
            }
      }

      private void setFlowGridMotion()
      {
            this.modeChange("flowgrid");
            for (int x = 0; x < this.sideSize; x++)
            {
                  for (int y = 0; y < this.sideSize; y++)
                  {
                        int index = sideSize * x + y;
                        float size = (Mathf.Sin(this.elapsed  + (float) y / sideSize * Mathf.PI * 2f) * this.particleSize);

                        if (this.isModeChange)
                        {
                              this._particleMoveModels[index].setGoalPosition(new Vector3(
                                    (float) x / (float) sideSize * 80f - 40f,
                                    (float) y / (float) sideSize * 80f - 40f,
                                    0f
                              ));
                        }

                        if (size > this.floatingThreshold && this.previousSizes[index] <= this.floatingThreshold)
                        {
                              this._particleMoveModels[index].setIsFloating(true);
                        }

                        if (size >= 0f && this.previousSizes[index] < 0f)
                        {
                              this._particleMoveModels[index].resetFloating();
                              this._particleMoveModels[index].setIsFloating(false);
                        }

                        this._particleMoveModels[index].setGoalColor(this.normalColor);

                        float perlin =(Mathf.PerlinNoise(this.elapsed / 2f+ (float)x * 0.2f, this.elapsed /2f + (float)y * 0.2f) - 0.5f) * this.noiseStrength;
                        this._particleMoveModels[index].setGoalSize( size * this.particleSizeRate + perlin);

                        this.previousSizes[index] = size;
                  }
            }
      }

      private void setRandomPosition()
      {
            this.modeChange("random");
            if (!isModeChange)
                  return;

            for (int x = 0; x < sideSize; x++)
            {
                  for (int y = 0; y < sideSize; y++)
                  {
                        this._particleMoveModels[sideSize * x + y].setGoalPosition(new Vector3(
                              Random.Range(-200f, 200f),
                              Random.Range(-200f, 200f),
                              Random.Range(-100f, 100f)
                        ));
                        this._particleMoveModels[sideSize * x + y].setGoalSize(this.floatingThreshold * 0.99f);
                  }
            }
      }

      public void setNoiseStrength(float __noiseStrength)
      {
            this.noiseStrength = __noiseStrength;
      }

}