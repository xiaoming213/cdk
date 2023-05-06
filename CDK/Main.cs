using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned.Player;
using Rocket.Unturned.Chat;
using UnityEngine;
using Rocket.Unturned;
using Rocket.API;
using Rocket.Core;
using SDG.Unturned;
using fr34kyn01535.Uconomy;
using System.IO;
using Rocket.Core.Logging;
using Newtonsoft.Json.Linq;

namespace CDK
{
    public class Main : RocketPlugin<Config>
    {
        public DatabaseManager Database;
        public static Main Instance;
        private static readonly string USER_AGENT = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/106.0.0.0 Safari/537.36";
        protected override void Load()
        {
            Instance = this;
            CheckUpdate();  
            Database = new DatabaseManager();
            U.Events.OnPlayerConnected += PlayerConnect;
            Rocket.Core.Logging.Logger.Log("CDK Plugin loaded");
        }

        protected override void Unload()
        {
            U.Events.OnPlayerConnected -= PlayerConnect;
            Rocket.Core.Logging.Logger.Log("CDK Plugin unloaded");
        }


        private void PlayerConnect(UnturnedPlayer player)
        {
            Database.CheckValid(player);
        }
        public override TranslationList DefaultTranslations =>
            new TranslationList
            {
                {"success","你成功兑换了CDK."},
                {"key_dones't_exist","钥匙并不存在!"},
                {"don't_have_permisson","你没有权限兑换这个CDK!" },
                {"maxcount_reached","这个CDK已经达到最大兑换量!" },
                {"items_give_fail","未能提供物品!" },
                {"already_redeemed","你已经兑换了这个CDK!" },
                {"permission_duplicate_entry","你已经在权限组中了:{0}." },
                {"permission_granted","你被添加到权限组: {0}" },
                {"permission_grant_error","添加权限组失败" },
                {"uconomy_gain","你得到了 {0} {1}" },
                {"error","错误!" },
                {"invaild_parameter","出局！正确语法:{0}"},
                {"key_renewed","你的钥匙已被更新!" },
                {"key_expired","你的钥匙已经过期:{0}" },
                {"already_purchased","你已经购买了这个权限组" },
                {"invaild_param","错误的使用方法:{0}"},
                {"player_not_match","这个CDK不属于你!" },
                {"cdk_config_error","CDK配置错误。请联系服务器所有者!" }
            };

        private void CheckUpdate()
        {
            
            string dlstring = "https://api.github.com/repos/zeng-github01/CDKey-CodeReward/releases/latest";
            WebClient webClient = new WebClient();
            webClient.Headers.Add("user-agent",USER_AGENT);
             string jsonstring =  webClient.DownloadString(dlstring);
              var json = JObject.Parse(jsonstring);
            Version version = new Version(json["tag_name"].ToString());
            Version crv = Assembly.GetName().Version;
            if(version > crv)
            {
                Rocket.Core.Logging.Logger.Log(String.Format("新的更新{0}已经发布", version.ToString()),ConsoleColor.Green);
                Rocket.Core.Logging.Logger.LogWarning($"{Name} 已被卸载");
                Rocket.Core.Logging.Logger.Log("Go to " + "https://github.com/zeng-github01/CDKey-CodeReward/releases/latest "+ "以获得最新的更新", ConsoleColor.Yellow);
            }
        }
    }
}
