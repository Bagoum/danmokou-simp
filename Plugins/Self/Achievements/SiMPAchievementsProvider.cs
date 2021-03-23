﻿using System;
using System.Collections.Generic;
using System.Linq;
using DMK.Achievements;
using DMK.Core;
using DMK.Danmaku;
using DMK.GameInstance;
using DMK.Player;
using JetBrains.Annotations;
using UnityEngine;
using static DMK.Core.GameManagement;

namespace SiMP.Achievements {
[CreateAssetMenu(menuName = "Achievements/SiMP Provider")]
public class SiMPAchievementsProvider : AchievementProviderSO {
    public override AchievementRepo MakeRepo() => new SiMPAchievementRepo();
}

public class SiMPAchievementRepo : AchievementRepo {
    protected override string LocalizationPrefix => "simp.acv";
    private const string smain = "simp.main";
    private const string sex = "simp.ex";
    private Func<InstanceRecord, bool> IsMain => i => i.Campaign == smain;
    private Func<InstanceRecord, bool> IsEx => i => i.Campaign == sex;


    public override State? SavedAchievementState(string key) => SaveData.r.GetAchievementState(key);
    
    public override IEnumerable<Achievement> MakeAchievements() {
        var main = L("maindone", () => new CompletedInstanceRequirement(IsMain));
        var ex = L("exdone", () => new CompletedInstanceRequirement(IsEx));
        return new[] {
            L("open", () => new CompletedFixedReq()),
            L("tutorial", () => new TutorialCompletedReq()),
            L("st1", () => new StageCompletedReq(smain, 0)).Delay(),
            L("st2", () => new StageCompletedReq(smain, 1)).Delay(),
            L("st3", () => new StageCompletedReq(smain, 2)).Delay(),
            L("st4", () => new StageCompletedReq(smain, 3)).Delay(),
            L("st5", () => new StageCompletedReq(smain, 4)).Delay(),
            main.Delay(),
            ex.Delay(),
            L("mainendgood", () => main.Lock(new EndingRequirement("main.good"))).Delay(),
            L("mainendbad", () => main.Lock(new EndingRequirement("main.bad"))).Delay(),
            L("main1cc", () => main.Lock(new Normal1CCRequirement(IsMain))).Delay(),
            L("ex1cc", () => ex.Lock(new Normal1CCRequirement(IsEx))).Delay(),
            L("mainnull", () => main.Lock(new Shot1CCReq("null", IsMain))).Delay(),
            L("exnull", () => ex.Lock(new Shot1CCReq("null", IsEx))).Delay(),
            L("nometer", () => new DidntUseMeter1CCReq(IsMain)).Delay(),
            L("meter", () => new UsedMeterReq()),
            L("c42",
                () => new CustomRequirement(i =>
                    IsMain(i) && i.HitsTaken <= 42 && i.Difficulty.customValueSlider == 42)).Delay(),
            L("aya", () => new StarsRequirement(smain, "simp.aya", 2).SelfLock()),
            L("kasen1", () => new StarsRequirement(smain, "simp.kasen", 5).SelfLock()),
            L("kaguya", () => new StarsRequirement(smain, "simp.kaguya-fake", 5).SelfLock()),
            L("yukari", () => new StarsRequirement(smain, "simp.yukari", 2).SelfLock()),
            L("nemo", () => new StarsRequirement(smain, "simp.eraa", 2).SelfLock()),
            L("seija", () => new StarsRequirement(smain, "simp.seija",
                (s, p) => s <= 0 && p >= 7).SelfLock()),
            L("mima", () => new StarsRequirement(smain, "simp.mima", 12).SelfLock()),
            L("kasen2", () => new StarsRequirement(smain, "simp.kasen2", 4).SelfLock()),
            L("reimu", () => new StarsRequirement(smain, "simp.reimu", 7).SelfLock()),
            L("mokou", () => new StarsRequirement(sex, "simp.mokou", 3).SelfLock()),
            L("yukariex", () => new StarsRequirement(sex, "simp.yukari2",
                (s, p) => s >= 6 && p >= 3).SelfLock()),
            L("shinki", () => new StarsRequirement(sex, "simp.shinki", 12).SelfLock()),
            L("recitation",
                () => new CardCompletionRequirement(sex, "simp.shinki", 11,
                    cr => cr.NoHits && cr.method == PhaseClearMethod.TIMEOUT)),
            L("options", () => new CompletedInstanceRequirement(ir =>
                ir.IsCampaign &&
                ir.SubshotSwitches == 0 &&
                ir.SharedInstanceMetadata.team.Shot != null &&
                ir.SharedInstanceMetadata.team.Shot.isMultiShot)).Delay(),
            L("emptypowershift", () => 
                new EventRequirement(InstanceData.UselessPowerupCollected, () => Instance.IsCampaign)),
            L("mushrooms", () => new CompletedInstanceRequirement(ir => ir.IsCampaign &&
                                                                        ir.OneUpItemsCollected == 0)
                .SelfLock()).Delay(),
            L("darksouls", () => new ListeningRequirement(() => 
                Instance.IsCampaign && Instance.campaignKey == smain &&
                Instance.HitsTaken > 0 && Instance.BossesEncountered.Count == 0, InstanceData.PlayerTookHit).SelfLock()),
            L("bombplz", () => new EventRequirement<PhaseCompletion>(InstanceData.PhaseCompleted, pc => 
                pc.props.phaseType?.IsCard() == true && pc.hits >= 4).SelfLock()),
            L("deathbombplz", () => new ListeningRequirement(() => 
                GameManagement.Instance.LastTookHitFrame > 0 && 
                GameManagement.Instance.LastMeterStartFrame - 
                GameManagement.Instance.LastTookHitFrame < 16, PlayerInput.PlayerActivatedMeter)),
            L("s100mil", () => new CampaignScoreReq(100000000)),
            L("s330mil", () => new CampaignScoreReq(330000000)),
            L("s666mil", () => new CampaignScoreReq(666000000)),
            L("s1bil", () => new CampaignScoreReq(1000000000)),
            L("smul420", () => new CampaignPIVReq(4.20)),
            L("smul690", () => new CampaignPIVReq(6.90)),
            L("graze1337", () => new CampaignGrazeReq(1337)),
            L("graze9000", () => new CampaignGrazeReq(9001)),
            L("maxlives", () => new ListeningRequirement(() => GameManagement.Instance.Lives > 18, 
                InstanceData.AnyExtendAcquired).SelfLock()),
            L("replay", () => new EventRequirement<InstanceRequest>(InstanceRequest.InstancedRequested, ir => ir.replay != null).SelfLock()).Delay(),
            L("practice", () => new EventRequirement<InstanceRequest>(InstanceRequest.InstancedRequested, ir => ir.Mode == InstanceMode.CARD_PRACTICE || ir.Mode == InstanceMode.STAGE_PRACTICE).SelfLock()).Delay()
        };
    }
}
}