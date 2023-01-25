using Sorux.Bot.Core.Interface.PluginsSDK.Attribute;
using Sorux.Bot.Core.Interface.PluginsSDK.Models;
using Sorux.Bot.Core.Interface.PluginsSDK.PluginsModels;
using Sorux.Bot.Core.Interface.PluginsSDK.Register;
using Sorux.Bot.Core.Interface.PluginsSDK.SDK.Basic;
using Sorux.Bot.Core.Kernel.Utils;

namespace EpicMo.Plugins.VideoParser;

public class VideoController : BotController
{
    private ILoggerService _loggerService;
    private IBasicAPI _basicApi;
    public VideoController(ILoggerService loggerService,IBasicAPI basicApi)
    {
        this._loggerService = loggerService;
        this._basicApi = basicApi;
    }

    [Event(EventType.SoloMessage)]
    [Command(CommandAttribute.Prefix.None,"[SF-ALL]")]
    [Permission("EpicMo.Plugins.VideoParser.TencentVideoParser")]
    public PluginFucFlag Parser(MessageContext messageContext,string content)
    {
        if (content.Contains("m.v.qq.com"))
        {
            string url = TextGainCenter("qqdocurl\":\"", ";\"scene\":", content);
            string cid = TextGainCenter("cid=", "&amp;", url);
            string vid = TextGainCenter("vid=", "&amp;", url);
            string web = "https://v.qq.com/x/cover/" + cid + "/" + vid + ".html";
            _basicApi.SendPrivateMessage(messageContext,"请复制下述链接到浏览器并打开.");
            Thread.Sleep(500);
            _basicApi.SendPrivateMessage(messageContext,"https://jx.playerjy.com/?ads=0&url="+web);
        }else if (content.Contains("v.youku.com"))
        {
            string url = TextGainCenter("https://v.qq.com/search_redirect.html?url=", "&amp;url_from=share", content);
            _basicApi.SendPrivateMessage(messageContext,"请复制下述链接到浏览器并打开.");
            Thread.Sleep(500);
            _basicApi.SendPrivateMessage(messageContext,"https://jx.playerjy.com/?ads=0&url="+url);
        }
        
        return PluginFucFlag.MsgFlag;
        
    }
    
    public static string TextGainCenter(string left, string right, string text) {
        //判断是否为null或者是empty
        if (string.IsNullOrEmpty(left))
            return "";
        if (string.IsNullOrEmpty(right))
            return "";
        if (string.IsNullOrEmpty(text))
            return "";

        int Lindex = text.IndexOf(left); //搜索left的位置
            
        if (Lindex == -1){ //判断是否找到left
            return "";
        }
            
        Lindex = Lindex + left.Length; //取出left右边文本起始位置
            
        int Rindex = text.IndexOf(right, Lindex);//从left的右边开始寻找right
           
        if (Rindex == -1){//判断是否找到right
            return "";   
        }
            
        return text.Substring(Lindex, Rindex - Lindex);//返回查找到的文本
    }
}