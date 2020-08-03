using System;
using System.CodeDom;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

public class ParticleMoveModel
{
      public bool isGravityZ = false;
      public float sizeThreshold = 7f;
      private ParticleSystem.Particle particle;

      private Vector3 goalPosition;
      private Vector3 loopPosition;
      private float goalSize;
      private Color32 goalColor;

      private int modeChangeClock = 0;
      private Vector3 acceralation;
      private Vector3 velocity;

      private bool isFloating = false;
      private float moveSpeed = 0.08f;
      private float colorSpeed = 0.04f;
      private float sizeSpeed = 0.05f;

      private float floatingRate = 1f;

      public ParticleMoveModel(Vector3 __position, Color32 __color, float __size)
      {
            this.particle = new ParticleSystem.Particle();

            this.particle.position = __position;
            this.particle.color = __color;
            this.particle.size = __size;

            this.particle.remainingLifetime = 10f;
            this.particle.startLifetime = 5f;

            this.goalPosition = new Vector3();
            this.loopPosition = new Vector3();
            this.goalSize = 0f;
            this.goalColor = new Color32();
      }

      public void update()
      {
            if (this.isFloating)
            {
                  this.velocity += this.acceralation;
                  this.particle.position += this.velocity;
                  this.particle.size *= 0.994f;
            }
            else
            {
                  this.particle.position = (this.goalPosition - particle.position) * this.moveSpeed + particle.position;
                  this.particle.size = (this.goalSize - particle.size) * this.sizeSpeed + particle.size;
            }

            this.particle.color = new Color32(
                  (byte) ((this.goalColor.r - particle.color.r) * this.colorSpeed + particle.color.r),
                  (byte) ((this.goalColor.g - particle.color.g) * this.colorSpeed + particle.color.g),
                  (byte) ((this.goalColor.b - particle.color.b) * this.colorSpeed + particle.color.b),
                  (byte) ((this.goalColor.a - particle.color.a) * this.colorSpeed + particle.color.a)
            );
      }

      public void setIsFloating(bool __isFloating)
      {
            this.isFloating = __isFloating;
      }

      public void setFloatingRate(float __rate)
      {
            this.floatingRate = __rate;
      }

      public void setGoalPosition(Vector3 __position)
      {
            this.goalPosition = __position;
            this.loopPosition = new Vector3(__position.x, __position.y, __position.z);
            this.resetSpeed();
      }

      public void setGoalSize(float __size)
      {
            this.goalSize = __size;
      }

      public void setGoalColor(Color32 __color)
      {
            this.goalColor = __color;
      }

      public void resetFloating()
      {
            this.isFloating = false;
            this.resetPosition();
            this.resetSpeed();
      }

      private void resetPosition()
      {
            this.particle.position = new Vector3(this.loopPosition.x, this.loopPosition.y, this.loopPosition.z);
      }

      private void resetSpeed()
      {
            float acceralationMin = 0.001f * this.floatingRate;
            float acceralationMax = 0.006f * this.floatingRate;
            float horizontalSpeed = 0.07f * this.floatingRate;

            if (this.isGravityZ)
            {
                  this.acceralation = new Vector3(
                        0f,
                        0f,
                        Random.Range(-acceralationMin, -acceralationMax)
                  );
                  this.velocity = new Vector3(
                        Random.Range(-horizontalSpeed, horizontalSpeed),
                        Random.Range(-horizontalSpeed, horizontalSpeed),
                        0f
                  );
            }
            else
            {
                  this.acceralation = new Vector3(
                        0f,
                        Random.Range(acceralationMin, acceralationMax),
                        0f
                  );
                  this.velocity = new Vector3(
                        Random.Range(-horizontalSpeed, horizontalSpeed),
                        0f,
                        Random.Range(-horizontalSpeed, horizontalSpeed)
                  );
            }
      }

      public ParticleSystem.Particle getParticle()
      {
            return this.particle;
      }
}