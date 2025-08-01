using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DrawPolygonToMaskFeature : ScriptableRendererFeature
{
    class CustomPass : ScriptableRenderPass
    {
        Mesh mesh;
        Material mat;
        RenderTexture target;

        public CustomPass(Mesh mesh, Material mat, RenderTexture target)
        {
            this.mesh = mesh;
            this.mat = mat;
            this.target = target;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get("DrawHoleMask");

            cmd.SetRenderTarget(target);
            cmd.ClearRenderTarget(true, true, Color.clear);
            cmd.DrawMesh(mesh, Matrix4x4.identity, mat);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }

    CustomPass pass;

    public Material fillMaterial;
    public Mesh polygonMesh;
    public RenderTexture maskTex;

    public override void Create()
    {
        if (polygonMesh != null && fillMaterial != null && maskTex != null)
        {
            pass = new CustomPass(polygonMesh, fillMaterial, maskTex)
            {
                renderPassEvent = RenderPassEvent.AfterRendering
            };
        }
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData data)
    {
        if (pass != null)
            renderer.EnqueuePass(pass);
    }
}
