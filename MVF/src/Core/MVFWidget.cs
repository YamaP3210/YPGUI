namespace MVF.Core;





public abstract class MVFWidget : MVFComponent
{
    internal async Task<MVFNode> CreateWidgetNodeAsync ( MVFNode placementNode )
    {
        string widgetHostId = $"{GetType ( ).Name}{MVFCoreNodeDefines.WidgetHostSuffix}";
        string widgetHostHtml = $"<section data-ui-id=\"{widgetHostId}\"></section>";

        await placementNode.AppendHtmlAsync ( widgetHostHtml );

        var widgetHostNode = await MVFNode.FindAsync ( widgetHostId );
        string widgetHtml = await ReadComponentHtmlAsync ( );

        await widgetHostNode.SetHtmlAsync ( widgetHtml );

        return await MVFNode.FindAsync ( GetType ( ).Name );
    }
}
