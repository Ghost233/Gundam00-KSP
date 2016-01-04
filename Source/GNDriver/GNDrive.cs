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

        //GN绿色粒子补充速率
        [KSPField]
        private float GNGreenParticleGenCountPerSecond = -1000.0f;

        //GN红色粒子补充速率
        [KSPField]
        private float GNRedParticleGenCountPerSecond = -1000.0f;

        //充电速率
        [KSPField]
        private float electricGenCountPerSecond = -5.0f;

        //有害粒子释放速率
        [KSPField]
        private float selfInjuryGenCountPerSecond = -1.0f;        

        //可同步炉子数量
        [KSPField]
        public float maxenginecount = 2F;
        
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
       
        public GNDrive()
        {
           
        }

        public override void OnStart(PartModule.StartState state)
        {
            lastFixedUpdate = Time.time;
            this.part.force_activate();
        }

        public override void OnFixedUpdate()
        {
            float time = Time.time;
            float timeInterval = time - lastFixedUpdate;
            float percentSecond = timeInterval / GNDriveDefine.timeSecond;

            float GNGreenParticleGen = this.GNGreenParticleGenCountPerSecond * percentSecond;
            float GNRedParticleGen = this.GNRedParticleGenCountPerSecond * percentSecond;
            float electricGen = this.electricGenCountPerSecond * percentSecond;
            float selfInjuryGen = this.selfInjuryGenCountPerSecond * percentSecond;

            float responseGNGreenParticleGen = this.part.RequestResource(GNDriveDefine.greenParticleResourceName, GNGreenParticleGen);
            float responseGNRedParticleGen = this.part.RequestResource(GNDriveDefine.redParticleResourceName, GNRedParticleGen);
            float responseelectricGen = this.part.RequestResource(GNDriveDefine.electricResourceName, electricGen);
            float responseselfInjuryGen = this.part.RequestResource(GNDriveDefine.selfInjuryResourceName, selfInjuryGen);

            lastFixedUpdate = time;
        }
    }
}