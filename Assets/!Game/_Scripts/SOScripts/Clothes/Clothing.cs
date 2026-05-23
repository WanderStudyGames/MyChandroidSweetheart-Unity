using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Clothing", menuName = "ScriptableObjects/Clothings/Clothing")]
public class Clothing : ScriptableObject
{
    public enum ClothingType
    {
        Top,
        Bottom,
        Hair,
        Accessory
    }
    [Serializable]
    public class ClothingCost
    {
        public Wallet Wallet;
        public int Amount;
    }
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public MaterialOverride[] MaterialOverrides { get; private set; } = { };
    [field: SerializeField] public ClothingType Type;
    [field: SerializeField] public ClothingCost[] Costs = new ClothingCost[0];

    public bool Affordable()
    {
        foreach (var cost in Costs)
        {
            if (cost.Wallet.Currency < cost.Amount) return false;
        }
        return true;
    }

    [SerializeField] private SkinnedMeshRenderer _prefabMesh;
    [SerializeField] private SkinnedMeshRenderer[] _additionalSubmeshes;

    private Mesh CreateCustomMesh(SkinnedMeshRenderer renderer)
    {
        //var mesh = Instantiate(_prefabMesh.sharedMesh);
        Mesh newMesh = new();
        CombineInstance[] combine = new CombineInstance[_additionalSubmeshes.Length];
        for (int i = 0; i < _additionalSubmeshes.Length; i++)
        {
            combine[i].mesh = _additionalSubmeshes[i].sharedMesh;
            combine[i].transform = renderer.transform.localToWorldMatrix;
        }
        newMesh.CombineMeshes(combine);
        BoneWeight[] weights = new BoneWeight[0];
        for (int i = 0; i < _additionalSubmeshes.Length; i++)
        {
            weights = weights.Concat(_additionalSubmeshes[i].sharedMesh.boneWeights).ToArray();
        }

        BoneWeight[] newboneweights = weights;
        Debug.Log(_additionalSubmeshes[0].bones.Length);
        Debug.Log(_additionalSubmeshes[1].bones.Length);
        Debug.Assert(_additionalSubmeshes[0].bones == _additionalSubmeshes[1].bones);

        //Realign boneweights
        int offset = 0;
        for (int i = 0; i < _additionalSubmeshes.Length; i++)
        {
            for (int k = 0; k < _additionalSubmeshes[i].sharedMesh.vertexCount; k++)
            {
                newboneweights[offset + k].boneIndex0 -= _additionalSubmeshes[i].bones.Length * i;
                newboneweights[offset + k].boneIndex1 -= _additionalSubmeshes[i].bones.Length * i;
                newboneweights[offset + k].boneIndex2 -= _additionalSubmeshes[i].bones.Length * i;
                newboneweights[offset + k].boneIndex3 -= _additionalSubmeshes[i].bones.Length * i;
            }
            offset += _additionalSubmeshes[i].sharedMesh.vertexCount;
        }



        newMesh.boneWeights = newboneweights;
        newMesh.bindposes = combine[0].mesh.bindposes;
        newMesh.name = renderer.sharedMesh.name + " (Combined)";
        return newMesh;
    }
    public SkinnedMeshRenderer SpawnRenderer(SkinnedMeshRenderer skinnedMeshRenderer)
    {

        SkinnedMeshRenderer smr;
        smr = Instantiate(_prefabMesh);

        //smr = MeshCombiner.CombineFast(skinnedMeshRenderer.rootBone, MaterialOverrides[1].Material, skinnedMeshRenderer.bones, new Mesh[2] { _prefabMesh.sharedMesh, _additionalSubmeshes[0] });
        //else

        //if (_additionalSubmeshes != null && _additionalSubmeshes.Length > 0)
        //{
        //    List<SkinnedMeshInstance> list = new();
        //    list.Add(new()
        //    {
        //        Material = MaterialOverrides[0].Material,
        //        Mesh = _prefabMesh.sharedMesh,
        //        SMR = skinnedMeshRenderer,
        //        SubMeshIndex = 0,
        //        Transform = skinnedMeshRenderer.transform.localToWorldMatrix
        //    });
        //    for (int i = 0; i < _additionalSubmeshes.Length; i++)
        //    {
        //        var submesh = _additionalSubmeshes[i];
        //        list.Add(new() { Material = MaterialOverrides[1].Material, Mesh = submesh.sharedMesh, SMR = submesh, SubMeshIndex = i + 1, Transform = skinnedMeshRenderer.transform.localToWorldMatrix });
        //    }
        //    var combiner = new SkinnedMeshCombiner();
        //    combiner.CombineMeshes(list, smr);
        //    //smr.sharedMesh = CreateCustomMesh(smr);
        //}

        if (MaterialOverrides != null && MaterialOverrides.Length > 0)
        {
            List<Material> materials = new();
            foreach (var mo in MaterialOverrides)
            {
                mo.ApplyOverrides();
                materials.Add(mo.Materials[0]);
            }
            smr.materials = materials.ToArray();
        }
        smr.ReassignArmature(skinnedMeshRenderer);
        smr.updateWhenOffscreen = skinnedMeshRenderer.updateWhenOffscreen;
        smr.gameObject.layer = skinnedMeshRenderer.gameObject.layer;
        return smr;
    }
}
