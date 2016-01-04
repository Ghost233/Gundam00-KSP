using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Gundam00
{
    public class GNTDBlanket : PartModule
    {
        //时间参数
        private float lastFixedUpdate = 0.0f;

        //GN红色粒子转换速率
        [KSPField]
        private float GNRedParticleConsumeCountPerSecond = 1000.0f;
        
        //有害粒子消除速率
        [KSPField]
        private float selfInjuryConsumeCountPerSecond = 2.0f;

        //过滤器当前状态
        [KSPField(guiName = "TD Blanke Status", guiActive = true)]
        private string ES = "Activated";

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

            float GNRedParticleGen = this.GNRedParticleConsumeCountPerSecond * percentSecond;
            float selfInjuryGen = this.selfInjuryConsumeCountPerSecond * percentSecond;
            //float GNGreenParticleGen = -GNRedParticleGen;

            float GNGreenParticleGen = -this.part.RequestResource(GNDriveDefine.redParticleResourceName, GNRedParticleGen);
            //float responseGNRedParticleGen = this.part.RequestResource(GNDriveDefine.redParticleResourceName, GNRedParticleGen);
            float responseGNGreenParticleGen = this.part.RequestResource(GNDriveDefine.greenParticleResourceName, GNGreenParticleGen);
            float responseselfInjuryGen = this.part.RequestResource(GNDriveDefine.selfInjuryResourceName, selfInjuryGen);

            //Debug.Log("GNTDBlanket 1 " + GNGreenParticleGen + "    " + GNRedParticleGen + "    " + selfInjuryGen);
            //Debug.Log("GNTDBlanket 2 " + responseGNGreenParticleGen + "    " + GNGreenParticleGen + "    " + responseselfInjuryGen);

            lastFixedUpdate = time;
        }
    }
}
