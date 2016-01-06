using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Gundam00
{
    public class GNDrive : PartModule
    {
        //时间参数
        private float lastFixedUpdate = 0.0f;

        //GN红色粒子补充速率
        [KSPField]
        private float GNRedParticleGenCountPerSecond = -1000.0f;

        //充电速率
        [KSPField]
        private float electricGenCountPerSecond = -5.0f;

        //Trans-AM
        [KSPField]
        public bool transamactivated = false;

        //太阳炉当前状态
        [KSPField(guiName = "GNDrive Status", guiActive = true)]
        private string ES = "Activated";

        [KSPEvent(name = "ActivateTransAM", guiName = "Trans-AM", active = true, guiActive = false)]
        public void Activateta()
        {
            transamactivated = true;
            Events["ActivateTransAM"].guiActive = false;
        }

        [KSPField(guiName = "Particle Generate Efficiency", guiActive = true)]
        private string particleGenerateEfficiencyString = "100%";

        [KSPField]
        private float particleGenerateEfficiencyCount = 10000;
        private float particleGenerateEfficiencyMax = 10000;
        private float particleGenerateEfficiency = 1;

        public GNDrive()
        {
           
        }

        public override void OnStart(PartModule.StartState state)
        {
            lastFixedUpdate = Time.time;
            this.part.force_activate();
        }

        public bool restoreGenerateEfficiency(float percent, out float restoneLast)
        {
            restoneLast = 0;
            if (particleGenerateEfficiencyCount + percent > particleGenerateEfficiencyMax)
            {
                if (particleGenerateEfficiencyCount == particleGenerateEfficiencyMax) return false;
                restoneLast = particleGenerateEfficiencyMax - particleGenerateEfficiencyCount;
                particleGenerateEfficiencyCount = particleGenerateEfficiencyMax;
            }
            else
            {
                particleGenerateEfficiencyCount += percent;
            }
            particleGenerateEfficiency = GNDriveDefine.expoEaseIn(particleGenerateEfficiencyCount / particleGenerateEfficiencyMax);
            particleGenerateEfficiencyString = string.Format("{0:P}", particleGenerateEfficiency);
            return true; 
        }

        public override void OnFixedUpdate()
        {
            float time = Time.time;
            float timeInterval = time - lastFixedUpdate;
            float percentSecond = timeInterval / GNDriveDefine.timeSecond;

            float restoneLast = 0;
            this.restoreGenerateEfficiency(-percentSecond, out restoneLast);

            Debug.Log(-percentSecond + "           " + particleGenerateEfficiencyCount);

            float GNRedParticleGen = this.GNRedParticleGenCountPerSecond * percentSecond * particleGenerateEfficiency;
            float electricGen = this.electricGenCountPerSecond * percentSecond * particleGenerateEfficiency;

            float responseGNRedParticleGen = this.part.RequestResource(GNDriveDefine.redParticleResourceName, GNRedParticleGen);
            float responseElectricGen = this.part.RequestResource(GNDriveDefine.electricResourceName, electricGen);

            Debug.Log("ClassID:" + this.ClassID + "   UnityID" + this.GetInstanceID() + "   particleGenerateEfficiency " + particleGenerateEfficiency + "   GNRedParticleGen:" + GNRedParticleGen + "   electricGen" + electricGen);

            lastFixedUpdate = time;
        }
    }
}