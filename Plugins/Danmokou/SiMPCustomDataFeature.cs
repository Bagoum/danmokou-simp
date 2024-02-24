using Danmokou.GameInstance;

namespace SiMP {
public class SiMPCustomDataFeature : BaseInstanceFeature, ICustomDataFeature {
    public InstanceData Inst { get; }
    public float SomeVariable { get; set; } = 4f;

    public SiMPCustomDataFeature(InstanceData inst) {
        Inst = inst;
    }
}
public class SiMPCustomDataFeatureCreator : IFeatureCreator<ICustomDataFeature> {
    public ICustomDataFeature Create(InstanceData instance) => new SiMPCustomDataFeature(instance);
}
}