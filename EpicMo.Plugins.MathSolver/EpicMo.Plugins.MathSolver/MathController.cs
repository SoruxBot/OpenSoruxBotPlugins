using System.IO.Compression;
using System.Text;
using System.Xml;
using RestSharp;
using Sorux.Framework.Bot.Core.Interface.PluginsSDK.Attribute;
using Sorux.Framework.Bot.Core.Interface.PluginsSDK.Models;
using Sorux.Framework.Bot.Core.Interface.PluginsSDK.PluginsModels;
using Sorux.Framework.Bot.Core.Interface.PluginsSDK.Register;
using Sorux.Framework.Bot.Core.Interface.PluginsSDK.SDK.Basic;
using Sorux.Framework.Bot.Core.Interface.PluginsSDK.SDK.FutureTest.QQ;
using Sorux.Framework.Bot.Core.Kernel.Interface;
using Sorux.Framework.Bot.Core.Kernel.Utils;

namespace EpicMo.Plugins.MathSolver;

public class MathController : BotController
{
    private IBasicAPI bot;
    private ILoggerService _loggerService;
    private IPluginsDataStorage _pluginsDataStorage;
    public MathController(ILoggerService loggerService, IBasicAPI bot, IPluginsDataStorage pluginsDataStorage)
    {
        this.bot = bot;
        this._loggerService = loggerService;
        this._pluginsDataStorage = pluginsDataStorage;
    }
    
    [Event(EventType.SoloMessage)]
    [Command(CommandAttribute.Prefix.Single,"mathappid [appid]")]
    public PluginFucFlag StorageAPI(MessageContext context,string appid)
    {
        if (!String.IsNullOrEmpty(_pluginsDataStorage.GetStringSettings("math",context.TriggerId)))
        {
            bot.SendPrivateMessage(context,"检测到重复添加的Wolfram API，正在重新添加....");
            _pluginsDataStorage.RemoveStringSettings("math", context.TriggerId);
        }
        _pluginsDataStorage.AddStringSettings("math", context.TriggerId, appid);
        bot.SendPrivateMessage(context,"已添加API:"+appid);
        return PluginFucFlag.MsgIntercepted;
    }

    [Event(EventType.GroupMessage)]
    [Command(CommandAttribute.Prefix.Single,"mathselect [q]")]
    public PluginFucFlag Select(MessageContext context, string q)
    {
        
        if (String.IsNullOrEmpty(_pluginsDataStorage.GetStringSettings("math",context.TriggerId)))
        {
            bot.SendGroupMessage(context,"并没有检测到你存储了相关的API，请私聊机器人'#mathappid [appid]'来添加你的API");
            return PluginFucFlag.MsgIntercepted;
        }

        string appid = _pluginsDataStorage.GetStringSettings("math", context.TriggerId);
        q = System.Web.HttpUtility.UrlEncode(q);
        string url = "http://api.wolframalpha.com/v2/query?appid=" + appid + "&input=" + q +
                     "&podstate=Step-by-step%20solution";
        XmlTextReader reader = new XmlTextReader(url);
        StringBuilder stringBuilder = new StringBuilder();
        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.Element) 
            {
                if (reader.Name.Equals("pod"))
                {
                    reader.MoveToNextAttribute();
                    stringBuilder.Append("#" + reader.Value);
                }else if (reader.Name.Equals("subpod"))
                {
                    reader.MoveToNextAttribute();
                    stringBuilder.Append(">"+reader.Value);
                }else if (reader.Name.Equals("img"))
                {
                    reader.MoveToNextAttribute();
                    stringBuilder.Append("[CQ:image,file=" + reader.Value +",subType=0,cache=0,id=40000]\n");
                }
            }
        }
        bot.SendGroupMessage(context,QqMessageExtension.QqCreateReply(context.UnderProperty["message_id"]
            ,null,null,null,null)+stringBuilder);
        return PluginFucFlag.MsgIntercepted;
    }
    
    [Event(EventType.GroupMessage)]
    [Command(CommandAttribute.Prefix.Single,"math [q]")]
    public PluginFucFlag MathSelect(MessageContext context, string q)
    {
        string appid = _pluginsDataStorage.GetStringSettings("math", "1728913755");
        q = System.Web.HttpUtility.UrlEncode(q);
        string url = "http://api.wolframalpha.com/v2/query?appid=" + appid + "&input=" + q +
                     "&podstate=Step-by-step%20solution";
        XmlTextReader reader = new XmlTextReader(url);
        StringBuilder stringBuilder = new StringBuilder();
        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.Element) 
            {
                if (reader.Name.Equals("pod"))
                {
                    reader.MoveToNextAttribute();
                    stringBuilder.Append("#" + reader.Value);
                }else if (reader.Name.Equals("subpod"))
                {
                    reader.MoveToNextAttribute();
                    stringBuilder.Append(">"+reader.Value);
                }else if (reader.Name.Equals("img"))
                {
                    reader.MoveToNextAttribute();
                    stringBuilder.Append("[CQ:image,file=" + reader.Value +",subType=0,cache=0,id=40000]\n");
                }
            }
        }
        bot.SendGroupMessage(context,QqMessageExtension.QqCreateReply(context.UnderProperty["message_id"]
            ,null,null,null,null)+stringBuilder);
        return PluginFucFlag.MsgIntercepted;
    }
    
    [Event(EventType.GroupMessage)]
    [Command(CommandAttribute.Prefix.Single,"help [msg]")]
    public PluginFucFlag HelpMath(MessageContext context, string msg)
    {
        if (msg.Equals("math"))
        {
            bot.SendGroupMessage(context,"数学计算器(Wolfram)版:\n"
                                         +"使用'#math [q]'来计算q表达式");
        }
        return PluginFucFlag.MsgIntercepted;
    }
}