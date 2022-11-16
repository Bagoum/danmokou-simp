using Danmokou;
using Danmokou.Achievements;
using Danmokou.Danmaku;
using Danmokou.GameInstance;
using UnityEngine;

namespace SiMP {
[CreateAssetMenu(menuName = "Data/GameDef/SiMP")]
public class SiMPGameDef : CampaignDanmakuGameDef {
    public override AchievementRepo MakeAchievements() => new SiMPAchievementRepo();

    public override InstanceFeatures MakeFeatures(DifficultySettings d, long? highScore) => new() {
        Score = new ScoreFeatureCreator(highScore),
        Power = new DisabledPowerFeatureCreator(),
        Faith = new FaithFeatureCreator(),
        ItemExt = new LifeItemExtendFeatureCreator(),
        Rank = new DisabledRankFeatureCreator(),
        ScoreExt = new ScoreExtendFeatureCreator(),
        Meter = d.meterEnabled ? 
            new MeterFeatureCreator() : 
            new DisabledMeterFeatureCreator()
    };
}
}