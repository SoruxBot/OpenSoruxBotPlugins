using Sorux.Bot.Core.Interface.PluginsSDK.Ability;
using Sorux.Bot.Core.Interface.PluginsSDK.Register;

namespace EpicMo.Plugins.HttpTest;

public class Register : IBasicInformationRegister,ICommandPermission,ICommandPrefix
{
    public string GetAuthor() => "EpicMo";

    public string GetDescription() => "用于快速发起HTTP请求测试";

    public string GetName() => "HttpTest";

    public string GetVersion() => "1.0.0-Release";

    public string GetDLL() => "EpicMo.Plugins.HttpTest.dll";

    public string GetPermissionDeniedMessage() => "你不具有本权限...";

    public bool IsPermissionDeniedAutoAt() => false;

    public bool IsPermissionDeniedLeakOut() => false;

    public bool IsPermissionDeniedAutoReply() => false;
    public string GetCommandPrefix() => "#";
}