using RestSharp;
using Sorux.Framework.Bot.Core.Interface.PluginsSDK.Attribute;
using Sorux.Framework.Bot.Core.Interface.PluginsSDK.Models;
using Sorux.Framework.Bot.Core.Interface.PluginsSDK.PluginsModels;
using Sorux.Framework.Bot.Core.Interface.PluginsSDK.Register;
using Sorux.Framework.Bot.Core.Interface.PluginsSDK.SDK.Basic;
using Sorux.Framework.Bot.Core.Kernel.Utils;

namespace EpicMo.Plugins.HttpTest;

public class HttpTestController : BotController
{
    private ILoggerService _loggerService;
    private IBasicAPI _bot;
    private ILongMessageCommunicate _longMessageCommunicate;

    public HttpTestController(ILoggerService loggerService, IBasicAPI bot,
        ILongMessageCommunicate longMessageCommunicate)
    {
        this._loggerService = loggerService;
        this._bot = bot;
        this._longMessageCommunicate = longMessageCommunicate;
    }

    [Event(EventType.GroupMessage)]
    [Command(CommandAttribute.Prefix.Single,"httptest")]
    [Permission("EpicMo.Plugins.HttpTest.TestPrivilege")]
    public PluginFucFlag AutoTest(MessageContext context)
    {
        _bot.SendGroupMessage(context,"请输入想要被测试的网站的url，请以https开头，不以/结尾.");
        string url = _longMessageCommunicate.ReadNextGroupMessageAsync(LongCommunicateType.TriggerUser, context,null)
            .Result!.Message!.GetRawMessage();
        if (string.IsNullOrEmpty(url))
        {
            _bot.SendGroupMessage(context,"你输入的url是错误的!");
            return PluginFucFlag.MsgIntercepted;
        }
        RestClient restClient = new RestClient(url);
        _bot.SendGroupMessage(context,"请输入想要被测试的网站的API指向，请以/开发，以Query前结尾，例如：/test,/s?");
        string path = _longMessageCommunicate.ReadNextGroupMessageAsync(LongCommunicateType.TriggerUser, context,null)
            .Result!.Message!.GetRawMessage();
        if (string.IsNullOrEmpty(path))
        {
            _bot.SendGroupMessage(context,"你输入的API指向是错误的!");
            return PluginFucFlag.MsgIntercepted;
        }
        _bot.SendGroupMessage(context,"请输入想要被测试的网站方法，可以为post或者是get");
        string method = _longMessageCommunicate.ReadNextGroupMessageAsync(LongCommunicateType.TriggerUser, context,null)
            .Result!.Message!.GetRawMessage();
        if (string.IsNullOrEmpty(method))
        {
            _bot.SendGroupMessage(context,"你输入的method指向是错误的!");
            return PluginFucFlag.MsgIntercepted;
        }else if (!method.Equals("post") && !method.Equals("get"))
        {
            _bot.SendGroupMessage(context,"暂时只支持post方法(请输入post)或者是get方法(请输入get)");
            return PluginFucFlag.MsgIntercepted;
        }
        switch (method)
        {
            case "post":
                RestRequest restRequestPost = new RestRequest(path,Method.Post);
                _bot.SendGroupMessage(context,"请输入Post的Header参数，如果输入完毕请发送EOF，参数添加方式：ParaA=w");
                bool loopPost = true;
                while (loopPost)
                {
                    string para = _longMessageCommunicate.ReadNextGroupMessageAsync(LongCommunicateType.TriggerUser, context,null)
                        .Result!.Message!.GetRawMessage();
                    if (para.Equals("EOF"))
                    {
                        loopPost = false;
                    }
                    else
                    {
                        if (para.Split("=").Length != 2)
                        {
                            _bot.SendGroupMessage(context,"参数输入错误，请重新输入！");
                            continue;
                        }
                        restRequestPost.AddHeader(para.Split("=")[0], para.Split("=")[1]);
                        _bot.SendGroupMessage(context,"已添加参数" + para.Split("=")[0] 
                                                              + "且值为：" + para.Split("=")[1]);
                    }
                }
                loopPost = true;
                _bot.SendGroupMessage(context,"请输入Post的Json参数，如果输入完毕请发送EOF，参数添加方式：ParaA=w");
                while (loopPost)
                {
                    string para = _longMessageCommunicate.ReadNextGroupMessageAsync(LongCommunicateType.TriggerUser, context,null)
                        .Result!.Message!.GetRawMessage();
                    if (para.Equals("EOF"))
                    {
                        loopPost = false;
                    }
                    else
                    {
                        if (para.Split("=").Length != 2)
                        {
                            _bot.SendGroupMessage(context,"参数输入错误，请重新输入！");
                            continue;
                        }
                        restRequestPost.AddParameter(para.Split("=")[0], para.Split("=")[1]);
                        _bot.SendGroupMessage(context,"已添加参数" + para.Split("=")[0] 
                                                              + "且值为：" + para.Split("=")[1]);
                    }
                }
                _bot.SendGroupMessage(context, "正在发送请求，若有结果将会返回，若返回内容过大可能无法发送");
                var resPost = restClient.Execute(restRequestPost);
                if (resPost.Content == null)
                {
                    _bot.SendGroupMessage(context, "没有被相应...");
                }
                else
                {
                    _bot.SendGroupMessage(context,resPost.Content!);
                }
                break;
            case "get":
                RestRequest restRequest = new RestRequest(path, Method.Get);
                _bot.SendGroupMessage(context,"请输入Get的Header参数，如果输入完毕请发送EOF，参数添加方式：ParaA=w");
                bool loop = true;
                while (loop)
                {
                    string para = _longMessageCommunicate.ReadNextGroupMessageAsync(LongCommunicateType.TriggerUser, context,null)
                        .Result!.Message!.GetRawMessage();
                    if (para.Equals("EOF"))
                    {
                        loop = false;
                    }
                    else
                    {
                        if (para.Split("=").Length != 2)
                        {
                            _bot.SendGroupMessage(context,"参数输入错误，请重新输入！");
                            continue;
                        }
                        restRequest.AddHeader(para.Split("=")[0], para.Split("=")[1]);
                        _bot.SendGroupMessage(context,"已添加参数" + para.Split("=")[0] 
                                                              + "且值为：" + para.Split("=")[1]);
                    }
                }
                _bot.SendGroupMessage(context,"请输入Get的Query参数，如果输入完毕请发送EOF，参数添加方式：ParaA=w");
                loop = true;
                while (loop)
                {
                    string para = _longMessageCommunicate.ReadNextGroupMessageAsync(LongCommunicateType.TriggerUser, context,null)
                        .Result!.Message!.GetRawMessage();
                    if (para.Equals("EOF"))
                    {
                        loop = false;
                    }
                    else
                    {
                        if (para.Split("=").Length != 2)
                        {
                            _bot.SendGroupMessage(context,"参数输入错误，请重新输入！");
                            continue;
                        }
                        restRequest.AddQueryParameter(para.Split("=")[0], para.Split("=")[1]);
                        _bot.SendGroupMessage(context,"已添加参数" + para.Split("=")[0] 
                                                              + "且值为：" + para.Split("=")[1]);
                    }
                }

                _bot.SendGroupMessage(context, "正在发送请求，若有结果将会返回，若返回内容过大可能无法发送");
                var resGet = restClient.Execute(restRequest);
                if (resGet.Content == null)
                {
                    _bot.SendGroupMessage(context, "没有被相应...");
                }
                else
                {
                    _bot.SendGroupMessage(context,resGet.Content!);
                }
                break;
            default:
                _bot.SendGroupMessage(context,"方法匹配失败，内部错误！");
                return PluginFucFlag.MsgIntercepted;
        }
        return PluginFucFlag.MsgIntercepted;
    }
    
    [Event(EventType.GroupMessage)]
    [Command(CommandAttribute.Prefix.Single,"httptestself [url] [path] [method] [headers] [paras]")]
    [Permission("EpicMo.Plugins.HttpTest.TestPrivilege")]
    public PluginFucFlag SelfTest(MessageContext context,string url,string path,string method,string headers,string paras)
    {
        //httptest [url] [path] [method] [header] [para]
        _bot.SendGroupMessage(context,$"正在尝试向{url}{path}，发送{method}请求，携带的Header是{headers},携带的参数是{paras}，" +
                                      $"如果有返回将会返回，若返回内容过大可能无法发送");
        Thread.Sleep(2000);
        RestClient restClient = new RestClient(url);
        List<string> para = paras.Split(";").ToList();
        List<string> header = headers.Split(";").ToList();
        switch (method)
        {
            case "post":
                RestRequest restRequestPost = new RestRequest(path,Method.Post);
                if (!header[0].Equals("null"))
                {
                    header.ForEach(sp =>
                    {
                        if (sp.Split("=").Length != 2)
                        {
                            _bot.SendGroupMessage(context,"检查到错误的参数格式："+sp + "。将会跳过这个参数");
                        }
                        else
                        {
                            restRequestPost.AddHeader(sp.Split("=")[0], sp.Split("=")[1]);
                        }
                    });
                }

                if (!para[0].Equals("null"))
                {
                    para.ForEach(sp =>
                    {
                        if (sp.Split("=").Length != 2)
                        {
                            _bot.SendGroupMessage(context,"检查到错误的参数格式："+sp + "。将会跳过这个参数");
                        }
                        else
                        {
                            restRequestPost.AddParameter(sp.Split("=")[0], sp.Split("=")[1]);
                        }
                    });
                }
                _bot.SendGroupMessage(context, "正在发送请求，若有结果将会返回");
                var resPost = restClient.Execute(restRequestPost);
                if (resPost.Content == null)
                {
                    _bot.SendGroupMessage(context, "没有被相应...");
                }
                else
                {
                    _bot.SendGroupMessage(context,resPost.Content!);
                }
                break;
            case "get":
                RestRequest restRequest = new RestRequest(path, Method.Get);
                if (!header[0].Equals("null"))
                {
                    header.ForEach(sp =>
                    {
                        if (sp.Split("=").Length != 2)
                        {
                            _bot.SendGroupMessage(context,"检查到错误的参数格式："+sp + "。将会跳过这个参数");
                        }
                        else
                        {
                            restRequest.AddHeader(sp.Split("=")[0], sp.Split("=")[1]);
                        }
                    });
                }

                if (!para[0].Equals("null"))
                {
                    para.ForEach(sp =>
                    {
                        if (sp.Split("=").Length != 2)
                        {
                            _bot.SendGroupMessage(context,"检查到错误的参数格式："+sp + "。将会跳过这个参数");
                        }
                        else
                        {
                            restRequest.AddParameter(sp.Split("=")[0], sp.Split("=")[1]);
                        }
                    });
                }
                _bot.SendGroupMessage(context, "正在发送请求，若有结果将会返回");
                var resGet = restClient.Execute(restRequest);
                if (resGet.Content == null)
                {
                    _bot.SendGroupMessage(context, "没有被相应...");
                }
                else
                {
                    _bot.SendGroupMessage(context,resGet.Content!);
                }
                break;
            default:
                _bot.SendGroupMessage(context,"方法匹配失败，内部错误！");
                return PluginFucFlag.MsgIntercepted;
        }
        return PluginFucFlag.MsgIntercepted;
    }
}