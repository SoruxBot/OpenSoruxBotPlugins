using Sorux.Framework.Bot.Core.Interface.PluginsSDK.Ability;
using Sorux.Framework.Bot.Core.Interface.PluginsSDK.Register;

namespace EpicMo.Plugins.MathSolver;

public class Register : IBasicInformationRegister , ICommandPrefix , ICommandPermission
{
    public string GetAuthor() => "EpicMo";

    public string GetDescription() => "数学求解插件";

    public string GetName() => "MathSolver";

    public string GetVersion() => "v1.0.0-release";

    public string GetDLL() => "EpicMo.Plugins.MathSolver.dll";
    
    public string GetCommandPrefix() => "#";

    public string GetPermissionDeniedMessage() => "";

    public bool IsPermissionDeniedAutoAt() => false;

    public bool IsPermissionDeniedLeakOut() => false;

    public bool IsPermissionDeniedAutoReply() => false;
}