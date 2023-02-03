﻿using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.Google;
using Owin;
using WingtipToys.Models;

namespace WingtipToys
{
    public partial class Startup {

        // 認証構成の詳細については、https://go.microsoft.com/fwlink/?LinkId=301883 を参照してください
        public void ConfigureAuth(IAppBuilder app)
        {
            // 要求ごとに 1 つのインスタンスを使用するよう db コンテキスト、ユーザー マネージャー、サインイン マネージャーを構成します
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // サインインしたユーザーの情報を保存するために、アプリケーションが Cookie を使用できるようにします
            // また、サード パーティ ログイン プロバイダーを使用してログインしたユーザーに関する情報を一時的に保存するために、Cookie を使用できるようにします。
            // サインイン Cookie を構成します
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
            // サード パーティ ログイン プロバイダーを使用してログインしたユーザーに関する情報を一時的に保存するために、Cookie を使用します。
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // 2 要素認証処理で第 2 の要素を確認する際に、アプリケーションがユーザー情報を一時的に保存できるようにします。
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // アプリケーションが電話やメールなどの第 2 のログイン確認要素を記憶できるようにします。
            // このオプションをオンにすると、ログイン処理における確認の 2 番目のステップが、ログインしたデバイスで記憶されます。
            // これは、ログインするときの RememberMe オプションと似ています。
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // 次の行のコメントを解除して、サード パーティ ログイン プロバイダーを使用したログインを有効にします
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "958242248308-v960tar6ptfb3hr2p7u3nn414ap1nh6d.apps.googleusercontent.com",
               ClientSecret = "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"
            });
        }
    }
}
