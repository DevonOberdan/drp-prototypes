using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DrawToMaskFeature : ScriptableRendererFeature
{
    class MaskRenderPass : ScriptableRenderPass
    {
        public Mesh meshToDraw;
        public Material material;
        public RenderTexture target;
        public Matrix4x4 matrix = Matrix4x4.identity;
        public bool shouldRender = false;

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (!shouldRender || meshToDraw == null || material == null || target == null)
                return;

            CommandBuffer cmd = CommandBufferPool.Get("DrawToMaskPass");

            cmd.SetRenderTarget(target);
            cmd.ClearRenderTarget(true, true, Color.clear);
            cmd.DrawMesh(meshToDraw, matrix, material);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);

            shouldRender = false; // reset trigger
        }
    }

    MaskRenderPass pass;

    public Material fillMaterial;
    public RenderTexture maskTexture;

    public override void Create()
    {
        pass = new MaskRenderPass
        {
            renderPassEvent = RenderPassEvent.AfterRendering
        };
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        // Fill in dynamic data before enqueuing
        pass.material = fillMaterial;
        pass.target = maskTexture;

        renderer.EnqueuePass(pass);
    }

    // External trigger
    public void TriggerDraw(Mesh mesh, Matrix4x4 transform)
    {
        pass.meshToDraw = mesh;
        pass.matrix = transform;
        pass.shouldRender = true;
    }
}
