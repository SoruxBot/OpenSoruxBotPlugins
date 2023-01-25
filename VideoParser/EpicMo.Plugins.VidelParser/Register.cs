using Sorux.Bot.Core.Interface.PluginsSDK.Ability;
using Sorux.Bot.Core.Interface.PluginsSDK.Register;

namespace EpicMo.Plugins.VideoParser;

public class Register : IBasicInformationRegister,ICommandPermission
{
    public string GetAuthor() => "EpicMo";

    public string GetDescription() => "VIP视频解析插件";

    public string GetName() => "VIP-VideoParser";

    public string GetVersion() => "1.0.0-release";

    public string GetDLL() => "EpicMo.Plugins.VideoParser.dll";

    public string GetPermissionDeniedMessage() => "You didn't have this permission";

    public bool IsPermissionDeniedAutoAt() => false;

    public bool IsPermissionDeniedLeakOut() => false;

    public bool IsPermissionDeniedAutoReply() => false;
}